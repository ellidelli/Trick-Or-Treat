using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;
using StardewValley.Locations;
using xTile;

namespace NewFestivals
{
    //mod entry point
    public class ModEntry : Mod
    {
        public static string FestivalName = "Trick Or Treat";

        public override void Entry(IModHelper helper)
        {
            //add to the game loop to check what the day is
            helper.Events.GameLoop.DayStarted += OnDayStarted;

            //load in the content
            helper.Events.Content.AssetRequested += this.OnAssetRequested;
        }

        private void OnDayStarted(object sender, EventArgs e)
        {
            //access smapi api
            IModHelper helper = this.Helper;

            //check if it's fall 28
            if (Game1.currentSeason == "fall" && Game1.dayOfMonth == 28)
            {
                //display a message in the smapi console
                this.Monitor.Log("it is fall 28", LogLevel.Debug);

            }
        }

        private void OnAssetRequested(object sender, AssetRequestedEventArgs e)
        {
            if (e.NameWithoutLocale.IsEquivalentTo("Data\\Festivals\\fall28"))
            {
                e.LoadFrom(() =>
                {
                    var data = this.BuildFestivalData();
                    ModEntry.FestivalName = data["name"];
                    return data;
                }, AssetLoadPriority.Exclusive);
            }
            else if (e.NameWithoutLocale.IsEquivalentTo("Maps\\Town"))
                e.LoadFromModFile<Map>("assets/TrickOrTreat.tmx", AssetLoadPriority.Exclusive);
            else if (e.NameWithoutLocale.IsEquivalentTo("Data\\Festivals\\FestivalDates"))
                e.Edit(static (asset) =>
                {
                    asset.AsDictionary<string, string>().Data.Add("fall28", ModEntry.FestivalName);
                });
        }

        private IDictionary<string, string> BuildFestivalData()
        {
            // base data
            var data = new Dictionary<string, string>
            {
                ["name"] = FestivalName,
                ["conditions"] = "TrickOrTreat/1800 2200",
                ["set-up"] = "changeToTemporaryMap TrickOrTreat"
            };
            return data;
        }
    }
}