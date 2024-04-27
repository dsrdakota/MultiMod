using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MultiMod
{
	[HarmonyPatch]
	internal class TimeOfDayPatch
	{
		[HarmonyPatch(typeof(TimeOfDay), "Awake")]
		[HarmonyPostfix]
		private static void AwakePatch(ref TimeOfDay __instance)
		{
			string currentSaveFile = GameNetworkManager.Instance.currentSaveFileName;
			bool isHost = RoundManager.Instance.NetworkManager.IsHost;
			float runCount = TimeOfDay.Instance.timesFulfilledQuota;

			int maxDays = MultiModPlugin.DeadlineConfig.Value;

			if (!isHost) return;

			MultiModPlugin.log.LogDebug("__instance.totalTime: " + __instance.totalTime.ToString());
			__instance.quotaVariables.deadlineDaysAmount = maxDays;
			__instance.quotaVariables.startingCredits = MultiModPlugin.StartingCreditsConfig.Value;
			__instance.quotaVariables.randomizerMultiplier = MultiModPlugin.QuotaMultiplierConfig.Value / 10;

			if (MultiModPlugin.InfiniteDeadlineConfig.Value == true) {
				__instance.timeUntilDeadline = (float)((int)(__instance.totalTime * (float)__instance.quotaVariables.deadlineDaysAmount));
				StartOfRound.Instance.deadlineMonitorText.text = "DEADLINE:\n INFINITE";
			} else {
				__instance.daysUntilDeadline = __instance.quotaVariables.deadlineDaysAmount;
				StartOfRound.Instance.deadlineMonitorText.text = "DEADLINE:\n " + __instance.daysUntilDeadline.ToString() + "/" + __instance.quotaVariables.deadlineDaysAmount.ToString();
			}
		}
		/*
		 * this does not run?
		[HarmonyPatch(typeof(TimeOfDay), "Start")]
		[HarmonyPostfix]
		private static void StartPatch(ref TimeOfDay __instance)
		{
			string currentSaveFile = GameNetworkManager.Instance.currentSaveFileName;
			bool isHost = RoundManager.Instance.NetworkManager.IsHost;
			float runCount = TimeOfDay.Instance.timesFulfilledQuota;

			int maxDays = MultiModPlugin.DeadlineConfig.Value;

			if (!isHost) return;

			MultiModPlugin.log.LogDebug("__instance.totalTime: " + __instance.totalTime.ToString());
			__instance.quotaVariables.deadlineDaysAmount = maxDays + 1;
			__instance.quotaVariables.startingCredits = MultiModPlugin.StartingCreditsConfig.Value;
			__instance.quotaVariables.randomizerMultiplier = MultiModPlugin.QuotaMultiplierConfig.Value / 10;

			if (MultiModPlugin.InfiniteDeadlineConfig.Value == true) {
				__instance.timeUntilDeadline = (float)((int)(__instance.totalTime * (float)__instance.quotaVariables.deadlineDaysAmount));
				StartOfRound.Instance.deadlineMonitorText.text = "DEADLINE 1:\n INFINITE";
			} else {
				__instance.daysUntilDeadline = __instance.quotaVariables.deadlineDaysAmount;
				StartOfRound.Instance.deadlineMonitorText.text = "DEADLINE 1:\n " + __instance.daysUntilDeadline.ToString() + "/" + __instance.quotaVariables.deadlineDaysAmount.ToString();
			}
		}
		*/

		// new game changes this
		[HarmonyPatch(typeof(TimeOfDay), "Update")]
		[HarmonyPostfix]
		private static void UpdatePatch(ref TimeOfDay __instance)
		{
			string currentSaveFile = GameNetworkManager.Instance.currentSaveFileName;
			bool isHost = RoundManager.Instance.NetworkManager.IsHost;
			float runCount = TimeOfDay.Instance.timesFulfilledQuota;

			int maxDays = MultiModPlugin.DeadlineConfig.Value;

			if (!isHost) return;

			MultiModPlugin.log.LogDebug("__instance.totalTime: " + __instance.totalTime.ToString());
			__instance.quotaVariables.deadlineDaysAmount = maxDays;
			__instance.quotaVariables.startingCredits = MultiModPlugin.StartingCreditsConfig.Value;
			__instance.quotaVariables.randomizerMultiplier = MultiModPlugin.QuotaMultiplierConfig.Value / 10;

			if (MultiModPlugin.InfiniteDeadlineConfig.Value == true) {
				__instance.timeUntilDeadline = (float)((int)(__instance.totalTime * (float)__instance.quotaVariables.deadlineDaysAmount));
				StartOfRound.Instance.deadlineMonitorText.text = "DEADLINE:\n INFINITE";
			} else {
				StartOfRound.Instance.deadlineMonitorText.text = "DEADLINE:\n " + __instance.daysUntilDeadline.ToString() + "/" + __instance.quotaVariables.deadlineDaysAmount.ToString();
			}

			/* Maybe change the difficulty?
				* __instance.timeUntilDeadline = (float)__instance.totalTime * Mathf.Clamp(Mathf.Ceil(__instance.profitQuota / DynamicDeadlineMod.legacyDailyValue.Value), minimumDays, maxDays);
				*/

		}

		[HarmonyPatch(typeof(TimeOfDay), nameof(TimeOfDay.SetNewProfitQuota))]
		[HarmonyReversePatch]
		private static void SetNewProfitQuotaPatch(ref TimeOfDay __instance)
		{
			string currentSaveFile = GameNetworkManager.Instance.currentSaveFileName;
			bool isHost = RoundManager.Instance.NetworkManager.IsHost;
			float runCount = TimeOfDay.Instance.timesFulfilledQuota;

			if (!isHost) return;


			__instance.quotaVariables.deadlineDaysAmount = MultiModPlugin.DeadlineConfig.Value;
			// We don't need this here: __instance.quotaVariables.startingCredits = MultiModPlugin.StartingCreditsConfig.Value;
			__instance.quotaVariables.randomizerMultiplier = MultiModPlugin.QuotaMultiplierConfig.Value / 10;

			__instance.timesFulfilledQuota++;
			int num = __instance.quotaFulfilled - __instance.profitQuota;
			float num2 = Mathf.Clamp(1f + (float)__instance.timesFulfilledQuota * ((float)__instance.timesFulfilledQuota / __instance.quotaVariables.increaseSteepness), 0f, 10000f);
			num2 = __instance.quotaVariables.baseIncrease * num2 * (__instance.quotaVariables.randomizerCurve.Evaluate(UnityEngine.Random.Range(0f, 1f)) * __instance.quotaVariables.randomizerMultiplier + 1f);
			__instance.profitQuota = (int)Mathf.Clamp((float)__instance.profitQuota + num2, 0f, 1E+09f);
			__instance.quotaFulfilled = 0;
			// apparently 4f is deadline days +1
			//__instance.timeUntilDeadline = __instance.totalTime * 4f;
			if (MultiModPlugin.InfiniteDeadlineConfig.Value == true) {
				__instance.timeUntilDeadline = (float)((int)(__instance.totalTime * (float)__instance.quotaVariables.deadlineDaysAmount));
				StartOfRound.Instance.deadlineMonitorText.text = "DEADLINE:\n INFINITE";
			} else {
				__instance.timeUntilDeadline = __instance.totalTime * __instance.quotaVariables.deadlineDaysAmount + 1;
				StartOfRound.Instance.deadlineMonitorText.text = "DEADLINE:\n " + __instance.daysUntilDeadline.ToString() + "/" + __instance.quotaVariables.deadlineDaysAmount.ToString();
			}
			// Here we add in our overtimeBonus multiplier if different
			int overtimeBonus = (int)Math.Ceiling((decimal)(num / 5 + 15 * __instance.daysUntilDeadline)*MultiModPlugin.BonusCreditsMultiplierConfig.Value);
			__instance.SyncNewProfitQuotaClientRpc(__instance.profitQuota, overtimeBonus, __instance.timesFulfilledQuota);

		}

		[HarmonyPatch(typeof(TimeOfDay), "UpdateProfitQuotaCurrentTime")]
		[HarmonyPostfix]
		private static void UpdateProfitQuotaCurrentTimePatch(ref TimeOfDay __instance)
		{
			__instance.quotaVariables.deadlineDaysAmount = MultiModPlugin.DeadlineConfig.Value;
			// We don't need this here: __instance.quotaVariables.startingCredits = MultiModPlugin.StartingCreditsConfig.Value;
			__instance.quotaVariables.randomizerMultiplier = MultiModPlugin.QuotaMultiplierConfig.Value / 10;

			if (MultiModPlugin.InfiniteDeadlineConfig.Value == true) {
				__instance.timeUntilDeadline = (float)((int)(__instance.totalTime * (float)__instance.quotaVariables.deadlineDaysAmount));
				StartOfRound.Instance.deadlineMonitorText.text = "DEADLINE:\n INFINITE";
			} else {
				StartOfRound.Instance.deadlineMonitorText.text = "DEADLINE:\n " + __instance.daysUntilDeadline.ToString() + "/" + __instance.quotaVariables.deadlineDaysAmount.ToString();
			}

		}

	}
}
