extern alias SteamVRRef;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using SteamVRRef::Valve.VR;
using SubmersedVR.Common;
using SubmersedVR.Input;
using SubmersedVR.Tweaks;
using SubmersedVR.Utils;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.XR;
using UWE;

/*
The VRCamera Rig handles the controllers together with their laser pointers to control the UI.
TODO: This class and file does too much at the moment. Need to refactor stuff out and clean things up a bit more.
*/
namespace SubmersedVR.VR;

extern alias SteamVRActions;
extern alias SteamVRRef;

public class VRCameraRig : MonoBehaviour
{
    // Setup and created in Start()
    public Camera vrCamera;
    public GameObject leftController;
    public GameObject rightController;
    // Those are used for the IK/Hands
    public GameObject leftHandTarget;
    public GameObject rightHandTarget;

    // TODO: Those should not be full laserpointers probably
    // The only thing I need is cameras at different positions for the UI or Worldspace Raycasts
    public LaserPointer laserPointer;
    public LaserPointer laserPointerLeft;

    public GameObject uiRig;
    public GameObject leftControllerUI;
    public GameObject rightControllerUI;
    public LaserPointer laserPointerUI;

    public GameObject modelL;
    public GameObject modelR;

    public static VRCameraRig instance;

    public Camera uiCamera = null;
    public GameObject worldTarget;
    public float worldTargetDistance;
    public Transform rigParentTarget;

    public Camera UIControllerCamera
    {
        get
        {
            if (laserPointerUI == null)
            {
                return null;
            }
            return laserPointerUI.eventCamera;
        }
    }
    public Camera WorldControllerCamera
    {
        get
        {
            if (laserPointer == null)
            {
                return null;
            }
            return laserPointer.eventCamera;
        }
    }

    private FPSInputModule fpsInput = null;

    // This transfroms forward vector determines where the equiped tool will be aiming
    public static readonly TransformOffset DefaultTargetTransform = new(Vector3.zero, new Vector3(45, 0, 0));
    private TransformOffset _targetTransform;
    public VRQuickSlots VrQuickSlots;

    public TransformOffset TargetTransform
    {
        get
        {
            return _targetTransform;
        }
        set
        {
            _targetTransform = value;
            value.Apply(laserPointerUI.transform);
            value.Apply(laserPointer.transform);
            value.Apply(laserPointerLeft.transform);
        }
    }

    public static Transform GetTargetTansform()
    {
        return VRCameraRig.instance.laserPointer.transform;
    }
    public static Transform GetLeftTargetTansform()
    {
        return VRCameraRig.instance.laserPointerLeft.transform;
    }

    public void SetCameraTrackTarget(Transform target)
    {
        this.rigParentTarget = target;
    }

    public void SetupControllers()
    {
        // TODO: Naming is inconsistent, clean this mess up, only need 1/2 pointers?
        leftController = new GameObject(nameof(leftController)).WithParent(transform);
        rightController = new GameObject(nameof(rightController)).WithParent(transform);

        leftController.SetActive(false);
        rightController.SetActive(false);
        var controller = leftController.AddComponent<SteamVRRef.Valve.VR.SteamVR_Behaviour_Pose>();
        controller.inputSource = SteamVRRef.Valve.VR.SteamVR_Input_Sources.LeftHand;
        controller.poseAction = SteamVRActions.Valve.VR.SteamVR_Actions.subnautica_LeftHandPose;
        controller = rightController.AddComponent<SteamVRRef.Valve.VR.SteamVR_Behaviour_Pose>();
        controller.inputSource = SteamVRRef.Valve.VR.SteamVR_Input_Sources.RightHand;
        controller.poseAction = SteamVRActions.Valve.VR.SteamVR_Actions.subnautica_RightHandPose;
        leftController.SetActive(true);
        rightController.SetActive(true);

        leftHandTarget = new GameObject(nameof(leftHandTarget)).WithParent(leftController);
        rightHandTarget = new GameObject(nameof(rightHandTarget)).WithParent(rightController);
        leftHandTarget.transform.localEulerAngles = new Vector3(270, 90, 0);
        var handOffset = new Vector3(90, 270, 0);
        rightHandTarget.transform.localEulerAngles = handOffset;

        // Laser Pointer Setup
        laserPointer = new GameObject(nameof(laserPointer)).WithParent(rightController.transform).AddComponent<LaserPointer>();
        laserPointerLeft = new GameObject(nameof(laserPointerLeft)).WithParent(leftController.transform).AddComponent<LaserPointer>();
        laserPointerLeft.gameObject.SetActive(false);
        // laserPointer.gameObject.SetActive(false);
        laserPointer.disableAfterCreation = true;

        // NOTE: These laserpointer and controllers is NOT parented to the Rig, since they act in UI space, not world space
        uiRig = new GameObject(nameof(uiRig));
        Object.DontDestroyOnLoad(uiRig);
        leftControllerUI = new GameObject(nameof(leftControllerUI)).WithParent(uiRig.transform);
        rightControllerUI = new GameObject(nameof(rightControllerUI)).WithParent(uiRig.transform);
        laserPointerUI = new GameObject(nameof(laserPointerUI)).WithParent(rightControllerUI.transform).AddComponent<LaserPointer>();
        // TODO: Constructors possible?
        laserPointerUI.doWorldRaycasts = true;
        laserPointerUI.useUILayer = true;

        leftControllerUI.SetActive(false);
        rightControllerUI.SetActive(false);
        controller = leftControllerUI.AddComponent<SteamVRRef.Valve.VR.SteamVR_Behaviour_Pose>();
        controller.inputSource = SteamVRRef.Valve.VR.SteamVR_Input_Sources.LeftHand;
        controller.poseAction = SteamVRActions.Valve.VR.SteamVR_Actions.subnautica_LeftHandPose;
        controller = rightControllerUI.AddComponent<SteamVRRef.Valve.VR.SteamVR_Behaviour_Pose>();
        controller.inputSource = SteamVRRef.Valve.VR.SteamVR_Input_Sources.RightHand;
        controller.poseAction = SteamVRActions.Valve.VR.SteamVR_Actions.subnautica_RightHandPose;
        leftControllerUI.SetActive(true);
        rightControllerUI.SetActive(true);
        TargetTransform = DefaultTargetTransform;

        SetupControllerModels();

        // Connect Input module and layer pointer together
        // TODO: This should be easier using singleton setup
        fpsInput = FindObjectOfType<FPSInputModule>();
        laserPointer.inputModule = fpsInput;
        laserPointerLeft.inputModule = fpsInput;
        laserPointerUI.inputModule = fpsInput;
    }

    public void Awake()
    {
        SteamVR.Initialize();
        SteamVR.settings.trackingSpace = ETrackingUniverseOrigin.TrackingUniverseSeated;
        SteamVrGameInput.IsSteamVrReady = SteamVR.initializedState == SteamVR.InitializedStates.InitializeSuccess;
    }

    public void Start()
    {
        Settings.AmbientOcclusionSettingsChanged -= OnAmbientOcclusionSettingsChanged;
        Settings.AmbientOcclusionSettingsChanged += OnAmbientOcclusionSettingsChanged;

        SetupControllers();
        StartCoroutine(DelayedRecenter(1.0f));
    }

    public IEnumerator DelayedRecenter(float delay)
    {
        yield return new WaitForSeconds(delay);
        VRUtil.Recenter();
    }

    private void SetupControllerModels()
    {
        modelL = new GameObject(nameof(modelL)).WithParent(leftControllerUI).ResetTransform();
        modelR = new GameObject(nameof(modelR)).WithParent(rightControllerUI).ResetTransform();

        var model = modelR.AddComponent<SteamVRRef.Valve.VR.SteamVR_RenderModel>();
        model.SetInputSource(SteamVRRef.Valve.VR.SteamVR_Input_Sources.RightHand);
        model = modelL.AddComponent<SteamVRRef.Valve.VR.SteamVR_RenderModel>();
        model.SetInputSource(SteamVRRef.Valve.VR.SteamVR_Input_Sources.LeftHand);
        modelL.layer = LayerID.UI;
        modelR.layer = LayerID.UI;

        Settings.AlwaysShowControllersChanged += (_) => { UpdateShowControllers(); };
    }

    public void UpdateShowControllers()
    {
        var inMainMenu = !uGUI.isMainLevel;
        var alwaysShow = Settings.AlwaysShowControllers;
        modelL?.SetActive(alwaysShow || inMainMenu);
        modelR?.SetActive(alwaysShow || inMainMenu);
    }

    public static void OnAmbientOcclusionSettingsChanged()
    {
        AmbientOcclusionVR.OnAmbientOcclusionSettingsChanged(VRCameraRig.instance.vrCamera);
    }

    // This is used to get the camera from the main menu
    // Main issue with making a new camera was the water surface but that should also be fixable
    // TODO: Maybe remove this, so we only have one common camera
    public void StealCamera(Camera camera)
    {
        // Destroy/Delete old camera
        // NOTE: Subnautica renderes the water using specific camera component which also renders when the camera is disabled

        if (camera != vrCamera && vrCamera != null)
        {
            vrCamera.enabled = false;
            Destroy(vrCamera.gameObject);
        }

        vrCamera = camera;
        var oldPos = camera.transform.position;
        transform.position = oldPos;
        vrCamera.transform.parent = this.transform;

        AmbientOcclusionVR.AddOcclusionEffect(vrCamera);
    }

    public void StealUICamera(Camera camera, bool fromGame = false)
    {
        uiRig.transform.SetPositionAndRotation(camera.transform.position, camera.transform.rotation);
        if (uiCamera != null)
        {
            uiCamera.transform.DetachChildren();
            Destroy(uiCamera.gameObject);
        }

        if (fromGame)
        {
            // This fixes a weird issue I had, where the UI Camera from the game would behave like it wasnt moving
            // even though the transform was changing properly.
            // Maybe it is because the tracking was once disabled in the main game, but I am not sure, since I tried enabling it too.
            // Copying the properties from the main camera and setting up the original important properties fixed it.
            uiRig.transform.position = Vector3.zero;
            var oldMask = camera.cullingMask;
            var oldClear = camera.clearFlags;
            var oldDepth = camera.depth;

            camera.CopyFrom(SNCameraRoot.main.mainCamera);
            camera.transform.localPosition = Vector3.zero;
            camera.transform.localRotation = Quaternion.identity;
            camera.renderingPath = RenderingPath.Forward;
            camera.cullingMask = oldMask;
            camera.clearFlags = CameraClearFlags.Depth;
            camera.depth = oldDepth;

            camera.transform.parent = uiRig.transform;
            camera.transform.localPosition = Vector3.zero;
            camera.transform.localRotation = Quaternion.identity;

            // Set all canvas scalers to static, which makes UI better usable
            FindObjectsOfType<uGUI_CanvasScaler>().Where(obj => !obj.name.Contains("PDA")).ForEach(cs => cs.vrMode = uGUI_CanvasScaler.Mode.Static);
            SetupPDA();
            VrQuickSlots = new GameObject("VRQuickSlots").ResetTransform().AddComponent<VRQuickSlots>();
            VrQuickSlots.Setup(SteamVRActions::Valve.VR.SteamVR_Actions.subnautica_OpenQuickSlotWheel);
        }
        else
        {
            camera.transform.parent = uiRig.transform;
            camera.transform.localPosition = Vector3.zero;
            camera.transform.localRotation = Quaternion.identity;
        }
        uiCamera = camera;
        VRHud.Setup(uiCamera, rightControllerUI.transform);
    }

    void SetupPDA()
    {
        // Move the quickslots to bottom of PDA bottom left and make it bigger
        var pda = uGUI_PDA.main;
        var targetParent = pda.tabInventory.transform;
        var qs = FindObjectOfType<uGUI_QuickSlots>();
        var qstf = qs.transform;

        qstf.parent = targetParent;
        qstf.localPosition = new Vector3(-250, -455, 4f);
        qstf.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        qstf.localRotation = Quaternion.identity;

        // Add Pasuse Menu Button to PDA to PDA
        var dialog = pda.GetComponentInChildren<uGUI_Dialog>(true);
        var buttonPrefab = dialog.buttonPrefab;
        var button = Object.Instantiate(buttonPrefab, targetParent).GetComponent<uGUI_DialogButton>();
        button.button.transform.parent = targetParent;
        button.button.gameObject.gameObject.name = "PauseMenuButton";
        button.text.text = "Pause Menu";
        button.button.onClick.RemoveAllListeners();
        button.button.onClick.AddListener(() =>
        {
            IngameMenu.main.Open();
        });
        // Move it to the bottom right
        button.rectTransform.anchoredPosition = new Vector2(1100, 50);
        button.rectTransform.pivot = new Vector2(1, 0);
        button.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 300);
        button.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 100);
        button.rectTransform.ForceUpdateRectTransforms();
        button.rectTransform.GetComponentsInChildren<RectTransform>().ForEach(rt => rt.ForceUpdateRectTransforms());
    }

    public IEnumerator SetupGameCameras()
    {
        var rig = VRCameraRig.instance;
        rig.StealCamera(SNCameraRoot.main.mainCamera);
        yield return new WaitForSeconds(1.0f);
        rig.StealUICamera(SNCameraRoot.main.guiCamera, true);
        yield return new WaitForSeconds(0.1f);

        FindObjectsOfType<uGUI_CanvasScaler>().ForEach(cs => cs.SetDirty());
    }

    public void LateUpdate()
    {
        // Move the camera rig to the player each frame and rotate the uiRig accordingly
        // TODO: This probably has to be changed for roomscale tracking.
        // Right now if you move too far away from the center, you will rotate the camera with the center as a pivot.
        if (rigParentTarget != null)
        {
            this.transform.SetPositionAndRotation(rigParentTarget.position, rigParentTarget.rotation);
            uiRig.transform.rotation = transform.rotation;
            /*TODO
                            RecenterBodyOnCameraOrientation(35f, 0.3f, 3.0f, 1.5f);

                            float zOffset = Player.main?.inSeatruckPilotingChair == true || Player.main?.inExosuit == true ? -0.2f : -0.08f;
                            float yOffset = Player.main?.inSeatruckPilotingChair == true || Player.main?.inExosuit == true ? -0.2f : -0.1f;
                            if(Player.main._cinematicModeActive == false)
                            {
                                Player.main.armsController.transform.position = SNCameraRoot.main.mainCamera.transform.position + (SNCameraRoot.main.mainCamera.transform.forward * zOffset) + new Vector3(0f, yOffset, 0f);
                                //Player.main.armsController.transform.position = MainCameraControl.main.transform.position + (MainCameraControl.main.transform.forward * zOffset) + new Vector3(0f, yOffset, 0f);
                            }
            */
        }
    }

    void DebugRaycasts()
    {
        if (false && Settings.IsDebugEnabled)
        {
            var uiTarget = fpsInput?.lastRaycastResult;
            DebugPanel.Show($"World Target: {worldTarget?.name}({worldTargetDistance})\nUI Target:{uiTarget?.gameObject?.name}({uiTarget?.distance})\nFocused: {EventSystem.current.isFocused}");
        }
    }

    // Gets set by GUIHand Patch, which already does world raycasting so we dont have to do it ourselfs
    public void SetWorldTarget(GameObject activeTarget, float activeHitDistance)
    {
        this.worldTarget = activeTarget;
        this.worldTargetDistance = activeHitDistance;
        this.laserPointerUI.SetWorldTarget(worldTarget, worldTargetDistance);
    }
}