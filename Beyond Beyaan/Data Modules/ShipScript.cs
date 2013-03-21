using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.CodeDom.Compiler;
using Microsoft.CSharp;

namespace Beyond_Beyaan.Data_Modules
{
	public class ShipScript
	{
		private Object scriptInstance;

		private MethodInfo onHit;
		private MethodInfo initialize;
		private MethodInfo updateShipInfo;
		private MethodInfo getShipInfo;
		private MethodInfo isShipDesignValid;

		public ShipScript(FileInfo filePath)
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
			scriptInstance = a.CreateInstance(instanceName, false, System.Reflection.BindingFlags.ExactBinding, null, null, null, null);

			onHit = a.GetType(instanceName).GetMethod("OnHit");
			getShipInfo = a.GetType(instanceName).GetMethod("GetShipInfo");
			initialize = a.GetType(instanceName).GetMethod("Initialize");
			updateShipInfo = a.GetType(instanceName).GetMethod("UpdateShipInfo");
			isShipDesignValid = a.GetType(instanceName).GetMethod("IsShipDesignValid");
			
		}

		public Dictionary<string, object>[] OnHit(int impactX, int impactY, int shipX, int shipY, int shipSize, Dictionary<string, object>[] currentValues, Dictionary<string, object> shipValues, Dictionary<string, object> particleValues)
		{
			return (Dictionary<string, object>[])onHit.Invoke(scriptInstance, new Object[] { impactX, impactY, shipX, shipY, shipSize, currentValues, shipValues, particleValues });
		}

		public Dictionary<string, object> GetInformation(Dictionary<string, object> shipValues)
		{
			return (Dictionary<string, object>)getShipInfo.Invoke(scriptInstance, new Object[] { new Dictionary<string, object>(shipValues) });
		}

		public Dictionary<string, object> Initialize(Dictionary<string, object> shipValues)
		{
			return (Dictionary<string, object>)initialize.Invoke(scriptInstance, new Object[] { new Dictionary<string, object> (shipValues) });
		}

		public Dictionary<string, object> UpdateShipInfo(Dictionary<string, object> shipValues)
		{
			return (Dictionary<string, object>)updateShipInfo.Invoke(scriptInstance, new object[] { new Dictionary<string, object>(shipValues) });
		}

		public string IsShipDesignValid(Dictionary<string, object> shipValues)
		{
			return (string)isShipDesignValid.Invoke(scriptInstance, new object[] { new Dictionary<string, object>(shipValues) });
		}
	}
}
