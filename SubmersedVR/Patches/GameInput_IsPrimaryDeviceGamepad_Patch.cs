using HarmonyLib;

namespace SubmersedVR.Patches;

[HarmonyPatch(typeof(GameInput), nameof(GameInput.IsPrimaryDeviceGamepad))]
public static class GameInput_IsPrimaryDeviceGamepad_Patch
{
    public static bool Prefix(ref bool __result)
    {
        __result = true;
        return false;
    }
}