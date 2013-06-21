using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Beyond_Beyaan.Data_Managers;
using Beyond_Beyaan.Data_Modules;

namespace Beyond_Beyaan
{
	public enum TechField { COMPUTER, ENGINE, SHIELD, ARMOR, INFRASTRUCTURE, BEAM, PARTICLE, MISSILE, TORPEDO, BOMB }

	public class TechnologyManager
	{
		#region Properties
		private List<Shield> Shields { get; set; }
		private List<Computer> Computers { get; set; }
		private List<Beam> Beams { get; set; }
		private List<Particle> Particles { get; set; }
		private List<Torpedo> Torpedoes { get; set; }
		private List<Missile> Missiles { get; set; }
		private List<Bomb> Bombs { get; set; }
		private List<Armor> Armors { get; set; }
		private List<Engine> Engines { get; set; }
		private List<Infrastructure> Infrastructures { get; set; }

		public int WhichShieldBeingResearched { get; set; }
		public int WhichComputerBeingResearched { get; set; }
		public int WhichBeamBeingResearched { get; set; }
		public int WhichParticleBeingResearched { get; set; }
		public int WhichTorpedoBeingResearched { get; set; }
		public int WhichMissileBeingResearched { get; set; }
		public int WhichBombBeingResearched { get; set; }
		public int WhichArmorBeingResearched { get; set; }
		public int WhichEngineBeingResearched { get; set; }
		public int WhichInfrastructureBeingResearched { get; set; }

		public List<Shield> VisibleShields { get; private set; }
		public List<Computer> VisibleComputers { get; private set; }
		public List<Beam> VisibleBeams { get; private set; }
		public List<Particle> VisibleParticles { get; private set; }
		public List<Torpedo> VisibleTorpedoes { get; private set; }
		public List<Missile> VisibleMissiles { get; private set; }
		public List<Bomb> VisibleBombs { get; private set; }
		public List<Armor> VisibleArmors { get; private set; }
		public List<Engine> VisibleEngines { get; private set; }
		public List<Infrastructure> VisibleInfrastructures { get; private set; }

		public int ShieldLevel { get; private set; }
		public int ComputerLevel { get; private set; }
		public int BeamLevel { get; private set; }
		public int ParticleLevel { get; private set; }
		public int TorpedoLevel { get; private set; }
		public int MissileLevel { get; private set; }
		public int BombLevel { get; private set; }
		public int ArmorLevel { get; private set; }
		public int EngineLevel { get; private set; }
		public int InfrastructureLevel { get; private set; }

		public bool ShieldLocked { get; set; }
		public bool ComputerLocked { get; set; }
		public bool BeamLocked { get; set; }
		public bool ParticleLocked { get; set; }
		public bool MissileLocked { get; set; }
		public bool TorpedoLocked { get; set; }
		public bool BombLocked { get; set; }
		public bool ArmorLocked { get; set; }
		public bool EngineLocked { get; set; }
		public bool InfrastructureLocked { get; set; }

		public int ShieldPercentage { get; private set; }
		public int ArmorPercentage { get; private set; }
		public int EnginePercentage { get; private set; }
		public int ComputerPercentage { get; private set; }
		public int InfrastructurePercentage { get; private set; }
		public int BeamPercentage { get; private set; }
		public int ParticlePercentage { get; private set; }
		public int MissilePercentage { get; private set; }
		public int TorpedoPercentage { get; private set; }
		public int BombPercentage { get; private set; }
		#endregion

		#region Constructor
		public TechnologyManager()
		{
			Shields = new List<Shield>();
			Computers = new List<Computer>();
			Beams = new List<Beam>();
			Particles = new List<Particle>();
			Torpedoes = new List<Torpedo>();
			Missiles = new List<Missile>();
			Bombs = new List<Bomb>();
			Armors = new List<Armor>();
			Engines = new List<Engine>();
			Infrastructures = new List<Infrastructure>();

			VisibleShields = new List<Shield>();
			VisibleComputers = new List<Computer>();
			VisibleBeams = new List<Beam>();
			VisibleParticles = new List<Particle>();
			VisibleTorpedoes = new List<Torpedo>();
			VisibleMissiles = new List<Missile>();
			VisibleBombs = new List<Bomb>();
			VisibleArmors = new List<Armor>();
			VisibleEngines = new List<Engine>();
			VisibleInfrastructures = new List<Infrastructure>();

			ShieldPercentage = 10;
			ArmorPercentage = 10;
			EnginePercentage = 10;
			ComputerPercentage = 10;
			InfrastructurePercentage = 10;
			BeamPercentage = 10;
			ParticlePercentage = 10;
			MissilePercentage = 10;
			TorpedoPercentage = 10;
			BombPercentage = 10;
		}
		#endregion

		#region Functions
		public bool LoadTechnologies(string filePath)
		{
			try
			{
				//This loads technologies from a technologies.txt file.  Each Empire have their own TechnologyManager instance, so it's possible to have racial specific technologies
				if (File.Exists(filePath))
				{
					List<string> invalidTechs = new List<string>();
					string[] lines = File.ReadAllLines(filePath);
					int currentLine = 1;
					foreach (string line in lines)
					{
						if (string.IsNullOrEmpty(line.Trim()) || line.StartsWith("//"))
						{
							//ignore whitespace or comments
							continue;
						}
						Dictionary<string, string> items = new Dictionary<string, string>();
						string[] parts = line.Split(new[] { '|' });
						foreach (string part in parts)
						{
							string[] values = part.Split(new[] { '=' });
							if (values.Length != 2)
							{
								invalidTechs.Add("Line " + currentLine + " have incomplete value");
								continue;
							}
							items.Add(values[0].ToLower(), values[1]);
						}
						if (!items.ContainsKey("techtype"))
						{
							invalidTechs.Add("Line " + currentLine + " don't have techType value.");
							continue;
						}
						string reason;
						switch (items["techtype"])
						{
							case "beam":
								{
									Beam tech = new Beam();
									if (!tech.Load(items, out reason))
									{
										invalidTechs.Add("Line " + currentLine + " has invalid data: " + reason);
										break;
									}
									Beams.Add(tech);
								} break;
							case "particle":
								{
									Particle tech = new Particle();
									if (!tech.Load(items, out reason))
									{
										invalidTechs.Add("Line " + currentLine + " has invalid data: " + reason);
										break;
									}
									Particles.Add(tech);
								} break;
							case "torpedo":
								{
									Torpedo tech = new Torpedo();
									if (!tech.Load(items, out reason))
									{
										invalidTechs.Add("Line " + currentLine + " has invalid data: " + reason);
										break;
									}
									Torpedoes.Add(tech);
								} break;
							case "missile":
								{
									Missile tech = new Missile();
									if (!tech.Load(items, out reason))
									{
										invalidTechs.Add("Line " + currentLine + " has invalid data: " + reason);
										break;
									}
									Missiles.Add(tech);
								} break;
							case "armor":
								{
									Armor tech = new Armor();
									if (!tech.Load(items, out reason))
									{
										invalidTechs.Add("Line " + currentLine + " has invalid data: " + reason);
										break;
									}
									Armors.Add(tech);
								} break;
							case "shield":
								{
									Shield tech = new Shield();
									if (!tech.Load(items, out reason))
									{
										invalidTechs.Add("Line " + currentLine + " has invalid data: " + reason);
										break;
									}
									Shields.Add(tech);
								} break;
							case "engine":
								{
									Engine tech = new Engine();
									if (!tech.Load(items, out reason))
									{
										invalidTechs.Add("Line " + currentLine + " has invalid data: " + reason);
										break;
									}
									Engines.Add(tech);
								} break;
							case "computer":
								{
									Computer tech = new Computer();
									if (!tech.Load(items, out reason))
									{
										invalidTechs.Add("Line " + currentLine + " has invalid data: " + reason);
										break;
									}
									Computers.Add(tech);
								} break;
							case "infrastructure":
								{
									Infrastructure tech = new Infrastructure();
									if (!tech.Load(items, out reason))
									{
										invalidTechs.Add("Line " + currentLine + " has invalid data: " + reason);
										break;
									}
									Infrastructures.Add(tech);
								} break;
							case "bomb":
								{
									Bomb tech = new Bomb();
									if (!tech.Load(items, out reason))
									{
										invalidTechs.Add("Line " + currentLine + " has invalid data: " + reason);
										break;
									}
									Bombs.Add(tech);
								} break;
							default:
								{
									invalidTechs.Add("Line " + currentLine + " techType tag have invalid value.");
								} break;
						}
						currentLine++;
					}
					UpdateVisibleTechs();
					return true;
				}
			}
			catch (Exception e)
			{
				MessageBox.Show(e.Message, "Exception!");
			}
			return false;
		}

		private void UpdateVisibleTechs()
		{
			UpdateTechFieldLevels();

			VisibleEngines.Clear();
			foreach (Engine engine in Engines)
			{
				if (EngineLevel >= engine.GetRequiredLevel())
				{
					VisibleEngines.Add(engine);
				}
			}

			VisibleComputers.Clear();
			foreach (Computer computer in Computers)
			{
				if (ComputerLevel >= computer.GetRequiredLevel())
				{
					VisibleComputers.Add(computer);
				}
			}

			VisibleArmors.Clear();
			foreach (Armor armor in Armors)
			{
				if (ArmorLevel >= armor.GetRequiredLevel())
				{
					VisibleArmors.Add(armor);
				}
			}

			VisibleShields.Clear();
			foreach (Shield shield in Shields)
			{
				if (ShieldLevel >= shield.GetRequiredLevel())
				{
					VisibleShields.Add(shield);
				}
			}

			VisibleInfrastructures.Clear();
			foreach (Infrastructure infrastructure in Infrastructures)
			{
				if (InfrastructureLevel >= infrastructure.GetRequiredLevel())
				{
					VisibleInfrastructures.Add(infrastructure);
				}
			}

			VisibleBeams.Clear();
			foreach (Beam beam in Beams)
			{
				if (BeamLevel >= beam.GetRequiredLevel())
				{
					VisibleBeams.Add(beam);
				}
			}

			VisibleParticles.Clear();
			foreach (Particle particle in Particles)
			{
				if (ParticleLevel >= particle.GetRequiredLevel())
				{
					VisibleParticles.Add(particle);
				}
			}

			VisibleMissiles.Clear();
			foreach (Missile missile in Missiles)
			{
				if (MissileLevel >= missile.GetRequiredLevel())
				{
					VisibleMissiles.Add(missile);
				}
			}

			VisibleTorpedoes.Clear();
			foreach (Torpedo torpedo in Torpedoes)
			{
				if (TorpedoLevel >= torpedo.GetRequiredLevel())
				{
					VisibleTorpedoes.Add(torpedo);
				}
			}

			VisibleBombs.Clear();
			foreach (Bomb bomb in Bombs)
			{
				if (BombLevel >= bomb.GetRequiredLevel())
				{
					VisibleBombs.Add(bomb);
				}
			}
		}

		private void UpdateTechFieldLevels()
		{
			EngineLevel = 0;
			foreach (Engine engine in Engines)
			{
				EngineLevel += engine.GetLevel();
			}

			ShieldLevel = 0;
			foreach (Shield shield in Shields)
			{
				ShieldLevel += shield.GetLevel();
			}

			ArmorLevel = 0;
			foreach (Armor armor in Armors)
			{
				ArmorLevel += armor.GetLevel();
			}

			ComputerLevel = 0;
			foreach (Computer computer in Computers)
			{
				ComputerLevel += computer.GetLevel();
			}

			InfrastructureLevel = 0;
			foreach (Infrastructure infrastructure in Infrastructures)
			{
				InfrastructureLevel += infrastructure.GetLevel();
			}

			BeamLevel = 0;
			foreach (Beam beam in Beams)
			{
				BeamLevel += beam.GetLevel();
			}

			ParticleLevel = 0;
			foreach (Particle particle in Particles)
			{
				ParticleLevel += particle.GetLevel();
			}

			MissileLevel = 0;
			foreach (Missile missile in Missiles)
			{
				MissileLevel += missile.GetLevel();
			}

			TorpedoLevel = 0;
			foreach (Torpedo torpedo in Torpedoes)
			{
				TorpedoLevel += torpedo.GetLevel();
			}

			BombLevel = 0;
			foreach (Bomb bomb in Bombs)
			{
				BombLevel += bomb.GetLevel();
			}
		}

		public void ProcessResearchTurn(float researchPoints, SitRepManager sitRepManager)
		{
			bool update = false;
			if (VisibleEngines[WhichEngineBeingResearched].UpdateResearch(researchPoints * (EnginePercentage * 0.01f)))
			{
				sitRepManager.AddItem(new SitRepItem(Screen.Research, null, null, new Point(), VisibleEngines[WhichEngineBeingResearched].GetName() +
					(VisibleEngines[WhichEngineBeingResearched].GetLevel() < 2 ? string.Empty : (" " + Utility.ConvertNumberToRomanNumberical(VisibleEngines[WhichEngineBeingResearched].GetLevel())))
					+ " has been researched."));
				update = true;
			}
			if (VisibleArmors[WhichArmorBeingResearched].UpdateResearch(researchPoints * (ArmorPercentage * 0.01f)))
			{
				sitRepManager.AddItem(new SitRepItem(Screen.Research, null, null, new Point(), VisibleArmors[WhichArmorBeingResearched].GetName() +
					(VisibleArmors[WhichArmorBeingResearched].GetLevel() < 2 ? string.Empty : (" " + Utility.ConvertNumberToRomanNumberical(VisibleArmors[WhichArmorBeingResearched].GetLevel())))
					+ " has been researched."));
				update = true;
			}
			if (VisibleShields[WhichShieldBeingResearched].UpdateResearch(researchPoints * (ShieldPercentage * 0.01f)))
			{
				sitRepManager.AddItem(new SitRepItem(Screen.Research, null, null, new Point(), VisibleShields[WhichShieldBeingResearched].GetName() +
					(VisibleShields[WhichShieldBeingResearched].GetLevel() < 2 ? string.Empty : (" " + Utility.ConvertNumberToRomanNumberical(VisibleShields[WhichShieldBeingResearched].GetLevel())))
					+ " has been researched."));
				update = true;
			}
			if (VisibleComputers[WhichComputerBeingResearched].UpdateResearch(researchPoints * (ComputerPercentage * 0.01f)))
			{
				sitRepManager.AddItem(new SitRepItem(Screen.Research, null, null, new Point(), VisibleComputers[WhichComputerBeingResearched].GetName() +
					(VisibleComputers[WhichComputerBeingResearched].GetLevel() < 2 ? string.Empty : (" " + Utility.ConvertNumberToRomanNumberical(VisibleComputers[WhichComputerBeingResearched].GetLevel())))
					+ " has been researched."));
				update = true;
			}
			if (VisibleInfrastructures[WhichInfrastructureBeingResearched].UpdateResearch(researchPoints * (InfrastructurePercentage * 0.01f)))
			{
				sitRepManager.AddItem(new SitRepItem(Screen.Research, null, null, new Point(), VisibleInfrastructures[WhichInfrastructureBeingResearched].GetName() +
					(VisibleInfrastructures[WhichInfrastructureBeingResearched].GetLevel() < 2 ? string.Empty : (" " + Utility.ConvertNumberToRomanNumberical(VisibleInfrastructures[WhichInfrastructureBeingResearched].GetLevel())))
					+ " has been researched."));
				update = true;
			}
			if (VisibleBeams[WhichBeamBeingResearched].UpdateResearch(researchPoints * (BeamPercentage * 0.01f)))
			{
				sitRepManager.AddItem(new SitRepItem(Screen.Research, null, null, new Point(), VisibleBeams[WhichBeamBeingResearched].GetName() +
					(VisibleBeams[WhichBeamBeingResearched].GetLevel() < 2 ? string.Empty : (" " + Utility.ConvertNumberToRomanNumberical(VisibleBeams[WhichBeamBeingResearched].GetLevel())))
					+ " has been researched."));
				update = true;
			}
			if (VisibleParticles[WhichParticleBeingResearched].UpdateResearch(researchPoints * (ParticlePercentage * 0.01f)))
			{
				sitRepManager.AddItem(new SitRepItem(Screen.Research, null, null, new Point(), VisibleParticles[WhichParticleBeingResearched].GetName() +
					(VisibleParticles[WhichParticleBeingResearched].GetLevel() < 2 ? string.Empty : (" " + Utility.ConvertNumberToRomanNumberical(VisibleParticles[WhichParticleBeingResearched].GetLevel())))
					+ " has been researched."));
				update = true;
			}
			if (VisibleMissiles[WhichMissileBeingResearched].UpdateResearch(researchPoints * (MissilePercentage * 0.01f)))
			{
				sitRepManager.AddItem(new SitRepItem(Screen.Research, null, null, new Point(), VisibleMissiles[WhichMissileBeingResearched].GetName() +
					(VisibleMissiles[WhichMissileBeingResearched].GetLevel() < 2 ? string.Empty : (" " + Utility.ConvertNumberToRomanNumberical(VisibleMissiles[WhichMissileBeingResearched].GetLevel())))
					+ " has been researched."));
				update = true;
			}
			if (VisibleTorpedoes[WhichTorpedoBeingResearched].UpdateResearch(researchPoints * (TorpedoPercentage * 0.01f)))
			{
				sitRepManager.AddItem(new SitRepItem(Screen.Research, null, null, new Point(), VisibleTorpedoes[WhichTorpedoBeingResearched].GetName() +
					(VisibleTorpedoes[WhichTorpedoBeingResearched].GetLevel() < 2 ? string.Empty : (" " + Utility.ConvertNumberToRomanNumberical(VisibleTorpedoes[WhichTorpedoBeingResearched].GetLevel())))
					+ " has been researched."));
				update = true;
			}
			if (VisibleBombs[WhichBombBeingResearched].UpdateResearch(researchPoints * (BombPercentage * 0.01f)))
			{
				sitRepManager.AddItem(new SitRepItem(Screen.Research, null, null, new Point(), VisibleBombs[WhichBombBeingResearched].GetName() +
					(VisibleBombs[WhichBombBeingResearched].GetLevel() < 2 ? string.Empty : (" " + Utility.ConvertNumberToRomanNumberical(VisibleBombs[WhichBombBeingResearched].GetLevel())))
					+ " has been researched."));
				update = true;
			}

			if (update)
			{
				UpdateVisibleTechs();
			}
		}

		public void SetPercentage(TechField whichField, int amount)
		{
			int remainingPercentile = 100;
			if (BeamLocked)
			{
				remainingPercentile -= BeamPercentage;
			}
			if (ParticleLocked)
			{
				remainingPercentile -= ParticlePercentage;
			}
			if (MissileLocked)
			{
				remainingPercentile -= MissilePercentage;
			}
			if (TorpedoLocked)
			{
				remainingPercentile -= TorpedoPercentage;
			}
			if (BombLocked)
			{
				remainingPercentile -= BombPercentage;
			}
			if (EngineLocked)
			{
				remainingPercentile -= EnginePercentage;
			}
			if (ShieldLocked)
			{
				remainingPercentile -= ShieldPercentage;
			}
			if (ArmorLocked)
			{
				remainingPercentile -= ArmorPercentage;
			}
			if (ComputerLocked)
			{
				remainingPercentile -= ComputerPercentage;
			}
			if (InfrastructureLocked)
			{
				remainingPercentile -= InfrastructurePercentage;
			}

			if (amount >= remainingPercentile)
			{
				if (!BeamLocked)
				{
					BeamPercentage = 0;
				}
				if (!ParticleLocked)
				{
					ParticlePercentage = 0;
				}
				if (!MissileLocked)
				{
					MissilePercentage = 0;
				}
				if (!TorpedoLocked)
				{
					TorpedoPercentage = 0;
				}
				if (!BombLocked)
				{
					BombPercentage = 0;
				}
				if (!EngineLocked)
				{
					EnginePercentage = 0;
				}
				if (!ShieldLocked)
				{
					ShieldPercentage = 0;
				}
				if (!ArmorLocked)
				{
					ArmorPercentage = 0;
				}
				if (!ComputerLocked)
				{
					ComputerPercentage = 0;
				}
				if (!InfrastructureLocked)
				{
					InfrastructurePercentage = 0;
				}
				amount = remainingPercentile;
			}

			//Now scale
			int totalPointsExcludingSelectedType = 0;
			switch (whichField)
			{
				case TechField.ARMOR:
					{
						ArmorPercentage = amount;
						remainingPercentile -= ArmorPercentage;
						totalPointsExcludingSelectedType = GetTotalPercentageExcludingTypeAndLocked(TechField.ARMOR);
					} break;
				case TechField.BEAM:
					{
						BeamPercentage = amount;
						remainingPercentile -= BeamPercentage;
						totalPointsExcludingSelectedType = GetTotalPercentageExcludingTypeAndLocked(TechField.BEAM);
					} break;
				case TechField.BOMB:
					{
						BombPercentage = amount;
						remainingPercentile -= BombPercentage;
						totalPointsExcludingSelectedType = GetTotalPercentageExcludingTypeAndLocked(TechField.BOMB);
					} break;
				case TechField.COMPUTER:
					{
						ComputerPercentage = amount;
						remainingPercentile -= ComputerPercentage;
						totalPointsExcludingSelectedType = GetTotalPercentageExcludingTypeAndLocked(TechField.COMPUTER);
					} break;
				case TechField.ENGINE:
					{
						EnginePercentage = amount;
						remainingPercentile -= EnginePercentage;
						totalPointsExcludingSelectedType = GetTotalPercentageExcludingTypeAndLocked(TechField.ENGINE);
					} break;
				case TechField.INFRASTRUCTURE:
					{
						InfrastructurePercentage = amount;
						remainingPercentile -= InfrastructurePercentage;
						totalPointsExcludingSelectedType = GetTotalPercentageExcludingTypeAndLocked(TechField.INFRASTRUCTURE);
					} break;
				case TechField.MISSILE:
					{
						MissilePercentage = amount;
						remainingPercentile -= MissilePercentage;
						totalPointsExcludingSelectedType = GetTotalPercentageExcludingTypeAndLocked(TechField.MISSILE);
					} break;
				case TechField.PARTICLE:
					{
						ParticlePercentage = amount;
						remainingPercentile -= ParticlePercentage;
						totalPointsExcludingSelectedType = GetTotalPercentageExcludingTypeAndLocked(TechField.PARTICLE);
					} break;
				case TechField.SHIELD:
					{
						ShieldPercentage = amount;
						remainingPercentile -= ShieldPercentage;
						totalPointsExcludingSelectedType = GetTotalPercentageExcludingTypeAndLocked(TechField.SHIELD);
					} break;
				case TechField.TORPEDO:
					{
						TorpedoPercentage = amount;
						remainingPercentile -= TorpedoPercentage;
						totalPointsExcludingSelectedType = GetTotalPercentageExcludingTypeAndLocked(TechField.TORPEDO);
					} break;
			}

			if (remainingPercentile < totalPointsExcludingSelectedType)
			{
				int amountToDeduct = totalPointsExcludingSelectedType - remainingPercentile;
				int prevValue;
				if (!InfrastructureLocked && whichField != TechField.INFRASTRUCTURE)
				{
					prevValue = InfrastructurePercentage;
					InfrastructurePercentage -= (InfrastructurePercentage >= amountToDeduct ? amountToDeduct : InfrastructurePercentage);
					amountToDeduct -= (prevValue - InfrastructurePercentage);
				}
				if (amountToDeduct > 0)
				{
					if (!EngineLocked && whichField != TechField.ENGINE)
					{
						prevValue = EnginePercentage;
						EnginePercentage -= (EnginePercentage >= amountToDeduct ? amountToDeduct : EnginePercentage);
						amountToDeduct -= (prevValue - EnginePercentage);
					}
				}
				if (amountToDeduct > 0)
				{
					if (!ShieldLocked && whichField != TechField.SHIELD)
					{
						prevValue = ShieldPercentage;
						ShieldPercentage -= (ShieldPercentage >= amountToDeduct ? amountToDeduct : ShieldPercentage);
						amountToDeduct -= (prevValue - ShieldPercentage);
					}
				}
				if (amountToDeduct > 0)
				{
					if (!ArmorLocked && whichField != TechField.ARMOR)
					{
						prevValue = ArmorPercentage;
						ArmorPercentage -= (ArmorPercentage >= amountToDeduct ? amountToDeduct : ArmorPercentage);
						amountToDeduct -= (prevValue - ArmorPercentage);
					}
				}
				if (amountToDeduct > 0)
				{
					if (!ComputerLocked && whichField != TechField.COMPUTER)
					{
						prevValue = ComputerPercentage;
						ComputerPercentage -= (ComputerPercentage >= amountToDeduct ? amountToDeduct : ComputerPercentage);
						amountToDeduct -= (prevValue - ComputerPercentage);
					}
				}
				if (amountToDeduct > 0)
				{
					if (!BombLocked && whichField != TechField.BOMB)
					{
						prevValue = BombPercentage;
						BombPercentage -= (BombPercentage >= amountToDeduct ? amountToDeduct : BombPercentage);
						amountToDeduct -= (prevValue - BombPercentage);
					}
				}
				if (amountToDeduct > 0)
				{
					if (!BeamLocked && whichField != TechField.BEAM)
					{
						prevValue = BeamPercentage;
						BeamPercentage -= (BeamPercentage >= amountToDeduct ? amountToDeduct : BeamPercentage);
						amountToDeduct -= (prevValue - BeamPercentage);
					}
				}
				if (amountToDeduct > 0)
				{
					if (!ParticleLocked && whichField != TechField.PARTICLE)
					{
						prevValue = ParticlePercentage;
						ParticlePercentage -= (ParticlePercentage >= amountToDeduct ? amountToDeduct : ParticlePercentage);
						amountToDeduct -= (prevValue - ParticlePercentage);
					}
				}
				if (amountToDeduct > 0)
				{
					if (!MissileLocked && whichField != TechField.MISSILE)
					{
						prevValue = MissilePercentage;
						MissilePercentage -= (MissilePercentage >= amountToDeduct ? amountToDeduct : MissilePercentage);
						amountToDeduct -= (prevValue - MissilePercentage);
					}
				}
				if (amountToDeduct > 0)
				{
					if (!TorpedoLocked && whichField != TechField.TORPEDO)
					{
						prevValue = TorpedoPercentage;
						TorpedoPercentage -= (TorpedoPercentage >= amountToDeduct ? amountToDeduct : TorpedoPercentage);
						amountToDeduct -= (prevValue - TorpedoPercentage);
					}
				}
			}

			if (remainingPercentile > totalPointsExcludingSelectedType) //excess points needed to allocate
			{
				int amountToAdd = remainingPercentile - totalPointsExcludingSelectedType;
				if (!TorpedoLocked && whichField != TechField.TORPEDO)
				{
					TorpedoPercentage += amountToAdd;
					amountToAdd = 0;
				}
				if (amountToAdd > 0)
				{
					if (!MissileLocked && whichField != TechField.MISSILE)
					{
						MissilePercentage += amountToAdd;
						amountToAdd = 0;
					}
				}
				if (amountToAdd > 0)
				{
					if (!ParticleLocked && whichField != TechField.PARTICLE)
					{
						ParticlePercentage += amountToAdd;
						amountToAdd = 0;
					}
				}
				if (amountToAdd > 0)
				{
					if (!BeamLocked && whichField != TechField.BEAM)
					{
						BeamPercentage += amountToAdd;
						amountToAdd = 0;
					}
				}
				if (amountToAdd > 0)
				{
					if (!BombLocked && whichField != TechField.BOMB)
					{
						BombPercentage += amountToAdd;
						amountToAdd = 0;
					}
				}
				if (amountToAdd > 0)
				{
					if (!ComputerLocked && whichField != TechField.COMPUTER)
					{
						ComputerPercentage += amountToAdd;
						amountToAdd = 0;
					}
				}
				if (amountToAdd > 0)
				{
					if (!ArmorLocked && whichField != TechField.ARMOR)
					{
						ArmorPercentage += amountToAdd;
						amountToAdd = 0;
					}
				}
				if (amountToAdd > 0)
				{
					if (!ShieldLocked && whichField != TechField.SHIELD)
					{
						ShieldPercentage += amountToAdd;
						amountToAdd = 0;
					}
				}
				if (amountToAdd > 0)
				{
					if (!EngineLocked && whichField != TechField.ENGINE)
					{
						EnginePercentage += amountToAdd;
						amountToAdd = 0;
					}
				}
				if (amountToAdd > 0)
				{
					if (!InfrastructureLocked && whichField != TechField.INFRASTRUCTURE)
					{
						InfrastructurePercentage += amountToAdd;
						amountToAdd = 0;
					}
				}
				if (amountToAdd > 0)
				{
					//All fields are already checked, so have to add the remaining back to the current tech field
					switch (whichField)
					{
						case TechField.ARMOR:
							ArmorPercentage += amountToAdd;
							break;
						case TechField.BEAM:
							BeamPercentage += amountToAdd;
							break;
						case TechField.BOMB:
							BombPercentage += amountToAdd;
							break;
						case TechField.COMPUTER:
							ComputerPercentage += amountToAdd;
							break;
						case TechField.ENGINE:
							EnginePercentage += amountToAdd;
							break;
						case TechField.INFRASTRUCTURE:
							InfrastructurePercentage += amountToAdd;
							break;
						case TechField.MISSILE:
							MissilePercentage += amountToAdd;
							break;
						case TechField.PARTICLE:
							ParticlePercentage += amountToAdd;
							break;
						case TechField.SHIELD:
							ShieldPercentage += amountToAdd;
							break;
						case TechField.TORPEDO:
							TorpedoPercentage += amountToAdd;
							break;
					}
				}
			}
		}

		private int GetTotalPercentageExcludingTypeAndLocked(TechField techField)
		{
			int total = 0;

			if (!BeamLocked && techField != TechField.BEAM)
			{
				total += BeamPercentage;
			}
			if (!ParticleLocked && techField != TechField.PARTICLE)
			{
				total += ParticlePercentage;
			}
			if (!BombLocked && techField != TechField.BOMB)
			{
				total += BombPercentage;
			}
			if (!MissileLocked && techField != TechField.MISSILE)
			{
				total += MissilePercentage;
			}
			if (!TorpedoLocked && techField != TechField.TORPEDO)
			{
				total += TorpedoPercentage;
			}
			if (!ComputerLocked && techField != TechField.COMPUTER)
			{
				total += ComputerPercentage;
			}
			if (!EngineLocked && techField != TechField.ENGINE)
			{
				total += EnginePercentage;
			}
			if (!ArmorLocked && techField != TechField.ARMOR)
			{
				total += ArmorPercentage;
			}
			if (!ShieldLocked && techField != TechField.SHIELD)
			{
				total += ShieldPercentage;
			}
			if (!InfrastructureLocked && techField != TechField.INFRASTRUCTURE)
			{
				total += InfrastructurePercentage;
			}

			return total;
		}
		#endregion
	}
}
