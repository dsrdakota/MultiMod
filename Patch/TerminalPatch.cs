using UnityEngine;
using Unity.Netcode;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiMod
{
	[HarmonyPatch(typeof(Terminal))]
	internal class TerminalPatch
	{
		[HarmonyPatch("Start")]
		[HarmonyPrefix]
		private static void StartPatch(Terminal __instance, ref int ___groupCredits)
		{
			bool isHost = RoundManager.Instance.NetworkManager.IsHost;
			if (!isHost) return;

			//Terminal __instance = UnityEngine.Object.FindObjectOfType<Terminal>();

			MultiModPlugin.log.LogInfo("OnDayChanged Event Running");
			MultiModPlugin.log.LogInfo(TimeOfDay.Instance.daysUntilDeadline == TimeOfDay.Instance.quotaVariables.deadlineDaysAmount);
			MultiModPlugin.log.LogInfo(TimeOfDay.Instance.profitQuota == TimeOfDay.Instance.quotaVariables.startingQuota);
			MultiModPlugin.log.LogInfo(RoundManager.Instance.currentLevel.name.ToLower());


			if (MultiModPlugin.InfiniteCreditsConfig.Value == true) {
				__instance.groupCredits = 1000000;
			} else {

				if (MultiModPlugin.ConsecutiveCreditsConfig.Value == true) {
					// we don't want to add credits on company day because of possible bonus
					if (RoundManager.Instance.currentLevel.name.ToLower() == "gordion" ||
						RoundManager.Instance.currentLevel.PlanetName.ToLower() == "71 gordion" || 
						RoundManager.Instance.currentLevel.levelID == 79) return;
					__instance.groupCredits = __instance.groupCredits += MultiModPlugin.CreditsToGiveConfig.Value;
				} else {
					__instance.groupCredits = MultiModPlugin.CreditsToGiveConfig.Value;
				}
			}
		}

		[HarmonyPatch("Update")]
		[HarmonyPostfix]
		private static void UpdatePatch()
		{
			/* We will not update the credits here but in the timeofday daychange event
			bool isHost = RoundManager.Instance.NetworkManager.IsHost;
			if (!isHost) return;

			Terminal __instance = UnityEngine.Object.FindObjectOfType<Terminal>();

			if (TimeOfDay.Instance.daysUntilDeadline != TimeOfDay.Instance.quotaVariables.deadlineDaysAmount || TimeOfDay.Instance.profitQuota != TimeOfDay.Instance.quotaVariables.startingQuota) return;
			if (MultiModPlugin.InfiniteCreditsConfig.Value == true) {
				__instance.groupCredits = 1000000;
			} else {

				if (MultiModPlugin.ConsecutiveCreditsConfig.Value == true) {

					__instance.groupCredits = __instance.groupCredits += MultiModPlugin.CreditsToGiveConfig.Value;
				} else {
					__instance.groupCredits = MultiModPlugin.CreditsToGiveConfig.Value;
				}
			}*/
		}
	}
}
