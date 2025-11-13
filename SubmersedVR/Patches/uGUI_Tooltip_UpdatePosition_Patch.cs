using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;

namespace SubmersedVR.Patches;

[HarmonyPatch(typeof(uGUI_Tooltip), nameof(uGUI_Tooltip.UpdatePosition))]
[HarmonyDebug]
internal static class uGUI_Tooltip_UpdatePosition_Patch
{
    private static PDA _pda;
    private const float PdaScaleFactor = 0.25f;

    private static float GetTooltipScaler()
    {
        if (_pda == null)
        {
            _pda = Player.main.GetPDA();
        }
        return _pda.isInUse ? PdaScaleFactor : 1.0f;
    }

    internal static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        var m = new CodeMatcher(instructions);
        var pos = m.MatchForward(false, [
            new CodeMatch(ci => ci.Calls(AccessTools.DeclaredMethod(typeof(Vector3), nameof(Vector3.Dot))))
        ]).Pos;
        m.Start().RemoveInstructionsInRange(0, pos).Insert(CodeInstruction.Call(typeof(uGUI_Tooltip_UpdatePosition_Patch), nameof(GetTooltipScaler)));
        return m.InstructionEnumeration();
    }
}