using HarmonyLib;

namespace SubmersedVR.Patches;

[HarmonyPatch(typeof(Player), nameof(Player.EnterPilotingMode))]
static class RecenterWhenPilotingCyclops
{
    public static void Postfix()
    {
        VRUtil.Recenter();
    }
}