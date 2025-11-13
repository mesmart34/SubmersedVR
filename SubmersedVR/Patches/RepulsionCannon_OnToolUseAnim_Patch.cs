using System.Collections.Generic;
using HarmonyLib;
using SubmersedVR.VR;

namespace SubmersedVR.Patches;

// Make the Propulsion canon shoot with the laser pointer/target transform
[HarmonyPatch(typeof(RepulsionCannon), nameof(RepulsionCannon.OnToolUseAnim))]
internal static class RepulsionCannon_OnToolUseAnim_Patch
{
    internal static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        var m = new CodeMatcher(instructions);
        m.MatchForward(false, new CodeMatch[] { new(ci => ci.Calls(AccessTools.DeclaredPropertyGetter(typeof(MainCamera), nameof(MainCamera.camera)))) });
        m.SetInstructionAndAdvance(CodeInstruction.Call(typeof(Aiming), nameof(Aiming.GetAimCamera)));
        m.MatchForward(false, new CodeMatch[] { new(ci => ci.Calls(AccessTools.DeclaredPropertyGetter(typeof(MainCamera), nameof(MainCamera.camera)))) });
        m.SetInstruction(CodeInstruction.Call(typeof(Aiming), nameof(Aiming.GetAimCamera)));
        return m.InstructionEnumeration();
    }
}