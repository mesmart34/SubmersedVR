using HarmonyLib;

namespace SubmersedVR.Patches;

[HarmonyPatch(typeof(GameInputSystem), nameof(GameInput.PrimaryDevice), MethodType.Getter)]
internal static class GameInputSystem_PrimaryDevice_Patch
{
    internal static bool Prefix(ref GameInput.Device __result)
    {
        __result = GameInput.Device.Controller;
        return false;
    }
}