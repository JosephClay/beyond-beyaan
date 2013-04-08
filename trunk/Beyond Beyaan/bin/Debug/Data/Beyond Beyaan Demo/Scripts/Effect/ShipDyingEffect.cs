using System;
using System.Collections.Generic;
using System.Text;

namespace Beyond_Beyaan
{
	public class ShipDyingEffect
	{
		public Dictionary<string, object> Spawn(Dictionary<string, object> itemValues)
		{
			itemValues.Add("LifeSpan", 1.0f);
			itemValues.Add("Iter", 0.0f);
			return itemValues;
		}
		public Dictionary<string, object>[] Update(Dictionary<string, object> itemValues, float frameDeltaTime)
		{
			List<Dictionary<string, object>> items = new List<Dictionary<string, object>>();
			items.Add(new Dictionary<string, object>(itemValues));
			float lifeSpan = (float)items[0]["LifeSpan"];
			lifeSpan -= frameDeltaTime;
			items[0]["LifeSpan"] = lifeSpan;

			if (lifeSpan <= 0)
			{
				items[0].Add("_remove", true);
			}

			float iter = (float)items[0]["Iter"];
			iter -= frameDeltaTime;
			while (iter < 0.0f)
			{
				iter += 0.5f;
				Dictionary<string, object> addExplosion = new Dictionary<string, object>();
				addExplosion.Add("SpawnParticle", "LargeExplosion");
				addExplosion.Add("ImpactX", items[0]["XPos"]);
				addExplosion.Add("ImpactY", items[0]["YPos"]);
				addExplosion.Add("Angle", 0.0f);
				addExplosion.Add("LifeSpan", 0.5f);
				items.Add(addExplosion);
				if (!items[0].ContainsKey("_removeShip"))
				{
					items[0].Add("_removeShip", true);
				}
			}
			items[0]["Iter"] = iter;
			return items.ToArray();
		}

	}
}