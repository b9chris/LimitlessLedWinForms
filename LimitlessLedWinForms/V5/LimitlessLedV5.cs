using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace LimitlessLedWinForms.V5
{
	public class LimitlessLedV5 : IDisposable
	{
		protected UdpClient udpClient;

		public LimitlessLedV5(string ip)
		{
			udpClient = new UdpClient(ip, 8899);
		}

		public async Task SendAsync(byte command)
		{
			var result = await udpClient.SendAsync(new byte[] { command, 0x00, 0x55 }, 3);
		}

		public async Task SendColorAsync(int group, byte color)
		{
			//http://www.limitlessled.com/dev/
			// To send color to group first group byte, wait 100ms, then 3-byte color
			// group bytes go 0x44 all groups, 0x45 group 1, 0x46 group 2 etc
			await SendAsync((byte)(0x43 + group));
			await Task.Delay(100);
			await udpClient.SendAsync(new byte[] { 0x40, color, 0x55 }, 3);
		}

		public async Task SendBrightnessAsync(int group, int brightness)
		{
			brightness = brightness < 2
				? 2
				: brightness > 27
				? 27
				: brightness;
			byte color = (byte)brightness;

			await SendAsync((byte)(0x43 + group*2));
			await Task.Delay(100);
			await udpClient.SendAsync(new byte[] { 0x4E, color, 0x55 }, 3);
		}

		public async Task SendWhiteAsync(int group)
		{
			await SendAsync((byte)(0x43 + group*2));
			await Task.Delay(100);
			await SendAsync((byte)(0xc3 + group*2));
		}

		public async Task SendOffAsync(int group)
		{
			await SendAsync((byte)(0x44 + group*2));
			await Task.Delay(100);
			await SendAsync((byte)(0x68 + group*2));
		}

		public void Dispose()
		{
			udpClient.Close();
		}
	}
}
