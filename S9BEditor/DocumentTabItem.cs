using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using TypeEdit.Interfaces.Editing;
using TypeEdit.Interfaces.UI;

namespace S9BEditor
{
	internal class DocumentTabItem : TabItem, IDisposable
	{
		private string mFileName;

		private WeakReference mLastFocusedElement;

		public static readonly DependencyProperty EditorOnlyModifiedProperty;

		public string FileName
		{
			get
			{
				return mFileName;
			}
			set
			{
				if (mFileName != value)
				{
					mFileName = value;
					updateHeader();
				}
			}
		}

		public bool Modified => Document.Modified | EditorOnlyModified;

		public bool CanClose => true;

		public IDocument Document { get; private set; }

		public IDocumentType DocumentType { get; private set; }

		public bool EditorOnlyModified
		{
			get
			{
				return (bool)base.GetValue(EditorOnlyModifiedProperty);
			}
			set
			{
				base.SetValue(EditorOnlyModifiedProperty, (object)value);
			}
		}

		public DocumentTabItem()
		{
			mLastFocusedElement = new WeakReference(null);
			PLogger.Write("DocumentTabItem(): " + mLastFocusedElement);
		}

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			updateVSM();
		}

		public DocumentTabItem(IDocument document, IDocumentType documentType, string fileName)
		{
			mLastFocusedElement = new WeakReference(null);
			PLogger.Write("DocumentTabItem() alternative constructor");
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Expected O, but got Unknown
			if (document != null)
			{
				mFileName = fileName;
				Document = document;
				DocumentType = documentType;
				Document.ModifiedChanged += document_ModfiedChanged;
				Document.LoadFrom(fileName);
				ContentControl val = new ContentControl();
				val.Content = Document.Content;
				((UIElement)val).Focusable = false;
				if (Document is IWPFDocument iWPFDocument)
				{
					val.ContentTemplate = iWPFDocument.ContentTemplate;
				}
				else
				{
					val.ContentTemplate = null;
				}
				base.Content = val;
				updateHeader();
			}
		}

		private void document_ModfiedChanged(object sender, EventArgs e)
		{
			updateHeader();
		}

		private void document_FileNameChanged(object sender, EventArgs e)
		{
			updateHeader();
		}

		private void updateHeader()
		{
			if (Document != null)
			{
				base.Header = (Path.GetFileName(FileName) + (Modified ? "*" : "")).Replace("_", "__");
			}
		}

		public void Dispose()
		{
			if (Document is IDisposable disposable)
			{
				disposable.Dispose();
			}
			Document.ModifiedChanged -= document_ModfiedChanged;
		}

		protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
		{
			base.IsSelected = true;
			FocusContent();
			base.OnMouseLeftButtonDown(e);
		}

		public void FocusContent()
		{
			PLogger.Write("DocumentTabItem.FocusContent " + getContentPresenter());
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Expected O, but got Unknown
			ContentPresenter contentPresenter = getContentPresenter();
			PLogger.Write("DocumentTabItem.FocusContent P1");
			if (contentPresenter == null)
			{
				return;
			}
			PLogger.Write("DocumentTabItem.FocusContent P2");
			base.UpdateLayout();
			PLogger.Write("DocumentTabItem.FocusContent P2-1: " + (mLastFocusedElement == null));
			object target = mLastFocusedElement.Target;
			PLogger.Write("DocumentTabItem.FocusContent P2-2");
			FrameworkElement val = (FrameworkElement)((target is FrameworkElement) ? target : null);
			PLogger.Write("DocumentTabItem.FocusContent P3");
			if (val != null)
			{
				PLogger.Write("DocumentTabItem.FocusContent P4 val!=null");
				bool flag = false;
				for (DependencyObject val2 = (DependencyObject)(object)val; val2 != null; val2 = VisualTreeHelper.GetParent(val2))
				{
					PLogger.Write("DocumentTabItem.FocusContent P5 forloop");
					if ((object)val2 == contentPresenter)
					{
						PLogger.Write("DocumentTabItem.FocusContent P6 for if");
						flag = true;
						break;
					}
					PLogger.Write("DocumentTabItem.FocusContent P7 nobreak");
				}
				if (flag)
				{
					PLogger.Write("DocumentTabItem.FocusContent P8 if flag");
					val.Focus();
				}
			}
			else
			{
				PLogger.Write("DocumentTabItem.FocusContent P8 else");
				((UIElement)contentPresenter).MoveFocus(new TraversalRequest((FocusNavigationDirection)2));
			}
			PLogger.Write("DocumentTabItem.FocusContent end (:");
		}

		private TabControl getParentTabControl()
		{
			ItemsControl obj = ItemsControl.ItemsControlFromItemContainer((DependencyObject)(object)this);
			return (TabControl)(object)((obj is TabControl) ? obj : null);
		}

		private ContentPresenter getContentPresenter()
		{
			TabControl parentTabControl = getParentTabControl();
			if (parentTabControl != null && ((Control)parentTabControl).Template != null)
			{
				object obj = ((FrameworkTemplate)((Control)parentTabControl).Template).FindName("PART_SelectedContentHost", (FrameworkElement)(object)parentTabControl);
				ContentPresenter val = (ContentPresenter)((obj is ContentPresenter) ? obj : null);
				if (val != null)
				{
					return val;
				}
			}
			return null;
		}

		protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			if (((DependencyPropertyChangedEventArgs)(e)).Property == TabItem.IsSelectedProperty || ((DependencyPropertyChangedEventArgs)(e)).Property == UIElement.IsMouseOverProperty)
			{
				updateVSM();
			}
			else if (((DependencyPropertyChangedEventArgs)(e)).Property == EditorOnlyModifiedProperty)
			{
				updateHeader();
			}
			else if (((DependencyPropertyChangedEventArgs)(e)).Property == UIElement.IsKeyboardFocusWithinProperty)
			{
				if (!(bool)((DependencyPropertyChangedEventArgs)(e)).NewValue)
				{
					IInputElement focusedElement = FocusManager.GetFocusedElement(FocusManager.GetFocusScope((DependencyObject)(object)this));
					mLastFocusedElement.Target = focusedElement;
				}
				updateVSM();
			}
			base.OnPropertyChanged(e);
		}

		private void updateVSM()
		{
			if (base.IsKeyboardFocusWithin || base.IsFocused)
			{
				if (base.IsSelected)
				{
					VisualStateManager.GoToState(this, "FocusedSelected", true);
				}
				else if (base.IsMouseOver)
				{
					VisualStateManager.GoToState(this, "DefaultMouseOver", true);
				}
				else
				{
					VisualStateManager.GoToState(this, "Default", true);
				}
			}
			else if (base.IsSelected)
			{
				VisualStateManager.GoToState(this, "UnfocusedSelected", true);
			}
			else if (base.IsMouseOver)
			{
				VisualStateManager.GoToState(this, "DefaultMouseOver", true);
			}
			else
			{
				VisualStateManager.GoToState(this, "Default", true);
			}
		}

		static DocumentTabItem()
		{
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Expected O, but got Unknown
			EditorOnlyModifiedProperty = DependencyProperty.Register("EditorOnlyModified", typeof(bool), typeof(DocumentTabItem), (PropertyMetadata)new UIPropertyMetadata((object)false));
		}
	}
}
