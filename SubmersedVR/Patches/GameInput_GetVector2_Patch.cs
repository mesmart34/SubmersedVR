extern alias SteamVRActions;
using HarmonyLib;
using SteamVRActions::Valve.VR;
using SubmersedVR.Input;
using SubmersedVR.VR;
using UnityEngine;

namespace SubmersedVR.Patches;

extern alias SteamVRRef;

[HarmonyPatch(typeof(GameInput), nameof(GameInput.GetVector2))]
public static class GameInput_GetVector2_Patch
{
    public static bool Prefix(GameInput.Button action, ref Vector2 __result)
    {
        var vec = Vector2.zero;
        if (SteamVrGameInput.InputLocked || !SteamVrGameInput.IsSteamVrReady || VRHands.instance == null)
        {
            return false;
        }

        switch (action)
        {
            case GameInput.Button.Look:
                vec = SteamVR_Actions.subnautica.Look.GetAxis(SteamVRRef::Valve.VR.SteamVR_Input_Sources.Any);

                // TODO: Add new setting
                var sensitivity = new Vector2(0.405f, 0.405f);

                var mag = vec.magnitude;
                var normVec = ((mag > 0f) ? (vec / mag) : Vector2.zero);
                mag = Mathf.Pow(mag, 2f) * 500f;
                vec = normVec * mag;
                vec *= sensitivity * Time.deltaTime;

                if (Settings.InvertYAxis)
                {
                    vec.y = -vec.y;
                }
                break;
            case GameInput.Button.Move:
                vec = SteamVR_Actions.subnautica.Move.GetAxis(SteamVRRef::Valve.VR.SteamVR_Input_Sources.Any);
                break;
        }

        __result = vec;

        return false;
    }
}