using UnityEngine;

namespace SubmersedVR.VR;

public static class Aiming
{
    public static Camera GetAimCamera()
    {
        return VRCameraRig.instance?.laserPointer.eventCamera;
    }
    
    public static Transform GetAimTransform()
    {
        return VRCameraRig.instance?.laserPointer.transform;
    }
}