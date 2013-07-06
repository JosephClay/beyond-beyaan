using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Beyond_Beyaan.Data_Modules;

namespace Beyond_Beyaan
{
	public class Ship
	{
		public const int SMALL = 1;
		public const int MEDIUM = 2;
		public const int LARGE = 3;
		public const int HUGE = 4;

		#region Properties
		public string Name { get; set; }
		public int Size { get; set; }
		public int WhichStyle { get; set; }
		public Engine engine;
		public Shield shield;
		public Armor armor;
		public Computer computer;
		public List<Weapon> weapons;
		public List<Special> specials;
		public float Maintenance { get { return Cost * 0.02f; } }
		public int TotalSpace 
		{ 
			get 
			{
				int space = 40;
				switch (Size)
				{
					case MEDIUM: 
						space = 200;
						break;
					case LARGE:
						space = 1000;
						break;
					case HUGE:
						space = 5000;
						break;
				}
				//Todo: Add 2% space for each level in construction tech
				//space *= (1.0f + (0.02 * ConstructionLevel));
				return space;
			} 
		}
		public int Cost
		{
			get
			{
				int cost = 6;
				switch (Size)
				{
					case MEDIUM:
						cost = 36;
						break;
					case LARGE:
						cost = 200;
						break;
					case HUGE:
						cost = 1200;
						break;
				}
				if (engine != null)
				{
					cost += engine.GetCost(TotalSpace);
				}
				if (armor != null)
				{
					cost += armor.GetCost(TotalSpace);
				}
				if (shield != null)
				{
					cost += shield.GetCost(TotalSpace);
				}
				if (computer != null)
				{
					cost += computer.GetCost(TotalSpace);
				}
				foreach (var weapon in weapons)
				{
					cost += weapon.GetCost();
				}
				foreach (var special in specials)
				{
					cost += special.GetCost(TotalSpace);
				}
				return cost;
			}
		}
		#endregion

		#region Constructors
		public Ship()
		{
			weapons = new List<Weapon>();
			specials = new List<Special>();
		}
		public Ship(Ship shipToCopy)
		{
			Name = shipToCopy.Name;
			Size = shipToCopy.Size;
			WhichStyle = shipToCopy.WhichStyle;
			engine = shipToCopy.engine;
			shield = shipToCopy.shield;
			armor = shipToCopy.armor;
			computer = shipToCopy.computer;
			weapons = new List<Weapon>();
			specials = new List<Special>();
			foreach (Weapon weapon in shipToCopy.weapons)
			{
				Weapon weaponToCopy = new Weapon(weapon);
				weaponToCopy.Mounts = weapon.Mounts;
				weapons.Add(new Weapon(weaponToCopy));
			}
			foreach (Special special in shipToCopy.specials)
			{
				specials.Add(special);
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
		#endregion

		#region Properties
		public int Mounts
		{
			get { return mounts; }
			set { mounts = value; }
		}
		#endregion

		#region Constructors
		public Weapon(Beam beamWeapon)
		{
			mounts = 1;
			this.beamWeapon = beamWeapon;
			particleWeapon = null;
			missileWeapon = null;
			torpedoWeapon = null;
			bombWeapon = null;
		}
		
		public Weapon(Particle particleWeapon)
		{
			mounts = 1; 
			beamWeapon = null;
			this.particleWeapon = particleWeapon;
			missileWeapon = null;
			torpedoWeapon = null;
			bombWeapon = null;
		}

		public Weapon(Missile missileWeapon)
		{
			mounts = 1;
			beamWeapon = null;
			particleWeapon = null;
			this.missileWeapon = missileWeapon;
			torpedoWeapon = null;
			bombWeapon = null;
		}

		public Weapon(Torpedo torpedoWeapon)
		{
			mounts = 1;
			beamWeapon = null;
			particleWeapon = null;
			missileWeapon = null;
			this.torpedoWeapon = torpedoWeapon;
			bombWeapon = null;
		}

		public Weapon(Bomb bombWeapon)
		{
			mounts = 1;
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
