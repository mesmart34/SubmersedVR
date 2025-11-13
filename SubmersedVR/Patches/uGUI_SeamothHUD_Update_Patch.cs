using System.Collections.Generic;
using System.Reflection.Emit;
using HarmonyLib;
using SubmersedVR.VR;

namespace SubmersedVR.Patches;

[HarmonyPatch(typeof(uGUI_SeamothHUD), nameof(uGUI_SeamothHUD.Update))]
internal static class uGUI_SeamothHUD_Update_Patch
{
    internal static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        var m = new CodeMatcher(instructions);
        m.MatchForward(true, new CodeMatch[] {
            new(OpCodes.Stloc_3),
        }).Advance(1).Insert(new CodeInstruction[] {
            new(OpCodes.Ldloc_3),
            CodeInstruction.LoadField(typeof(WristHud), nameof(WristHud.isHudOn)),
            new(OpCodes.And),
            new(OpCodes.Stloc_3),
        });
        return m.InstructionEnumeration();
    }
}