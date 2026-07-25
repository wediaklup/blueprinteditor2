using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using TypeEdit.Interfaces;

namespace S9BEditor.Scripts;

internal class ScriptDefinition
{
	private static Dictionary<string, ScriptDefinition> ScriptDefinitionMap;

	public string Name { get; private set; }

	public string Description { get; private set; }

	public string[] Errors { get; private set; }

	public bool IsValid { get; private set; }

	public Type ScriptType { get; private set; }

	public string ScriptPath { get; private set; }

	public DateTime LastModified { get; private set; }

	public List<ScriptPropertyDefinition> Properties { get; private set; }

	public IScript CreateScript()
	{
		if (IsValid)
		{
			IScript script = Activator.CreateInstance(ScriptType) as IScript;
			{
				foreach (ScriptPropertyDefinition property in Properties)
				{
					Type propertyType = property.PropertyInfo.PropertyType;
					object value = Convert.ChangeType(property.Value, propertyType);
					property.PropertyInfo.SetValue(script, value, null);
				}
				return script;
			}
		}
		return null;
	}

	private ScriptDefinition()
	{
		Properties = new List<ScriptPropertyDefinition>();
	}

	static ScriptDefinition()
	{
		ScriptDefinitionMap = new Dictionary<string, ScriptDefinition>();
	}

	public static ScriptDefinition FromFile(string scriptPath)
	{
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Expected O, but got Unknown
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_013c: Expected O, but got Unknown
		FileInfo fileInfo = new FileInfo(scriptPath);
		if (!fileInfo.Exists)
		{
			throw new FileNotFoundException("Could not find script file", scriptPath);
		}
		if (ScriptDefinitionMap.TryGetValue(scriptPath, out var value))
		{
			if (value.LastModified < fileInfo.LastWriteTime)
			{
				value = null;
			}
		}
		else
		{
			value = null;
		}
		if (value == null)
		{
			value = new ScriptDefinition();
			try
			{
				CodeDomProvider val = null;
				if (scriptPath.EndsWith(".cs", StringComparison.OrdinalIgnoreCase))
				{
					val = CodeDomProvider.CreateProvider("CSharp");
				}
				else if (scriptPath.EndsWith(".vb", StringComparison.OrdinalIgnoreCase))
				{
					val = CodeDomProvider.CreateProvider("VisualBasic");
				}
				if (val != null)
				{
					CompilerParameters val2 = new CompilerParameters();
					AssemblyName[] referencedAssemblies = Assembly.GetExecutingAssembly().GetReferencedAssemblies();
					AssemblyName[] array = referencedAssemblies;
					foreach (AssemblyName assemblyName in array)
					{
						Assembly assembly = Assembly.Load(assemblyName.ToString());
						val2.ReferencedAssemblies.Add(assembly.Location);
						val2.ReferencedAssemblies.Add("Microsoft.CSharp.dll");
					}
					val2.CompilerOptions = "/optimize";
					val2.GenerateInMemory = true;
					List<string> list = new List<string>();
					CompilerResults val3 = val.CompileAssemblyFromFile(val2, new string[1] { scriptPath });
					foreach (CompilerError item in (CollectionBase)(object)val3.Errors)
					{
						CompilerError val4 = item;
						list.Add($"{val4.ErrorText}\r\nLine: {val4.Line}\r\nColumn: {val4.Column}");
					}
					value.Errors = list.ToArray();
					if (((CollectionBase)(object)val3.Errors).Count == 0)
					{
						Assembly compiledAssembly = val3.CompiledAssembly;
						if (compiledAssembly != null)
						{
							Type type = null;
							Type[] exportedTypes = compiledAssembly.GetExportedTypes();
							foreach (Type type2 in exportedTypes)
							{
								if (Enumerable.Contains<Type>(type2.GetInterfaces(), typeof(IScript)))
								{
									type = type2;
									break;
								}
							}
							if (type != null)
							{
								object[] customAttributes = type.GetCustomAttributes(typeof(ScriptAttribute), inherit: false);
								if (customAttributes.Length > 0)
								{
									ScriptAttribute scriptAttribute = customAttributes[0] as ScriptAttribute;
									value.ScriptType = type;
									value.Name = scriptAttribute.Name;
									value.Description = scriptAttribute.Description;
									value.IsValid = true;
									value.LastModified = fileInfo.LastWriteTime;
									value.ScriptPath = scriptPath;
									ScriptDefinitionMap[scriptPath] = value;
									PropertyInfo[] properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetProperty);
									foreach (PropertyInfo propertyInfo in properties)
									{
										object[] customAttributes2 = propertyInfo.GetCustomAttributes(typeof(ScriptPropertyAttribute), inherit: true);
										if (customAttributes2.Length > 0)
										{
											ScriptPropertyAttribute scriptPropertyAttribute = customAttributes2[0] as ScriptPropertyAttribute;
											value.Properties.Add(new ScriptPropertyDefinition(propertyInfo, scriptPropertyAttribute.InitialValue));
										}
									}
								}
								else
								{
									value.Errors = new string[1] { "ScriptAttribute not found on IScript" };
								}
							}
							else
							{
								value.Errors = new string[1] { "No IScript type found" };
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				value.Errors = new string[1] { "An exception occured: " + ex.Message };
			}
		}
		if (value != null && value.Errors.Length > 0)
		{
			value.Name = Path.GetFileName(scriptPath);
			value.Description = "SCRIPT INVALID" + Environment.NewLine + string.Join(Environment.NewLine + "--------" + Environment.NewLine, value.Errors);
		}
		return value;
	}
}
