using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace LimitlessLedWinForms.V6
{
	public class LimitlessLedV6 : IDisposable
	{
		protected UdpClient udp;
		public LLedSession Leds;

		protected UdpClient makeUdp(string ip, int port)
		{
			var _udp = new UdpClient(ip, port);
			_udp.EnableBroadcast = true;
			_udp.DontFragment = true;
			_udp.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, 1000);
			return _udp;
		}

		protected async Task<int> sendAsync(UdpClient udp, params byte[] bytes)
		{
			int re = await udp.SendAsync(bytes, bytes.Length);
			return re;
		}
		protected async Task<int> sendAsync(UdpClient udp, string s)
		{
			int re = await sendAsync(udp, Encoding.UTF8.GetBytes(s));
			return re;
		}

		//protected byte lastSequenceNumber = 0;

		//protected async Task<int> sendCmdAsync(UdpClient udp, )


		public async Task StartAsync()
		{
			udp = makeUdp("192.168.9.2", 5987);
			Leds = new LLedSession(udp);
			await Leds.StartAsync();
		}

		// http://www.limitlessled.com/dev/
		// https://github.com/domoticz/domoticz/blob/master/hardware/Limitless.cpp
		public async Task TestAsync()
		{
			using(udp = makeUdp("192.168.9.2", 5987))
			{
				var led = new LLedSession(udp);
				await led.StartAsync();
				//await led.GroupChangeAsync(2, true);
				//await led.TestChangeAsync();

				/*
				int l = await sendAsync(udp, 0x20, 0x00, 0x00, 0x00, 0x16, 0x02, 0x62, 0x3A, 0xD5, 0xED, 0xA3, 0x01, 0xAE, 0x08, 0x2D, 0x46, 0x61, 0x41, 0xA7, 0xF6, 0xDC, 0xAF, 0xD3, 0xE6, 0x00, 0x00, 0x1E);
				var re = await udp.ReceiveAsync();
				var buffer = re.Buffer;
				byte bridge1 = buffer[19];
				byte bridge2 = buffer[20];

				l = await sendAsync(udp, 0x80, 0x00, 0x00, 0x00, 0x11,
					bridge1, bridge2, 0x00, (byte)lastSequenceNumber++, 0x00,
						0x31, 0x00, 0x00, 0x08, 0x04, 0x01,
						0x00, 0x00, 0x00,
					0x02, 0x00, 0x40);
				re = await udp.ReceiveAsync();
				*/
			}
		}

		// Does not work, even in their sample application. Probably bad documentation.
		protected async Task findBridgeAsync()
		{
			//try
			//{
				// Find bridge
				using (var udpAsk = new UdpClient("255.255.255.255", 48899))
				{
					udpAsk.EnableBroadcast = true;
					udpAsk.DontFragment = true;
					udpAsk.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, 3000);
					int i = await sendAsync(udpAsk, "HF-A11ASSISTHREAD");

					using (var udpListen = new UdpClient("255.255.255.255", 48899))
					{

					var re = await udpAsk.ReceiveAsync();
					var reS = Encoding.UTF8.GetString(re.Buffer);
					var split = reS.Split(',');
					var ip = split[0];
					}
				}
			//}
			//catch (Exception ex)
			//{
//				string s = ex.Message;
	//		}
		}



		public void Dispose()
		{
			if (udp != null)
				udp.Close();
		}
	}
}
