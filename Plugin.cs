using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using System;
using System.Net;


//using LethalCompanyInputUtils.Api;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MultiMod
{
	public enum CustomSeedsType
	{
		UseGameDefault = 0,
		UseModdedSeeds = 1,
		UsedFixedSeeds = 2
	}
	[BepInPlugin("com.dsrdakota.mods", "Multi Mod", "1.1.0")]
	//[BepInDependency("com.rune580.LethalCompanyInputUtils", BepInDependency.DependencyFlags.HardDependency)]
	[BepInDependency("ainavt.lc.lethalconfig", BepInDependency.DependencyFlags.SoftDependency)]
	public class MultiModPlugin : BaseUnityPlugin
	{
		public static ManualLogSource log;

		public static ConfigEntry<int> StartingCreditsConfig;
		public static ConfigEntry<int> CreditsToGiveConfig;
		public static ConfigEntry<bool> ConsecutiveCreditsConfig;
		public static ConfigEntry<bool> InfiniteCreditsConfig;
		public static ConfigEntry<int> BonusCreditsMultiplierConfig;
		public static ConfigEntry<int> BuyingRateConfig;
		public static ConfigEntry<int> QuotaMultiplierConfig;
		public static ConfigEntry<bool> InfiniteDeadlineConfig;
		public static ConfigEntry<int> DeadlineConfig;

		public static ConfigEntry<float> ScrapAmountMultiplierConfig;
		public static ConfigEntry<float> ScrapValueMultiplierConfig;

		public static ConfigEntry<CustomSeedsType> CustomSeedsTypeConfig;
		public static ConfigEntry<int> LevelRandomConfig;
		public static ConfigEntry<int> EnemySpawnRandomConfig;
		public static ConfigEntry<int> EnemyOutsideRandomConfig;
		public static ConfigEntry<int> AnomalyRandomConfig;
		public static ConfigEntry<int> MapRandomConfig;
		public static ConfigEntry<int> BreakerBoxRandomConfig;
		public static ConfigEntry<int> ChallengeMoonRandomConfig;
		public static ConfigEntry<int> ScrapValueRandomConfig;


		public static ConfigEntry<bool> InfiniteHealthEnabledConfig;

		public static ConfigEntry<bool> InfiniteStaminaEnabledConfig;
		public static ConfigEntry<int> SprintSpeedConfig;
		public static ConfigEntry<int> WalkSpeedConfig;

		public static ConfigEntry<int> ThrowPowerConfig;
		public static ConfigEntry<int> JumpPowerConfig;

		public static ConfigEntry<bool> ClimbSpeedEnabledConfig;
		public static ConfigEntry<int> ClimbSpeedMultiplierConfig;
		public static ConfigEntry<int> WeightMultiplierConfig;

		public static System.Random BaseRandomizer;
		public static System.Random ScrapValueRandomizer;
		public static System.Random MapSeedRandomizer;
		public static System.Random LevelSeedRandomizer;
		public static System.Random EnemySpawnRandomizer;
		public static System.Random OutsideEnemySpawnRandomizer;
		public static System.Random AnomalyRandomizer;
		public static System.Random BreakerBoxRandomizer;
		public static System.Random ChallengeMoonSeedRandomizer;

		private void Awake()
		{
			BaseRandomizer = new System.Random();

			ScrapValueMultiplierConfig = Config.Bind("General", "Scrap Value Multiplier", 1f, "Multiplier for the value of scrap spawned.");
			ScrapAmountMultiplierConfig = Config.Bind("General", "Scrap Amount Multiplier", 1.05f, "Multiplier for the amount of scrap spawned.");
			StartingCreditsConfig = Config.Bind("General", "Starting Credits", 450, "Infinitely have credits.");
			InfiniteCreditsConfig = Config.Bind("General", "Infinite Credits", false, "Infinitely have credits.");
			CreditsToGiveConfig = Config.Bind("General", "Credits To Give", 210, "Start the game with this amount of credits.");
			ConsecutiveCreditsConfig = Config.Bind("General", "Give Credits Consecutively", false, "Consecutively give credits after every round with the Credits config.");
			BonusCreditsMultiplierConfig = Config.Bind("General", "Bonus Credits Multiplier", 10, "Bonus multiplier to use when getting an over-quota bonus.");
			BuyingRateConfig = Config.Bind("General", "Company Buying Rate", 10, "Sets the default company buying rate.");

			CustomSeedsTypeConfig = Config.Bind("Random Seeds", "Custom Random Seeds", CustomSeedsType.UseGameDefault, "Seed randomization will be controlled by the mod, fixed by the mod or game default.");
			CustomSeedsTypeConfig.SettingChanged += (obj, args) =>
			{
				log.LogInfo($"Slider value changed to {CustomSeedsTypeConfig.Value}");
				UpdateAllRandomSeeds();
			};
			ScrapValueRandomConfig = Config.Bind("Random Seeds", "Scrap Random Seed", 16542, "Random seed for the scrap spawned.");
			ScrapValueRandomConfig.SettingChanged += (obj, args) =>
			{
				log.LogInfo($"Slider value changed to {ScrapValueRandomConfig.Value}");
				UpdateAllRandomSeeds();
			};
			MapRandomConfig = Config.Bind("Random Seeds", "Map Random Seed", 52782, "Random seed for the map itself (shown upon landing).");
			MapRandomConfig.SettingChanged += (obj, args) =>
			{
				log.LogInfo($"Slider value changed to {MapRandomConfig.Value}");
				UpdateAllRandomSeeds();
			};
			LevelRandomConfig = Config.Bind("Random Seeds", "Level Random Seed", 878277, "Random seed for the level (Mansion/Facility?).");
			LevelRandomConfig.SettingChanged += (obj, args) =>
			{
				log.LogInfo($"Slider value changed to {LevelRandomConfig.Value}");
				UpdateAllRandomSeeds();
			};
			EnemySpawnRandomConfig = Config.Bind("Random Seeds", "Enemy Random Seed", 57447010, "Random seed for the enemies spawned in general.");
			EnemySpawnRandomConfig.SettingChanged += (obj, args) =>
			{
				log.LogInfo($"Slider value changed to {CustomSeedsTypeConfig.Value}");
				UpdateAllRandomSeeds();
			};
			EnemyOutsideRandomConfig = Config.Bind("Random Seeds", "Enemies Outside Random Seed", 478755, "Random seed for the enemies spawned outside.");
			EnemyOutsideRandomConfig.SettingChanged += (obj, args) =>
			{
				log.LogInfo($"Slider value changed to {EnemyOutsideRandomConfig.Value}");
				UpdateAllRandomSeeds();
			};
			BreakerBoxRandomConfig = Config.Bind("Random Seeds", "Breaker Box Random Seed", 757522, "Random seed for the breaker box spawned.");
			BreakerBoxRandomConfig.SettingChanged += (obj, args) =>
			{
				log.LogInfo($"Slider value changed to {BreakerBoxRandomConfig.Value}");
				UpdateAllRandomSeeds();
			};
			AnomalyRandomConfig = Config.Bind("Random Seeds", "Anomaly Random Seed", 754447, "Random seed for the any anomalies.");
			CustomSeedsTypeConfig.SettingChanged += (obj, args) =>
			{
				log.LogInfo($"Slider value changed to {CustomSeedsTypeConfig.Value}");
				UpdateAllRandomSeeds();
			};
			ChallengeMoonRandomConfig = Config.Bind("Random Seeds", "Challenge Moon Random Seed", 880088, "Random seed specific to the Challenge Moon.");
			ChallengeMoonRandomConfig.SettingChanged += (obj, args) =>
			{
				log.LogInfo($"Slider value changed to {ChallengeMoonRandomConfig.Value}");
				UpdateAllRandomSeeds();
			};

			InfiniteHealthEnabledConfig = Config.Bind("Player", "Infinte Health", false, "Whether or not you have infinite health.");
			InfiniteStaminaEnabledConfig = Config.Bind("Player", "Infinte Stamina", false, "Whether or not you have infinite stamina.");
			WalkSpeedConfig = Config.Bind("Player", "Walk Speed", 10, "Change default walk speed.");
			SprintSpeedConfig = Config.Bind("Player", "Sprint Speed", 20, "Change default sprint speed.");
			ThrowPowerConfig = Config.Bind("Player", "Throw Power", 170, "Change default throwing power.");
			JumpPowerConfig = Config.Bind("Player", "Jump Power", 150, "Change default jump power.");

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
			harmony.PatchAll(typeof(TerminalPatch));

			log = Logger;
			// ASCII Art
			Logger.LogInfo(@"");
		}

		public static void UpdateAllRandomSeeds()
		{
			// HERE is where we set all of our Random variables at start
			switch (MultiModPlugin.CustomSeedsTypeConfig.Value) {
				case CustomSeedsType.UsedFixedSeeds:
					// Here we use the seeds given by the config
					MultiModPlugin.ScrapValueRandomizer = new System.Random(MultiModPlugin.ScrapValueRandomConfig.Value);
					MultiModPlugin.MapSeedRandomizer = new System.Random(MultiModPlugin.MapRandomConfig.Value);
					MultiModPlugin.LevelSeedRandomizer = new System.Random(MultiModPlugin.LevelRandomConfig.Value);
					MultiModPlugin.EnemySpawnRandomizer = new System.Random(MultiModPlugin.EnemySpawnRandomConfig.Value);
					MultiModPlugin.OutsideEnemySpawnRandomizer = new System.Random(MultiModPlugin.EnemyOutsideRandomConfig.Value);
					MultiModPlugin.AnomalyRandomizer = new System.Random(MultiModPlugin.AnomalyRandomConfig.Value);
					MultiModPlugin.BreakerBoxRandomizer = new System.Random(MultiModPlugin.BreakerBoxRandomConfig.Value);
					MultiModPlugin.ChallengeMoonSeedRandomizer = new System.Random(MultiModPlugin.ChallengeMoonRandomConfig.Value);

					if (StartOfRound.Instance.isChallengeFile == true) // We are currently are on Challenge Moon save
						{
						RoundManager.Instance.ChallengeMoonRandom = new System.Random(MultiModPlugin.ChallengeMoonRandomConfig.Value);
					}
					StartOfRound.Instance.overrideRandomSeed = (MultiModPlugin.CustomSeedsTypeConfig.Value != CustomSeedsType.UseGameDefault) ? true : false;
					StartOfRound.Instance.overrideSeedNumber = (MultiModPlugin.CustomSeedsTypeConfig.Value != CustomSeedsType.UseGameDefault) ? MultiModPlugin.MapSeedRandomizer.Next(13458240, Int32.MaxValue) : StartOfRound.Instance.randomMapSeed;
					RoundManager.Instance.ScrapValuesRandom = MultiModPlugin.ScrapValueRandomizer;
					RoundManager.Instance.LevelRandom = MultiModPlugin.LevelSeedRandomizer;
					RoundManager.Instance.EnemySpawnRandom = MultiModPlugin.EnemySpawnRandomizer;
					RoundManager.Instance.OutsideEnemySpawnRandom = MultiModPlugin.EnemySpawnRandomizer;
					RoundManager.Instance.AnomalyRandom = MultiModPlugin.AnomalyRandomizer;
					RoundManager.Instance.BreakerBoxRandom = MultiModPlugin.BreakerBoxRandomizer;

					break;
				case CustomSeedsType.UseModdedSeeds:
					// Here we use the seeds given by the mod itself (uses BaseRandomizer at start)
					MultiModPlugin.ScrapValueRandomizer = new System.Random(MultiModPlugin.BaseRandomizer.Next(707, 77345663));
					MultiModPlugin.MapSeedRandomizer = new System.Random(MultiModPlugin.BaseRandomizer.Next(707, 77345663));
					MultiModPlugin.LevelSeedRandomizer = new System.Random(MultiModPlugin.BaseRandomizer.Next(707, 77345663));
					MultiModPlugin.EnemySpawnRandomizer = new System.Random(MultiModPlugin.BaseRandomizer.Next(707, 77345663));
					MultiModPlugin.OutsideEnemySpawnRandomizer = new System.Random(MultiModPlugin.BaseRandomizer.Next(707, 77345663));
					MultiModPlugin.AnomalyRandomizer = new System.Random(MultiModPlugin.BaseRandomizer.Next(707, 77345663));
					MultiModPlugin.BreakerBoxRandomizer = new System.Random(MultiModPlugin.BaseRandomizer.Next(707, 77345663));
					MultiModPlugin.ChallengeMoonSeedRandomizer = new System.Random(MultiModPlugin.BaseRandomizer.Next(707, 77345663));

					if (StartOfRound.Instance.isChallengeFile == true) // We are currently are on Challenge Moon save
					{
						RoundManager.Instance.ChallengeMoonRandom = new System.Random(MultiModPlugin.BaseRandomizer.Next(707, 77345663));
					}
					StartOfRound.Instance.overrideRandomSeed = (MultiModPlugin.CustomSeedsTypeConfig.Value != CustomSeedsType.UseGameDefault) ? true : false;
					StartOfRound.Instance.overrideSeedNumber = (MultiModPlugin.CustomSeedsTypeConfig.Value != CustomSeedsType.UseGameDefault) ? MultiModPlugin.MapSeedRandomizer.Next(13458240, Int32.MaxValue) : StartOfRound.Instance.randomMapSeed;
					RoundManager.Instance.ScrapValuesRandom = MultiModPlugin.ScrapValueRandomizer;
					RoundManager.Instance.LevelRandom = MultiModPlugin.LevelSeedRandomizer;
					RoundManager.Instance.EnemySpawnRandom = MultiModPlugin.EnemySpawnRandomizer;
					RoundManager.Instance.OutsideEnemySpawnRandom = MultiModPlugin.EnemySpawnRandomizer;
					RoundManager.Instance.AnomalyRandom = MultiModPlugin.AnomalyRandomizer;
					RoundManager.Instance.BreakerBoxRandom = MultiModPlugin.BreakerBoxRandomizer;

					break;
				case CustomSeedsType.UseGameDefault:
				default:
					break;
			}
		}
	}

	/* dont need keybind yet
	public class ScrapModInputBindings : LcInputActions
	{
		[InputAction("<Keyboard>/g", Name = "Explode")]
		public InputAction ExplodeKey { get; set; }
		[InputAction("<Keyboard>/h", Name = "Another")]
		public InputAction AnotherKey { get; set; }
	}*/

}
