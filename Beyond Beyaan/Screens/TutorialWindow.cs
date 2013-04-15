using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Beyond_Beyaan.Data_Managers;

namespace Beyond_Beyaan.Screens
{
	public class TutorialWindow : WindowInterface
	{
		private TextBox displayingPage;
		private List<string> pages;
		private int pageIndex;

		bool invalidTutorial; //If there's no pages at all, don't handle the tutorial

		private Label pageNumber;
		private Button prevButton;
		private Button firstButton;
		private Button lastButton;
		private Button nextButton;
		private Button closeButton;

		public TutorialWindow(GameMain gameMain, GorgonLibrary.Graphics.Font font)
			: base(0, 0, 250, 300, string.Empty, gameMain, true)
		{
			backGroundImage = new StretchableImage(0, 0, 250, 300, 30, 13, DrawingManagement.BoxBorder);
			displayingPage = new TextBox(5, 5, 240, 260, "tutorialTextBox", string.Empty, font, DrawingManagement.VerticalScrollBar);

			prevButton = new Button(SpriteName.PrevPageBG, SpriteName.PrevPageFG, string.Empty, 10, 262, 30, 30, gameMain.FontManager.GetDefaultFont());
			firstButton = new Button(SpriteName.FirstPageBG, SpriteName.FirstPageFG, string.Empty, 45, 262, 30, 30, gameMain.FontManager.GetDefaultFont());
			lastButton = new Button(SpriteName.LastPageBG, SpriteName.LastPageFG, string.Empty, 155, 262, 30, 30, gameMain.FontManager.GetDefaultFont());
			nextButton = new Button(SpriteName.NextPageBG, SpriteName.NextPageFG, string.Empty, 190, 262, 30, 30, gameMain.FontManager.GetDefaultFont());
			closeButton = new Button(SpriteName.CancelBackground, SpriteName.CancelForeground, string.Empty, 224, 272, 16, 16, gameMain.FontManager.GetDefaultFont());

			prevButton.SetToolTip(DrawingManagement.BoxBorderBG, gameMain.FontManager.GetDefaultFont(), "Previous Page", "previousPageToolTip", 30, 13, gameMain.ScreenWidth, gameMain.ScreenHeight);
			firstButton.SetToolTip(DrawingManagement.BoxBorderBG, gameMain.FontManager.GetDefaultFont(), "First Page", "firstPageToolTip", 30, 13, gameMain.ScreenWidth, gameMain.ScreenHeight);
			lastButton.SetToolTip(DrawingManagement.BoxBorderBG, gameMain.FontManager.GetDefaultFont(), "Last Page", "lastPageToolTip", 30, 13, gameMain.ScreenWidth, gameMain.ScreenHeight);
			nextButton.SetToolTip(DrawingManagement.BoxBorderBG, gameMain.FontManager.GetDefaultFont(), "Next Page", "nextPageToolTip", 30, 13, gameMain.ScreenWidth, gameMain.ScreenHeight);

			pageNumber = new Label(80, 267, gameMain.FontManager.GetDefaultFont());
		}

		public bool LoadTutorial(string filePath, out string reason)
		{
			invalidTutorial = false;
			try
			{
				XDocument file = XDocument.Load(filePath);
				XElement root = file.Element("Tutorial");
				pages = new List<string>();
				pageIndex = 0;

				foreach (XElement element in root.Elements())
				{
					pages.Add(element.Attribute("text").Value);
				}
			}
			catch (Exception e)
			{
				reason = e.Message;
				return false;
			}
			LoadPage();
			reason = null;
			return true;
		}

		public override void DrawWindow(DrawingManagement drawingManagement)
		{
			if (invalidTutorial)
			{
				return;
			}

			backGroundImage.MoveTo(xPos, yPos);
			displayingPage.MoveTo(xPos + 5, yPos + 5);
			firstButton.MoveTo(xPos + 45, yPos + 262);
			lastButton.MoveTo(xPos + 155, yPos + 262);
			nextButton.MoveTo(xPos + 190, yPos + 262);
			prevButton.MoveTo(xPos + 10, yPos + 262);
			closeButton.MoveTo(xPos + 224, yPos + 272);
			pageNumber.MoveTo(xPos + 80, yPos + 267);

			base.DrawWindow(drawingManagement);
			displayingPage.Draw(drawingManagement);

			firstButton.Draw(drawingManagement);
			lastButton.Draw(drawingManagement);
			nextButton.Draw(drawingManagement);
			prevButton.Draw(drawingManagement);
			closeButton.Draw(drawingManagement);
			pageNumber.Draw();

			firstButton.DrawToolTip(drawingManagement);
			lastButton.DrawToolTip(drawingManagement);
			nextButton.DrawToolTip(drawingManagement);
			prevButton.DrawToolTip(drawingManagement);
		}

		public override bool MouseHover(int x, int y, float frameDeltaTime)
		{
			if (invalidTutorial)
			{
				return false;
			}
			bool result = base.MouseHover(x, y, frameDeltaTime);

			result = displayingPage.MouseHover(x, y, frameDeltaTime) || result;
			result = firstButton.MouseHover(x, y, frameDeltaTime) || result;
			result = lastButton.MouseHover(x, y, frameDeltaTime) || result;
			result = nextButton.MouseHover(x, y, frameDeltaTime) || result;
			result = prevButton.MouseHover(x, y, frameDeltaTime) || result;
			result = closeButton.MouseHover(x, y, frameDeltaTime) || result;

			return result;
		}

		public override bool MouseDown(int x, int y)
		{
			if (invalidTutorial)
			{
				return false;
			}
			bool result;

			result = displayingPage.MouseDown(x, y);
			result = firstButton.MouseDown(x, y) || result;
			result = lastButton.MouseDown(x, y) || result;
			result = nextButton.MouseDown(x, y) || result;
			result = prevButton.MouseDown(x, y) || result;
			result = closeButton.MouseDown(x, y) || result;

			if (!result)
			{
				result = base.MouseDown(x, y);
			}

			return result;
		}

		public override bool MouseUp(int x, int y)
		{
			if (invalidTutorial)
			{
				return false;
			}
			bool result;

			result = displayingPage.MouseUp(x, y);

			if (firstButton.MouseUp(x, y))
			{
				pageIndex = 0;
				result = true;
				LoadPage();
			}
			if (lastButton.MouseUp(x, y))
			{
				pageIndex = pages.Count - 1;
				result = true;
				LoadPage();
			}
			if (nextButton.MouseUp(x, y))
			{
				pageIndex++;
				result = true;
				LoadPage();
			}
			if (prevButton.MouseUp(x, y))
			{
				pageIndex--;
				result = true;
				LoadPage();
			}
			if (closeButton.MouseUp(x, y))
			{
				GameConfiguration.ShowTutorial = false;
				result = true;
			}
			if (!result)
			{
				result = base.MouseUp(x, y);
			}
			return result;
		}

		private void LoadPage()
		{
			firstButton.Active = true;
			nextButton.Active = true;
			prevButton.Active = true;
			lastButton.Active = true;
			//Sanity check
			if (pageIndex < pages.Count)
			{
				displayingPage.SetMessage(pages[pageIndex]);
				if (pageIndex == 0)
				{
					firstButton.Active = false;
					prevButton.Active = false;
				}
				else if (pageIndex == pages.Count - 1)
				{
					nextButton.Active = false;
					lastButton.Active = false;
				}
				pageNumber.SetText((pageIndex + 1) + " / " + pages.Count, gameMain.FontManager.GetDefaultFont());
			}
			else
			{
				invalidTutorial = true;
			}
		}
	}
}
