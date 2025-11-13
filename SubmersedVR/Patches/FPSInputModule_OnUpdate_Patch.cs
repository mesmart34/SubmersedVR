using System.Collections.Generic;
using System.Reflection.Emit;
using HarmonyLib;
using UnityEngine.EventSystems;

namespace SubmersedVR.Patches;

[HarmonyPatch(typeof(FPSInputModule), nameof(FPSInputModule.OnUpdate))]
internal static class FPSInputModule_OnUpdate_Patch
{
    internal static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        // var getter = AccessTools.DeclaredPropertyGetter(typeof(BaseInputModule), "eventSystem");
        var m = new CodeMatcher(instructions);
        m.Clone();

        var patched = m.MatchForward(false,
                new CodeMatch(OpCodes.Ldarg_0),
                new CodeMatch(OpCodes.Call),
                new CodeMatch(OpCodes.Callvirt))
            .ThrowIfInvalid("Could not find target")
            .RemoveInstructions(7);

        return patched.InstructionEnumeration();
    }
}