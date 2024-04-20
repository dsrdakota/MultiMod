using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiMod
{
	[HarmonyPatch(typeof(TerminalPatch))]
	internal class TerminalPatch
	{
		[HarmonyPatch("Start")]
		[HarmonyPostfix]
		private static void StartPatch(Terminal __instance, ref int ___groupCredits)
		{
			if (!MultiModPlugin.ConsecutiveGiveCreditsConfig.Value) {
				if (TimeOfDay.Instance.daysUntilDeadline != TimeOfDay.Instance.quotaVariables.deadlineDaysAmount || TimeOfDay.Instance.profitQuota != TimeOfDay.Instance.quotaVariables.startingQuota) return;
				switch (MultiModPlugin.InfiniteCreditsConfig.Value) {
					case true:
						___groupCredits = MultiModPlugin.ConsecutiveCreditsToGiveConfig.Value;
						break;
					default:
						___groupCredits += MultiModPlugin.ConsecutiveCreditsToGiveConfig.Value;
						break;
				}
			} else {
				switch (MultiModPlugin.InfiniteCreditsConfig.Value) {
					case true:
						___groupCredits = MultiModPlugin.ConsecutiveCreditsToGiveConfig.Value;
						break;
					default:
						___groupCredits += MultiModPlugin.ConsecutiveCreditsToGiveConfig.Value;
						break;
				}
			}
		}
	}
}
