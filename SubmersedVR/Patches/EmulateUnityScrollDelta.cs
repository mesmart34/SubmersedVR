using HarmonyLib;
using SubmersedVR.Input;
using UnityEngine;

namespace SubmersedVR.Patches;

[HarmonyPatch(typeof(UnityEngine.Input), nameof(UnityEngine.Input.mouseScrollDelta), MethodType.Getter)]
internal static class EmulateUnityScrollDelta
{
    internal static bool Prefix(ref Vector2 __result)
    {
        __result = SteamVrGameInput.GetScrollDelta();
        return false;
    }
}