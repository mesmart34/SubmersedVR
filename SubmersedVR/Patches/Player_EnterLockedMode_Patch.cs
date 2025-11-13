using HarmonyLib;

namespace SubmersedVR.Patches;

[HarmonyPatch(typeof(Player), nameof(Player.EnterLockedMode))]
internal static class Player_EnterLockedMode_Patch
{
    internal static void Postfix()
    {
        VRUtil.Recenter();
    }
}