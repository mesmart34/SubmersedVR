namespace SubmersedVR.Tweaks;

public static class VehiclesVR
{
    public static Exosuit PilotedExosuit()
    {
        return Player.main?.currentMountedVehicle == null ? null : Player.main?.currentMountedVehicle as Exosuit;
    }
}

//This gets called when starting to pilot the seamoth and exosuit but not the cyclops

//This gets called when, while piloting the cyclops, camera mode is turned on

/*
[HarmonyPatch(typeof(SeaMoth), nameof(SeaMoth.OnPlayerEntered))]
static class RecenterInSeamoth
{
    public static void Postfix()
    {
        VRUtil.Recenter();
    }
}

[HarmonyPatch(typeof(Exosuit), nameof(Exosuit.OnPlayerEntered))]
static class RecenterInExosuit
{
    public static void Postfix()
    {
        VRUtil.Recenter();
    }
}
*/