using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Markup;

namespace S9BEditor
{
	public partial class TypeSelectionWindow : CustomWindow, IComponentConnector
	{
		public static readonly DependencyProperty ItemsSourceProperty;

		public static readonly DependencyProperty DisplayMemberPathProperty;

		public static readonly DependencyProperty SelectedItemProperty;

		public object ItemsSource
		{
			get
			{
				return ((DependencyObject)this).GetValue(ItemsSourceProperty);
			}
			set
			{
				((DependencyObject)this).SetValue(ItemsSourceProperty, value);
			}
		}

		public string DisplayMemberPath
		{
			get
			{
				return (string)((DependencyObject)this).GetValue(DisplayMemberPathProperty);
			}
			set
			{
				((DependencyObject)this).SetValue(DisplayMemberPathProperty, (object)value);
			}
		}

		public object SelectedItem
		{
			get
			{
				return ((DependencyObject)this).GetValue(SelectedItemProperty);
			}
			set
			{
				((DependencyObject)this).SetValue(SelectedItemProperty, value);
			}
		}

		public TypeSelectionWindow()
		{
			InitializeComponent();
		}

		private void button3_Click(object sender, RoutedEventArgs e)
		{
			((Window)this).DialogResult = true;
		}

		static TypeSelectionWindow()
		{
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Expected O, but got Unknown
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Expected O, but got Unknown
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Expected O, but got Unknown
			ItemsSourceProperty = DependencyProperty.Register("ItemsSource", typeof(object), typeof(TypeSelectionWindow), (PropertyMetadata)new UIPropertyMetadata((PropertyChangedCallback)null));
			DisplayMemberPathProperty = DependencyProperty.Register("DisplayMemberPath", typeof(string), typeof(TypeSelectionWindow), (PropertyMetadata)new UIPropertyMetadata((PropertyChangedCallback)null));
			SelectedItemProperty = DependencyProperty.Register("SelectedItem", typeof(object), typeof(TypeSelectionWindow), (PropertyMetadata)new UIPropertyMetadata((PropertyChangedCallback)null));
		}
	}
}
