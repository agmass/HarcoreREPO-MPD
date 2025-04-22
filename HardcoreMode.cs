using System.Collections;
using HarmonyLib;
using Photon.Pun;
using UnityEngine;

namespace HardcoreREPO;

[HarmonyPatch(typeof(PlayerAvatar))]
static class HardcoreREPO
{
    public static ArrayList permaDeadIndividuals = new ArrayList();
    [HarmonyPrefix, HarmonyPatch(nameof(PlayerAvatar.PlayerDeathDone))]
    private static void Death_Prefix(PlayerAvatar __instance)
    {
        if (!__instance.isLocal && SemiFunc.IsMasterClientOrSingleplayer()) {
            permaDeadIndividuals.Add(__instance.steamID);
        }
    }


    [HarmonyPrefix, HarmonyPatch(nameof(PlayerAvatar.Update))]
    private static void Respawn_Prefix(PlayerAvatar __instance)
    {
        if (!__instance.isLocal && SemiFunc.IsMasterClientOrSingleplayer()) {
            if (permaDeadIndividuals.Contains(__instance.steamID) && !__instance.deadSet) {
                __instance.PlayerDeath(-1);
            }
        }
    }


			
}