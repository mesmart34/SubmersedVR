using System.Collections.Generic;
using HarmonyLib;
using SubmersedVR.VR;

namespace SubmersedVR.Patches;

// Make the Propulsion canon shoot with the laser pointer/target transform
[HarmonyPatch(typeof(PropulsionCannon), nameof(PropulsionCannon.OnShoot))]
internal class PropulsionCannon_OnShoot_Patch_Patch
{
    internal static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        var m = new CodeMatcher(instructions);
        m.MatchForward(false, new CodeMatch[] { new(ci => ci.Calls(AccessTools.DeclaredPropertyGetter(typeof(MainCamera), nameof(MainCamera.camera)))) });
        m.SetInstruction(CodeInstruction.Call(typeof(Aiming), nameof(Aiming.GetAimCamera)));
        return m.InstructionEnumeration();
    }
}