using HarmonyLib;
using SubmersedVR.VR;

namespace SubmersedVR.Patches;

// This resets the PDA configuration for the Hand configuration for the PDA
[HarmonyPatch(typeof(PDA), nameof(PDA.Close))]
public static class PDA_Close_Patch
{
    [HarmonyPostfix]
    public static void Postfix()
    {
        VRHands.instance.OnClosePDA();
    }
}