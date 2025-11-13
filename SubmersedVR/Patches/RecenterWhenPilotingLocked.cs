using HarmonyLib;

namespace SubmersedVR.Patches;

[HarmonyPatch(typeof(Player), nameof(Player.EnterLockedMode))]
static class RecenterWhenPilotingLocked
{
    public static void Postfix()
    {
        VRUtil.Recenter();
    }
}