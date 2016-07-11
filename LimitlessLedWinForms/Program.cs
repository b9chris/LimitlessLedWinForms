using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LimitlessLedWinForms
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			/*Task.Run(async () =>
			{
				await FireLights();
			}).ConfigureAwait(false);
			 */

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new Form1());

		}

		/*
		public static async Task FireLights()
		{
			using(var leds = new LimitlessLed("192.168.9.3"))
			{
				//await leds.SendColorAsync(1, 0x55);
				//await leds.SendBrightnessAsync(1, 27);
				//await leds.SendWhiteAsync(1);
			}

			
			//using(var udpClient = new UdpClient("192.168.9.102", 8899))
			//{
				//udpClient.Send(new byte[] { 0x38, 0x0, 0x55 }, 3);//0x22, 0x0, 0x55
				// Apparently we can send more, now...?
			//}
			
		}
		*/
	}
}
