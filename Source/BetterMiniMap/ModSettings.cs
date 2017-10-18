﻿using UnityEngine;
using Verse;
using SettingsHelper;

namespace BetterMiniMap
{
    class BetterMiniMapSettings : ModSettings
    {
        public class UpdatePeriods : IExposable
        {
            public int viewpoint = 5;
            public int colonists = 5;
            public int noncolonists = 5;
            public int robots = 5;
            public int ships = 60;
            public int powerGrid = 60;
            public int wildlife = 80;
            public int buildings = 250;
            public int mining = 250;
            public int areas = 250;
            public int terrain = 2500;
            public int fog = 2500;

            public void ExposeData()
            {
                Scribe_Values.Look(ref this.viewpoint, "viewpoint", 5);
                Scribe_Values.Look(ref this.colonists, "colonists", 5);
                Scribe_Values.Look(ref this.noncolonists, "noncolonists", 5);
                Scribe_Values.Look(ref this.robots, "robots", 5);
                Scribe_Values.Look(ref this.ships, "ships", 60);
                Scribe_Values.Look(ref this.powerGrid, "powerGrid", 60);
                Scribe_Values.Look(ref this.wildlife, "wildlife", 80);
                Scribe_Values.Look(ref this.buildings, "buildings", 250);
                Scribe_Values.Look(ref this.mining, "mining", 250);
                Scribe_Values.Look(ref this.areas, "areas", 250);
                Scribe_Values.Look(ref this.terrain, "terrain", 2500);
                Scribe_Values.Look(ref this.fog, "fog", 2500);
            }
        }

        public class IndicatorSizes : IExposable
        {
            public float colonists = 3f;
            public float tamedAnimals = 2f;
            public float enemyPawns = 2f;
            public float traderPawns = 2f;
            public float visitorPawns = 2f;
            public float robots = 3f;
            public float ships = 3f;
            public float wildlife = 1f;
            public float wildlifeTaming = 1f;
            public float wildlifeHunting = 1f;
            public float wildlifeHostiles = 1f;

            public void ExposeData()
            {
                Scribe_Values.Look(ref this.colonists, "colonists", 3f);
                Scribe_Values.Look(ref this.tamedAnimals, "tamedAnimals", 2f);
                Scribe_Values.Look(ref this.enemyPawns, "enemyPawns", 2f);
                Scribe_Values.Look(ref this.traderPawns, "traderPawns", 2f);
                Scribe_Values.Look(ref this.visitorPawns, "visitorPawns", 2f);
                Scribe_Values.Look(ref this.robots, "robots", 3f);
                Scribe_Values.Look(ref this.ships, "ships", 3f);
                Scribe_Values.Look(ref this.wildlife, "wildlife", 1f);
                Scribe_Values.Look(ref this.wildlifeTaming, "wildlifeTaming", 1f);
                Scribe_Values.Look(ref this.wildlifeHunting, "wildlifeHunting", 1f);
                Scribe_Values.Look(ref this.wildlifeHostiles, "wildlifeHostiles", 1f);
            }
        }

        // Initializes settings for users without config file
        public BetterMiniMapSettings()
        {
            this.updatePeriods = new UpdatePeriods();
            this.indicatorSizes = new IndicatorSizes();
        }

        public UpdatePeriods updatePeriods;
        public IndicatorSizes indicatorSizes;

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Deep.Look(ref this.updatePeriods, "updatePeriods");
            Scribe_Deep.Look(ref this.indicatorSizes, "indicatorSizes");

            // Handles upgrading settings
            if (Scribe.mode == LoadSaveMode.LoadingVars)
            {
                if (this.updatePeriods == null)
                    this.updatePeriods = new UpdatePeriods();
                if (this.indicatorSizes == null)
                    this.indicatorSizes = new IndicatorSizes();
            }
        }

    }

    class BetterMiniMapMod : Mod
    {
        public static BetterMiniMapSettings settings;

        public BetterMiniMapMod(ModContentPack content) : base(content)
        {
            settings = GetSettings<BetterMiniMapSettings>();
        }

        public override string SettingsCategory() => "BMM_SettingsCategoryLabel".Translate();

        public override void DoSettingsWindowContents(Rect rect)
        {
            Listing_Standard listing_Standard = new Listing_Standard() { ColumnWidth = rect.width / 2f };
            listing_Standard.Begin(rect);

            listing_Standard.AddLabelLine("BMM_TimeUpdateLabel".Translate(), 2f * Text.LineHeight);

            listing_Standard.AddLabeledNumericalTextField<int>("BMM_AreasOverlayLabel".Translate(), ref settings.updatePeriods.areas);
            listing_Standard.AddLabeledNumericalTextField<int>("BMM_BuildingsOverlayLabel".Translate(), ref settings.updatePeriods.buildings);
            listing_Standard.AddLabeledNumericalTextField<int>("BMM_ColonistsOverlayLabel".Translate(), ref settings.updatePeriods.colonists);
            listing_Standard.AddLabeledNumericalTextField<int>("BMM_MiningOverlayLabel".Translate(), ref settings.updatePeriods.mining);
            listing_Standard.AddLabeledNumericalTextField<int>("BMM_NoncolonistOverlayLabel".Translate(), ref settings.updatePeriods.noncolonists);
            listing_Standard.AddLabeledNumericalTextField<int>("BMM_PowerGridOverlayLabel".Translate(), ref settings.updatePeriods.powerGrid);
            listing_Standard.AddLabeledNumericalTextField<int>("BMM_RobotsOverlayLabel".Translate(), ref settings.updatePeriods.robots);
            listing_Standard.AddLabeledNumericalTextField<int>("BMM_ShipsOverlayLabel".Translate(), ref settings.updatePeriods.ships);
            listing_Standard.AddLabeledNumericalTextField<int>("BMM_TerrainOverlayLabel".Translate(), ref settings.updatePeriods.terrain);
            listing_Standard.AddLabeledNumericalTextField<int>("BMM_WildlifeOverlayLabel".Translate(), ref settings.updatePeriods.wildlife);

            listing_Standard.AddLabeledNumericalTextField<int>("BMM_FogOverlayLabel".Translate(), ref settings.updatePeriods.fog);
            listing_Standard.AddLabeledNumericalTextField<int>("BMM_ViewpointOverlayLabel".Translate(), ref settings.updatePeriods.viewpoint);

            listing_Standard.NewColumn();

            listing_Standard.AddLabelLine("BMM_IndicatorSizeLabel".Translate(), 2f * Text.LineHeight);

            listing_Standard.AddLabeledNumericalTextField<float>("BMM_ColonistIndicatorSizeLabel".Translate(), ref settings.indicatorSizes.colonists);
            listing_Standard.AddLabeledNumericalTextField<float>("BMM_AnimalIndicatorSizeLabel".Translate(), ref settings.indicatorSizes.tamedAnimals);
            listing_Standard.AddLabeledNumericalTextField<float>("BMM_RobotsIndicatorSizeLabel".Translate(), ref settings.indicatorSizes.robots);

            listing_Standard.AddLabeledNumericalTextField<float>("BMM_EnemyIndicatorSizeLabel".Translate(), ref settings.indicatorSizes.enemyPawns);
            listing_Standard.AddLabeledNumericalTextField<float>("BMM_TraderIndicatorSizeLabel".Translate(), ref settings.indicatorSizes.traderPawns);
            listing_Standard.AddLabeledNumericalTextField<float>("BMM_VisitorIndicatorSizeLabel".Translate(), ref settings.indicatorSizes.visitorPawns);

            listing_Standard.AddLabeledNumericalTextField<float>("BMM_ShipsIndicatorSizeLabel".Translate(), ref settings.indicatorSizes.ships);

            listing_Standard.AddLabeledNumericalTextField<float>("BMM_WildlifeIndicatorSizeLabel".Translate(), ref settings.indicatorSizes.wildlife);
            listing_Standard.AddLabeledNumericalTextField<float>("BMM_TamingIndicatorSizeLabel".Translate(), ref settings.indicatorSizes.wildlifeTaming);
            listing_Standard.AddLabeledNumericalTextField<float>("BMM_HostileAnimalIndicatorSizeLabel".Translate(), ref settings.indicatorSizes.wildlifeHostiles);
            listing_Standard.AddLabeledNumericalTextField<float>("BMM_HuntingIndicatorSizeLabel".Translate(), ref settings.indicatorSizes.wildlifeHunting);

            listing_Standard.End();
            settings.Write();
        }

    }

}
