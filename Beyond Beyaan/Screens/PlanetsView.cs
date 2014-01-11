using System;
using Beyond_Beyaan.Data_Managers;
using Beyond_Beyaan.Data_Modules;

namespace Beyond_Beyaan.Screens
{
	public class PlanetsView : WindowInterface
	{
		public Action CloseWindow;

		private BBStretchableImage _infrastructureBackground;
		private BBStretchableImage _researchBackground;
		private BBStretchableImage _environmentBackground;
		private BBStretchableImage _defenseBackground;
		private BBStretchButton _constructionProjectButton;

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

		private BBSprite _infrastructureIcon;
		private BBSprite _defenseIcon;
		private BBSprite _researchIcon;
		private BBSprite _environmentIcon;
		private BBSprite _constructionIcon;

		//private StarSystem _selectedSystem;
		//private Empire _currentEmpire;

		#region Constructor
		public bool Initialize(GameMain gameMain, out string reason)
		{
			int x = (gameMain.ScreenWidth / 2) - 400;
			int y = (gameMain.ScreenHeight / 2) - 300;

			if (!base.Initialize(x, y, 800, 600, StretchableImageType.MediumBorder, gameMain, false, gameMain.Random, out reason))
			{
				return false;
			}

			_infrastructureIcon = SpriteManager.GetSprite("InfrastructureIcon", gameMain.Random);
			_defenseIcon = SpriteManager.GetSprite("MilitaryIcon", gameMain.Random);
			_researchIcon = SpriteManager.GetSprite("ResearchIcon", gameMain.Random);
			_environmentIcon = SpriteManager.GetSprite("EnvironmentIcon", gameMain.Random);
			_constructionIcon = SpriteManager.GetSprite("ConstructionIcon", gameMain.Random);
			if (_infrastructureIcon == null || _defenseIcon == null || _researchIcon == null || _environmentIcon == null || _constructionIcon == null)
			{
				reason = "One or more of the following sprites does not exist: InfrastructureIcon, MilitaryIcon, ResearchIcon, EnvironmentIcon, and/or ConstructionIcon";
				return false;
			}

			_infrastructureBackground = new BBStretchableImage();
			_researchBackground = new BBStretchableImage();
			_environmentBackground = new BBStretchableImage();
			_defenseBackground = new BBStretchableImage();
			_constructionProjectButton = new BBStretchButton();

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

			if (!_infrastructureBackground.Initialize(x + 505, y + 15, 280, 55, StretchableImageType.ThinBorderBG, gameMain.Random, out reason))
			{
				return false;
			}
			if (!_researchBackground.Initialize(x + 505, y + 70, 280, 55, StretchableImageType.ThinBorderBG, gameMain.Random, out reason))
			{
				return false;
			}
			if (!_environmentBackground.Initialize(x + 505, y + 125, 280, 55, StretchableImageType.ThinBorderBG, gameMain.Random, out reason))
			{
				return false;
			}
			if (!_defenseBackground.Initialize(x + 505, y + 180, 280, 55, StretchableImageType.ThinBorderBG, gameMain.Random, out reason))
			{
				return false;
			}
			if (!_constructionProjectButton.Initialize(string.Empty, ButtonTextAlignment.LEFT, StretchableImageType.ThinBorderBG, StretchableImageType.ThinBorderFG, x + 505, y + 235, 280, 55, gameMain.Random, out reason))
			{
				return false;
			}

			if (!_infrastructureLabel.Initialize(x + 560, y + 25, string.Empty, System.Drawing.Color.White, out reason))
			{
				return false;
			}
			if (!_infrastructureSlider.Initialize(x + 560, y + 45, 200, 1, 101, true, true, gameMain.Random, out reason))
			{
				return false;
			}
			if (!_infrastructureLockButton.Initialize("LockBG", "LockFG", string.Empty, ButtonTextAlignment.CENTER, x + 762, y + 45, 16, 16, gameMain.Random, out reason))
			{
				return false;
			}
			if (!_researchLabel.Initialize(x + 560, y + 80, string.Empty, System.Drawing.Color.White, out reason))
			{
				return false;
			}
			if (!_researchSlider.Initialize(x + 560, y + 100, 200, 1, 101, true, true, gameMain.Random, out reason))
			{
				return false;
			}
			if (!_researchLockButton.Initialize("LockBG", "LockFG", string.Empty, ButtonTextAlignment.CENTER, x + 762, y + 100, 16, 16, gameMain.Random, out reason))
			{
				return false;
			}
			if (!_environmentLabel.Initialize(x + 560, y + 135, string.Empty, System.Drawing.Color.White, out reason))
			{
				return false;
			}
			if (!_environmentSlider.Initialize(x + 560, y + 155, 200, 1, 101, true, true, gameMain.Random, out reason))
			{
				return false;
			}
			if (!_environmentLockButton.Initialize("LockBG", "LockFG", string.Empty, ButtonTextAlignment.CENTER, x + 762, y + 155, 16, 16, gameMain.Random, out reason))
			{
				return false;
			}
			if (!_defenseLabel.Initialize(x + 560, y + 190, string.Empty, System.Drawing.Color.White, out reason))
			{
				return false;
			}
			if (!_defenseSlider.Initialize(x + 560, y + 210, 200, 1, 101, true, true, gameMain.Random, out reason))
			{
				return false;
			}
			if (!_defenseLockButton.Initialize("LockBG", "LockFG", string.Empty, ButtonTextAlignment.CENTER, x + 762, y + 210, 16, 16, gameMain.Random, out reason))
			{
				return false;
			}
			if (!_constructionLabel.Initialize(x + 560, y + 245, string.Empty, System.Drawing.Color.White, out reason))
			{
				return false;
			}
			if (!_constructionSlider.Initialize(x + 560, y + 265, 200, 1, 101, true, true, gameMain.Random, out reason))
			{
				return false;
			}
			if (!_constructionLockButton.Initialize("LockBG", "LockFG", string.Empty, ButtonTextAlignment.CENTER, x + 762, y + 265, 16, 16, gameMain.Random, out reason))
			{
				return false;
			}

			return true;
		}
		#endregion

		public void Load()
		{
			
		}

		public override void Draw()
		{
			base.Draw();

			_infrastructureBackground.Draw();
			_researchBackground.Draw();
			_environmentBackground.Draw();
			_defenseBackground.Draw();
			_constructionProjectButton.Draw();
			_infrastructureIcon.Draw(_xPos + 515, _yPos + 23);
			_researchIcon.Draw(_xPos + 515, _yPos + 78);
			_environmentIcon.Draw(_xPos + 515, _yPos + 133);
			_defenseIcon.Draw(_xPos + 515, _yPos + 188);
			_constructionIcon.Draw(_xPos + 515, _yPos + 243);
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

		public override bool MouseHover(int x, int y, float frameDeltaTime)
		{
			return base.MouseHover(x, y, frameDeltaTime);
		}

		public override bool MouseDown(int x, int y)
		{
			return base.MouseDown(x, y);
		}

		public override bool MouseUp(int x, int y)
		{
			if (!base.MouseUp(x, y))
			{
				if (CloseWindow != null)
				{
					CloseWindow();
					return true;
				}
			}
			return false;
		}
	}
}
