using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.CodeDom.Compiler;
using Microsoft.CSharp;

namespace Beyond_Beyaan.Data_Modules
{
	public class TechnologyScript
	{
		private Object scriptInstance;

		private MethodInfo getTargetReticle;
		private MethodInfo activate;
		private MethodInfo onHit;
		private MethodInfo getEquipmentInfo;
		private MethodInfo updateShipInfo;

		public TechnologyScript(FileInfo filePath)
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

			System.Reflection.Assembly a = result.CompiledAssembly;
			string instanceName = "Beyond_Beyaan." + filePath.Name.Substring(0, filePath.Name.Length - 3);
			scriptInstance = a.CreateInstance(instanceName, false, System.Reflection.BindingFlags.ExactBinding, null, null, null, null);

			getTargetReticle = a.GetType(instanceName).GetMethod("GetTargetReticle");
			activate = a.GetType(instanceName).GetMethod("Activate");
			onHit = a.GetType(instanceName).GetMethod("OnHit");
			getEquipmentInfo = a.GetType(instanceName).GetMethod("CompileEquipmentInfo");
			updateShipInfo = a.GetType(instanceName).GetMethod("UpdateShipInfo");
		}

		public int GetTargetReticle(int shipSize)
		{
			return (int)getTargetReticle.Invoke(scriptInstance, new Object[] { shipSize });
		}

		public Dictionary<string, object>[] Activate(int origX, int origY, int targetX, int targetY, int maxX, int maxY, Dictionary<string, object> itemValues)
		{
			return (Dictionary<string, object>[])activate.Invoke(scriptInstance, new Object[] { origX, origY, targetX, targetY, maxX, maxY, itemValues });
		}

		public Dictionary<string, object>[] OnHit(int impactX, int impactY, int shipX, int shipY, int shipSize, Dictionary<string, object> itemValues, Dictionary<string, object> particleValues)
		{
			return (Dictionary<string, object>[])onHit.Invoke(scriptInstance, new Object[] { impactX, impactY, shipX, shipY, shipSize, itemValues, particleValues });
		}

		public Dictionary<string, object> GetEquipmentInfo(Dictionary<string, object> mainItemValues, Dictionary<string, object> mountItemValues, Dictionary<string, object>[] modifierItemValues, Dictionary<string, object> shipValues, Dictionary<string, object> modifiableValues)
		{
			return (Dictionary<string, object>)getEquipmentInfo.Invoke(scriptInstance, new Object[] { mainItemValues, mountItemValues, modifierItemValues, shipValues, modifiableValues });
		}

		public Dictionary<string, object> UpdateShipInfo(Dictionary<string, object> shipValues, Dictionary<string, object> equipmentValues)
		{
			return (Dictionary<string, object>)updateShipInfo.Invoke(scriptInstance, new Object[] { shipValues, equipmentValues });
		}
	}
}
