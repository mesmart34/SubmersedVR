using System.Collections.Generic;
using System.Reflection.Emit;
using HarmonyLib;
using SubmersedVR.VR;
using UnityEngine;

namespace SubmersedVR.Patches;

[HarmonyPatch(typeof(Targeting), nameof(Targeting.GetTarget))]
[HarmonyPatch([typeof(float), typeof(GameObject), typeof(float)], [ArgumentType.Normal, ArgumentType.Out, ArgumentType.Out])]
internal static class Targeting_GetTarget_Patch
{
    internal static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        var method = typeof(MainCamera).GetProperty(nameof(MainCamera.camera))?.GetGetMethod();
        
        foreach (var ins in instructions)
        {
            if (ins.Calls(method))
            {
                var targetTransformGetter = typeof(VRCameraRig).GetMethod(nameof(VRCameraRig.GetTargetTansform));
                yield return new CodeInstruction(OpCodes.Call, targetTransformGetter);
            }
            else
            {
                yield return ins;
            }
        }
    }
}