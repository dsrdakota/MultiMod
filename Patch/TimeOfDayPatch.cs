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
		[HarmonyPostfix]
		[HarmonyPatch(typeof(TimeOfDay), "Awake")]
		private static void AwakePatch()
		{

		}

		[HarmonyPatch(typeof(TimeOfDay), "Start")]
		private static void StartPatch(TimeOfDay __instance)
		{
			string currentSaveFile = GameNetworkManager.Instance.currentSaveFileName;
			bool isHost = RoundManager.Instance.NetworkManager.IsHost;
			float runCount = TimeOfDay.Instance.timesFulfilledQuota;

			int maxDays = MultiModPlugin.DeadlineConfig.Value;

			if (!isHost) return;

			if (MultiModPlugin.InfiniteDeadlineConfig.Value == true) {
				__instance.timeUntilDeadline = 0;
			}

			//__instance.daysUntilDeadline = MultiModPlugin.DeadlineConfig.Value;
			/* We do not want to reset these here, only when quota is met
				* int num = __instance.quotaFulfilled - __instance.profitQuota;
				* float num2 = Mathf.Clamp(1f + (float)__instance.timesFulfilledQuota * ((float)__instance.timesFulfilledQuota / __instance.quotaVariables.increaseSteepness), 0f, 10000f);
				* num2 = __instance.quotaVariables.baseIncrease * num2 * (__instance.quotaVariables.randomizerCurve.Evaluate(UnityEngine.Random.Range(0f, 1f)) * __instance.quotaVariables.randomizerMultiplier + 1f);
				*
				* __instance.profitQuota = (int)Mathf.Clamp((float)__instance.profitQuota + num2, 0f, 1E+09f);
				* __instance.quotaFulfilled = 0; */

			/* Maybe change the difficulty?
				* __instance.timeUntilDeadline = (float)__instance.totalTime * Mathf.Clamp(Mathf.Ceil(__instance.profitQuota / DynamicDeadlineMod.legacyDailyValue.Value), minimumDays, maxDays);
				*/

			MultiModPlugin.log.LogDebug("__instance.totalTime: " + __instance.totalTime.ToString());
			__instance.quotaVariables.deadlineDaysAmount = maxDays + 1;
			__instance.quotaVariables.startingCredits = MultiModPlugin.StartingCreditsConfig.Value;
			__instance.quotaVariables.randomizerMultiplier = MultiModPlugin.QuotaMultiplierConfig.Value / 10;
		}

		[HarmonyPatch(typeof(TimeOfDay), "Update")]
		private static void UpdatePatch(TimeOfDay __instance)
		{
			string currentSaveFile = GameNetworkManager.Instance.currentSaveFileName;
			bool isHost = RoundManager.Instance.NetworkManager.IsHost;
			float runCount = TimeOfDay.Instance.timesFulfilledQuota;

			int maxDays = MultiModPlugin.DeadlineConfig.Value;

			if (!isHost) return;

			if (MultiModPlugin.InfiniteDeadlineConfig.Value == true) {
				__instance.timeUntilDeadline = 0;
			}

			//__instance.daysUntilDeadline = MultiModPlugin.DeadlineConfig.Value;
			/* We do not want to reset these here, only when quota is met
				* int num = __instance.quotaFulfilled - __instance.profitQuota;
				* float num2 = Mathf.Clamp(1f + (float)__instance.timesFulfilledQuota * ((float)__instance.timesFulfilledQuota / __instance.quotaVariables.increaseSteepness), 0f, 10000f);
				* num2 = __instance.quotaVariables.baseIncrease * num2 * (__instance.quotaVariables.randomizerCurve.Evaluate(UnityEngine.Random.Range(0f, 1f)) * __instance.quotaVariables.randomizerMultiplier + 1f);
				*
				* __instance.profitQuota = (int)Mathf.Clamp((float)__instance.profitQuota + num2, 0f, 1E+09f);
				* __instance.quotaFulfilled = 0; */

			/* Maybe change the difficulty?
				* __instance.timeUntilDeadline = (float)__instance.totalTime * Mathf.Clamp(Mathf.Ceil(__instance.profitQuota / DynamicDeadlineMod.legacyDailyValue.Value), minimumDays, maxDays);
				*/

			MultiModPlugin.log.LogDebug("__instance.totalTime: " + __instance.totalTime.ToString());
			__instance.quotaVariables.deadlineDaysAmount = maxDays + 1;
			__instance.quotaVariables.startingCredits = MultiModPlugin.StartingCreditsConfig.Value;
			__instance.quotaVariables.randomizerMultiplier = MultiModPlugin.QuotaMultiplierConfig.Value / 10;
		}

		[HarmonyPatch(typeof(TimeOfDay), nameof(TimeOfDay.SetNewProfitQuota))]
		[HarmonyReversePatch]
		private static void SetNewProfitQuotaPatch(TimeOfDay __instance)
		{
			string currentSaveFile = GameNetworkManager.Instance.currentSaveFileName;
			bool isHost = RoundManager.Instance.NetworkManager.IsHost;
			float runCount = TimeOfDay.Instance.timesFulfilledQuota;

			int maxDays = MultiModPlugin.DeadlineConfig.Value;

			if (!isHost) return;

			if (MultiModPlugin.InfiniteDeadlineConfig.Value == true) {
				__instance.timeUntilDeadline = 0;
			}


			__instance.quotaVariables.deadlineDaysAmount = maxDays + 1;
			// We don't need this here: __instance.quotaVariables.startingCredits = MultiModPlugin.StartingCreditsConfig.Value;
			__instance.quotaVariables.randomizerMultiplier = MultiModPlugin.QuotaMultiplierConfig.Value / 10;

			__instance.timesFulfilledQuota++;
			int num = __instance.quotaFulfilled - __instance.profitQuota;
			float num2 = Mathf.Clamp(1f + (float)__instance.timesFulfilledQuota * ((float)__instance.timesFulfilledQuota / __instance.quotaVariables.increaseSteepness), 0f, 10000f);
			num2 = __instance.quotaVariables.baseIncrease * num2 * (__instance.quotaVariables.randomizerCurve.Evaluate(UnityEngine.Random.Range(0f, 1f)) * __instance.quotaVariables.randomizerMultiplier + 1f);
			__instance.profitQuota = (int)Mathf.Clamp((float)__instance.profitQuota + num2, 0f, 1E+09f);
			__instance.quotaFulfilled = 0;
			__instance.timeUntilDeadline = __instance.totalTime * 4f;
			// Here we add in our overtimeBonus multiplier if different
			int overtimeBonus = (int)Math.Ceiling((decimal)(num / 5 + 15 * __instance.daysUntilDeadline)*MultiModPlugin.BonusCreditsMultiplierConfig.Value);
			__instance.SyncNewProfitQuotaClientRpc(__instance.profitQuota, overtimeBonus, __instance.timesFulfilledQuota);

		}
	}
}
