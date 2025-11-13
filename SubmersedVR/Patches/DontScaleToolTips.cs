using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;

namespace SubmersedVR.Patches;

[HarmonyPatch(typeof(uGUI_Tooltip), nameof(uGUI_Tooltip.UpdatePosition))]
[HarmonyDebug]
static class DontScaleToolTips
{
    public static PDA pda;
    public const float PDA_ScaleFactor = 0.25f;

    public static float GetTooltipScaler()
    {
        if (pda == null)
        {
            pda = Player.main.GetPDA();
        }
        return pda.isInUse ? PDA_ScaleFactor : 1.0f;
    }

    static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        var m = new CodeMatcher(instructions);
        var pos = m.MatchForward(false, new CodeMatch[] {
            // new CodeMatch(OpCodes.Stloc_0)
            new(ci => ci.Calls(AccessTools.DeclaredMethod(typeof(Vector3), nameof(Vector3.Dot))))
        }).Pos;
        m.Start().RemoveInstructionsInRange(0, pos).Insert(new CodeInstruction[] {
            CodeInstruction.Call(typeof(DontScaleToolTips), nameof(DontScaleToolTips.GetTooltipScaler)),
        });
        return m.InstructionEnumeration();
    }
}