using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace LimitlessLedWinForms.V6
{
	public class LLedSession
	{
		protected UdpClient udp;
		protected byte bridge1;
		protected byte bridge2;
		protected byte lastSequenceNumber = 0;
		protected DateTime lastStartTime;

		protected bool[] limitedOn = { false, false, false, false };
		protected bool[] fullRgbOn = { false, false, false, false };


		public LLedSession(UdpClient udp)
		{
			this.udp = udp;
		}

		public async Task StartAsync()
		{
			var buffer = await sendAsync(0x20, 0x00, 0x00, 0x00, 0x16, 0x02, 0x62, 0x3A, 0xD5, 0xED, 0xA3, 0x01, 0xAE, 0x08, 0x2D, 0x46, 0x61, 0x41, 0xA7, 0xF6, 0xDC, 0xAF, 0xD3, 0xE6, 0x00, 0x00, 0x1E);
			bridge1 = buffer[19];
			bridge2 = buffer[20];
			lastSequenceNumber = 0;
			lastStartTime = DateTime.UtcNow;
		}


		public async Task SwitchFullRgbGroupAsync(int group, bool shouldSetOn)
		{
			fullRgbOn[group-1] = shouldSetOn;
			var lightCmd = new byte[] { 0x08, 0x04, (byte)(shouldSetOn ? 0x01 : 0x02) };
			await sendGroupAsync(group, lightCmd);
		}
		public async Task SwitchLimitedGroupAsync(int group, bool shouldSetOn)
		{
			limitedOn[group-1] = shouldSetOn;
			var lightCmd = new byte[] { 0x07, 0x03, (byte)(shouldSetOn ? 0x01 : 0x02) };
			await sendGroupAsync(group, lightCmd);
		}


		public async Task SetFullRgbBrightness(int group, int brightness)
		{
			if (!fullRgbOn[group - 1])
				await SwitchFullRgbGroupAsync(group, true);

			var lightCmd = new byte[] { 0x08, 0x03, (byte)brightness };
			await sendGroupAsync(group, lightCmd);
		}
		public async Task SetLimitedBrightness(int group, int brightness)
		{
			if (!limitedOn[group - 1])
				await SwitchLimitedGroupAsync(group, true);

			var lightCmd = new byte[] { 0x07, 0x02, (byte)brightness };
			await sendGroupAsync(group, lightCmd);
		}


		protected async Task<byte[]> sendAsync(params byte[] bytes)
		{
			await udp.SendAsync(bytes, bytes.Length);
			var re = await udp.ReceiveAsync();
			return re.Buffer;
		}

		protected async Task sendGroupAsync(int group, byte[] lightCmd)
		{
			// Need to periodically renew Session Keys with Bridge.
			// Sessions are very short, definitely less than 2 minutes.
			// 1 minute is working in tests
			var now = DateTime.UtcNow;
			var sinceLastStart = now - lastStartTime;
			if (sinceLastStart.TotalSeconds >= 60)
				await StartAsync();	// Renew the Session first

			var preamble = new byte[] { 0x80, 0x00, 0x00, 0x00, 0x11,
				bridge1, bridge2, 0x00, (byte)lastSequenceNumber++, 0x00, };

			var items = new byte[] { 0x31, 0x00, 0x00 }
				.Concat(lightCmd);

			if (lightCmd.Length == 3)	// Allow short commands - zero-fill
				items = items.Concat(new byte[] { 0x00, 0x00, 0x00 });

			var cmd = items.Concat(new byte[] { (byte)group } ).ToArray();

			var chk = getChecksum(cmd);

			await sendAsync(preamble.Concat(cmd).Concat(chk).ToArray());
			
			// Let the lights catch up 100ms between commands
			await Task.Delay(100);
		}

		protected byte[] getChecksum(byte[] cmd)
		{
			int sum = cmd.Sum(b => (int)b);
			var bytes = BitConverter.GetBytes(sum);
			// We need to account for local CPU Endianess when converting its ints to bytes
			// avg Intel x64 is Big Endian, but we'll get it right either way with BitConverter.IsLittleEndian
			// https://stackoverflow.com/a/1318948/176877
			byte[] chk;
			if (BitConverter.IsLittleEndian)
				chk = new byte[] { bytes[1], bytes[0] };
			else
				chk = new byte[] { bytes[2], bytes[3] };	
			return chk;
		}


		/*
		public async Task TestChangeAsync()
		{
			//await switchGroupAsync(2, true);
			//await sendGroupAsync(2, new byte[] { 0x08, 0x03, 0x40, 0x00, 0x00, 0x00 });
			//await sendGroupAsync(1, new byte[] { 0x07, 0x03, 0x01, 0x00, 0x00, 0x00 });
			//await Task.Delay(100);
			//await sendGroupAsync(1, new byte[] { 0x07, 0x02, 0x64, 0x00, 0x00, 0x00 });
			//await SwitchLimitedGroupAsync(1, false);
			//await SwitchFullRgbGroupAsync(2, false);

			//await SetFullRgbBrightness(2, 48);
			await SetLimitedBrightness(1, 60);
			//for(int i = 1; i < 65; i++)
				//await SetLimitedBrightness(1, i);
		}
		*/
	}
}
