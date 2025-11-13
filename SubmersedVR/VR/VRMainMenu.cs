using System.Linq;
using UnityEngine;

namespace SubmersedVR.VR;

public class VRMainMenu : MonoBehaviour
{
    // This simply makes the VRCameraRig rig steal the main menu cameras
    // TODO: Can probably be done without an extra behaviour.
    public static void SetupMainMenu()
    {
        var uiCamera = FindObjectsOfType<Camera>().First(c => c.name.Equals("UI Camera"));
        VRCameraRig.instance.StealUICamera(uiCamera);
        var mainCamera = GameObject.FindGameObjectsWithTag("MainCamera").First(c => c.name.Equals("Main Camera")).GetComponent<Camera>();
        VRCameraRig.instance.StealCamera(mainCamera);
    }
}