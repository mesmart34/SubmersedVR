using System.Collections.Generic;
using System.Reflection.Emit;
using HarmonyLib;

namespace SubmersedVR.Patches;

// This replaces the main camera transform in `Targeting` with the event camera from `VRCameraRig`
// TODO: Rewrite with CodeMatcher

// TODO: Reorganize/Move patches
// Remove/Disable the movement of the HandReticle in LateUpdate by rewriting the if(XRSettings.enabled) to if(false)

// Sets the default DesiredIcon in LateUpdate to None(0) instead of Default(1)
[HarmonyPatch(typeof(HandReticle), nameof(HandReticle.LateUpdate))]
internal static class NoDefaultReticleIcon
{
    internal static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        var desiredIconField = AccessTools.Field(typeof(HandReticle), nameof(HandReticle.desiredIconType));
        return new CodeMatcher(instructions).MatchForward(false, new CodeMatch[] {
            // /* 0x0012B387 17           */ IL_015F: ldc.i4.1
            // /* 0x0012B388 7DB5320004   */ IL_0160: stfld     valuetype HandReticle/IconType HandReticle::desiredIconType
            new(OpCodes.Ldc_I4_1), // Store 1
            new(opc => opc.StoresField(desiredIconField)),
        }).SetOpcodeAndAdvance(OpCodes.Ldc_I4_0).InstructionEnumeration(); // Replace by 0
    }
}