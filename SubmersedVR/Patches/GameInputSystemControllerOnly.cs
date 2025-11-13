using HarmonyLib;

namespace SubmersedVR.Patches;

[HarmonyPatch(typeof(GameInputSystem), nameof(GameInput.PrimaryDevice), MethodType.Getter)]
public static class GameInputSystemControllerOnly
{
    public static bool Prefix(ref GameInput.Device __result)
    {
        __result = GameInput.Device.Controller;
        return false;
    }
}