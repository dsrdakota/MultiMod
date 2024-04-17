using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using LethalCompanyInputUtils.Api;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MultiMod
{
    [BepInPlugin("com.dsrdakota.mods", "Multi Mod", "1.0.2")]
    [BepInDependency("com.rune580.LethalCompanyInputUtils", BepInDependency.DependencyFlags.HardDependency)]
    public class MultiModPlugin : BaseUnityPlugin
    {

        public static ConfigEntry<float> StartingCreditsConfig;
        public static ConfigEntry<float> BonusCreditsMultiplierConfig;
        public static ConfigEntry<float> QuotaMultiplierConfig;
        public static ConfigEntry<float> DeadlineConfig;

        public static ConfigEntry<float> ScrapAmountMultiplierConfig;
        public static ConfigEntry<float> ScrapValueMultiplierConfig;
        
        public static ConfigEntry<int> LevelRandomConfig;
        public static ConfigEntry<int> EnemySpawnRandomConfig;
        public static ConfigEntry<int> EnemyOutsideRandomConfig;
        public static ConfigEntry<int> AnomalyRandomConfig;
        public static ConfigEntry<int> MapRandomConfig;
        public static ConfigEntry<int> BreakerBoxRandomConfig;
        public static ConfigEntry<int> ChallengeMoonRandomConfig;
        public static ConfigEntry<int> ScrapValueRandomConfig;


        public static ConfigEntry<bool> ClimbSpeedEnabledConfig;
        public static ConfigEntry<float> ClimbSpeedMultiplierConfig;
        public static ConfigEntry<float> WeightMultiplierConfig;

        private void Awake()
        {
            ScrapValueMultiplierConfig = Config.Bind("General", "Scrap Value Multiplier", 1f, "Multiplier for the value of scrap spawned.");
            ScrapAmountMultiplierConfig = Config.Bind("General", "Scrap Amount Multiplier", 1.05f, "Multiplier for the amount of scrap spawned.");

            ScrapValueRandomConfig = Config.Bind("Random Seeds", "Scrap Random Seed", -1, "Random seed for the scrap spawned. (-1 = disable)");
            MapRandomConfig = Config.Bind("Random Seeds", "Map Random Seed", -1, "Random seed for the map itself (shown upon landing). (-1 = disable)");
            LevelRandomConfig = Config.Bind("Random Seeds", "Level Random Seed", -1, "Random seed for the level (Mansion/Facility?). (-1 = disable)");
            EnemySpawnRandomConfig = Config.Bind("Random Seeds", "Enemy Random Seed", -1, "Random seed for the enemies spawned in general. (-1 = disable)");
            EnemyOutsideRandomConfig = Config.Bind("Random Seeds", "Enemies Outside Random Seed", -1, "Random seed for the enemies spawned outside. (-1 = disable)");
            BreakerBoxRandomConfig = Config.Bind("Random Seeds", "Breaker Box Random Seed", -1, "Random seed for the breaker box spawned. (-1 = disable)");
            AnomalyRandomConfig = Config.Bind("Random Seeds", "Anomaly Random Seed", -1, "Random seed for the any anomalies. (-1 = disable)");
            ChallengeMoonRandomConfig = Config.Bind("Random Seeds", "Challenge Moon Random Seed", -1, "Random seed specific to the Challenge Moon. (-1 = disable)");

            ClimbSpeedEnabledConfig = Config.Bind("Player", "Climb Speed Enabled", false, "Change default climb speed for climbing ladders.");
            ClimbSpeedMultiplierConfig = Config.Bind("Player", "Climb Speed", 4f, "Change default climb speed for ladders.");
            WeightMultiplierConfig = Config.Bind("Player", "Weight", 1f, "Change the default weight multiplier. Useful only when carrying heavy items and not recommended below 0.5 (Def: 1.0)");

            var harmony = new Harmony("com.dsrdakota.mods.multimod");
            harmony.PatchAll(typeof(RoundManagerPatch));
            harmony.PatchAll(typeof(PlayerControllerBPatch));

            // ASCII Art
            Logger.LogInfo(@"");
        }
    }

    public class ScrapModInputBindings : LcInputActions
    {
        [InputAction("<Keyboard>/g", Name = "Explode")]
        public InputAction ExplodeKey { get; set; }
        [InputAction("<Keyboard>/h", Name = "Another")]
        public InputAction AnotherKey { get; set; }
    }

}
