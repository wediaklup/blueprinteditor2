using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using TypeEdit.Base;
using TypeEdit.Interfaces;

namespace TypeEdit.Diagnostics
{
	public class AssertionDialog : Form
	{
		private IContainer components;

		private Button btnContinue;

		private Button btnIgnore;

		private Button btnBreak;

		private Label label1;

		private Label label3;

		private Panel panel1;

		private Panel panel2;

		private AssertionDialogResult mAssertionDialogResult;

		public AssertionDialogResult AssertionDialogResult
		{
			get
			{
				return mAssertionDialogResult;
			}
			protected set
			{
				mAssertionDialogResult = value;
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && components != null)
			{
				components.Dispose();
			}
			((Form)this).Dispose(disposing);
		}

		private void InitializeComponent()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Expected O, but got Unknown
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Expected O, but got Unknown
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Expected O, but got Unknown
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Expected O, but got Unknown
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Expected O, but got Unknown
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Expected O, but got Unknown
			btnContinue = new Button();
			btnIgnore = new Button();
			btnBreak = new Button();
			label1 = new Label();
			label3 = new Label();
			panel1 = new Panel();
			panel2 = new Panel();
			((Control)panel1).SuspendLayout();
			((Control)panel2).SuspendLayout();
			((Control)this).SuspendLayout();
			((Control)btnContinue).Anchor = (AnchorStyles)10;
			((Control)btnContinue).Location = new Point(229, 10);
			((Control)btnContinue).Name = "btnContinue";
			((Control)btnContinue).Size = new Size(75, 23);
			((Control)btnContinue).TabIndex = 0;
			((Control)btnContinue).Text = "Continue";
			((ButtonBase)btnContinue).UseVisualStyleBackColor = true;
			((Control)btnContinue).Click += btnContinue_Click;
			((Control)btnIgnore).Anchor = (AnchorStyles)10;
			((Control)btnIgnore).Location = new Point(310, 10);
			((Control)btnIgnore).Name = "btnIgnore";
			((Control)btnIgnore).Size = new Size(75, 23);
			((Control)btnIgnore).TabIndex = 1;
			((Control)btnIgnore).Text = "Ignore";
			((ButtonBase)btnIgnore).UseVisualStyleBackColor = true;
			((Control)btnIgnore).Click += btnIgnore_Click;
			((Control)btnBreak).Anchor = (AnchorStyles)10;
			((Control)btnBreak).Location = new Point(391, 10);
			((Control)btnBreak).Name = "btnBreak";
			((Control)btnBreak).Size = new Size(75, 23);
			((Control)btnBreak).TabIndex = 2;
			((Control)btnBreak).Text = "Break";
			((ButtonBase)btnBreak).UseVisualStyleBackColor = true;
			((Control)btnBreak).Click += btnBreak_Click;
			((Control)label1).Anchor = (AnchorStyles)13;
			((Control)label1).Location = new Point(12, 9);
			((Control)label1).Name = "label1";
			((Control)label1).Size = new Size(454, 98);
			((Control)label1).TabIndex = 3;
			((Control)label1).Text = "Message1";
			((Control)label3).Anchor = (AnchorStyles)13;
			((Control)label3).Location = new Point(12, 107);
			((Control)label3).Name = "label3";
			((Control)label3).Size = new Size(454, 91);
			((Control)label3).TabIndex = 5;
			((Control)label3).Text = "Message3";
			((Control)panel1).BackColor = SystemColors.Window;
			((Control)panel1).Controls.Add((Control)(object)label3);
			((Control)panel1).Controls.Add((Control)(object)label1);
			((Control)panel1).Dock = (DockStyle)5;
			((Control)panel1).Location = new Point(0, 0);
			((Control)panel1).Name = "panel1";
			((Control)panel1).Size = new Size(478, 201);
			((Control)panel1).TabIndex = 6;
			((Control)panel2).Controls.Add((Control)(object)btnBreak);
			((Control)panel2).Controls.Add((Control)(object)btnContinue);
			((Control)panel2).Controls.Add((Control)(object)btnIgnore);
			((Control)panel2).Dock = (DockStyle)2;
			((Control)panel2).Location = new Point(0, 201);
			((Control)panel2).Name = "panel2";
			((Control)panel2).Size = new Size(478, 45);
			((Control)panel2).TabIndex = 7;
			((Form)this).AcceptButton = (IButtonControl)(object)btnContinue;
			((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
			((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
			((Form)this).ClientSize = new Size(478, 246);
			((Control)this).Controls.Add((Control)(object)panel1);
			((Control)this).Controls.Add((Control)(object)panel2);
			((Form)this).FormBorderStyle = (FormBorderStyle)3;
			((Form)this).MaximizeBox = false;
			((Form)this).MinimizeBox = false;
			((Control)this).Name = "AssertionDialog";
			((Control)this).Text = "Title";
			((Control)panel1).ResumeLayout(false);
			((Control)panel2).ResumeLayout(false);
			((Control)this).ResumeLayout(false);
		}

		private AssertionDialog()
		{
			InitializeComponent();
			mAssertionDialogResult = AssertionDialogResult.Continue;
		}

		private void btnContinue_Click(object sender, EventArgs e)
		{
			AssertionDialogResult = AssertionDialogResult.Continue;
			((Form)this).Close();
		}

		private void btnIgnore_Click(object sender, EventArgs e)
		{
			AssertionDialogResult = AssertionDialogResult.Ignore;
			((Form)this).Close();
		}

		private void btnBreak_Click(object sender, EventArgs e)
		{
			AssertionDialogResult = AssertionDialogResult.Break;
			((Form)this).Close();
		}

		public static AssertionDialogResult ShowDialog(IWin32Window parent, string message1, string message2, string title)
		{
			AssertionDialog ad = new AssertionDialog();
			((Control)ad.label1).Text = message1;
			((Control)ad.label3).Text = message2;
			((Control)ad).Text = title;
			((Form)ad).StartPosition = (FormStartPosition)1;
			Singleton<IServiceLocator>.Instance.UIManager.Context.Send(delegate(object o)
			{
				//IL_000c: Unknown result type (might be due to invalid IL or missing references)
				((Form)ad).ShowDialog((IWin32Window)((o is IWin32Window) ? o : null));
			}, parent);
			return ad.AssertionDialogResult;
		}
	}
}
