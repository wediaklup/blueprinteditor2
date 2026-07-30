using System;
using System.Threading;
using System.Windows.Forms;
using S9BEditor;
using TypeEdit.Interfaces;
using TypeEdit.Interfaces.Diagnostics;
using TypeEdit.Interfaces.Editing;

namespace TypeEdit.Diagnostics
{
	public class OutputManager : IOutputManager
	{
		private SynchronizationContext mContext;

		public bool ConsoleOutputEnabled { get; set; }

		public event EventHandler<ErrorReportedEventArgs> ErrorReported;

		public event EventHandler<ErrorsClearedEventArgs> NotifyClearErrors;

		public event EventHandler<OutputReportedEventArgs> OutputWritten;

		public event EventHandler<OutputClearedEventArgs> NotifyClearOutput;

		public event EventHandler<OutputShowEventArgs> NotifyShowOutput;

		public OutputManager()
		{
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			base._002Ector();
			mContext = SynchronizationContext.Current;
			if (mContext == null)
			{
				mContext = new SynchronizationContext();
				MessageBox.Show("Output Manager has no Synchronization Context. Was this set?");
			}
			ConsoleOutputEnabled = false;
		}

		protected virtual void OnErrorReported(ErrorReportedEventArgs outputEventArgs)
		{
			ErrorReported?.Invoke(this, outputEventArgs);
		}

		protected virtual void OnNotifyClearErrors(ErrorsClearedEventArgs outputEventArgs)
		{
			NotifyClearErrors?.Invoke(this, outputEventArgs);
		}

		protected virtual void OnNotifyClearOutput(OutputClearedEventArgs outputEventArgs)
		{
			NotifyClearOutput?.Invoke(this, outputEventArgs);
		}

		protected virtual void OnNotifyShowOutput(OutputShowEventArgs outputEventArgs)
		{
			NotifyShowOutput?.Invoke(this, outputEventArgs);
		}

		public IErrorItem ReportError(string outputFilter, OutputMessageTarget target, ErrorMessageType messageType, string message, object errorSource = null)
		{
			ErrorItem errorItem = new ErrorItem(outputFilter, DateTime.Now, target, messageType, message, errorSource);
			mContext.Post(delegate(object o)
			{
				if (o is ErrorReportedEventArgs e)
				{
					ErrorReportedEventArgs e2 = new ErrorReportedEventArgs(e.ErrorItem);
					OnErrorReported(e2);
					if (ConsoleOutputEnabled)
					{
						Console.WriteLine(string.Concat(new object[5]
						{
							e2.ErrorItem.Time,
							" ",
							e2.ErrorItem.MessageType,
							": ",
							e2.ErrorItem.Message
						}));
					}
				}
			}, new ErrorReportedEventArgs(errorItem));
			return errorItem;
		}

		public void ClearErrors(string outputFilter, OutputMessageTarget target, IDocument document = null)
		{
			mContext.Post(delegate(object o)
			{
				if (o is ErrorsClearedEventArgs e)
				{
					ErrorsClearedEventArgs outputEventArgs = new ErrorsClearedEventArgs(e.Filter, e.Target, e.Document);
					OnNotifyClearErrors(outputEventArgs);
				}
			}, new ErrorsClearedEventArgs(outputFilter, target, document));
		}

		public void WriteOutput(string outputFilter, string debugText)
		{
			if (ConsoleOutputEnabled)
			{
				Console.WriteLine(outputFilter + ": " + debugText);
			}
			mContext.Post(delegate(object o)
			{
				if (o is OutputReportedEventArgs e)
				{
					OutputReportedEventArgs outputEventArgs = new OutputReportedEventArgs(e.Filter, e.Line);
					OnOutputWritten(outputEventArgs);
				}
			}, new OutputReportedEventArgs(outputFilter, debugText));
		}

		public void ClearOutput(string outputFilter)
		{
			mContext.Post(delegate(object o)
			{
				if (o is OutputClearedEventArgs e)
				{
					OutputClearedEventArgs outputEventArgs = new OutputClearedEventArgs(e.Filter);
					OnNotifyClearOutput(outputEventArgs);
				}
			}, new OutputClearedEventArgs(outputFilter));
		}

		protected virtual void OnOutputWritten(OutputReportedEventArgs outputEventArgs)
		{
			OutputWritten?.Invoke(this, outputEventArgs);
		}

		public void ShowOutput(string outputFilter)
		{
			mContext.Post(delegate(object o)
			{
				if (o is OutputShowEventArgs e)
				{
					OutputShowEventArgs outputEventArgs = new OutputShowEventArgs(e.Filter);
					OnNotifyShowOutput(outputEventArgs);
				}
			}, new OutputShowEventArgs(outputFilter));
		}
	}
}
