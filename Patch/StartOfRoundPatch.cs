using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiMod
{
	[HarmonyPatch]
	internal class StartOfRoundPatch
	{
		[HarmonyPatch(typeof(StartOfRound), "Start")]
		[HarmonyPostfix]
		public static void StartPatch(ref StartOfRound ___instance)
		{
			/* moved
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
					RoundManager.Instance.AnomalyRandom =  MultiModPlugin.AnomalyRandomizer;
					RoundManager.Instance.BreakerBoxRandom = MultiModPlugin.BreakerBoxRandomizer;

					break;
				case CustomSeedsType.UseGameDefault:
				default:
					break;
			}*/
			MultiModPlugin.UpdateAllRandomSeeds();
		}
		[HarmonyPatch(typeof(StartOfRound), "Awake")]
		[HarmonyPostfix]
		public static void InvinciblePatch(ref bool ___allowLocalPlayerDeath)
		{
			___allowLocalPlayerDeath = !MultiModPlugin.InfiniteHealthEnabledConfig.Value;
		}
	}
}
