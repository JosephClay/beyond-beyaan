using System;
using Beyond_Beyaan.Data_Modules;
using Beyond_Beyaan.Data_Managers;

namespace Beyond_Beyaan.Screens
{
	public class SystemView : WindowInterface
	{
		private BBSingleLineTextBox _name;
		private BBStretchableImage _infrastructureBackground;
		private BBStretchableImage _researchBackground;
		private BBStretchableImage _environmentBackground;
		private BBStretchableImage _defenseBackground;
		private BBStretchableImage _constructionBackground;

		private BBScrollBar _infrastructureSlider;
		private BBScrollBar _researchSlider;
		private BBScrollBar _environmentSlider;
		private BBScrollBar _defenseSlider;
		private BBScrollBar _constructionSlider;

		private BBButton _infrastructureLockButton;
		private BBButton _researchLockButton;
		private BBButton _environmentLockButton;
		private BBButton _defenseLockButton;
		private BBButton _constructionLockButton;

		private BBLabel _infrastructureLabel;
		private BBLabel _researchLabel;
		private BBLabel _environmentLabel;
		private BBLabel _defenseLabel;
		private BBLabel _constructionLabel;

		private BBLabel _terrainLabel;
		private BBLabel _popLabel;
		private BBLabel _productionLabel;

		private BBSprite _infrastructureIcon;
		private BBSprite _defenseIcon;
		private BBSprite _researchIcon;
		private BBSprite _environmentIcon;
		private BBSprite _constructionIcon;
		private StarSystem currentSystem;

		private bool _isOwnedSystem; //To determine whether or not to display/handle sliders
		private bool _isExplored;

		#region Constructor
		public bool Initialize(GameMain gameMain, Random r, out string reason)
		{
			this.gameMain = gameMain;
			_isExplored = false;
			_isOwnedSystem = false;
			if (!base.Initialize(gameMain.ScreenWidth - 300, gameMain.ScreenHeight / 2 - 220, 300, 440, gameMain, true, r, out reason))
			{
				return false;
			}
			if (!backGroundImage.Initialize(gameMain.ScreenWidth - 300, gameMain.ScreenHeight / 2 - 220, 300, 440, StretchableImageType.ThinBorder, r, out reason))
			{
				return false;
			}
			_infrastructureIcon = SpriteManager.GetSprite("InfrastructureIcon", r);
			_defenseIcon = SpriteManager.GetSprite("MilitaryIcon", r);
			_researchIcon = SpriteManager.GetSprite("ResearchIcon", r);
			_environmentIcon = SpriteManager.GetSprite("EnvironmentIcon", r);
			_constructionIcon = SpriteManager.GetSprite("ConstructionIcon", r);

			if (_infrastructureIcon == null || _defenseIcon == null || _researchIcon == null || _environmentIcon == null || _constructionIcon == null)
			{
				reason = "One or more of the following sprites does not exist: InfrastructureIcon, MilitaryIcon, ResearchIcon, EnvironmentIcon, and/or ConstructionIcon";
				return false;
			}

			_name = new BBSingleLineTextBox();
			if (!_name.Initialize(string.Empty, xPos + 10, yPos + 15, 280, 35, false, r, out reason))
			{
				return false;
			}
			_infrastructureBackground = new BBStretchableImage();
			_researchBackground = new BBStretchableImage();
			_environmentBackground = new BBStretchableImage();
			_defenseBackground = new BBStretchableImage();
			_constructionBackground = new BBStretchableImage();
			_popLabel = new BBLabel();
			_terrainLabel = new BBLabel();
			_productionLabel = new BBLabel();

			_infrastructureLabel = new BBLabel();
			_researchLabel = new BBLabel();
			_environmentLabel = new BBLabel();
			_defenseLabel = new BBLabel();
			_constructionLabel = new BBLabel();

			_infrastructureSlider = new BBScrollBar();
			_researchSlider = new BBScrollBar();
			_environmentSlider = new BBScrollBar();
			_defenseSlider = new BBScrollBar();
			_constructionSlider = new BBScrollBar();

			_infrastructureLockButton = new BBButton();
			_researchLockButton = new BBButton();
			_environmentLockButton = new BBButton();
			_defenseLockButton = new BBButton();
			_constructionLockButton = new BBButton();

			if (!_infrastructureBackground.Initialize(xPos + 10, yPos + 130, 280, 60, StretchableImageType.ThinBorder, r, out reason))
			{
				return false;
			}
			if (!_researchBackground.Initialize(xPos + 10, yPos + 190, 280, 60, StretchableImageType.ThinBorder, r, out reason))
			{
				return false;
			}
			if (!_environmentBackground.Initialize(xPos + 10, yPos + 250, 280, 60, StretchableImageType.ThinBorder, r, out reason))
			{
				return false;
			}
			if (!_defenseBackground.Initialize(xPos + 10, yPos + 310, 280, 60, StretchableImageType.ThinBorder, r, out reason))
			{
				return false;
			}
			if (!_constructionBackground.Initialize(xPos + 10, yPos + 370, 280, 60, StretchableImageType.ThinBorder, r, out reason))
			{
				return false;
			}
			if (!_terrainLabel.Initialize(xPos + 55, yPos + 60, string.Empty, System.Drawing.Color.White, out reason))
			{
				return false;
			}
			if (!_popLabel.Initialize(xPos + 55, yPos + 80, string.Empty, System.Drawing.Color.White, out reason))
			{
				return false;
			}
			if (!_productionLabel.Initialize(xPos + 55, yPos + 100, string.Empty, System.Drawing.Color.White, out reason))
			{
				return false;
			}

			if (!_infrastructureLabel.Initialize(xPos + 65, yPos + 140, string.Empty, System.Drawing.Color.White, out reason))
			{
				return false;
			}
			if (!_infrastructureSlider.Initialize(xPos + 65, yPos + 160, 200, 1, 100, true, true, r, out reason))
			{
				return false;
			}
			if (!_infrastructureLockButton.Initialize("LockBG", "LockFG", string.Empty, xPos + 267, yPos + 160, 16, 16, r, out reason))
			{
				return false;
			}
			if (!_researchLabel.Initialize(xPos + 65, yPos + 200, string.Empty, System.Drawing.Color.White, out reason))
			{
				return false;
			}
			if (!_researchSlider.Initialize(xPos + 65, yPos + 220, 200, 1, 100, true, true, r, out reason))
			{
				return false;
			}
			if (!_researchLockButton.Initialize("LockBG", "LockFG", string.Empty, xPos + 267, yPos + 220, 16, 16, r, out reason))
			{
				return false;
			}
			if (!_environmentLabel.Initialize(xPos + 65, yPos + 260, string.Empty, System.Drawing.Color.White, out reason))
			{
				return false;
			}
			if (!_environmentSlider.Initialize(xPos + 65, yPos + 280, 200, 1, 100, true, true, r, out reason))
			{
				return false;
			}
			if (!_environmentLockButton.Initialize("LockBG", "LockFG", string.Empty, xPos + 267, yPos + 280, 16, 16, r, out reason))
			{
				return false;
			}
			if (!_defenseLabel.Initialize(xPos + 65, yPos + 320, string.Empty, System.Drawing.Color.White, out reason))
			{
				return false;
			}
			if (!_defenseSlider.Initialize(xPos + 65, yPos + 340, 200, 1, 100, true, true, r, out reason))
			{
				return false;
			}
			if (!_defenseLockButton.Initialize("LockBG", "LockFG", string.Empty, xPos + 267, yPos + 340, 16, 16, r, out reason))
			{
				return false;
			}
			if (!_constructionLabel.Initialize(xPos + 65, yPos + 380, string.Empty, System.Drawing.Color.White, out reason))
			{
				return false;
			}
			if (!_constructionSlider.Initialize(xPos + 65, yPos + 400, 200, 1, 100, true, true, r, out reason))
			{
				return false;
			}
			if (!_constructionLockButton.Initialize("LockBG", "LockFG", string.Empty, xPos + 267, yPos + 400, 16, 16, r, out reason))
			{
				return false;
			}

			reason = null;
			return true;
		}
		#endregion

		public void LoadSystem()
		{
			currentSystem = gameMain.EmpireManager.CurrentEmpire.SelectedSystem;
			if (currentSystem.IsThisSystemExploredByEmpire(gameMain.EmpireManager.CurrentEmpire))
			{
				_isExplored = true;
				var planet = currentSystem.Planets[0];
				_name.SetString(currentSystem.Name);
				_isOwnedSystem = currentSystem.Planets[0].Owner != null;
				_name.SetColor(_isOwnedSystem ? currentSystem.Planets[0].Owner.EmpireColor : System.Drawing.Color.White);
				_popLabel.SetText(_isOwnedSystem ? string.Format("{0}/{1} M", (int)currentSystem.Planets[0].TotalPopulation, currentSystem.Planets[0].PopulationMax) : string.Format("{0} M", currentSystem.Planets[0].PopulationMax));
				_terrainLabel.SetText(Utility.PlanetTypeToString(currentSystem.Planets[0].PlanetType));
				_productionLabel.SetText(_isOwnedSystem ? string.Format("{0:0.0} ({1:0.0}) Production", currentSystem.Planets[0].ActualProduction, currentSystem.Planets[0].TotalProduction) : "Unknown");
				_infrastructureLabel.SetText(_isOwnedSystem ? currentSystem.Planets[0].InfrastructureStringOutput : "Unknown");
				_researchLabel.SetText(_isOwnedSystem ? currentSystem.Planets[0].ResearchStringOutput : "Unknown");
				_environmentLabel.SetText(_isOwnedSystem ? currentSystem.Planets[0].EnvironmentStringOutput : "Unknown");
				_defenseLabel.SetText(_isOwnedSystem ? currentSystem.Planets[0].DefenseStringOutput : "Unknown");
				_constructionLabel.SetText(_isOwnedSystem ? currentSystem.Planets[0].ConstructionStringOutput : "Unknown");
				_infrastructureSlider.TopIndex = planet.InfrastructureAmount;
				_researchSlider.TopIndex = planet.ResearchAmount;
				_environmentSlider.TopIndex = planet.EnvironmentAmount;
				_defenseSlider.TopIndex = planet.DefenseAmount;
				_constructionSlider.TopIndex = planet.ConstructionAmount;
				
				_infrastructureLockButton.Selected = planet.InfrastructureLocked;
				_infrastructureSlider.SetEnabledState(!planet.InfrastructureLocked);
				_researchLockButton.Selected = planet.ResearchLocked;
				_researchSlider.SetEnabledState(!planet.ResearchLocked);
				_environmentLockButton.Selected = planet.EnvironmentLocked;
				_environmentSlider.SetEnabledState(!planet.EnvironmentLocked);
				_defenseLockButton.Selected = planet.DefenseLocked;
				_defenseSlider.SetEnabledState(!planet.DefenseLocked);
				_constructionLockButton.Selected = planet.ConstructionLocked;
				_constructionSlider.SetEnabledState(!planet.ConstructionLocked);
			}
			else
			{
				_isExplored = false;
				_name.SetString("Unexplored");
				_name.SetColor(System.Drawing.Color.White);
				_popLabel.SetText(string.Empty);
				_terrainLabel.SetText(string.Empty);
				_productionLabel.SetText(string.Empty);
				_infrastructureLabel.SetText(string.Empty);
				_researchLabel.SetText(string.Empty);
				_environmentLabel.SetText(string.Empty);
				_defenseLabel.SetText(string.Empty);
				_constructionLabel.SetText(string.Empty);
			}
			if (currentSystem.Planets[0].Owner == gameMain.EmpireManager.CurrentEmpire)
			{
				_name.SetReadOnly(false);
			}
			else
			{
				_name.SetReadOnly(true);
			}
		}

		public override void Draw()
		{
			base.Draw();
			_name.Draw();
			if (_isExplored)
			{
				_infrastructureBackground.Draw();
				_researchBackground.Draw();
				_environmentBackground.Draw();
				_defenseBackground.Draw();
				_constructionBackground.Draw();
				currentSystem.Planets[0].SmallSprite.Draw(xPos + 10, yPos + 60);
				_infrastructureIcon.Draw(xPos + 20, yPos + 140);
				_researchIcon.Draw(xPos + 20, yPos + 200);
				_environmentIcon.Draw(xPos + 20, yPos + 260);
				_defenseIcon.Draw(xPos + 20, yPos + 320);
				_constructionIcon.Draw(xPos + 20, yPos + 380);
				_popLabel.Draw();
				_terrainLabel.Draw();
				_productionLabel.Draw();
				if (_isOwnedSystem)
				{
					_infrastructureLabel.Draw();
					_infrastructureSlider.Draw();
					_infrastructureLockButton.Draw();
					_researchLabel.Draw();
					_researchSlider.Draw();
					_researchLockButton.Draw();
					_environmentLabel.Draw();
					_environmentSlider.Draw();
					_environmentLockButton.Draw();
					_defenseLabel.Draw();
					_defenseSlider.Draw();
					_defenseLockButton.Draw();
					_constructionLabel.Draw();
					_constructionSlider.Draw();
					_constructionLockButton.Draw();
				}
			}
		}

		public override void MoveWindow()
		{
			base.MoveWindow();
			_name.MoveTo(xPos + 10, yPos + 15);
			_terrainLabel.Move(xPos + 55, yPos + 60);
			_popLabel.Move(xPos + 55, yPos + 80);
			_infrastructureBackground.MoveTo(xPos + 10, yPos + 130);
			_researchBackground.MoveTo(xPos + 10, yPos + 190);
			_environmentBackground.MoveTo(xPos + 10, yPos + 250);
			_defenseBackground.MoveTo(xPos + 10, yPos + 310);
			_constructionBackground.MoveTo(xPos + 10, yPos + 370);
			_productionLabel.Move(xPos + 55, yPos + 100);
			_infrastructureLabel.Move(xPos + 65, yPos + 140);
			_infrastructureSlider.MoveScrollBar(xPos + 65, yPos + 160);
			_infrastructureLockButton.MoveButton(xPos + 267, yPos + 160);
			_researchLabel.Move(xPos + 65, yPos + 200);
			_researchSlider.MoveScrollBar(xPos + 65, yPos + 220);
			_researchLockButton.MoveButton(xPos + 267, yPos + 220);
			_environmentLabel.Move(xPos + 65, yPos + 260);
			_environmentSlider.MoveScrollBar(xPos + 65, yPos + 280);
			_environmentLockButton.MoveButton(xPos + 267, yPos + 280);
			_defenseLabel.Move(xPos + 65, yPos + 320);
			_defenseSlider.MoveScrollBar(xPos + 65, yPos + 340);
			_defenseLockButton.MoveButton(xPos + 267, yPos + 340);
			_constructionLabel.Move(xPos + 65, yPos + 380);
			_constructionSlider.MoveScrollBar(xPos + 65, yPos + 400);
			_constructionLockButton.MoveButton(xPos + 267, yPos + 400);
		}

		public override bool MouseDown(int x, int y)
		{
			bool result = _name.MouseDown(x, y);
			return result || base.MouseDown(x, y);
		}

		public override bool MouseUp(int x, int y)
		{
			bool result = _name.MouseUp(x, y);
			return result || base.MouseUp(x, y);
		}

		public override bool MouseHover(int x, int y, float frameDeltaTime)
		{
			_name.Update(frameDeltaTime);
			return base.MouseHover(x, y, frameDeltaTime);
		}

		public override bool KeyDown(GorgonLibrary.InputDevices.KeyboardInputEventArgs e)
		{
			if (_name.KeyDown(e))
			{
				currentSystem.Name = _name.Text;
				return true;
			}
			return false;
		}
	}
}
