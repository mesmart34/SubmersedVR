using HarmonyLib;

namespace SubmersedVR.Patches;

extern alias SteamVRActions;
extern alias SteamVRRef;

[HarmonyPatch(typeof(GamepadInputModule), nameof(GamepadInputModule.IsInputAllowed))]
internal static class AllowGamepadUnfocused
{
    internal static bool Prefix(ref bool __result)
    {
        __result = !WaitScreen.IsWaiting;
        return false;
    }
}