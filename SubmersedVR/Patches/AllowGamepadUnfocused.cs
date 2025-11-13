using HarmonyLib;

namespace SubmersedVR.Patches;

extern alias SteamVRActions;
extern alias SteamVRRef;

// Raycast from the middle of the event "camera" on the controller for accurate laserpointing

// Since we do the dragging/raycasting in worldspace now the drag threshold has to be way lower.
// Can't change the EventSystem.pixelThreshold because that is only integer.

// Instead of saving the screen space position of the pointer, we save the world space one. Needed to fix things like drag & drop.

// Use UICancel(uGUI.button2) as middle mouse button, which makes certain pointer ui work better.
// It's needed to get X to work for pinning recipes in the fabricator.
// TODO: Might be better to rewrite this using transpiler or rewrite completely.
// In any case it's painful because you gotta call base methods: https://harmony.pardeike.net/articles/patching-edgecases.html#calling-base-methods

// Makes it so that you can still interact with the UI, even when the Game is not focused, which only makes sense in VR I guess.

// TODO: Not sure if needed. Is the GamepadInputModule used?
// Wonder if it actually conflicts with the FPSInputModule or not
[HarmonyPatch(typeof(GamepadInputModule), nameof(GamepadInputModule.IsInputAllowed))]
static class AllowGamepadUnfocused
{
    public static bool Prefix(ref bool __result)
    {
        __result = !WaitScreen.IsWaiting; // && Application.isFocused;
        return false;
    }
}