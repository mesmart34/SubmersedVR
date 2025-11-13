using System.Collections.Generic;
using System.Reflection.Emit;
using HarmonyLib;
using UnityEngine.XR;

namespace SubmersedVR.Patches;

[HarmonyPatch(typeof(HandReticle), nameof(HandReticle.LateUpdate))]
internal static class HandReticle_LateUpdate_Patch
{
    internal static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        var desiredIconField = AccessTools.Field(typeof(HandReticle), nameof(HandReticle.desiredIconType));
        var matcher = new CodeMatcher(instructions);

        matcher.MatchForward(false,
                new CodeMatch(OpCodes.Ldc_I4_1),
                new CodeMatch(opc => opc.StoresField(desiredIconField)))
            .SetOpcodeAndAdvance(OpCodes.Ldc_I4_0);

        foreach (var ins in matcher.InstructionEnumeration())
        {
            if (ins.Calls(AccessTools.DeclaredPropertyGetter(typeof(XRSettings), nameof(XRSettings.enabled))))
            {
                yield return new CodeInstruction(OpCodes.Ldc_I4_0);
            }
            else
            {
                yield return ins;
            }
        }
    }
}