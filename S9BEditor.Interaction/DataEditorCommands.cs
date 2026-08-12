using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using S9BEditor.ViewModels;
using TypeEdit.Base;
using TypeEdit.DataHandling;
using TypeEdit.Interfaces.Data.Descriptors;
using TypeEdit.Interfaces.Data.Instancing;
using TypeEdit.Interfaces.Editing;

namespace S9BEditor.Interaction
{
	internal static class DataEditorCommands
	{
		public static ICommand CopyAsPath = new DelegateCommand<VMTypeEditComponent>(delegate(VMTypeEditComponent o)
		{
			if (o is VMTypeDatumBase vMTypeDatumBase)
			{
				Clipboard.Clear();
				Clipboard.SetText(TypeEditSchemaUtil.GetDatumPath(vMTypeDatumBase.Datum));
			}
			if (o is VMProperty vMProperty)
			{
				Clipboard.Clear();
				Clipboard.SetText(TypeEditSchemaUtil.GetDatumPath(vMProperty.PropertyContent.Datum));
			}
		}, delegate(VMTypeEditComponent o)
		{
			if (o is VMTypeDatumBase)
			{
				return true;
			}
			return o is VMProperty;
		});

		public static ICommand ResetToDefault = new DelegateCommand<VMTypeEditComponent>(delegate(VMTypeEditComponent o)
		{
			if (o is VMProperty vMProperty)
			{
				vMProperty.ResetToDefault();
			}
		}, (VMTypeEditComponent o) => o is VMProperty vMProperty && vMProperty.CanResetToDefault);

		public static ICommand InsertItemBefore = new DelegateCommand<VMTypeDatumBase>(delegate(VMTypeDatumBase asTypeDatum)
		{
			if (asTypeDatum != null && asTypeDatum.Owner is VMIndexedTypeDatum indexed)
			{
				insertItem(indexed, asTypeDatum);
			}
		}, (VMTypeDatumBase asTypeDatum) => (asTypeDatum != null && asTypeDatum.Owner is VMIndexedTypeDatum) ? true : false);

		public static ICommand InsertItemAfter = new DelegateCommand<VMTypeDatumBase>(delegate(VMTypeDatumBase asTypeDatum)
		{
			if (asTypeDatum != null && asTypeDatum.Owner is VMIndexedTypeDatum vMIndexedTypeDatum)
			{
				VMTypeDatumBase beforeDatum = null;
				bool flag = false;
				foreach (VMTypeDatumBase item in vMIndexedTypeDatum.Items)
				{
					if (flag)
					{
						beforeDatum = item;
						break;
					}
					if (item == asTypeDatum)
					{
						flag = true;
					}
				}
				insertItem(vMIndexedTypeDatum, beforeDatum);
			}
		}, (VMTypeDatumBase asTypeDatum) => (asTypeDatum != null && asTypeDatum.Owner is VMIndexedTypeDatum) ? true : false);

		public static ICommand AddItem = new DelegateCommand<VMIndexedTypeDatum>(delegate(VMIndexedTypeDatum vitd)
		{
			if (vitd != null)
			{
				insertItem(vitd);
			}
		});

		public static ICommand OpenChildEditWindow = new DelegateCommand<VMCustomDatumEditor>(delegate(VMCustomDatumEditor asCustomDatumEditor)
		{
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Expected O, but got Unknown
			if (asCustomDatumEditor.Editor is IChildWindowDataEditor childWindowDataEditor)
			{
				Window val = AppServices.UIManager.CreateStyledWindow(childWindowDataEditor.CreateWindowFlags);
				((FrameworkElement)val).Width = childWindowDataEditor.InitialWidth;
				((FrameworkElement)val).Height = childWindowDataEditor.InitialHeight;
				((ContentControl)val).Content = asCustomDatumEditor.Editor.Content;
				val.Owner = (Window)(object)AppServices.MainWindow;
				val.WindowStartupLocation = (WindowStartupLocation)2;
				val.ShowInTaskbar = false;
				Binding val2 = new Binding("Title");
				val2.Source = asCustomDatumEditor.Editor;
				val2.Mode = (BindingMode)1;
				((FrameworkElement)val).SetBinding(Window.TitleProperty, (BindingBase)(object)val2);
				val.ShowDialog();
			}
		}, (VMCustomDatumEditor asCustomDatumEditor) => true);

		public static ICommand OpenFile = new DelegateCommand<VMProperty>(delegate(VMProperty asProp)
		{
			if (asProp != null && asProp.IsFileID)
			{
				VMTypeEditContainer vMTypeEditContainer = asProp.FindTopMost<VMTypeEditContainer>();
				if (vMTypeEditContainer != null && asProp.PropertyContent.Datum is IPrimitiveTypeDatum)
				{
					bool flag = false;
					if (asProp.Property.Descriptor.Name == "BlueprintID")
					{
						VMClassTypeDatum vMClassTypeDatum = asProp.FindAncestor<VMClassTypeDatum>();
						if (vMClassTypeDatum != null && vMClassTypeDatum.Datum.TypeDescriptor.Name == "iBlueprintLibrary-cAbsoluteBlueprintID")
						{
							string value = TypeEditSchemaUtil.GetSingleDatum((VMTypeDatumBase)vMClassTypeDatum, "BlueprintSetID/Provider", 0).GetValue(CultureInfo.InvariantCulture);
							string value2 = TypeEditSchemaUtil.GetSingleDatum((VMTypeDatumBase)vMClassTypeDatum, "BlueprintSetID/Product", 0).GetValue(CultureInfo.InvariantCulture);
							string value3 = TypeEditSchemaUtil.GetSingleDatum((VMTypeDatumBase)vMClassTypeDatum, "BlueprintID", 0).GetValue(CultureInfo.InvariantCulture);
							if (!string.IsNullOrEmpty(value3) && !string.IsNullOrEmpty(value) && !string.IsNullOrEmpty(value2) && value3.IndexOfAny(Path.GetInvalidPathChars()) < 0)
							{
								string text;
								try
								{
									text = Path.Combine(vMTypeEditContainer.SourceDirectory, value, value2, value3);
								}
								catch (Exception)
								{
									text = string.Empty;
								}
								if (!Path.HasExtension(text))
								{
									text += ".xml";
								}
								if (File.Exists(text))
								{
									AppServices.FileManager.OpenFile(text);
								}
							}
							flag = true;
						}
					}
					if (!flag)
					{
						string value4 = asProp.PropertyContent.GetValue(CultureInfo.InvariantCulture);
						if (!string.IsNullOrEmpty(value4) && !string.IsNullOrEmpty(vMTypeEditContainer.Provider) && !string.IsNullOrEmpty(vMTypeEditContainer.Product) && value4.IndexOfAny(Path.GetInvalidPathChars()) < 0)
						{
							string text2;
							try
							{
								text2 = Path.Combine(vMTypeEditContainer.SourceDirectory, vMTypeEditContainer.Provider, vMTypeEditContainer.Product, value4);
							}
							catch (Exception)
							{
								text2 = string.Empty;
							}
							if (!Path.HasExtension(text2))
							{
								IEnumerable<string> extensionsForFileType = TypeEditSchemaUtil.GetExtensionsForFileType(asProp.FileIDType);
								{
									foreach (string item2 in extensionsForFileType)
									{
										if (File.Exists(text2 + item2))
										{
											AppServices.FileManager.OpenFile(text2 + item2);
										}
									}
									return;
								}
							}
							if (File.Exists(text2))
							{
								AppServices.FileManager.OpenFile(text2);
							}
						}
					}
				}
			}
		}, delegate(VMProperty asProp)
		{
			if (asProp != null && asProp.IsFileID)
			{
				VMTypeEditContainer vMTypeEditContainer = asProp.FindTopMost<VMTypeEditContainer>();
				if (vMTypeEditContainer != null && asProp.PropertyContent.Datum is IPrimitiveTypeDatum)
				{
					bool flag = false;
					if (asProp.Property.Descriptor.Name == "BlueprintID")
					{
						VMClassTypeDatum vMClassTypeDatum = asProp.FindAncestor<VMClassTypeDatum>();
						if (vMClassTypeDatum != null && vMClassTypeDatum.Datum.TypeDescriptor.Name == "iBlueprintLibrary-cAbsoluteBlueprintID")
						{
							string value = TypeEditSchemaUtil.GetSingleDatum((VMTypeDatumBase)vMClassTypeDatum, "BlueprintSetID/Provider", 0).GetValue(CultureInfo.InvariantCulture);
							string value2 = TypeEditSchemaUtil.GetSingleDatum((VMTypeDatumBase)vMClassTypeDatum, "BlueprintSetID/Product", 0).GetValue(CultureInfo.InvariantCulture);
							string value3 = TypeEditSchemaUtil.GetSingleDatum((VMTypeDatumBase)vMClassTypeDatum, "BlueprintID", 0).GetValue(CultureInfo.InvariantCulture);
							if (!string.IsNullOrEmpty(value3) && !string.IsNullOrEmpty(value) && !string.IsNullOrEmpty(value2) && value3.IndexOfAny(Path.GetInvalidPathChars()) < 0)
							{
								string text;
								try
								{
									text = Path.Combine(vMTypeEditContainer.SourceDirectory, value, value2, value3);
								}
								catch (Exception)
								{
									text = string.Empty;
								}
								if (!Path.HasExtension(text))
								{
									text += ".xml";
								}
								if (File.Exists(text))
								{
									return true;
								}
							}
							flag = true;
						}
					}
					if (!flag)
					{
						string value4 = asProp.PropertyContent.GetValue(CultureInfo.InvariantCulture);
						if (!string.IsNullOrEmpty(value4) && !string.IsNullOrEmpty(vMTypeEditContainer.Provider) && !string.IsNullOrEmpty(vMTypeEditContainer.Product) && value4.IndexOfAny(Path.GetInvalidPathChars()) < 0)
						{
							string text2;
							try
							{
								text2 = Path.Combine(vMTypeEditContainer.SourceDirectory, vMTypeEditContainer.Provider, vMTypeEditContainer.Product, value4);
							}
							catch (Exception)
							{
								text2 = string.Empty;
							}
							if (!Path.HasExtension(text2))
							{
								IEnumerable<string> extensionsForFileType = TypeEditSchemaUtil.GetExtensionsForFileType(asProp.FileIDType);
								foreach (string item3 in extensionsForFileType)
								{
									if (File.Exists(text2 + item3))
									{
										return true;
									}
								}
							}
							else if (File.Exists(text2))
							{
								return true;
							}
						}
					}
				}
			}
			return false;
		});

		private static void insertItem(VMIndexedTypeDatum indexed, VMTypeDatumBase beforeDatum = null)
		{
			ITypeDescriptor baseTypeDescriptor = indexed.Datum.TypeDescriptor.BaseTypeDescriptor;
			baseTypeDescriptor.GetTypeEditSchema();
			List<ITypeDescriptor> list = new List<ITypeDescriptor>(TypeEditSchemaUtil.GetDerivedTypes(baseTypeDescriptor));
			ITypeDescriptor typeDescriptor = null;
			if (list.Count > 1)
			{
				VMTypeSelectionItem[] array = new VMTypeSelectionItem[list.Count];
				for (int i = 0; i < list.Count; i++)
				{
					array[i] = new VMTypeSelectionItem(null, list[i]);
				}
				Window mainWindow = (Window)(object)AppServices.MainWindow;
				TypeSelectionWindow typeSelectionWindow = new TypeSelectionWindow();
				((Window)typeSelectionWindow).Owner = mainWindow;
				typeSelectionWindow.ItemsSource = array;
				typeSelectionWindow.SelectedItem = array[0];
				typeSelectionWindow.DisplayMemberPath = "Name";
				bool? flag = ((Window)typeSelectionWindow).ShowDialog();
				if (flag.HasValue && flag == true && typeSelectionWindow.SelectedItem is VMTypeSelectionItem vMTypeSelectionItem)
				{
					typeDescriptor = vMTypeSelectionItem.TypeDescriptor;
				}
				typeSelectionWindow.ItemsSource = null;
				for (int j = 0; j < array.Length; j++)
				{
					array[j].Dispose();
				}
			}
			else
			{
				typeDescriptor = list[0];
			}
			if (typeDescriptor != null)
			{
				VMTypeDatumBase vMTypeDatumBase = indexed.InsertItem(typeDescriptor, beforeDatum);
				if (vMTypeDatumBase != null)
				{
					vMTypeDatumBase.IsSelected = true;
				}
			}
		}
	}
}
