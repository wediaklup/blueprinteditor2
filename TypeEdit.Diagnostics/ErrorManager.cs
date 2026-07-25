using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using TypeEdit.Base;
using TypeEdit.Interfaces;
using TypeEdit.Interfaces.Diagnostics;

namespace TypeEdit.Diagnostics;

public class ErrorManager : IErrorManager
{
	private List<int> mIgnoredAssertions;

	private object mAssertionLock;

	public ErrorManager()
	{
		mIgnoredAssertions = new List<int>();
		mAssertionLock = new object();
	}

	[DebuggerHidden]
	public bool Assert(bool assertion, string message = "")
	{
		return assertion;
	}

	[DebuggerHidden]
	public bool Assert<T>(T assertion, string message = "")
	{
		return assertion != null;
	}

	public void ErrorMessage(string message, Exception e)
	{
		ErrorMessage(message + Environment.NewLine + Environment.NewLine + e.Message);
	}

	public void ErrorMessage(string message)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		MessageBox.Show(Singleton<IServiceLocator>.Instance.MainWindow, message, "Error!", (MessageBoxButtons)0, (MessageBoxIcon)16);
		Singleton<IServiceLocator>.Instance.OutputManager.WriteOutput("Application", message);
	}
}
