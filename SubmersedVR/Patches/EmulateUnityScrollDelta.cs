using HarmonyLib;
using SubmersedVR.Input;
using UnityEngine;

namespace SubmersedVR.Patches;

[HarmonyPatch(typeof(UnityEngine.Input), nameof(UnityEngine.Input.mouseScrollDelta), MethodType.Getter)]
static class EmulateUnityScrollDelta
{
    static bool Prefix(ref Vector2 __result)
    {
        __result = SteamVrGameInput.GetScrollDelta();
        return false;
    }
}