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
        [HarmonyPrefix]
        public static void GenerateNewLevelClientRpcPrefix(int randomSeed, int levelID)
        {

            if(StartOfRound.Instance.isChallengeFile == true) 
            {
                RoundManager.Instance.ChallengeMoonRandom = new System.Random(MultiModPlugin.ChallengeMoonRandomConfig.Value);
            }
        }
    }

}
