using System.Collections.Generic;
using HarmonyLib;
using UnityEngine.XR;

namespace SubmersedVR.Patches;

// Don't disable the the automatic camera tracking of the UI Camera in the Main Game
[HarmonyPatch(typeof(ManagedCanvasUpdate), nameof(ManagedCanvasUpdate.GetUICamera))]
internal static class ManagedCanvasUpdate_GetUICamera_Patch
{
    internal static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        return new CodeMatcher(instructions).MatchForward(false, [
            new CodeMatch(ci => ci.Calls(typeof(XRDevice).GetMethod(nameof(XRDevice.DisableAutoXRCameraTracking))))
        ]).ThrowIfNotMatch("Could not find XRDevice Deactivation").Advance(-2).RemoveInstructions(3).InstructionEnumeration();
    }
}