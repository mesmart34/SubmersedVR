using SubmersedVR.Common;
using SubmersedVR.Utils;
using UnityEngine;

namespace SubmersedVR.VR;

public static class WristHud
{
    private static TransformOffset wristOffset = new(new Vector3(-0.079f, 0.148f, -0.158f), new Vector3(350.494f, 88.400f, 244.161f));
    private static GameObject wristTarget;
    private static Canvas canvas;
    private static CanvasGroup canvasGroup;

    // Cached Values
    private static Transform hudContent;
    private static Transform uiCamera;
    private static Transform cachedIndexTip;
    private static FMODAsset turnOnSound;
    private static FMODAsset turnOffSound;

    // State
    public static bool isHudOn = true;
    private static bool touchingWrist = false;
    private static bool prevTouchingWrist = false;

    public static FMODAsset CreateFMODAsset(string eventPath)
    {
        var asset = ScriptableObject.CreateInstance<FMODAsset>();
        asset.path = eventPath;
        return asset;
    }

    // Create Wrist World Canvas
    public static void Setup()
    {
        var rig = VRCameraRig.instance;
        uiCamera = rig.uiCamera.transform;
        hudContent = uGUI.main.hud.transform.GetChild(0);

        if (wristTarget == null)
        {
            wristTarget = new GameObject("WristTarget").WithParent(rig.leftControllerUI).ResetTransform();
            var wristCanvasGo = new GameObject("WristCanvas").WithParent(wristTarget).ResetTransform();
            canvas = wristCanvasGo.CreateWorldCanvas();
            canvasGroup = wristCanvasGo.AddComponent<CanvasGroup>();
            wristCanvasGo.transform.localScale = new Vector3(0.0004f, 0.0004f, 0.0004f);
            wristOffset.Apply(wristTarget.transform);
        }

        Settings.PutBarsOnWristChanged -= OnPutBarsOnHandChanged;
        Settings.PutBarsOnWristChanged += OnPutBarsOnHandChanged;
        Toggle(Settings.PutBarsOnWrist);

        turnOnSound = CreateFMODAsset("event:/tools/flashlight/turn_on");
        turnOffSound = CreateFMODAsset("event:/tools/flashlight/turn_off");
    }

    public static Transform GetIndexFingerTip()
    {
        if (cachedIndexTip != null)
        {
            return cachedIndexTip;
        }
        var animator = Player.main?.playerAnimator;
        if (animator is Animator anim)
        {
            var tip = anim.transform.Find("export_skeleton/head_rig/neck/chest/clav_R/clav_R_aim/shoulder_R/hand_R/hand_R_point_base/hand_R_point_mid/hand_R_point_tip_rig");
            if (tip != null)
            {
                cachedIndexTip = tip;
                return tip;
            }
        }
        return null;
    }

    public static void OnPutBarsOnHandChanged(bool isOn)
    {
        Toggle(isOn);
    }


    public static void OnUpdate()
    {
        if (!uGUI.isMainLevel)
        {
            return;
        }
        var camPos = uiCamera.transform.position;
        var worldRigPos = VRCameraRig.instance.rigParentTarget.position;
        var wristPos = wristTarget.transform.position;

        var wristDir = wristTarget.transform.TransformDirection(Vector3.forward);
        var toCam = (wristPos - camPos).normalized;

        var wristCamDot = Vector3.Dot(wristDir, toCam);
        var isFacingCamera = wristCamDot > 0.1f;
        // DebugPanel.Show($"dot = {dot} <= {wristDir}, {toCam}");
        canvasGroup.alpha = Mathf.Max(wristCamDot, 0.0f);

        if (isFacingCamera && GetIndexFingerTip() is Transform indexTip)
        {
            var uiIndexPos = indexTip.position - worldRigPos;
            var wristDistance = Vector3.Distance(uiIndexPos, wristPos);
            // DebugPanel.Show($"wristDistance = {wristDistance} <= uiPos{uiIndexPos}, {wristPos}");
            const float threshold = 0.1f;
            touchingWrist = wristDistance < threshold;
            if (touchingWrist && !prevTouchingWrist)
            {
                isHudOn = !isHudOn;
                global::Utils.PlayFMODAsset(isHudOn ? turnOnSound : turnOffSound);
            }
            prevTouchingWrist = wristDistance < threshold;
        }
    }

    public static void Toggle(bool isOn)
    {
        if (canvas == null)
        {
            Setup();
        }

        var barsPanel = uGUI.main.barsPanel;
        if (isOn)
        {
            // Move to wrist
            Mod.logger.LogDebug("Turning WristHud on");
            barsPanel.WithParent(canvas.transform).ResetTransform();
            barsPanel.GetComponent<RectTransform>().pivot = new Vector2(0, 0);
            ManagedUpdate.Subscribe(ManagedUpdate.Queue.PreCanvasFirst, new ManagedUpdate.OnUpdate(OnUpdate));
        }
        else
        {
            // Move back
            Mod.logger.LogDebug("Turning WristHud off");
            barsPanel.transform.SetParent(hudContent.transform, false);
            barsPanel.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
            barsPanel.GetComponent<RectTransform>().anchoredPosition = new Vector2(0.0f, 0.0f);
            ManagedUpdate.Unsubscribe(ManagedUpdate.Queue.PreCanvasFirst, new ManagedUpdate.OnUpdate(OnUpdate));
            isHudOn = true;
        }

    }
}