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
			__instance.scrapValueMultiplier = MultiModPlugin.ScrapValueMultiplierConfig.Value;
			__instance.scrapAmountMultiplier = MultiModPlugin.ScrapAmountMultiplierConfig.Value;
			MultiModPlugin.UpdateAllRandomSeeds();
			__instance.playersManager.shipDoorsEnabled = true;

		}


		[HarmonyPatch(typeof(RoundManager), "GenerateNewLevelClientRpc")]
		[HarmonyPostfix]
		public static void GenerateNewLevelClientRpc(ref RoundManager __instance)
		{
			__instance.scrapValueMultiplier = MultiModPlugin.ScrapValueMultiplierConfig.Value;
			__instance.scrapAmountMultiplier = MultiModPlugin.ScrapAmountMultiplierConfig.Value;
			MultiModPlugin.UpdateAllRandomSeeds();

		}

		[HarmonyPatch("GenerateNewLevelClientRpc")]
		[HarmonyPostfix]
		public static void GenerateNewLevelClientRpcPostfix(int randomSeed, int levelID)
		{
			MultiModPlugin.UpdateAllRandomSeeds();
		}
	}

}
