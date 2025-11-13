using System.Collections.Generic;
using HarmonyLib;
using SubmersedVR.VR;

namespace SubmersedVR.Patches;

// Make the Propulsion canon aim with the laser pointer/target transform
[HarmonyPatch(typeof(PropulsionCannon), nameof(PropulsionCannon.GetObjectPosition))]
internal static class PropulsionCannon_GetObjectPosition_Patch
{
    // Replace the first line/instruction in GetObjectPosition() that is Camera camera = MainCamera.camera, with our own above.
    internal static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        var m = new CodeMatcher(instructions);
        m.Start().SetInstruction(CodeInstruction.Call(typeof(Aiming), nameof(Aiming.GetAimCamera)));
        return m.InstructionEnumeration();
    }
}