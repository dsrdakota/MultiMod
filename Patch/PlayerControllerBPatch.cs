using GameNetcodeStuff;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiMod
{
	[HarmonyPatch]
	internal class PlayerControllerBPatch
	{
		[HarmonyPrefix]
		[HarmonyPatch(typeof(PlayerControllerB), "Start")]
		private static void OnStart(PlayerControllerB __instance)
		{

			if (!__instance.IsServer) {
				HUDManager.Instance.DisplayTip("YO!", "I AM SERVER");
			}
		}

		// Credits to  Brew/my own rework
		[HarmonyPatch(typeof(PlayerControllerB), "Update")]
		[HarmonyPostfix]
		private static void UpdatePatch(ref float ___sprintMultiplier, ref float ___sprintMeter, ref bool ___isSprinting)
		{
			if (MultiModPlugin.InfiniteStaminaEnabledConfig.Value) {
				___sprintMeter = 1f;
			}
			if (___isSprinting) {
				___sprintMultiplier = MultiModPlugin.SprintSpeedConfig.Value / 10;
			} else {
				___sprintMultiplier = MultiModPlugin.WalkSpeedConfig.Value / 10;
			}
		}
		[HarmonyPatch(typeof(PlayerControllerB), "Update")]
		[HarmonyPostfix]
		private static void UpdateJumpClimbWeightPatch(PlayerControllerB __instance)
		{
			if (MultiModPlugin.ClimbSpeedEnabledConfig.Value == true) {
				__instance.climbSpeed = MultiModPlugin.ClimbSpeedMultiplierConfig.Value / 10;
			} else {
				__instance.climbSpeed = 4f;
			}

			if (MultiModPlugin.ThrowPowerConfig.Value > -1) {
				__instance.throwPower = MultiModPlugin.ThrowPowerConfig.Value /10 ;
			} else {
				__instance.throwPower = 17f;
			}

			if (MultiModPlugin.WeightMultiplierConfig.Value > -1) {
				__instance.carryWeight = MultiModPlugin.WeightMultiplierConfig.Value / 10;
			} else {
				__instance.carryWeight = 1f;
			}

			if (MultiModPlugin.JumpPowerConfig.Value > -1) {
				__instance.jumpForce = MultiModPlugin.JumpPowerConfig.Value / 10;
			} else {
				__instance.jumpForce = 5f;
			}
		}
	}


}
