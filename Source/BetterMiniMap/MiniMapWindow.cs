using System.Collections.Generic;
using UnityEngine;
using Verse;
using RimWorld;

using BetterMiniMap.Overlays;

// TODO: get rid of these delegates...

namespace BetterMiniMap
{
	[StaticConstructorOnStartup]
    class MiniMapTextures
    {
        public static readonly Texture2D config;
        public static readonly Texture2D dragA;
        public static readonly Texture2D dragD;
        public static readonly Texture2D homeA;
        public static readonly Texture2D resizeA;
        public static readonly Texture2D resizeD;

        static MiniMapTextures()
        {
            MiniMapTextures.config = ContentFinder<Texture2D>.Get("UI/config", true);
            MiniMapTextures.dragA = ContentFinder<Texture2D>.Get("UI/dragA", true);
            MiniMapTextures.dragD = ContentFinder<Texture2D>.Get("UI/dragD", true);
            MiniMapTextures.homeA = ContentFinder<Texture2D>.Get("UI/homeA", true);
            MiniMapTextures.resizeA = ContentFinder<Texture2D>.Get("UI/resizeA", true);
            MiniMapTextures.resizeD = ContentFinder<Texture2D>.Get("UI/resizeD", true);
        }
    }

	internal class MiniMapWindow : Window
	{
		private const float buttonMargin = 15f;
		private const float buttonWidth = 20f;
		private const float minimumSize = 40f;

        private float configx;
        private float configy;
        private float dragx;
        private float dragy;
        private float homex;
        private float homey;
        private float resizex;
        private float resizey;

        private bool resize = false;

		private List<FloatMenuOption> areasoptions;
		private List<FloatMenuOption> options;

		private FloatMenu floatMenu;
		private FloatMenu areafloatMenu;

		private Vector3 lastpositionmouse = new Vector3(-1f, -1f, -1f);

		private MiniMapController mmc;

        // TODO: how to get rid of this default...
		private int idMapActual = -2147483648;

        public MiniMapWindow()
        {
            this.closeOnEscapeKey = false;
            this.preventCameraMotion = false;
            this.mmc = new MiniMapController();
        }

		protected override float Margin { get => 0f; }

        // TODO: we can probably be smarter here...
		public override void Notify_ResolutionChanged()
		{
			base.Notify_ResolutionChanged();
			this.PostOpen();
		}

		public override void DoWindowContents(Rect inRect)
		{
			if (Find.VisibleMap.uniqueID != this.idMapActual)
			{
				this.idMapActual = Find.VisibleMap.uniqueID;
				this.mmc.Refresh();
			}

			new WaitForEndOfFrame();
			this.mmc.Update();

			foreach (Overlay current in this.mmc.Overlays)
				if (current.Visible)
					GUI.DrawTexture(inRect, current.Texture);

			if (this.mmc.selectedArea != null)
				GUI.DrawTexture(inRect, this.mmc.selectedArea.Texture);

			if (!this.draggable && !this.resize && Mouse.IsOver(inRect) && Input.GetMouseButton(0))
			{
				Vector2 mousePosition = Event.current.mousePosition;
				Vector2 vector = new Vector2(mousePosition.x, inRect.height - mousePosition.y);
				Vector2 vector2 = new Vector2((float)Find.VisibleMap.Size.x / inRect.width, (float)Find.VisibleMap.Size.z / inRect.height);
				Find.CameraDriver.JumpToVisibleMapLoc(new Vector3(vector.x * vector2.x, 0f, vector.y * vector2.y));
			}
			else
			{
				if (this.resize && Mouse.IsOver(inRect) && Input.GetMouseButton(0))
				{
					if (this.lastpositionmouse.x == -1f)
					{
						this.lastpositionmouse = Input.mousePosition;
					}
					this.windowRect.width = this.windowRect.width + (this.lastpositionmouse.x - Input.mousePosition.x);
					this.windowRect.height = this.windowRect.width;
					this.lastpositionmouse = Input.mousePosition;
				}
                else if (this.resize && Mouse.IsOver(inRect) && !Input.GetMouseButton(0))
                    this.lastpositionmouse = new Vector3(-1f, -1f, -1f);
			}
		}

		public void MakeOptions()
		{
            this.options = new List<FloatMenuOption>()
            {
                new FloatMenuOptionItem(this.mmc.overlayColonists.Visible, "BMM_ColonistsOverlayLabel".Translate(), delegate
                {
                    this.mmc.overlayColonists.Visible = !this.mmc.overlayColonists.Visible;
                    this.MakeOptions();
                    MiniMap_GameComponent.OverlayColonists = this.mmc.overlayColonists.Visible;
                }),
                new FloatMenuOptionItem(this.mmc.overlayNoncolonist.Visible, "overlay_NonColonistPawnst".Translate(), delegate
                {
                    this.mmc.overlayNoncolonist.Visible = !this.mmc.overlayNoncolonist.Visible;
                    this.MakeOptions();
                    MiniMap_GameComponent.OverlayNoncolonist = this.mmc.overlayNoncolonist.Visible;
                }),
                new FloatMenuOptionItem(this.mmc.overlayWild.Visible, "overlay_Wildlifet".Translate(), delegate
                {
                    this.mmc.overlayWild.Visible = !this.mmc.overlayWild.Visible;
                    this.MakeOptions();
                    MiniMap_GameComponent.OverlayWild = this.mmc.overlayWild.Visible;
                }),
                new FloatMenuOptionItem(this.mmc.overlayBuilding.Visible, "BMM_BuildingsOverlayLabel".Translate(),  delegate
                {
                    this.mmc.overlayBuilding.Visible = !this.mmc.overlayBuilding.Visible;
                    this.MakeOptions();
                    MiniMap_GameComponent.OverlayBuilding = this.mmc.overlayBuilding.Visible;
                }),
                new FloatMenuOptionItem(this.mmc.overlayPower.Visible, "overlay_PowerGridt".Translate(), delegate
                {
                    this.mmc.overlayPower.Visible = !this.mmc.overlayPower.Visible;
                    this.MakeOptions();
                    MiniMap_GameComponent.OverlayPower = this.mmc.overlayPower.Visible;
                }),
                new FloatMenuOptionItem(this.mmc.overlayMining.Visible, "overlay_Miningt".Translate(), delegate
                {
                    this.mmc.overlayMining.Visible = !this.mmc.overlayMining.Visible;
                    this.MakeOptions();
                    MiniMap_GameComponent.OverlayMining = this.mmc.overlayMining.Visible;
                }),
                new FloatMenuOptionItem(this.mmc.overlayFog.Visible, "overlay_Fogt".Translate(), delegate
                {
                    this.mmc.overlayFog.Visible = !this.mmc.overlayFog.Visible;
                    this.MakeOptions();
                    MiniMap_GameComponent.OverlayFog = this.mmc.overlayFog.Visible;
                }),
                new FloatMenuOptionItem(this.mmc.overlayTerrain.Visible, "overlay_Terraint".Translate(), delegate
                {
                    this.mmc.overlayTerrain.Visible = !this.mmc.overlayTerrain.Visible;
                    this.MakeOptions();
                    MiniMap_GameComponent.OverlayTerrain = this.mmc.overlayTerrain.Visible;
                }),
                new FloatMenuOptionItem(this.mmc.overlayShips.Visible, "overlay_Shipst".Translate(), delegate
                {
                    this.mmc.overlayShips.Visible = !this.mmc.overlayShips.Visible;
                    this.MakeOptions();
                    MiniMap_GameComponent.OverlayShips = this.mmc.overlayShips.Visible;
                }),
                new FloatMenuOptionItem(this.mmc.overlayRobots.Visible, "overlay_Robotst".Translate(), delegate
                {
                    this.mmc.overlayRobots.Visible = !this.mmc.overlayRobots.Visible;
                    this.MakeOptions();
                    MiniMap_GameComponent.OverlayRobots = this.mmc.overlayRobots.Visible;
                })

            };
			this.floatMenu = new FloatMenu(this.options)
            {
                closeOnEscapeKey = true,
                preventCameraMotion = false,
            };
		}

		public void MakeAreaOptions()
		{
			if (this.mmc.AreaOverlays?.Count > 0)
			{
				this.areasoptions = new List<FloatMenuOption>();

                foreach (Areas_Overlay overlayArea in this.mmc.AreaOverlays)
                {
                    this.areasoptions.Add(new FloatMenuOptionItem(overlayArea.Visible, overlayArea.area.Label, delegate
                    {
                        overlayArea.Visible = !overlayArea.Visible;
                        if (this.mmc.selectedArea == null)
                            this.mmc.selectedArea = overlayArea;
                        else if (this.mmc.selectedArea == overlayArea)
                            this.mmc.selectedArea = null;
                        else
                        {
                            this.mmc.selectedArea.Visible = false;
                            this.mmc.selectedArea = overlayArea;
                        }
                        this.MakeAreaOptions();
                    }));
                }

				this.areafloatMenu = new FloatMenu(this.areasoptions)
                {
                    closeOnEscapeKey = true,
                    preventCameraMotion = false,
                };
			}
		}

		public override void PostOpen()
		{
            this.windowRect = new Rect(MiniMap_GameComponent.Position, MiniMap_GameComponent.Size);

            MiniMap_GameComponent.ResolutionX = UI.screenWidth;
            MiniMap_GameComponent.ResolutionY = UI.screenHeight;

            this.SetLocality();
		}

        public void UpdateWindow(Vector2 position, Vector2 size)
        {
            this.windowRect.position = position;
            this.windowRect.size = size;
            this.SetLocality();
        }

        private void SetLocality()
        {
            // NOTE: why the +1s here?
            this.configx = this.windowRect.x + this.windowRect.width - buttonWidth - buttonMargin;
            this.configy = this.windowRect.y + this.windowRect.height + 1f;
            this.dragx = this.windowRect.x + this.windowRect.width - (2f * buttonWidth) - (2f * buttonMargin);
            this.dragy = this.windowRect.y + this.windowRect.height + 1f;
            this.homex = this.windowRect.x + this.windowRect.width - (3f * buttonWidth) - (3f * buttonMargin);
            this.homey = this.windowRect.y + this.windowRect.height + 1f;
            this.resizex = this.windowRect.x + this.windowRect.width - (4f * buttonWidth) - (4f * buttonMargin);
            this.resizey = this.windowRect.y + this.windowRect.height + 1f;
        }

		public override void ExtraOnGUI()
		{
			//new WaitForEndOfFrame(); // why?
			this.ClampWindowToScreen();
			this.DrawOverlayButtons();
		}

        // TODO: there should be a better way to do this but this helps with the lazy load
        /*public void OnGui()
        {
            if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.F8)
            {
                if (Find.WindowStack.Windows.IndexOf(this) == -1)
                    Find.WindowStack.Add(this);
                else
                    Find.WindowStack.TryRemove(this, true);
            }
        }*/

        public void Toggle(bool add = false)
        {
            if (add)
            {
                if (Find.WindowStack.Windows.IndexOf(this) == -1) // keep from double add...
                    Find.WindowStack.Add(this);
            }
            else
                Find.WindowStack.TryRemove(this, true);
        }

        private void DrawOverlayButtons()
		{
			if (this.draggable || this.resize)
                this.SetLocality();

			if (Widgets.ButtonImage(new Rect(this.configx, this.configy, buttonWidth, buttonWidth), MiniMapTextures.config))
			{
				if (Event.current.button == 1) // right click
                    Find.WindowStack.Add(this.floatMenu);
			}

			if (Widgets.ButtonImage(new Rect(this.dragx, this.dragy, buttonWidth, buttonWidth), this.draggable ? MiniMapTextures.dragA : MiniMapTextures.dragD))
			{
				if (Event.current.button == 1) // right click
                {
					this.draggable = !this.draggable;
                    if (!this.draggable)
                        MiniMap_GameComponent.Position = this.windowRect.position;
					this.resize = false;
				}
			}

			if (Widgets.ButtonImage(new Rect(this.homex, this.homey, buttonWidth, buttonWidth), MiniMapTextures.homeA))
			{
				if (Event.current.button == 1) // right click
                {
					this.UpdateAreaOverlays();
                    Find.WindowStack.Add(this.areafloatMenu);
				}
			}

			if (Widgets.ButtonImage(new Rect(this.resizex, this.resizey, buttonWidth, buttonWidth), this.resize ? MiniMapTextures.resizeA : MiniMapTextures.resizeD))
			{
				if (Event.current.button == 1) // right click
                {
					this.resize = !this.resize;
					if (!this.resize)
                        MiniMap_GameComponent.Size = this.windowRect.size;
					this.draggable = false;
				}
			}
		}

		private void UpdateAreaOverlays()
		{
			bool remakeOptions = false;
			if (Find.VisibleMap.areaManager.AllAreas.Count != this.mmc.AreaOverlays.Count)
			{
                foreach (Area area in Find.VisibleMap.areaManager.AllAreas)
                {
                    if (!this.mmc.AreaOverlays.Any((Areas_Overlay w) => w.area == area))
                    {
                        this.mmc.AreaOverlays.Add(new Areas_Overlay(area, false));
                        remakeOptions = true;
                    }
                }

				if (this.mmc.AreaOverlays.Any<Areas_Overlay>())
				{
					for (int i = this.mmc.AreaOverlays.Count - 1; i >= 0; i--)
					{
						Area area = this.mmc.AreaOverlays[i].area;
						if (area == null || !Find.VisibleMap.areaManager.AllAreas.Contains(area))
						{
							this.mmc.AreaOverlays.RemoveAt(i);
                            remakeOptions = true;
						}
					}
				}

				if (remakeOptions)
					this.MakeAreaOptions();
			}
		}

        // TODO: consider tabs at the bottom?
		private void ClampWindowToScreen()
		{
			if (this.windowRect.width < minimumSize)
				this.windowRect.width = minimumSize;
			
            // NOTE: unsure if this is needed (as we are square)
			/*if (this.windowRect.height < this.minimumSize)
				this.windowRect.height = this.minimumSize;*/

            if (this.windowRect.xMax > UI.screenWidth)
                this.windowRect.x = UI.screenWidth - this.windowRect.width;

            if (this.windowRect.yMax > UI.screenHeight - buttonWidth - 1)
				this.windowRect.y = UI.screenHeight - this.windowRect.height - buttonWidth - 1;

			if (this.windowRect.xMin < 0f)
				this.windowRect.x = this.windowRect.x - this.windowRect.xMin;

			if (this.windowRect.yMin < 0f)
				this.windowRect.y = this.windowRect.y - this.windowRect.yMin;

			this.windowRect.height = this.windowRect.width;

            MiniMap_GameComponent.Position = this.windowRect.position;
            MiniMap_GameComponent.Size = this.windowRect.size;
		}
	}
}