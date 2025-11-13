namespace SubmersedVR.Tweaks;

public static class VehiclesVR
{
    public static Exosuit PilotedExosuit()
    {
        return Player.main?.currentMountedVehicle == null ? null : Player.main?.currentMountedVehicle as Exosuit;
    }
}