using System;
using System.Collections.Generic;
using System.Text;

namespace Beyond_Beyaan
{
	public class BasicShieldParticle
	{
		public Dictionary<string, object> Spawn(Dictionary<string, object> itemValues)
		{
			itemValues.Add("PosX", (float)((int)itemValues["ImpactX"]));
			itemValues.Add("PosY", (float)((int)itemValues["ImpactY"]));
			itemValues.Add("Width", float.Parse((string)itemValues["frameWidth"], System.Globalization.CultureInfo.InvariantCulture));
			itemValues.Add("Height", float.Parse((string)itemValues["frameHeight"], System.Globalization.CultureInfo.InvariantCulture));
			itemValues.Add("ProcessCollisions", false);
			return itemValues;
		}
		public Dictionary<string, object>[] Update(Dictionary<string, object> itemValues, float frameDeltaTime)
		{
			float lifeSpan = (float)itemValues["LifeSpan"];
			lifeSpan -= frameDeltaTime;
			itemValues["LifeSpan"] = lifeSpan;

			if (lifeSpan <= 0)
			{
				itemValues.Add("RemoveParticle", true);
			}
			
			if (itemValues.ContainsKey("baseDamage"))
			{
				if (itemValues.ContainsKey("Damage"))
				{
					itemValues["Damage"] = itemValues["baseDamage"];
				}
				else
				{
					itemValues.Add("Damage", itemValues["baseDamage"]);
				}
			}
			return new Dictionary<string, object>[] { itemValues };
		}
		public Dictionary<string, object>[] PostHit(Dictionary<string, object> newValues, int impactX, int impactY)
		{
			//Do nothing, this shouldn't be called
			return new Dictionary<string, object>[] { newValues };
		}

	}
}