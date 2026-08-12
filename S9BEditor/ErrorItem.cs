using System;
using TypeEdit.Interfaces;
using TypeEdit.Interfaces.Diagnostics;

namespace S9BEditor
{
	internal class ErrorItem : IErrorItem
	{
		public string Message { get; private set; }

		public string Filter { get; private set; }

		public OutputMessageTarget Target { get; private set; }

		public ErrorMessageType MessageType { get; private set; }

		public DateTime Time { get; private set; }

		public object ErrorSource { get; private set; }

		public ErrorItem(string filter, DateTime time, OutputMessageTarget target, ErrorMessageType messageType, string message, object errorSource)
		{
			Filter = filter;
			Time = time;
			Target = target;
			MessageType = messageType;
			Message = message;
			ErrorSource = errorSource;
		}
	}
}
