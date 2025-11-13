using System.Collections.Generic;
using System.Reflection.Emit;
using HarmonyLib;
using UnityEngine.XR;

namespace SubmersedVR.Patches;

[HarmonyPatch(typeof(HandReticle), nameof(HandReticle.LateUpdate))]
internal static class NoReticleMovementInVR
{
    internal static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        foreach (var ins in instructions)
        {
            // Instead of the call to the enabled property we just push 0/false on to the stack to skip the if
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