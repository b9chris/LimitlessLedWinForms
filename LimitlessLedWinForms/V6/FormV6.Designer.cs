namespace LimitlessLedWinForms.V6
{
	partial class FormV6
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.radioGroupBox = new System.Windows.Forms.GroupBox();
			this.brightnessBar = new System.Windows.Forms.VScrollBar();
			this.lightLabel = new System.Windows.Forms.Label();
			this.radioGroupBox.SuspendLayout();
			this.SuspendLayout();
			// 
			// radioGroupBox
			// 
			this.radioGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.radioGroupBox.Controls.Add(this.lightLabel);
			this.radioGroupBox.Location = new System.Drawing.Point(13, 13);
			this.radioGroupBox.Name = "radioGroupBox";
			this.radioGroupBox.Size = new System.Drawing.Size(196, 236);
			this.radioGroupBox.TabIndex = 0;
			this.radioGroupBox.TabStop = false;
			// 
			// brightnessBar
			// 
			this.brightnessBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.brightnessBar.Location = new System.Drawing.Point(223, 13);
			this.brightnessBar.Name = "brightnessBar";
			this.brightnessBar.Size = new System.Drawing.Size(49, 236);
			this.brightnessBar.TabIndex = 1;
			this.brightnessBar.Value = 100;
			this.brightnessBar.Scroll += new System.Windows.Forms.ScrollEventHandler(this.brightnessBar_Scroll);
			// 
			// lightLabel
			// 
			this.lightLabel.AutoSize = true;
			this.lightLabel.Location = new System.Drawing.Point(7, 6);
			this.lightLabel.Name = "lightLabel";
			this.lightLabel.Size = new System.Drawing.Size(121, 13);
			this.lightLabel.TabIndex = 0;
			this.lightLabel.Text = "Limited             Full RGB";
			// 
			// FormV6
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(284, 261);
			this.Controls.Add(this.brightnessBar);
			this.Controls.Add(this.radioGroupBox);
			this.Name = "FormV6";
			this.Text = "FormV6";
			this.radioGroupBox.ResumeLayout(false);
			this.radioGroupBox.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox radioGroupBox;
		private System.Windows.Forms.VScrollBar brightnessBar;
		private System.Windows.Forms.Label lightLabel;
	}
}