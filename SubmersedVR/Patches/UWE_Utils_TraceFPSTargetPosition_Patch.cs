using System.Collections.Generic;
using HarmonyLib;
using SubmersedVR.VR;
using UnityEngine;

namespace SubmersedVR.Patches;

// Make the Knife, Fire Extinguisher and Exosuit aim with the laserpointer instead of camera
[HarmonyPatch(typeof(UWE.Utils), nameof(UWE.Utils.TraceFPSTargetPosition))]
[HarmonyPatch([
        typeof(GameObject),
        typeof(float),
        typeof(GameObject),
        typeof(Vector3),
        typeof(Vector3),
        typeof(bool)
    ],
    [
        ArgumentType.Normal,
        ArgumentType.Normal,
        ArgumentType.Ref,
        ArgumentType.Ref,
        ArgumentType.Out,
        ArgumentType.Normal
    ])]
internal static class UWE_Utils_TraceFPSTargetPosition_Patch
{
    internal static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        var m = new CodeMatcher(instructions);
        m.MatchForward(false, new CodeMatch[] { new(ci => ci.Calls(AccessTools.DeclaredPropertyGetter(typeof(MainCamera), nameof(MainCamera.camera)))) });
        m.SetInstructionAndAdvance(CodeInstruction.Call(typeof(Aiming), nameof(Aiming.GetAimCamera)));
        return m.InstructionEnumeration();
    }
}