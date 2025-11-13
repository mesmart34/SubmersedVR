using HarmonyLib;
using SubmersedVR.Input;
using UnityEngine;

namespace SubmersedVR.Patches;

[HarmonyPatch(typeof(GameInput), nameof(GameInput.GetLookDelta))]
internal static class GameInput_GetLookDelta_Patch
{
    internal static void Postfix(ref Vector2 __result)
    {
        var isInVehicle = Player.main?.currentMountedVehicle != null;
        if (!Settings.IsSnapTurningEnabled || isInVehicle)
        {
            return;
        }
        
        var lookX = __result.x;
        var absX = Mathf.Abs(lookX);
        const float threshold = 0.5f;
        
        if (absX > threshold && !SteamVrGameInput.SnapTurned) {
            __result.x = Settings.SnapTurningAngle * Mathf.Sign(lookX);
            SteamVrGameInput.SnapTurned = true;
        } else  {
            __result.x = 0;
            if (absX <= threshold) {
                SteamVrGameInput.SnapTurned = false;
            }
        }
    }
}