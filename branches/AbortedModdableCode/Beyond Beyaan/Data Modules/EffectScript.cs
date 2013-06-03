using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.CodeDom.Compiler;
using Microsoft.CSharp;

namespace Beyond_Beyaan.Data_Modules
{
	public class EffectScript
	{
		private Object scriptInstance;

		private MethodInfo update;
		private MethodInfo spawn;

		public EffectScript(FileInfo filePath)
		{
			CompilerParameters cp = new CompilerParameters();
			cp.GenerateExecutable = false;
			cp.GenerateInMemory = true;
			cp.ReferencedAssemblies.Add("system.dll");

			CSharpCodeProvider provider = new CSharpCodeProvider();

			CompilerResults result = provider.CompileAssemblyFromFile(cp, filePath.FullName);

			if (result.Errors.HasErrors)
			{
				string errors = string.Empty;
				foreach (CompilerError err in result.Errors)
				{
					errors += err.ToString() + "\n";
				}
				throw new Exception(errors);
			}

			Assembly a = result.CompiledAssembly;
			string instanceName = "Beyond_Beyaan." + filePath.Name.Substring(0, filePath.Name.Length - 3);
			scriptInstance = a.CreateInstance(instanceName, false, BindingFlags.ExactBinding, null, null, null, null);

			update = a.GetType(instanceName).GetMethod("Update");
			spawn = a.GetType(instanceName).GetMethod("Spawn");
		}

		public Dictionary<string, object> Spawn(Dictionary<string, object> itemValues)
		{
			//Sets up the initial particle script
			return (Dictionary<string, object>)spawn.Invoke(scriptInstance, new Object[] { itemValues });
		}

		public Dictionary<string, object>[] Update(Dictionary<string, object> itemValues, float frameDeltaTime)
		{
			return (Dictionary<string, object>[])update.Invoke(scriptInstance, new Object[] { itemValues, frameDeltaTime });
		}
	}
}
