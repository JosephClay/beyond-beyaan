using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Linq;

namespace Beyond_Beyaan.Data_Modules
{
	public class Effect
	{
		//private string name;
		public EffectScript Script { get; private set; }
		private Dictionary<string, string> values;

		public Effect(XElement effect, string effectScriptPath)
		{
			values = new Dictionary<string, string>();
			//Load the attributes
			foreach (XAttribute attribute in effect.Attributes())
			{
				values.Add(attribute.Name.LocalName, attribute.Value);
			}
			if (values.ContainsKey("script"))
			{
				Script = new EffectScript(new FileInfo(Path.Combine(effectScriptPath, values["script"] + ".cs")));
			}
		}

		public Dictionary<string, object> AddValues(Dictionary<string, object> currentValues)
		{
			foreach (KeyValuePair<string, string> item in values)
			{
				if (!currentValues.ContainsKey(item.Key))
				{
					currentValues.Add(item.Key, item.Value);
				}
				//If it exists, don't replace, as it may be overriden by other scripts
			}
			return currentValues;
		}
	}
	public class EffectInstance
	{
		private Effect whichEffect;
		public Dictionary<string, object> Values { get; private set; }
		public ShipInstance ShipAffected { get; private set; }

		public EffectInstance(Effect whichEffect, ShipInstance ship, Dictionary<string, object> values)
		{
			this.whichEffect = whichEffect;
			this.Values = new Dictionary<string, object>(values);
			ShipAffected = ship;
			this.Values = whichEffect.AddValues(this.Values);
			this.Values = whichEffect.AddValues(ship.Values);
			this.Values = whichEffect.Script.Spawn(this.Values);
		}

		public Dictionary<string, object>[] Update(float frameDeltaTime)
		{
			Dictionary<string, object>[] result = whichEffect.Script.Update(Values, frameDeltaTime);
			Values = result[0];
			return result;
		}
	}
}
