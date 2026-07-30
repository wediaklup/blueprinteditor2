using System.CodeDom.Compiler;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows;

namespace S9BEditor.Properties
{
	[CompilerGenerated]
	[GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "10.0.0.0")]
	internal sealed class Settings : ApplicationSettingsBase
	{
		private static Settings defaultInstance = (Settings)(object)SettingsBase.Synchronized((SettingsBase)(object)new Settings());

		public static Settings Default => defaultInstance;

		[UserScopedSetting]
		[DefaultSettingValue("")]
		[DebuggerNonUserCode]
		public string DeploymentFolder
		{
			get
			{
				return (string)((SettingsBase)this)["DeploymentFolder"];
			}
			set
			{
				((SettingsBase)this)["DeploymentFolder"] = value;
			}
		}

		[DefaultSettingValue("")]
		[UserScopedSetting]
		[DebuggerNonUserCode]
		public string LastSelectedFile
		{
			get
			{
				return (string)((SettingsBase)this)["LastSelectedFile"];
			}
			set
			{
				((SettingsBase)this)["LastSelectedFile"] = value;
			}
		}

		[DebuggerNonUserCode]
		[DefaultSettingValue("False")]
		[UserScopedSetting]
		public bool AdvancedDataEditorView
		{
			get
			{
				return (bool)((SettingsBase)this)["AdvancedDataEditorView"];
			}
			set
			{
				((SettingsBase)this)["AdvancedDataEditorView"] = value;
			}
		}

		[DebuggerNonUserCode]
		[DefaultSettingValue("1")]
		[UserScopedSetting]
		public int ThreadCount
		{
			get
			{
				return (int)((SettingsBase)this)["ThreadCount"];
			}
			set
			{
				((SettingsBase)this)["ThreadCount"] = value;
			}
		}

		[DefaultSettingValue("False")]
		[DebuggerNonUserCode]
		[UserScopedSetting]
		public bool ThreadCountSet
		{
			get
			{
				return (bool)((SettingsBase)this)["ThreadCountSet"];
			}
			set
			{
				((SettingsBase)this)["ThreadCountSet"] = value;
			}
		}

		[DefaultSettingValue("200")]
		[DebuggerNonUserCode]
		[UserScopedSetting]
		public uint TypeEditorNameColumnWidth
		{
			get
			{
				return (uint)((SettingsBase)this)["TypeEditorNameColumnWidth"];
			}
			set
			{
				((SettingsBase)this)["TypeEditorNameColumnWidth"] = value;
			}
		}

		[DebuggerNonUserCode]
		[DefaultSettingValue("300")]
		[UserScopedSetting]
		public uint TypeEditorValueColumnWidth
		{
			get
			{
				return (uint)((SettingsBase)this)["TypeEditorValueColumnWidth"];
			}
			set
			{
				((SettingsBase)this)["TypeEditorValueColumnWidth"] = value;
			}
		}

		[DebuggerNonUserCode]
		[DefaultSettingValue("200")]
		[UserScopedSetting]
		public uint TypeEditorTypeColumnWidth
		{
			get
			{
				return (uint)((SettingsBase)this)["TypeEditorTypeColumnWidth"];
			}
			set
			{
				((SettingsBase)this)["TypeEditorTypeColumnWidth"] = value;
			}
		}

		[DefaultSettingValue("-1, -1, 0, 0")]
		[UserScopedSetting]
		[DebuggerNonUserCode]
		public Rectangle WindowRect2
		{
			get
			{
				return (Rectangle)((SettingsBase)this)["WindowRect2"];
			}
			set
			{
				((SettingsBase)this)["WindowRect2"] = value;
			}
		}

		[UserScopedSetting]
		[DefaultSettingValue("False")]
		[DebuggerNonUserCode]
		public bool Maximized
		{
			get
			{
				return (bool)((SettingsBase)this)["Maximized"];
			}
			set
			{
				((SettingsBase)this)["Maximized"] = value;
			}
		}

		[DefaultSettingValue("*")]
		[UserScopedSetting]
		[DebuggerNonUserCode]
		public GridLength WindowCol1Size
		{
			get
			{
				//IL_000b: Unknown result type (might be due to invalid IL or missing references)
				return (GridLength)((SettingsBase)this)["WindowCol1Size"];
			}
			set
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				((SettingsBase)this)["WindowCol1Size"] = value;
			}
		}

		[UserScopedSetting]
		[DefaultSettingValue("2*")]
		[DebuggerNonUserCode]
		public GridLength WindowCol2Size
		{
			get
			{
				//IL_000b: Unknown result type (might be due to invalid IL or missing references)
				return (GridLength)((SettingsBase)this)["WindowCol2Size"];
			}
			set
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				((SettingsBase)this)["WindowCol2Size"] = value;
			}
		}

		[DefaultSettingValue("2*")]
		[UserScopedSetting]
		[DebuggerNonUserCode]
		public GridLength WindowRow1Size
		{
			get
			{
				//IL_000b: Unknown result type (might be due to invalid IL or missing references)
				return (GridLength)((SettingsBase)this)["WindowRow1Size"];
			}
			set
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				((SettingsBase)this)["WindowRow1Size"] = value;
			}
		}

		[UserScopedSetting]
		[DefaultSettingValue("*")]
		[DebuggerNonUserCode]
		public GridLength WindowRow2Size
		{
			get
			{
				//IL_000b: Unknown result type (might be due to invalid IL or missing references)
				return (GridLength)((SettingsBase)this)["WindowRow2Size"];
			}
			set
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				((SettingsBase)this)["WindowRow2Size"] = value;
			}
		}

		[UserScopedSetting]
		[DefaultSettingValue("True")]
		[DebuggerNonUserCode]
		public bool OutputIsExpanded
		{
			get
			{
				return (bool)((SettingsBase)this)["OutputIsExpanded"];
			}
			set
			{
				((SettingsBase)this)["OutputIsExpanded"] = value;
			}
		}

		[DefaultSettingValue("100")]
		[UserScopedSetting]
		[DebuggerNonUserCode]
		public double OutputExpandedHeight
		{
			get
			{
				return (double)((SettingsBase)this)["OutputExpandedHeight"];
			}
			set
			{
				((SettingsBase)this)["OutputExpandedHeight"] = value;
			}
		}

		[DebuggerNonUserCode]
		[DefaultSettingValue("True")]
		[UserScopedSetting]
		public bool UseSystemDataFormat
		{
			get
			{
				return (bool)((SettingsBase)this)["UseSystemDataFormat"];
			}
			set
			{
				((SettingsBase)this)["UseSystemDataFormat"] = value;
			}
		}
	}
}
