using System;

namespace S9BEditor;

internal class PreviewAsyncTask : ExportAsyncTask
{
	private bool mCancel;

	private TypeEditDocument mDocument;

	public PreviewAsyncTask(TypeEditDocument doc)
	{
		mDocument = doc;
	}

	protected override void OnTaskCompleted(EventArgs e)
	{
		bool lastExportFailed = AppServices.Exporter.LastExportFailed;
		base.OnTaskCompleted(e);
		if (!mCancel && !lastExportFailed)
		{
			mDocument.Preview();
		}
	}

	public override void Cancel()
	{
		base.Cancel();
		mCancel = true;
	}
}
