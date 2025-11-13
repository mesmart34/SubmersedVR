using HarmonyLib;
using SubmersedVR.Input;

namespace SubmersedVR.Patches;

[HarmonyPatch(typeof(WaitScreen), nameof(WaitScreen.Update))]
static class LockInputWhileLoading
{
    static void Postfix(WaitScreen __instance)
    {
        SteamVrGameInput.InputLocked = __instance.isWaiting;
    }
}