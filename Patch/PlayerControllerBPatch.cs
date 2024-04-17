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
        //[HarmonyPatch(typeof(PlayerControllerB), nameof(PlayerControllerB.OnNetworkSpawn))]
        [HarmonyPatch(typeof(PlayerControllerB),"Start")]
        private static void OnStart(PlayerControllerB __instance)
        {
            if(MultiModPlugin.ClimbSpeedEnabledConfig.Value == true)
            {
                __instance.climbSpeed = MultiModPlugin.ClimbSpeedMultiplierConfig.Value;
            } else
            {
                __instance.climbSpeed = 4f;
            }
            __instance.carryWeight = MultiModPlugin.WeightMultiplierConfig.Value;
            if (!__instance.IsServer)
            {
                HUDManager.Instance.DisplayTip("YO!", "I AM SERVER");
            }
        }
    }
}
