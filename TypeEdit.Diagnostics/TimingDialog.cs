using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace TypeEdit.Diagnostics
{
	public class TimingDialog : Form
	{
		private IContainer components;

		private Timer timer1;

		private Label label1;

		private Button button1;

		private Button button2;

		private Panel panel2;

		private Button button3;

		private FlowLayoutPanel flowLayoutPanel1;

		private Panel panel1;

		private TimingManager mTimingManager;

		protected override void Dispose(bool disposing)
		{
			if (disposing && components != null)
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Expected O, but got Unknown
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Expected O, but got Unknown
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Expected O, but got Unknown
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Expected O, but got Unknown
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Expected O, but got Unknown
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Expected O, but got Unknown
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Expected O, but got Unknown
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Expected O, but got Unknown
			//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
			components = new Container();
			timer1 = new Timer(components);
			label1 = new Label();
			button1 = new Button();
			button2 = new Button();
			panel2 = new Panel();
			button3 = new Button();
			flowLayoutPanel1 = new FlowLayoutPanel();
			panel1 = new Panel();
			((Control)panel2).SuspendLayout();
			((Control)flowLayoutPanel1).SuspendLayout();
			((Control)this).SuspendLayout();
			timer1.Enabled = true;
			timer1.Interval = 20;
			timer1.Tick += timer1_Tick;
			((Control)label1).AutoSize = true;
			((Control)label1).ForeColor = SystemColors.WindowText;
			((Control)label1).Location = new Point(8, 8);
			((Control)label1).Margin = new Padding(8);
			((Control)label1).Name = "label1";
			((Control)label1).Size = new Size(35, 13);
			((Control)label1).TabIndex = 0;
			((Control)label1).Text = "label1";
			((Control)button1).Anchor = (AnchorStyles)4;
			((Control)button1).Location = new Point(3, 3);
			((Control)button1).Name = "button1";
			((Control)button1).Size = new Size(108, 23);
			((Control)button1).TabIndex = 1;
			((Control)button1).Text = "Reset Iterations";
			((ButtonBase)button1).UseVisualStyleBackColor = true;
			((Control)button1).Click += button1_Click;
			((Control)button2).Anchor = (AnchorStyles)4;
			((Control)button2).Location = new Point(117, 3);
			((Control)button2).Name = "button2";
			((Control)button2).Size = new Size(127, 23);
			((Control)button2).TabIndex = 2;
			((Control)button2).Text = "Clear Dead Threads";
			((ButtonBase)button2).UseVisualStyleBackColor = true;
			((Control)button2).Click += button2_Click;
			((ScrollableControl)panel2).AutoScroll = true;
			((Control)panel2).BackColor = SystemColors.Window;
			((Control)panel2).Controls.Add((Control)(object)label1);
			((Control)panel2).Dock = (DockStyle)5;
			((Control)panel2).Location = new Point(0, 29);
			((Control)panel2).Name = "panel2";
			((Control)panel2).Size = new Size(428, 254);
			((Control)panel2).TabIndex = 4;
			((Control)button3).Anchor = (AnchorStyles)4;
			((Control)button3).Location = new Point(250, 3);
			((Control)button3).Name = "button3";
			((Control)button3).Size = new Size(80, 23);
			((Control)button3).TabIndex = 3;
			((Control)button3).Text = "Clear All";
			((ButtonBase)button3).UseVisualStyleBackColor = true;
			((Control)button3).Click += button3_Click;
			((Control)flowLayoutPanel1).AutoSize = true;
			((Control)flowLayoutPanel1).Controls.Add((Control)(object)button1);
			((Control)flowLayoutPanel1).Controls.Add((Control)(object)button2);
			((Control)flowLayoutPanel1).Controls.Add((Control)(object)button3);
			((Control)flowLayoutPanel1).Dock = (DockStyle)1;
			((Control)flowLayoutPanel1).Location = new Point(0, 0);
			((Control)flowLayoutPanel1).Name = "flowLayoutPanel1";
			((Control)flowLayoutPanel1).Size = new Size(428, 29);
			((Control)flowLayoutPanel1).TabIndex = 5;
			((Control)panel1).BackColor = SystemColors.ControlDark;
			((Control)panel1).Dock = (DockStyle)1;
			((Control)panel1).Location = new Point(0, 29);
			((Control)panel1).Name = "panel1";
			((Control)panel1).Size = new Size(428, 1);
			((Control)panel1).TabIndex = 6;
			((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
			((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
			((Form)this).ClientSize = new Size(428, 283);
			((Control)this).Controls.Add((Control)(object)panel1);
			((Control)this).Controls.Add((Control)(object)panel2);
			((Control)this).Controls.Add((Control)(object)flowLayoutPanel1);
			((Form)this).MaximizeBox = false;
			((Form)this).MinimizeBox = false;
			((Control)this).Name = "TimingDialog";
			((Control)this).Text = "Timing";
			((Control)panel2).ResumeLayout(false);
			((Control)panel2).PerformLayout();
			((Control)flowLayoutPanel1).ResumeLayout(false);
			((Control)this).ResumeLayout(false);
			((Control)this).PerformLayout();
		}

		public TimingDialog(TimingManager timingMan)
		{
			mTimingManager = timingMan;
			InitializeComponent();
			base.SetStyle((ControlStyles)139282, true);
		}

		private void timer1_Tick(object sender, EventArgs e)
		{
			((Control)label1).Text = mTimingManager.ToString();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			mTimingManager.ClearDeadThreads();
		}

		private void button2_Click(object sender, EventArgs e)
		{
			mTimingManager.ResetIterations();
		}

		private void button3_Click(object sender, EventArgs e)
		{
			mTimingManager.ResetAll();
		}
	}
}
