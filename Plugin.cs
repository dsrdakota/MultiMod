using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
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
		public static ManualLogSource log;

		public static ConfigEntry<int> StartingCreditsConfig;
		public static ConfigEntry<int> ConsecutiveCreditsToGiveConfig;
		public static ConfigEntry<bool> InfiniteCreditsConfig;
		public static ConfigEntry<bool> ConsecutiveGiveCreditsConfig;
		public static ConfigEntry<int> BonusCreditsMultiplierConfig;
		public static ConfigEntry<int> QuotaMultiplierConfig;
		public static ConfigEntry<bool> InfiniteDeadlineConfig;
		public static ConfigEntry<int> DeadlineConfig;

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


		public static ConfigEntry<bool> InfiniteStaminaEnabledConfig;
		public static ConfigEntry<int> SprintSpeedConfig;
		public static ConfigEntry<int> WalkSpeedConfig;

		public static ConfigEntry<int> ThrowPowerConfig;
		public static ConfigEntry<int> JumpPowerConfig;

		public static ConfigEntry<bool> ClimbSpeedEnabledConfig;
		public static ConfigEntry<int> ClimbSpeedMultiplierConfig;
		public static ConfigEntry<int> WeightMultiplierConfig;

		private void Awake()
		{
			ScrapValueMultiplierConfig = Config.Bind("General", "Scrap Value Multiplier", 1f, "Multiplier for the value of scrap spawned.");
			ScrapAmountMultiplierConfig = Config.Bind("General", "Scrap Amount Multiplier", 1.05f, "Multiplier for the amount of scrap spawned.");
			InfiniteCreditsConfig = Config.Bind("General", "Infinite Credits", false, "Start the game with this amount of credits.");
			StartingCreditsConfig = Config.Bind("General", "Starting Credits", 450, "Start the game with this amount of credits.");
			BonusCreditsMultiplierConfig = Config.Bind("General", "Bonus Credits Multiplier", 10, "Bonus multiplier to use when getting an over-quota bonus.");

			ScrapValueRandomConfig = Config.Bind("Random Seeds", "Scrap Random Seed", -1, "Random seed for the scrap spawned. (-1 = disable)");
			MapRandomConfig = Config.Bind("Random Seeds", "Map Random Seed", -1, "Random seed for the map itself (shown upon landing). (-1 = disable)");
			LevelRandomConfig = Config.Bind("Random Seeds", "Level Random Seed", -1, "Random seed for the level (Mansion/Facility?). (-1 = disable)");
			EnemySpawnRandomConfig = Config.Bind("Random Seeds", "Enemy Random Seed", -1, "Random seed for the enemies spawned in general. (-1 = disable)");
			EnemyOutsideRandomConfig = Config.Bind("Random Seeds", "Enemies Outside Random Seed", -1, "Random seed for the enemies spawned outside. (-1 = disable)");
			BreakerBoxRandomConfig = Config.Bind("Random Seeds", "Breaker Box Random Seed", -1, "Random seed for the breaker box spawned. (-1 = disable)");
			AnomalyRandomConfig = Config.Bind("Random Seeds", "Anomaly Random Seed", -1, "Random seed for the any anomalies. (-1 = disable)");
			ChallengeMoonRandomConfig = Config.Bind("Random Seeds", "Challenge Moon Random Seed", -1, "Random seed specific to the Challenge Moon. (-1 = disable)");

			InfiniteStaminaEnabledConfig = Config.Bind("Player", "Infinte Stamina", false, "Whether or not you have infinite stamina.");
			WalkSpeedConfig = Config.Bind("Player", "Walk Speed", 10, "Change default walk speed.");
			SprintSpeedConfig = Config.Bind("Player", "Sprint Speed", 20, "Change default sprint speed.");

			ThrowPowerConfig = Config.Bind("Player", "Throw Power", 170, "Change default throwing power.");
			JumpPowerConfig = Config.Bind("Player", "Jump Power", 50, "Change default jump power.");

			ClimbSpeedEnabledConfig = Config.Bind("Player", "Climb Speed Enabled", false, "Change default climb speed for climbing ladders.");
			ClimbSpeedMultiplierConfig = Config.Bind("Player", "Climb Speed", 40, "Change default climb speed for ladders.");
			WeightMultiplierConfig = Config.Bind("Player", "Weight", 10, "Change the default weight multiplier. Useful only when carrying heavy items and not recommended below 0.5 (Def: 1.0)");

			QuotaMultiplierConfig = Config.Bind("Quota", "Quota Multiplier", 10, "Multiplier to use after each quota met.");
			InfiniteDeadlineConfig = Config.Bind("Quota", "Infinite Deadline", false, "Make the deadline infinite.");
			DeadlineConfig = Config.Bind("Quota", "Days to Deadline", 3, "Days left to deadline. (Deadline is only infinite when Infinite Deadline is enable).");

			var harmony = new Harmony("com.dsrdakota.mods.multimod");
			harmony.PatchAll(typeof(MultiModPlugin));
			harmony.PatchAll(typeof(RoundManagerPatch));
			harmony.PatchAll(typeof(PlayerControllerBPatch));
			harmony.PatchAll(typeof(TimeOfDayPatch));

			log = Logger;
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
