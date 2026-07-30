using System;
using TypeEdit.DataHandling.Exporting;
using TypeEdit.Interfaces;
using TypeEdit.Interfaces.UI;

namespace S9BEditor
{
	internal class ExportAsyncTask : IAsyncTask
	{
		public float Progress => AppServices.Exporter.Progress;

		public string ProgressText
		{
			get
			{
				if (AppServices.Exporter.CancellationPending)
				{
					return "Cancelling export...";
				}
				return "Exporting...";
			}
		}

		public event EventHandler ProgressChanged;

		public event EventHandler TaskComplete;

		public void Run()
		{
			AppServices.OutputManager.ShowOutput("Export");
			if (AppServices.Exporter.IsExporting)
			{
				throw new InvalidOperationException("Export already in progress!");
			}
			AppServices.Exporter.ExportCompleted += Exporter_ExportCompleted;
			AppServices.Exporter.ProgressChanged += Exporter_ProgressChanged;
			AppServices.Exporter.CancellationPendingChanged += Exporter_ProgressChanged;
			AppServices.Exporter.OutputWritten += Exporter_OutputWritten;
			AppServices.Exporter.ErrorReported += Exporter_ErrorReported;
			AppServices.OutputManager.ClearErrors("Export", OutputMessageTarget.Build);
			AppServices.OutputManager.ClearOutput("Export");
			AppServices.Exporter.DoExport();
		}

		private void Exporter_ErrorReported(object sender, ErrorReportedEventArgs e)
		{
			AppServices.OutputManager.ReportError("Export", e.Target, e.MessageType, e.Message);
		}

		private void Exporter_ProgressChanged(object sender, EventArgs e)
		{
			OnProgressChanged(EventArgs.Empty);
		}

		private void Exporter_OutputWritten(object sender, OutputWrittenEventArgs e)
		{
			AppServices.OutputManager.WriteOutput("Export", e.Output);
		}

		protected virtual void OnProgressChanged(EventArgs e)
		{
			ProgressChanged?.Invoke(this, e);
		}

		private void Exporter_ExportCompleted(object sender, EventArgs e)
		{
			OnTaskCompleted(EventArgs.Empty);
		}

		protected virtual void OnTaskCompleted(EventArgs e)
		{
			AppServices.Exporter.ExportCompleted -= Exporter_ExportCompleted;
			AppServices.Exporter.ProgressChanged -= Exporter_ProgressChanged;
			AppServices.Exporter.CancellationPendingChanged -= Exporter_ProgressChanged;
			AppServices.Exporter.OutputWritten -= Exporter_OutputWritten;
			AppServices.Exporter.ErrorReported -= Exporter_ErrorReported;
			TaskComplete?.Invoke(this, e);
		}

		public virtual void Cancel()
		{
			AppServices.Exporter.CancelExport();
		}
	}
}
