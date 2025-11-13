extern alias SteamVRRef;
extern alias SteamVRActions;
using SteamVRActions::Valve.VR;
using SubmersedVR.Common;
using UnityEngine;
using SteamVR_Behaviour_Pose = SteamVRRef::Valve.VR.SteamVR_Behaviour_Pose;
using SteamVR_Input_Sources = SteamVRRef::Valve.VR.SteamVR_Input_Sources;
using SteamVR_RenderModel = SteamVRRef::Valve.VR.SteamVR_RenderModel;

namespace SubmersedVR.VR;

public class MotionController : MonoBehaviour
{
    private SteamVR_Behaviour_Pose _behaviourPose;
    private GameObject _controllerModel;

    public void Initialize(HandType handType, Transform parent)
    {
        _controllerModel = new GameObject($"{nameof(MotionController)}:{handType}");
        var renderModel = _controllerModel.AddComponent<SteamVR_RenderModel>();
        _controllerModel.layer = LayerID.UI;
        
        switch (handType)
        {
            case HandType.Left:
                _behaviourPose = gameObject.AddComponent<SteamVR_Behaviour_Pose>();
                _behaviourPose.inputSource = SteamVR_Input_Sources.LeftHand;
                _behaviourPose.poseAction = SteamVR_Actions.subnautica_LeftHandPose;
                renderModel.SetInputSource(SteamVR_Input_Sources.LeftHand);
                break;
            case HandType.Right:
                _behaviourPose = gameObject.AddComponent<SteamVR_Behaviour_Pose>();
                _behaviourPose.inputSource = SteamVR_Input_Sources.RightHand;
                _behaviourPose.poseAction = SteamVR_Actions.subnautica_RightHandPose;
                renderModel.SetInputSource(SteamVR_Input_Sources.RightHand);
                break;
        }

        gameObject.transform.SetParent(parent);
        
        Settings.AlwaysShowControllersChanged += (_) => { UpdateShowControllers(); };
    }
    
    private void UpdateShowControllers()
    {
        var inMainMenu = !uGUI.isMainLevel;
        var alwaysShow = Settings.AlwaysShowControllers;
        _controllerModel?.SetActive(alwaysShow || inMainMenu);
    }
}