using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Beyond_Beyaan.Data_Modules;

namespace Beyond_Beyaan
{
	public class Ship
	{
		#region Properties
		public string Name { get; set; }
		public int Cost { get; set; }
		public int Size { get; set; }
		public int WhichStyle { get; set; }
		public Engine engine;
		public Shield shield;
		public Armor armor;
		public Computer computer;
		public List<Weapon> weapons;
		public float Maintenance { get { return Cost * 0.1f; } }
		public int TotalSpace { get { return Size * Size * 40; } }
		#endregion

		#region Constructors
		public Ship()
		{
			weapons = new List<Weapon>();
		}
		public Ship(Ship shipToCopy)
		{
			Name = shipToCopy.Name;
			Cost = shipToCopy.Cost;
			Size = shipToCopy.Size;
			WhichStyle = shipToCopy.WhichStyle;
			engine = shipToCopy.engine;
			shield = shipToCopy.shield;
			armor = shipToCopy.armor;
			computer = shipToCopy.computer;
			weapons = new List<Weapon>();
			foreach (Weapon weapon in shipToCopy.weapons)
			{
				Weapon weaponToCopy = new Weapon(weapon);
				weaponToCopy.Mounts = weapon.Mounts;
				weaponToCopy.Ammo = weapon.Ammo;
				weapons.Add(new Weapon(weaponToCopy));
			}
		}
		#endregion
	}

	public class TransportShip
	{
		public Race raceOnShip;
		public int amount;
	}

	public enum WeaponType { BEAM, PARTICLE, MISSILE, TORPEDO, BOMB, UNKNOWN }
	public class Weapon
	{
		//This class wraps the five different weapon types, allowing for easier management

		#region Variables
		private Beam beamWeapon;
		private Particle particleWeapon;
		private Missile missileWeapon;
		private Torpedo torpedoWeapon;
		private Bomb bombWeapon;

		private int mounts;
		private int ammo;
		#endregion

		#region Properties
		public int Mounts
		{
			get { return mounts; }
			set { mounts = value; }
		}
		public int Ammo
		{
			get { return ammo; }
			set { ammo = value; }
		}
		#endregion

		#region Constructors
		public Weapon(Beam beamWeapon)
		{
			mounts = 1;
			ammo = -1; //We don't care about this value if it's beam weapon
			this.beamWeapon = beamWeapon;
			particleWeapon = null;
			missileWeapon = null;
			torpedoWeapon = null;
			bombWeapon = null;
		}
		
		public Weapon(Particle particleWeapon)
		{
			mounts = 1; 
			ammo = 1;
			beamWeapon = null;
			this.particleWeapon = particleWeapon;
			missileWeapon = null;
			torpedoWeapon = null;
			bombWeapon = null;
		}

		public Weapon(Missile missileWeapon)
		{
			mounts = 1;
			ammo = 1;
			beamWeapon = null;
			particleWeapon = null;
			this.missileWeapon = missileWeapon;
			torpedoWeapon = null;
			bombWeapon = null;
		}

		public Weapon(Torpedo torpedoWeapon)
		{
			mounts = 1;
			ammo = -1; //We don't care about this value if it's torpedo weapon
			beamWeapon = null;
			particleWeapon = null;
			missileWeapon = null;
			this.torpedoWeapon = torpedoWeapon;
			bombWeapon = null;
		}

		public Weapon(Bomb bombWeapon)
		{
			mounts = 1;
			ammo = 1;
			beamWeapon = null;
			particleWeapon = null;
			missileWeapon = null;
			torpedoWeapon = null;
			this.bombWeapon = bombWeapon;
		}

		public Weapon(Weapon weapon)
		{
			beamWeapon = weapon.beamWeapon;
			particleWeapon = weapon.particleWeapon;
			missileWeapon = weapon.missileWeapon;
			torpedoWeapon = weapon.torpedoWeapon;
			bombWeapon = weapon.bombWeapon;

			mounts = 1;
			ammo = 1;

			if (beamWeapon != null || torpedoWeapon != null)
			{
				ammo = -1;
			}
		}
		#endregion

		public string GetName()
		{
			if (beamWeapon != null)
			{
				return beamWeapon.GetName();
			}
			if (particleWeapon != null)
			{
				return particleWeapon.GetName();
			}
			if (missileWeapon != null)
			{
				return missileWeapon.GetName();
			}
			if (torpedoWeapon != null)
			{
				return torpedoWeapon.GetName();
			}
			if (bombWeapon != null)
			{
				return bombWeapon.GetName();
			}
			return "Undefined";
		}

		public float GetDamage(Computer computer)
		{
			if (beamWeapon != null)
			{
				return (int)(beamWeapon.GetDamage() * (computer.beamEfficiency / 100.0f));
			}
			if (particleWeapon != null)
			{
				return (int)(particleWeapon.GetDamage() * (computer.particleEfficiency / 100.0f));
			}
			if (missileWeapon != null)
			{
				return (int)(missileWeapon.GetDamage() * (computer.missileEfficiency / 100.0f));
			}
			if (torpedoWeapon != null)
			{
				return (int)(torpedoWeapon.GetDamage() * (computer.torpedoEfficiency / 100.0f));
			}
			if (bombWeapon != null)
			{
				return (int)(bombWeapon.GetDamage() * (computer.bombEfficiency / 100.0f));
			}
			return -1; //Unknown
		}

		public int GetAccuracy(Computer computer)
		{
			//This will multiply the computer's accuracy for this type of weapon with this weapon's accuracy
			if (beamWeapon != null)
			{
				return (int)(beamWeapon.GetAccuracy() * (computer.beamEfficiency / 100.0f));
			}
			if (particleWeapon != null)
			{
				return (int)(particleWeapon.GetAccuracy() * (computer.beamEfficiency / 100.0f));
			}
			return -1; //Unknown or not applicable
		}

		public int GetSpace()
		{
			if (beamWeapon != null)
			{
				return beamWeapon.GetSpace();
			}
			if (particleWeapon != null)
			{
				return particleWeapon.GetSpace();
			}
			if (missileWeapon != null)
			{
				return missileWeapon.GetSpace();
			}
			if (torpedoWeapon != null)
			{
				return torpedoWeapon.GetSpace();
			}
			if (bombWeapon != null)
			{
				return bombWeapon.GetSpace();
			}
			return -1; //Unknown
		}

		public int GetCost()
		{
			if (beamWeapon != null)
			{
				return beamWeapon.GetCost();
			}
			if (particleWeapon != null)
			{
				return particleWeapon.GetCost();
			}
			if (missileWeapon != null)
			{
				return missileWeapon.GetCost();
			}
			if (torpedoWeapon != null)
			{
				return torpedoWeapon.GetCost();
			}
			if (bombWeapon != null)
			{
				return bombWeapon.GetCost();
			}
			return -1; //Unknown
		}

		public WeaponType GetWeaponType()
		{
			if (beamWeapon != null)
			{
				return WeaponType.BEAM;
			}
			if (particleWeapon != null)
			{
				return WeaponType.PARTICLE;
			}
			if (missileWeapon != null)
			{
				return WeaponType.MISSILE;
			}
			if (torpedoWeapon != null)
			{
				return WeaponType.TORPEDO;
			}
			if (bombWeapon != null)
			{
				return WeaponType.BOMB;
			}
			return WeaponType.UNKNOWN;
		}
	}
}
