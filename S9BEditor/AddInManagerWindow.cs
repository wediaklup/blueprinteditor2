using System;
using System.CodeDom.Compiler;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Markup;
using S9BEditor.ViewModels;
using TypeEdit.Interfaces.AddIns;

namespace S9BEditor;

public class AddInManagerWindow : CustomWindow, IComponentConnector
{
	private AddInManager mAddInManager;

	public static readonly DependencyProperty AddInsProperty;

	internal ListBox addInListbox;

	private bool _contentLoaded;

	internal AddInManager AddInManager
	{
		get
		{
			return mAddInManager;
		}
		set
		{
			if (mAddInManager != value)
			{
				mAddInManager = value;
				OnAddInManagerChanged(EventArgs.Empty);
			}
		}
	}

	internal ObservableCollection<VMAddIn> AddIns
	{
		get
		{
			return (ObservableCollection<VMAddIn>)((DependencyObject)this).GetValue(AddInsProperty);
		}
		private set
		{
			((DependencyObject)this).SetValue(AddInsProperty, (object)value);
		}
	}

	public AddInManagerWindow()
	{
		InitializeComponent();
	}

	protected virtual void OnAddInManagerChanged(EventArgs eventArgs)
	{
		AddIns = null;
		if (mAddInManager != null)
		{
			AddIns = new ObservableCollection<VMAddIn>();
			foreach (IAddIn addIn in mAddInManager.AddIns)
			{
				AddIns.Add(new VMAddIn(addIn));
			}
		}
		if (((ItemsControl)addInListbox).HasItems)
		{
			((Selector)addInListbox).SelectedIndex = 0;
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	public void InitializeComponent()
	{
		if (!_contentLoaded)
		{
			_contentLoaded = true;
			Uri uri = new Uri("/BlueprintEditor2;component/addinmanagerwindow.xaml", UriKind.Relative);
			Application.LoadComponent((object)this, uri);
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	internal Delegate _CreateDelegate(Type delegateType, string handler)
	{
		return Delegate.CreateDelegate(delegateType, this, handler);
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	void IComponentConnector.Connect(int connectionId, object target)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Expected O, but got Unknown
		if (connectionId == 1)
		{
			addInListbox = (ListBox)target;
		}
		else
		{
			_contentLoaded = true;
		}
	}

	static AddInManagerWindow()
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Expected O, but got Unknown
		AddInsProperty = DependencyProperty.Register("AddIns", typeof(ObservableCollection<VMAddIn>), typeof(AddInManagerWindow), (PropertyMetadata)new UIPropertyMetadata((PropertyChangedCallback)null));
	}
}
