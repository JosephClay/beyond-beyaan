using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beyond_Beyaan.Data_Modules
{
	public class ShipInstance
	{
		public ShipDesign BaseShipDesign { get; private set; }

		public List<EquipmentInstance> Equipments { get; private set; }

		public Empire Owner { get; private set; }

		public Dictionary<string, object> Values { get; set; }

		// To-do: Re-work the maintenance formula
		public float Maintenance { get { return BaseShipDesign.Maintenance; } }

		public List<EffectInstance> PermEffects { get; private set; }

		public ShipInstance()
		{
			BaseShipDesign = null;
			Equipments = new List<EquipmentInstance>();
			Values = new Dictionary<string, object>();
			PermEffects = new List<EffectInstance>();
		}

		public ShipInstance(ShipDesign baseShipDesign, Empire owner)
		{
			BaseShipDesign = baseShipDesign;
			PermEffects = new List<EffectInstance>();
			Values = new Dictionary<string, object>();
			foreach (KeyValuePair<string, object> keyValuePair in baseShipDesign.ShipClass.Values)
			{
				Values.Add(keyValuePair.Key, keyValuePair.Value);
			}
			Equipments = new List<EquipmentInstance>();
			foreach (Equipment equipment in BaseShipDesign.Equipments)
			{
				EquipmentInstance newEquipment = new EquipmentInstance(equipment, baseShipDesign.ShipClass.Size);
				newEquipment.Values = newEquipment.GetEquipmentInfo(Values);
				Equipments.Add(newEquipment);
			}
			ResetShipInfo();
			Owner = owner;
		}

		public ShipInstance(ShipInstance baseShipInstance)
		{
			BaseShipDesign = baseShipInstance.BaseShipDesign;
			PermEffects = new List<EffectInstance>();
			Equipments = new List<EquipmentInstance>();
			foreach (Equipment equipment in BaseShipDesign.Equipments)
			{
				Equipments.Add(new EquipmentInstance(equipment, baseShipInstance.BaseShipDesign.ShipClass.Size));
			}
			Values = new Dictionary<string, object>();
			foreach (KeyValuePair<string, object> keyValuePair in baseShipInstance.Values)
			{
				Values.Add(keyValuePair.Key, keyValuePair.Value);
			}
			Owner = baseShipInstance.Owner;
		}

		public float GetGalaxySpeed()
		{
			if (Values.ContainsKey("galaxySpeed"))
			{
				return (float)Values["galaxySpeed"];
			}
			return 0.0f;
		}

		public void ResetShipInfo()
		{
			Values = BaseShipDesign.ShipClass.ShipScript.Initialize(BaseShipDesign.ShipClass.Values);
			RefreshShipInfo();
		}

		public void RefreshShipInfo()
		{
			Values = BaseShipDesign.ShipClass.ShipScript.UpdateShipInfo(Values);
			foreach (EquipmentInstance equipment in Equipments)
			{
				if (equipment.ItemType.Script != null)
				{
					Values = equipment.ItemType.Script.UpdateShipInfo(Values, equipment.Values);
				}
			}
		}
	}
}
