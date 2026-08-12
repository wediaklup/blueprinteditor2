using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using TypeEdit.Base;
using TypeEdit.Interfaces.AddIns;
using TypeEdit.Interfaces.Data.Descriptors;
using TypeEdit.Interfaces.Data.Instancing;
using TypeEdit.Interfaces.Editing;

namespace S9BEditor.ViewModels
{
	internal class TypeDatumViewModelFactory : IDisposable
	{
		private List<IDataEditorProvider> mDataEditorProviders;

		private List<string> mForcedSimpleEditorTypes;

		private Dictionary<IEnumTypeDescriptor, ReadOnlyCollection<VMEnumItem>> mEnumItemLists;

		public TypeDatumViewModelFactory()
		{
			mForcedSimpleEditorTypes = new List<string>();
			string[] commandLineArgs = Environment.GetCommandLineArgs();
			string text = "-SimpleTypes=";
			string[] array = commandLineArgs;
			foreach (string text2 in array)
			{
				if (!text2.StartsWith(text, StringComparison.InvariantCultureIgnoreCase) || text2.Length <= text.Length)
				{
					continue;
				}
				string text3 = text2.Substring(text.Length);
				if (text3.StartsWith("\"") && text3.EndsWith("\""))
				{
					text3 = text2.Substring(1);
				}
				string[] array2 = text3.Split(new char[1] { ',' });
				string[] array3 = array2;
				foreach (string text4 in array3)
				{
					string text5 = text4.Trim();
					if (text5.Length > 0 && !mForcedSimpleEditorTypes.Contains(text5))
					{
						mForcedSimpleEditorTypes.Add(text5);
					}
				}
			}
			mEnumItemLists = new Dictionary<IEnumTypeDescriptor, ReadOnlyCollection<VMEnumItem>>();
			AddInManager addInManager = Application.Current.Resources[(object)"addInManager"] as AddInManager;
			mDataEditorProviders = new List<IDataEditorProvider>();
			foreach (IAddIn addIn in addInManager.AddIns)
			{
				if (addIn.DataEditorProvider != null)
				{
					mDataEditorProviders.Add(addIn.DataEditorProvider);
				}
			}
		}

		public IDataEditor CreateIndexedElementCustomEditor(ITypeDatum datum, IIndexedTypeDatum owner)
		{
			IDataEditor dataEditor = null;
			foreach (IDataEditorProvider mDataEditorProvider in mDataEditorProviders)
			{
				dataEditor = mDataEditorProvider.GetIndexedElementDataEditor(datum, owner);
				if (dataEditor != null)
				{
					break;
				}
			}
			return dataEditor;
		}

		public IDataEditor CreateRootCustomEditor(ITypeDatum datum)
		{
			IDataEditor dataEditor = null;
			foreach (IDataEditorProvider mDataEditorProvider in mDataEditorProviders)
			{
				dataEditor = mDataEditorProvider.GetRootDataEditor(datum);
				if (dataEditor != null)
				{
					break;
				}
			}
			return dataEditor;
		}

		public IDataEditor CreateCustomEditorForProperty(ITypeDatum datum, IProperty property)
		{
			IDataEditor dataEditor = null;
			foreach (IDataEditorProvider mDataEditorProvider in mDataEditorProviders)
			{
				dataEditor = mDataEditorProvider.GetPropertyDataEditor(datum, property);
				if (dataEditor != null)
				{
					break;
				}
			}
			return dataEditor;
		}

		public VMTypeDatumBase CreateViewModel(ViewModelBase owner, ITypeDatum datum)
		{
			IDataEditor dataEditor = null;
			if (!mForcedSimpleEditorTypes.Contains(datum.TypeDescriptor.Name))
			{
				if (owner is VMProperty vMProperty)
				{
					dataEditor = CreateCustomEditorForProperty(datum, vMProperty.Property);
				}
				else if (owner is VMIndexedTypeDatum vMIndexedTypeDatum)
				{
					dataEditor = CreateIndexedElementCustomEditor(datum, vMIndexedTypeDatum.Datum);
				}
				else if (owner is VMTypeEditContainer)
				{
					dataEditor = CreateRootCustomEditor(datum);
				}
			}
			if (dataEditor != null)
			{
				return new VMCustomDatumEditor(owner, datum, dataEditor);
			}
			if (datum is IClassTypeDatum datum2)
			{
				return new VMClassTypeDatum(owner, datum2);
			}
			if (datum is IEnumTypeDatum datum3)
			{
				return new VMEnumTypeDatum(owner, datum3);
			}
			if (datum is IIndexedTypeDatum datum4)
			{
				return new VMIndexedTypeDatum(owner, datum4);
			}
			if (datum is IPrimitiveTypeDatum datum5)
			{
				return new VMPrimitiveTypeDatum(owner, datum5);
			}
			if (datum is IReferenceProxyTypeDatum datum6)
			{
				return new VMReferenceTypeDatum(owner, datum6);
			}
			return null;
		}

		public ReadOnlyCollection<VMEnumItem> GetEnumItems(IEnumTypeDescriptor descriptor)
		{
			if (!mEnumItemLists.TryGetValue(descriptor, out var value))
			{
				VMEnumItem[] array = new VMEnumItem[descriptor.Items.Count];
				for (int i = 0; i < descriptor.Items.Count; i++)
				{
					array[i] = new VMEnumItem(null, descriptor.Items[i]);
				}
				value = new ReadOnlyCollection<VMEnumItem>(array);
				mEnumItemLists.Add(descriptor, value);
			}
			return value;
		}

		public void Dispose()
		{
			foreach (KeyValuePair<IEnumTypeDescriptor, ReadOnlyCollection<VMEnumItem>> mEnumItemList in mEnumItemLists)
			{
				foreach (VMEnumItem item in mEnumItemList.Value)
				{
					item.Dispose();
				}
			}
			mEnumItemLists.Clear();
			mDataEditorProviders.Clear();
		}
	}
}
