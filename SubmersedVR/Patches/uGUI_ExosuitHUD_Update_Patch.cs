using System.Collections.Generic;
using System.Reflection.Emit;
using HarmonyLib;
using SubmersedVR.VR;

namespace SubmersedVR.Patches;

[HarmonyPatch(typeof(uGUI_ExosuitHUD), nameof(uGUI_ExosuitHUD.Update))]
internal class uGUI_ExosuitHUD_Update_Patch
{
    internal static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        var m = new CodeMatcher(instructions);
        m.MatchForward(true, [
            new CodeMatch(OpCodes.Stloc_3)
        ])
        .Advance(1)
        .Insert(new CodeInstruction(OpCodes.Ldloc_3),
            CodeInstruction.LoadField(typeof(WristHud),
                nameof(WristHud.isHudOn)),
            new CodeInstruction(OpCodes.And),
            new CodeInstruction(OpCodes.Stloc_3));
        return m.InstructionEnumeration();
    }
}