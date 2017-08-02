using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LimitlessLedWinForms.V5
{
	public partial class FormV5 : Form
	{
		protected int lightGroupRadio = 0;

		public FormV5()
		{
			InitializeComponent();
		}

		private void hScrollBar1_ValueChanged(object sender, EventArgs e)
		{
			//handleBrightness();
		}

		private void radioButton1_CheckedChanged(object sender, EventArgs e)
		{
			lightGroupRadio = 1;
		}

		private void radioButton2_CheckedChanged(object sender, EventArgs e)
		{
			lightGroupRadio = 2;
		}

		private void radioButton3_CheckedChanged(object sender, EventArgs e)
		{
			lightGroupRadio = 3;
		}

		private void radioButton4_CheckedChanged(object sender, EventArgs e)
		{
			lightGroupRadio = 4;
		}


		private bool brightnessThrottle = false;
		private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
		{
			// Scroll fires twice, once leading once trailing - only respond to trailing
			brightnessThrottle = !brightnessThrottle;
			if (brightnessThrottle)
			{
				return;
			}

			handleBrightness();
		}

		private void handleBrightness()
		{
			if (lightGroupRadio == 0)
				return;

			string ip = ipTextBox.Text;

			int val = hScrollBar1.Value;
			int brightness = val;

			Task.Run(async () =>
			{
				using (var leds = new LimitlessLedV5(ip))
				{
					if (brightness == 1)
					{
						// off - 1 is an invalid value anyway, it's 2-27
						await leds.SendOffAsync(lightGroupRadio);
					}
					else
					{
						await leds.SendWhiteAsync(lightGroupRadio);
						await leds.SendBrightnessAsync(lightGroupRadio, brightness);
					}
				}
			}).ConfigureAwait(false);
		}
	}
}
