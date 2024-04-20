using GameNetcodeStuff;
using HarmonyLib;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace MultiMod
{
	[HarmonyPatch(typeof(RoundManager))]
	public static class RoundManagerPatch
	{

		[HarmonyPatch("SpawnScrapInLevel")]
		[HarmonyPrefix]
		public static void SpawnScrapInLevelPrefix(RoundManager __instance)
		{

			//__instance.playersManager.ReviveDeadPlayers();
			__instance.scrapValueMultiplier = MultiModPlugin.ScrapAmountMultiplierConfig.Value;
			__instance.scrapAmountMultiplier = MultiModPlugin.ScrapAmountMultiplierConfig.Value;
			__instance.ScrapValuesRandom = new System.Random(MultiModPlugin.ScrapValueRandomConfig.Value);

		}

		[HarmonyPatch("GenerateNewLevelClientRpc")]
		[HarmonyPostfix]
		public static void GenerateNewLevelClientRpcPostfix(int randomSeed, int levelID)
		{

			if (StartOfRound.Instance.isChallengeFile == true) // We are currently are on Challenge Moon save
			{
				RoundManager.Instance.ChallengeMoonRandom = new System.Random(MultiModPlugin.ChallengeMoonRandomConfig.Value);
			}
			StartOfRound.Instance.overrideRandomSeed = (MultiModPlugin.MapRandomConfig.Value != -1) ? true : false;
			StartOfRound.Instance.overrideSeedNumber = (MultiModPlugin.MapRandomConfig.Value != -1) ? MultiModPlugin.MapRandomConfig.Value : StartOfRound.Instance.randomMapSeed;
			RoundManager.Instance.LevelRandom = (MultiModPlugin.LevelRandomConfig.Value != -1) ? new System.Random(MultiModPlugin.LevelRandomConfig.Value) : RoundManager.Instance.LevelRandom;
			RoundManager.Instance.EnemySpawnRandom = (MultiModPlugin.EnemySpawnRandomConfig.Value != -1) ? new System.Random(MultiModPlugin.EnemySpawnRandomConfig.Value) : RoundManager.Instance.EnemySpawnRandom;
			RoundManager.Instance.OutsideEnemySpawnRandom = (MultiModPlugin.EnemyOutsideRandomConfig.Value != -1) ? new System.Random(MultiModPlugin.EnemyOutsideRandomConfig.Value) : RoundManager.Instance.OutsideEnemySpawnRandom;
			RoundManager.Instance.AnomalyRandom = (MultiModPlugin.AnomalyRandomConfig.Value != -1) ? new System.Random(MultiModPlugin.AnomalyRandomConfig.Value) : RoundManager.Instance.AnomalyRandom;
			RoundManager.Instance.BreakerBoxRandom = (MultiModPlugin.BreakerBoxRandomConfig.Value != -1) ? new System.Random(MultiModPlugin.BreakerBoxRandomConfig.Value) : RoundManager.Instance.BreakerBoxRandom;



		}
	}

}
