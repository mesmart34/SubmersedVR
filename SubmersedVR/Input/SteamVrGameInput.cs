extern alias SteamVRActions;
using System;
using SteamVRActions::Valve.VR;
using UnityEngine;

#pragma warning disable Harmony003
namespace SubmersedVR.Input;

extern alias SteamVRRef;
extern alias SteamVRActions;

public static class SteamVrGameInput
{
    public static bool InputLocked = false;
    public static bool IsSteamVrReady = false;
    public static bool SnapTurned = false;

    public static bool ShouldIgnore(GameInput.Button action)
    {
        return !IsSteamVrReady || InputLocked
                               || action == GameInput.Button.Slot1
                               || action == GameInput.Button.Slot2
                               || action == GameInput.Button.Slot3
                               || action == GameInput.Button.Slot4
                               || action == GameInput.Button.Slot5
                               || action == GameInput.Button.AutoMove
                               || action.ToString() == "45" || action.ToString() == "46";
    }

    public static Vector2 GetScrollDelta()
    {
        if (!IsSteamVrReady || InputLocked)
        {
            return Vector2.zero;
        }
        return SteamVR_Actions.subnautica.UIScroll.GetAxis(SteamVRRef::Valve.VR.SteamVR_Input_Sources.Any);
    }
}