using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Globalization;
using System.IO;
using S9BEditor.Properties;
using TypeEdit.Base;
using TypeEdit.DataHandling;
using TypeEdit.Interfaces.Data.Descriptors;
using TypeEdit.Interfaces.Data.Instancing;

namespace S9BEditor.ViewModels
{
	public class VMProperty : VMTypeEditComponent
	{
		private bool mIsEnabled;

		private bool mIsFileID;

		private string mFileIDType;

		private string mMinimum;

		private string mMaximum;

		private string mDefault;

		public bool IsEnabled
		{
			get
			{
				return mIsEnabled;
			}
			set
			{
				if (mIsEnabled != value)
				{
					mIsEnabled = value;
					RaisePropertyChanged("IsEnabled");
				}
			}
		}

		public bool IsFileID => mIsFileID;

		public string FileIDType => mFileIDType;

		public string Minimum
		{
			get
			{
				return mMinimum;
			}
			set
			{
				if (mMinimum != value)
				{
					mMinimum = value;
					RaisePropertyChanged("Minimum");
				}
			}
		}

		public string Maximum
		{
			get
			{
				return mMaximum;
			}
			set
			{
				if (mMaximum != value)
				{
					mMaximum = value;
					RaisePropertyChanged("Maximum");
				}
			}
		}

		public string Default
		{
			get
			{
				return mDefault;
			}
			set
			{
				if (mDefault != value)
				{
					mDefault = value;
					RaisePropertyChanged("Default");
				}
			}
		}

		public string PropertyName
		{
			get
			{
				if (Settings.Default.AdvancedDataEditorView)
				{
					return Property.Descriptor.Name;
				}
				if (Property.Descriptor.TryGetEditHintValue("FriendlyName", "value", out var value))
				{
					return value;
				}
				return S9BEUtil.CamelCaseToNormal(Property.Descriptor.Name);
			}
		}

		public string PropertyType => Property.Datum.TypeDescriptor.Name;

		public VMTypeDatumBase PropertyContent { get; private set; }

		[Browsable(false)]
		internal IProperty Property { get; private set; }

		public override bool CanDelete
		{
			get
			{
				if (PropertyContent.CanDelete)
				{
					return IsEnabled;
				}
				return false;
			}
		}

		public override bool CanCopy => PropertyContent.CanCopy;

		public override bool CanPaste
		{
			get
			{
				if (PropertyContent.CanPaste)
				{
					return IsEnabled;
				}
				return false;
			}
		}

		public override bool CanSelect => true;

		public bool CanResetToDefault
		{
			get
			{
				if (PropertyContent.CanResetToDefault)
				{
					return IsEnabled;
				}
				return false;
			}
		}

		internal VMProperty(ViewModelBase owner, IProperty prop)
			: base(owner)
		{
			Property = prop;
			VMTypeEditContainer vMTypeEditContainer = FindTopMost<VMTypeEditContainer>();
			if (vMTypeEditContainer != null)
			{
				PropertyContent = vMTypeEditContainer.Factory.CreateViewModel(this, Property.Datum);
			}
			mIsEnabled = true;
			updateAdditionalInfo();
		}

		protected void SetCurrentError(string errorMsg)
		{
			if (errorMsg == null)
			{
				if (base.Errors != null)
				{
					base.Errors = null;
				}
				return;
			}
			if (base.Errors == null || base.Errors.Length != 1)
			{
				base.Errors = new string[1];
			}
			base.Errors[0] = errorMsg + Environment.NewLine + "@ " + TypeEditSchemaUtil.GetDatumPath(Property.Datum);
		}

		protected void SetCurrentWarning(string warningMsg)
		{
			if (warningMsg == null)
			{
				if (base.Warnings != null)
				{
					base.Warnings = null;
				}
				return;
			}
			if (base.Warnings == null || base.Warnings.Length != 1)
			{
				base.Warnings = new string[1];
			}
			base.Warnings[0] = warningMsg + Environment.NewLine + "@ " + TypeEditSchemaUtil.GetDatumPath(Property.Datum);
		}

		public virtual bool CommitChanges(bool resetModifiedState)
		{
			string currentWarning = null;
			string currentError = null;
			if (IsFileID)
			{
				string value = PropertyContent.GetValue(CultureInfo.InvariantCulture);
				if (value.Length > 0)
				{
					VMTypeEditContainer vMTypeEditContainer = FindTopMost<VMTypeEditContainer>();
					if (vMTypeEditContainer != null)
					{
						string text;
						string text2;
						if (Property.Descriptor.Name == "BlueprintID" && Property.Datum.Owner != null && Property.Datum.Owner.TypeDescriptor.Name == "iBlueprintLibrary-cAbsoluteBlueprintID")
						{
							text = Property.Datum.GetValue("../BlueprintSetID/Provider");
							text2 = Property.Datum.GetValue("../BlueprintSetID/Product");
						}
						else
						{
							text = vMTypeEditContainer.Provider;
							text2 = vMTypeEditContainer.Product;
						}
						if (string.IsNullOrWhiteSpace(text))
						{
							currentWarning = "Empty provider field but blueprint ID set. Is this intentional? Referenced blueprint will not be exported";
						}
						else if (!text.Equals(vMTypeEditContainer.Provider, StringComparison.OrdinalIgnoreCase))
						{
							currentWarning = "Referenced provider does not match the provider of this file - File will not be included in export";
						}
						else if (string.IsNullOrWhiteSpace(text2))
						{
							currentWarning = "Empty product field but blueprint ID set. Is this intentional? Referenced blueprint will not be exported";
						}
						else
						{
							if (!text2.Equals(vMTypeEditContainer.Product, StringComparison.OrdinalIgnoreCase))
							{
								currentWarning = "Referenced product does not match the product of this file";
							}
							bool flag = false;
							bool flag2 = false;
							if (value.IndexOfAny(Path.GetInvalidPathChars()) < 0)
							{
								try
								{
									string fullPath = Path.GetFullPath(Path.Combine(vMTypeEditContainer.SourceDirectory, text, text2, value));
									flag = true;
									if (!Path.HasExtension(fullPath))
									{
										IEnumerable<string> extensionsForFileID = getExtensionsForFileID();
										if (extensionsForFileID != null)
										{
											foreach (string item in extensionsForFileID)
											{
												if (File.Exists(fullPath + item))
												{
													flag2 = true;
													break;
												}
											}
										}
									}
									else if (File.Exists(fullPath))
									{
										flag2 = true;
									}
								}
								catch (Exception)
								{
								}
							}
							if (!flag)
							{
								currentError = "Path is not valid: " + text + "\\" + text2 + "\\" + value;
							}
							else if (!flag2)
							{
								currentError = "Referenced file does not exist: " + text + "\\" + text2 + "\\" + value;
							}
						}
					}
				}
			}
			SetCurrentError(currentError);
			SetCurrentWarning(currentWarning);
			return PropertyContent.CommitChanges(resetModifiedState);
		}

		private IEnumerable<string> getExtensionsForFileID()
		{
			if (mFileIDType == null)
			{
				return new string[1] { ".xml" };
			}
			return TypeEditSchemaUtil.GetExtensionsForFileType(mFileIDType);
		}

		public override void Copy()
		{
			PropertyContent.Copy();
		}

		public override void Paste()
		{
			PropertyContent.Paste();
		}

		public override void Delete()
		{
			PropertyContent.Delete();
		}

		private void globalSettingsPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "AdvancedDataEditorView")
			{
				RaisePropertyChanged("PropertyName");
				updateAdditionalInfo();
			}
		}

		private void updateAdditionalInfo()
		{
			IEditHint editHint = Property.Descriptor.GetEditHint("Range");
			if (editHint != null)
			{
				Minimum = editHint["minimum"];
				Maximum = editHint["maximum"];
			}
			editHint = Property.Descriptor.GetEditHint("Default");
			if (editHint != null)
			{
				string text = editHint["value"];
				if (PropertyContent.Datum is IEnumTypeDatum && !Settings.Default.AdvancedDataEditorView)
				{
					text = S9BEUtil.CamelCaseToNormal(text);
				}
				Default = text;
			}
			string[] allFileTypes = TypeEditFileTypes.AllFileTypes;
			foreach (string editHintType in allFileTypes)
			{
				if (Property.Descriptor.GetEditHint(editHintType) != null)
				{
					mFileIDType = editHintType;
					mIsFileID = true;
					break;
				}
			}
			if (!mIsFileID && Property.Descriptor.Name == "BlueprintID" && Property.Datum.Owner != null && Property.Datum.Owner.TypeDescriptor.Name == "iBlueprintLibrary-cAbsoluteBlueprintID")
			{
				mIsFileID = true;
			}
		}

		public override void Dispose()
		{
			if (Property.Descriptor.IsReferenceTarget)
			{
				FindTopMost<VMTypeEditContainer>()?.UnregisterReferenceTarget(this);
			}
			PropertyContent.Dispose();
			((ApplicationSettingsBase)Settings.Default).PropertyChanged -= globalSettingsPropertyChanged;
			base.Dispose();
		}

		internal override void Initialise()
		{
			((ApplicationSettingsBase)Settings.Default).PropertyChanged += globalSettingsPropertyChanged;
			PropertyContent.Initialise();
			if (Property.Descriptor.IsReferenceTarget)
			{
				FindTopMost<VMTypeEditContainer>()?.RegisterReferenceTarget(this);
			}
			base.Initialise();
		}

		public void ResetToDefault()
		{
			if (!PropertyContent.CanResetToDefault)
			{
				return;
			}
			PropertyContent.ResetToDefault();
			bool wasValueSet = false;
			Property.ForEachDefaultEditHint(delegate(string name, string defaultValue)
			{
				string value = PropertyContent.Datum.GetValue(name);
				if (PropertyContent.Datum.TrySetValue(name, defaultValue) && value != PropertyContent.Datum.GetValue(name))
				{
					wasValueSet = true;
				}
			});
			if (wasValueSet)
			{
				PropertyContent.Reload();
			}
		}
	}
}
