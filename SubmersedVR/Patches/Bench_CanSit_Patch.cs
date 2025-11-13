using HarmonyLib;

namespace SubmersedVR.Patches;

[HarmonyPatch(typeof(Bench), nameof(Bench.CanSit))]
internal static class Bench_CanSit_Patch
{
    internal static bool Prefix(ref bool __result)
    {
        __result = true;
        return false;
    }
}