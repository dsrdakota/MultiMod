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
			if(!isHost) return;
			
			TimeOfDay.Instance.quotaVariables.startingCredits = MultiModPlugin.StartingCreditsConfig.Value;
			if (TimeOfDay.Instance.daysUntilDeadline != TimeOfDay.Instance.quotaVariables.deadlineDaysAmount || TimeOfDay.Instance.profitQuota != TimeOfDay.Instance.quotaVariables.startingQuota) return;
			if (MultiModPlugin.InfiniteCreditsConfig.Value == true) {
				___groupCredits = 1000000;
			} else {

				if (MultiModPlugin.ConsecutiveCreditsConfig.Value == true) {
					//if (TimeOfDay.Instance.daysUntilDeadline != TimeOfDay.Instance.quotaVariables.deadlineDaysAmount || TimeOfDay.Instance.profitQuota != TimeOfDay.Instance.quotaVariables.startingQuota) return;
					___groupCredits = ___groupCredits += MultiModPlugin.CreditsToGiveConfig.Value;
				} else {
					___groupCredits =  ___groupCredits = MultiModPlugin.CreditsToGiveConfig.Value;
				}
			}
			//__instance.groupCredits = ___groupCredits;
			//__instance.startingCreditsAmount = ___groupCredits;
		}

		[HarmonyPatch("Update")]
		[HarmonyPostfix]
		private static void UpdatePatch()
		{
			bool isHost = RoundManager.Instance.NetworkManager.IsHost;
			if (!isHost) return;

			Terminal __instance = UnityEngine.Object.FindObjectOfType<Terminal>();

			int ___groupCredits = 0;
			___groupCredits = __instance.groupCredits;
			if (TimeOfDay.Instance.daysUntilDeadline != TimeOfDay.Instance.quotaVariables.deadlineDaysAmount || TimeOfDay.Instance.profitQuota != TimeOfDay.Instance.quotaVariables.startingQuota) return;
			if (MultiModPlugin.InfiniteCreditsConfig.Value == true) {
				___groupCredits = 1000000;
			} else {

				if (MultiModPlugin.ConsecutiveCreditsConfig.Value == true) {
					
					___groupCredits = ___groupCredits += MultiModPlugin.CreditsToGiveConfig.Value;
				} else {
					___groupCredits = ___groupCredits = MultiModPlugin.CreditsToGiveConfig.Value;
				}
			}

			__instance.groupCredits = ___groupCredits;
		}
	}
}
