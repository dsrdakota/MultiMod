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

			if (__instance.IsServer) {
				HUDManager.Instance.DisplayTip("MultiMod", "We are the server");
			}
			if(__instance.IsHost) {
				HUDManager.Instance.DisplayTip("MultiMod", "We are the host");
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
			
			__instance.sinkingSpeedMultiplier = 0f;

			if (MultiModPlugin.InfiniteHealthEnabledConfig.Value == true) {
				// These are ugly hacks that may or may not work
				if(__instance.isSinking == true) {
					__instance.TeleportPlayer(__instance.oldPlayerPosition);
					__instance.isSinking = false;
					__instance.UpdateSpecialAnimationValue(true);
				}
				if(__instance.isUnderwater == true) {
					__instance.TeleportPlayer(__instance.oldPlayerPosition);
					__instance.isUnderwater = false;
					__instance.UpdateSpecialAnimationValue(true);
				}
				__instance.health = int.MaxValue;

			} else {
				__instance.health = __instance.health;
			}
		}

		[HarmonyPatch(typeof(PlayerControllerB), "Update")]
		[HarmonyPostfix]
		private static void UpdateHealthPatch(ref float ___health)
		{
			if (MultiModPlugin.InfiniteHealthEnabledConfig.Value == true) {
				___health = int.MaxValue;

			} else {
				___health = ___health;
			}
		}
	}


}
