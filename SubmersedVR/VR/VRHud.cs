using SubmersedVR.Utils;
using UnityEngine;

namespace SubmersedVR.VR;

extern alias SteamVRActions;
extern alias SteamVRRef;

// Tweaks regarding the HUD of the game
public static class VRHud
{
    private static Transform screenCanvas;
    private static Transform overlayCanvas;
    private static Transform hud;

    private static Canvas staticHudCanvas = null;
    // private static OffsetCalibrationTool calibrationTool;

    // TODO: Hud Distance needs dedicated canvas, since the Pips seem to assume the 1 meter canvas distance.
#if false
        public static float hudDistance = 1.0f;
        public static float HudDistance {
            get {
                return hudDistance;
            }
            set {
                hudDistance = value;
                if (staticHudCanvas == null || screenCanvas == null) {
                    return;
                }
                hud.transform.localPosition = Vector3.forward * (hudDistance - 1.0f);
            }
        }
        public static void OnHudDistanceChanged(float value) {
            HudDistance = value;
        }
#endif

    public static void SetupHandReticle(bool onLaserPointer, Camera uiCamera, Transform rightControllerUI)
    {
        if (onLaserPointer)
        {
            SetupHandReticleLaserPointer(uiCamera, rightControllerUI);
        }
        else
        {
            SetupHandReticleOnHand(uiCamera, rightControllerUI);
        }
    }

    public static void SetupHandReticleOnHand(Camera uiCamera, Transform rightControllerUI)
    {
        // Steal Reticle and attach to the right hand
        var handReticle = HandReticle.main.gameObject.WithParent(rightControllerUI.transform);
        handReticle.GetOrAddComponent<Canvas>().worldCamera = uiCamera;
        handReticle.transform.localEulerAngles = new Vector3(90, 0, 0);
        handReticle.transform.localPosition = new Vector3(0, 0, 0.05f);
        handReticle.transform.localScale = new Vector3(0.001f, 0.001f, 0.001f);
    }

    public static void SetupHandReticleLaserPointer(Camera uiCamera, Transform rightControllerUI)
    {
        var handReticle = HandReticle.main.gameObject.WithParent(VRCameraRig.instance.laserPointerUI.pointerDot.transform);
        handReticle.transform.LookAt(uiCamera.transform.position);
        handReticle.transform.localRotation = Quaternion.Euler(40, 0, 0);
        handReticle.transform.localPosition = new Vector3(0, -5, VRCameraRig.instance.laserPointerUI.pointerDot.transform.localPosition.z);//new Vector3(0, 0, 0.05f);
        handReticle.transform.localScale = VRCameraRig.instance.laserPointerUI.pointerDot.transform.localScale * 2;//new Vector3(0.001f, 0.001f, 0.001f);
    }

    public static void OnHandReticleSettingChanged(bool onLaserPointer)
    {
        var rig = VRCameraRig.instance;
        if (!rig)
        {
            return;
        }
        SetupHandReticle(onLaserPointer, rig.uiCamera, rig.rightControllerUI.transform);
    }

    public static Canvas CreateWorldCanvas(this GameObject go)
    {
        var canvas = go.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;
        go.layer = LayerID.UI;
        return canvas;
    }

    public static void Setup(Camera uiCamera, Transform rightControllerUI)
    {
        Mod.logger.LogDebug($"Setting up HUD for {uiCamera.name}");

        screenCanvas = uGUI.main.screenCanvas.gameObject.transform;
        overlayCanvas = uGUI.main.overlays.gameObject.transform.parent;
        hud = uGUI.main.hud.transform;

        if (staticHudCanvas == null)
        {
            var uiRig = VRCameraRig.instance.uiRig.transform;
            var go = new GameObject("StaticHUDCanvas").WithParent(uiRig);
            staticHudCanvas = go.CreateWorldCanvas();
            var rt = go.GetComponent<RectTransform>();
            go.transform.localScale = screenCanvas.localScale;
            rt.sizeDelta = screenCanvas.GetComponent<RectTransform>().sizeDelta;
            rt.anchoredPosition = screenCanvas.GetComponent<RectTransform>().anchoredPosition;
            go.transform.localPosition = Vector3.forward;
            go.transform.localRotation = Quaternion.identity;
        }
        staticHudCanvas.worldCamera = uiCamera;

        screenCanvas.SetParent(uiCamera.transform, true);
        overlayCanvas.SetParent(uiCamera.transform, true);

        SetupHandReticle(Settings.PutHandReticleOnLaserPointer, uiCamera, rightControllerUI);
        Settings.PutHandReticleOnLaserPointerChanged -= OnHandReticleSettingChanged;
        Settings.PutHandReticleOnLaserPointerChanged += OnHandReticleSettingChanged;

        WristHud.Setup();

        var compo = screenCanvas.GetComponent<uGUI_CanvasScaler>();
        if (compo != null)
        {
            compo.SetDirty();
        }
        screenCanvas.GetComponentsInChildren<uGUI_CanvasScaler>().ForEach(cs => cs.SetDirty());
    }

    public static void OnEnterVehicle()
    {
        var player = Player.main;
        if (player != null)
        {
            hud.SetParent(staticHudCanvas.transform, false);
        }
    }

    public static void OnExitVehicle()
    {
        hud.SetParent(screenCanvas, false);
    }
}