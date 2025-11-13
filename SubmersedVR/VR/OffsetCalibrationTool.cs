extern alias SteamVRRef;
using System;
using SteamVRRef::Valve.VR;
using SubmersedVR.Common;
using UnityEngine;

namespace SubmersedVR.VR;

extern alias SteamVRActions;
extern alias SteamVRRef;

// The following four methods are used to calibrate offsets of items/tools in hands
internal class OffsetCalibrationTool
{

    SteamVR_Action_Boolean holdToMoveAction;
    SteamVR_Action_Boolean saveTransformAction;
    Transform target;
    Transform parent;

    private OffsetCalibrationTool() { }

    public OffsetCalibrationTool(Transform target, SteamVR_Action_Boolean holdToMoveAction, SteamVR_Action_Boolean saveTransformAction)
    {
        this.target = target ?? throw new ArgumentNullException(nameof(target));
        this.holdToMoveAction = holdToMoveAction ?? throw new ArgumentNullException(nameof(holdToMoveAction));
        this.saveTransformAction = saveTransformAction ?? throw new ArgumentNullException(nameof(saveTransformAction));
    }

    private bool _enabled = false;
    public bool enabled
    {
        set
        {
            if (value == _enabled)
            {
                return;
            }
            if (value)
            {
                holdToMoveAction.onStateDown += UnparentTarget;
                holdToMoveAction.onStateUp += ReparentTarget;
                saveTransformAction.onStateDown += SaveTransform;
            }
            else
            {
                holdToMoveAction.onStateDown -= UnparentTarget;
                holdToMoveAction.onStateUp -= ReparentTarget;
                saveTransformAction.onStateDown -= SaveTransform;
            }
            _enabled = value;
        }
        get
        {
            return _enabled;
        }
    }

    // Unparent Target, so we can finetune the position
    private void UnparentTarget(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        parent = target.parent;
        Mod.logger.LogInfo($"Unparenting {target.name}, remembering {parent.name}");
        target.SetParent(null, true);
    }

    // Reparent the target to hand/controller again
    private void ReparentTarget(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        Mod.logger.LogInfo($"Reparenting {target.name} to {parent.name}");
        target.SetParent(parent, true);
        // var angles = target.localEulerAngles;
        // var snapAngle = 15;
        // target.localEulerAngles = new Vector3(angles.x.Snap(snapAngle), angles.y.Snap(snapAngle), angles.z.Snap(snapAngle));
        // target.localEulerAngles = new Vector3(angles.x, angles.y, angles.z);
    }

    // Save the transform by logging it to a logfile
    private void SaveTransform(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        // Print it out for putting it into the mod
        var tool = global::Player.main.armsController.lastTool;
        var targetName = tool?.GetType()?.Name ?? "Hands";
        var offset = new TransformOffset(target).SwitchString(targetName);
        Mod.logger.LogInfo(offset);
        ErrorMessage.AddDebug($"Saved Offset for {targetName}!\n{offset}");
    }
}