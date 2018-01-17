using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using LimitlessLedWinForms.Brass9.Threading.TPL;

namespace LimitlessLedWinForms.V6
{
	public partial class FormV6 : Form
	{
		protected LimitlessLedV6 leds;

		protected int index;
		protected bool isFullRgb;
		protected int brightness;

		protected DateTime lastUpdate;
		protected bool isThrottleScheduled;
		protected object lockObj = new object();

		protected StringBuilder log = new StringBuilder();

		public FormV6()
		{
			InitializeComponent();

			for (int i = 0; i < 4; i++)
			{
				var radio = new RadioButton();
				radio.Tag = i;
				radio.Location = new Point(20, i*40+20);
				radio.Size = new Size(20, 20);
				radio.Click += radio_Click;
				radioGroupBox.Controls.Add(radio);
			}

			for (int i = 0; i < 4; i++)
			{
				var radio = new RadioButton();
				radio.Tag = i + 4;
				radio.Location = new Point(90, i*40+20);
				radio.Size = new Size(20, 20);
				radio.Click += radio_Click;
				radioGroupBox.Controls.Add(radio);
			}
			
			TaskHelper.RunBg(async () =>
			{
				leds = new LimitlessLedV6();
				await leds.StartAsync();
				//await leds.TestAsync();
			});
		}

		void radio_Click(object sender, EventArgs ev)
		{
			RadioButton radio = (RadioButton)sender;
			index = (int)radio.Tag;
			isFullRgb = false;
			if (index > 3)
			{
				isFullRgb = true;
				index -= 4;
			}
		}

		protected void brightnessBar_Scroll(object sender, ScrollEventArgs ev)
		{
			// v scrollbar runs 100 top to 0 bottom, flip it, and, 91 seems to be real bottom
			int amt = 80 - ev.NewValue;
			brightness = (int)Math.Round(amt / .8m * .64m);

			log.AppendLine("amt " + amt + " b " + brightness);

			// Throttle, don't update more than every 500ms
			DateTime now = DateTime.UtcNow;
			lock (lockObj)
			{
				var future = lastUpdate.AddSeconds(1);//lastUpdate.AddMilliseconds(500);
				if (future > now)
				{
					if (isThrottleScheduled)
					{
						log.AppendLine("thottled");
						return;
					}

					isThrottleScheduled = true;
					log.AppendLine("scheduled");
					TaskHelper.RunBg(async () =>
					{
						await Task.Delay(800);
						log.AppendLine("tail");
						updateLight();
						isThrottleScheduled = false;
						lastUpdate = DateTime.UtcNow;
					});
					return;
				}

				lastUpdate = now;
			}

			log.AppendLine("immediate");
			updateLight();
		}


		protected int isSending = 0;

		protected void updateLight()
		{
			int group = index + 1;

			TaskHelper.RunBg(async () =>
			{
				if (1 == Interlocked.CompareExchange(ref isSending, 1, 0))
					return;				

				log.AppendLine("group " + group + " b " + brightness);
				if (isFullRgb)
				{
					if (brightness <= 0)
						await leds.Leds.SwitchFullRgbGroupAsync(group, false);
					else
						await leds.Leds.SetFullRgbBrightness(group, brightness);
				}
				else
				{
					if (brightness <= 0)
						await leds.Leds.SwitchLimitedGroupAsync(group, false);
					else
						await leds.Leds.SetLimitedBrightness(group, brightness);
				}

				isSending = 0;
			});
		}
	}
}
