using System.Runtime.CompilerServices;
using HarmonyLib;
using UnityEngine.EventSystems;

namespace SubmersedVR.Patches;

[HarmonyPatch(typeof(FPSInputModule), nameof(FPSInputModule.UpdateMouseState))]
class EmulateMiddleMosueButtonToo : PointerInputModule
{
    [HarmonyReversePatch]
    [HarmonyPatch(typeof(PointerInputModule), "GetPointerData")]
    [MethodImpl(MethodImplOptions.NoInlining)]
    static bool GetPointerData(FPSInputModule instance, int id, out PointerEventData data, bool create)
    {
        data = null;
        return false;
    }

    public static void Postfix(FPSInputModule __instance, PointerEventData leftData)
    {
        GetPointerData(__instance, -3, out var data2, create: true);
        __instance.CopyFromTo(leftData, data2);
        data2.button = PointerEventData.InputButton.Middle;
        if (GameInput.PrimaryDevice == GameInput.Device.Controller)
        {
            var buttonDown = GameInput.GetButtonDown(GameInput.button2);
            var buttonUp = GameInput.GetButtonUp(GameInput.button2);
            if (__instance.m_MouseState.GetButtonState(PointerEventData.InputButton.Middle).eventData.buttonState == PointerEventData.FramePressState.NotChanged)
            {
                __instance.m_MouseState.SetButtonState(PointerEventData.InputButton.Middle, FPSInputModule.ConstructPressState(buttonDown, buttonUp), data2);
            }
        }
    }

    public override void Process()
    {
    }
}