using HarmonyLib;
using SubmersedVR.VR;

namespace SubmersedVR.Patches;

// This configures the left hands offset for the PDA
[HarmonyPatch(typeof(PDA), nameof(PDA.Open))]
public class PDA_Open_Patch
{
    [HarmonyPostfix]
    public static void Postfix()
    {
        VRHands.instance.OnOpenPDA();
    }
}