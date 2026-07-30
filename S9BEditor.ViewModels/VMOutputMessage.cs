using System;
using System.Windows.Media.Imaging;
using TypeEdit.Base;
using TypeEdit.Interfaces;
using TypeEdit.Interfaces.Diagnostics;

namespace S9BEditor.ViewModels
{
	internal class VMOutputMessage : ViewModelBase
	{
		private static WeakReference mErrorBitmap = new WeakReference(null);

		private static WeakReference mWarningBitmap = new WeakReference(null);

		private static WeakReference mInformationBitmap = new WeakReference(null);

		private static WeakReference mDocumentErrorBitmap = new WeakReference(null);

		public IErrorItem ErrorItem { get; private set; }

		public string Message => ErrorItem.Message;

		public string Filter => ErrorItem.Filter;

		public BitmapImage Icon
		{
			get
			{
				//IL_001a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0020: Expected O, but got Unknown
				//IL_005b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0061: Expected O, but got Unknown
				//IL_0030: Unknown result type (might be due to invalid IL or missing references)
				//IL_0036: Expected O, but got Unknown
				//IL_0099: Unknown result type (might be due to invalid IL or missing references)
				//IL_009f: Expected O, but got Unknown
				//IL_006e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0074: Expected O, but got Unknown
				//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
				//IL_00b2: Expected O, but got Unknown
				BitmapImage val = null;
				if (ErrorItem.MessageType == ErrorMessageType.Error)
				{
					val = (BitmapImage)mErrorBitmap.Target;
					if (val == null)
					{
						val = new BitmapImage(new Uri("pack://application:,,,/Resources/109_AllAnnotations_Error_16x16_72.png"));
						mErrorBitmap.Target = val;
					}
				}
				else if (ErrorItem.MessageType == ErrorMessageType.Warning)
				{
					val = (BitmapImage)mWarningBitmap.Target;
					if (val == null)
					{
						val = new BitmapImage(new Uri("pack://application:,,,/Resources/109_AllAnnotations_Warning_16x16_72.png"));
						mWarningBitmap.Target = val;
					}
				}
				else if (ErrorItem.MessageType == ErrorMessageType.Information)
				{
					val = (BitmapImage)mInformationBitmap.Target;
					if (val == null)
					{
						val = new BitmapImage(new Uri("pack://application:,,,/Resources/109_AllAnnotations_Info_16x16_72.png"));
						mInformationBitmap.Target = val;
					}
				}
				return val;
			}
		}

		public VMOutputMessage(IErrorItem errorItem)
			: base(null)
		{
			ErrorItem = errorItem;
		}
	}
}
