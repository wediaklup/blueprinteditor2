using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using S9BEditor.Scripts;
using TypeEdit.Base;
using TypeEdit.DataHandling;
using TypeEdit.Interfaces;

namespace S9BEditor.ViewModels;

internal class VMScript : ViewModelBase
{
	private string[] mProviders;

	private string[] mProducts;

	private string mProvider;

	private string mProduct;

	public ScriptDefinition ScriptDefinition { get; private set; }

	public string Name => ScriptDefinition.Name;

	public string Description => ScriptDefinition.Description;

	public ReadOnlyObservableCollection<VMScriptProperty> Properties { get; private set; }

	public bool HasProperties => Properties.Count > 0;

	public string[] Providers
	{
		get
		{
			return mProviders;
		}
		set
		{
			if (mProviders != value)
			{
				mProviders = value;
				RaisePropertyChanged("Providers");
			}
		}
	}

	public string[] Products
	{
		get
		{
			return mProducts;
		}
		set
		{
			if (mProducts != value)
			{
				mProducts = value;
				RaisePropertyChanged("Products");
			}
		}
	}

	public string Provider
	{
		get
		{
			return mProvider;
		}
		set
		{
			if (mProvider != value)
			{
				mProvider = value;
				RaisePropertyChanged("Provider");
			}
		}
	}

	public string Product
	{
		get
		{
			return mProduct;
		}
		set
		{
			if (mProduct != value)
			{
				mProduct = value;
				RaisePropertyChanged("Product");
			}
		}
	}

	public VMScript(ScriptDefinition scriptDef)
		: base(null)
	{
		ScriptDefinition = scriptDef;
		ObservableCollection<VMScriptProperty> observableCollection = new ObservableCollection<VMScriptProperty>();
		foreach (ScriptPropertyDefinition property in scriptDef.Properties)
		{
			VMScriptProperty vMScriptProperty = new VMScriptProperty(property, this);
			observableCollection.Add(vMScriptProperty);
			vMScriptProperty.PropertyChanged += propVm_PropertyChanged;
		}
		Properties = new ReadOnlyObservableCollection<VMScriptProperty>(observableCollection);
		string[] providers = DataUtil.GetProviders(AppServices.ResourceManager.DevSourcePath);
		string[] providers2 = DataUtil.GetProviders(AppServices.ResourceManager.SourcePath);
		string[] array = new string[((providers != null) ? providers.Length : 0) + ((providers2 != null) ? providers2.Length : 0)];
		if (providers != null)
		{
			Array.Copy(providers, array, providers.Length);
		}
		if (providers2 != null)
		{
			Array.Copy(providers2, 0, array, providers.Length, providers2.Length);
		}
		Providers = array;
	}

	private void propVm_PropertyChanged(object sender, PropertyChangedEventArgs e)
	{
		if (e.PropertyName == "Value" && sender is VMScriptProperty vMScriptProperty && vMScriptProperty.ScriptPropertyAttribute.SpecialType == ScriptPropertySpecialType.Provider)
		{
			Provider = vMScriptProperty.Value;
		}
	}

	protected override void OnPropertyChanged(PropertyChangedEventArgs e)
	{
		if (e.PropertyName == "Provider")
		{
			string[] products = DataUtil.GetProducts(AppServices.ResourceManager.DevSourcePath, Provider);
			string[] products2 = DataUtil.GetProducts(AppServices.ResourceManager.SourcePath, Provider);
			string[] array = new string[((products != null) ? products.Length : 0) + ((products2 != null) ? products2.Length : 0)];
			if (products != null)
			{
				Array.Copy(products, array, products.Length);
			}
			if (products2 != null)
			{
				Array.Copy(products2, 0, array, products.Length, products2.Length);
			}
			Products = array;
		}
		base.OnPropertyChanged(e);
	}
}
