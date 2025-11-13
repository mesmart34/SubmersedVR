using HarmonyLib;

namespace SubmersedVR.Patches;

[HarmonyPatch(typeof(GameInput), nameof(GameInput.IsPrimaryDeviceGamepad))]
public static class ControllerOnly
{
    public static bool Prefix(ref bool __result)
    {
        __result = true;
        return false;
    }
}