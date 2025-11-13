extern alias SteamVRRef;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using RootMotion.FinalIK;
using SteamVRRef::Valve.VR;
using SubmersedVR.Common;
using UnityEngine;

namespace SubmersedVR.VR;

extern alias SteamVRActions;
extern alias SteamVRRef;

// Define all the Offsets

internal class VRHands : MonoBehaviour
{
    public FullBodyBipedIK ik = null;

    public Transform leftTarget;
    public Transform rightTarget;

    public Transform leftHand;
    public Transform rightHand;
    public Transform leftElbow;
    public Transform rightElbow;

    private Vector3 leftElbowOffset;
    private Vector3 rightElbowOffset;

    public static VRHands instance;

    public Transform[] leftHandFingers;
    public Transform[] rightHandFingers;
    public Vector3[] minRotation;
    public Vector3[] maxRotation;
    public int currentEditFinger = (int)HandSkeletonBone.eBone_IndexFinger1;

    // private OffsetCalibrationTool calibrationTool;

    public void Setup(FullBodyBipedIK ik)
    {
        instance = this;
        this.ik = ik;

        leftHand = ik.solver.leftHandEffector.bone;
        rightHand = ik.solver.rightHandEffector.bone;
        leftElbow = leftHand.parent;
        rightElbow = rightHand.parent;
        leftHand.parent = leftElbow.parent;
        rightHand.parent = rightElbow.parent;

        var camRig = VRCameraRig.instance;
        leftTarget = camRig.leftHandTarget.transform;
        rightTarget = camRig.rightHandTarget.transform;

        leftElbowOffset = leftElbow.transform.position - leftHand.transform.position;
        rightElbowOffset = rightElbow.transform.position - rightHand.transform.position;

        ResetHandTargets();

        UpdateBody();
        //StartCoroutine(DisableBodyRendering());

        //var laserPointer = VRCameraRig.instance.laserPointerUI.transform;

        SetupFingers();
        /*
                    var calibrationTool = new OffsetCalibrationTool(rightTarget, SteamVR_Actions.subnautica_MoveDown, SteamVR_Actions.subnautica_AltTool);
                    calibrationTool.enabled = Settings.IsDebugEnabled;
                    Settings.IsDebugChanged += (enabled) =>
                    {
                        calibrationTool.enabled = enabled;
                    };
        */
    }

    public void ResetHandTargets()
    {
        HandOffsets.LeftHand.Apply(leftTarget);
        HandOffsets.RightHand.Apply(rightTarget);

    }
    public void OnOpenPDA()
    {
        HandOffsets.PDA.Apply(leftTarget);
    }
    public void OnClosePDA()
    {
        ResetHandTargets();
    }

    public void SetupFingers()
    {
        var boneNamesLeft = new string[(int)HandSkeletonBone.eBone_Count];
        boneNamesLeft[(int)HandSkeletonBone.eBone_Thumb1] = "/hand_L_thumb_base";
        boneNamesLeft[(int)HandSkeletonBone.eBone_Thumb2] = "/hand_L_thumb_base/hand_L_thumb_mid";
        boneNamesLeft[(int)HandSkeletonBone.eBone_Thumb3] = "/hand_L_thumb_base/hand_L_thumb_mid/hand_L_thumb_tip";
        boneNamesLeft[(int)HandSkeletonBone.eBone_IndexFinger1] = "/hand_L_point_base";
        boneNamesLeft[(int)HandSkeletonBone.eBone_IndexFinger2] = "/hand_L_point_base/hand_L_point_mid";
        boneNamesLeft[(int)HandSkeletonBone.eBone_IndexFinger3] = "/hand_L_point_base/hand_L_point_mid/hand_L_point_tip";
        boneNamesLeft[(int)HandSkeletonBone.eBone_MiddleFinger1] = "/hand_L_midl_base";
        boneNamesLeft[(int)HandSkeletonBone.eBone_MiddleFinger2] = "/hand_L_midl_base/hand_L_midl_mid";
        boneNamesLeft[(int)HandSkeletonBone.eBone_MiddleFinger3] = "/hand_L_midl_base/hand_L_midl_mid/hand_L_midl_tip";
        boneNamesLeft[(int)HandSkeletonBone.eBone_RingFinger1] = "/hand_L_ring_base";
        boneNamesLeft[(int)HandSkeletonBone.eBone_RingFinger2] = "/hand_L_ring_base/hand_L_ring_mid";
        boneNamesLeft[(int)HandSkeletonBone.eBone_RingFinger3] = "/hand_L_ring_base/hand_L_ring_mid/hand_L_ring_tip";
        boneNamesLeft[(int)HandSkeletonBone.eBone_PinkyFinger1] = "/hand_L_pinky_base";
        boneNamesLeft[(int)HandSkeletonBone.eBone_PinkyFinger2] = "/hand_L_pinky_base/hand_L_pinky_mid";
        boneNamesLeft[(int)HandSkeletonBone.eBone_PinkyFinger3] = "/hand_L_pinky_base/hand_L_pinky_mid/hand_L_pinky_tip";

        var boneNamesRight = new string[(int)HandSkeletonBone.eBone_Count];
        boneNamesRight[(int)HandSkeletonBone.eBone_Thumb1] = "/hand_R_thumb_base";
        boneNamesRight[(int)HandSkeletonBone.eBone_Thumb2] = "/hand_R_thumb_base/hand_R_thumb_mid";
        boneNamesRight[(int)HandSkeletonBone.eBone_Thumb3] = "/hand_R_thumb_base/hand_R_thumb_mid/hand_R_thumb_tip_rig";
        boneNamesRight[(int)HandSkeletonBone.eBone_IndexFinger1] = "/hand_R_point_base";
        boneNamesRight[(int)HandSkeletonBone.eBone_IndexFinger2] = "/hand_R_point_base/hand_R_point_mid";
        boneNamesRight[(int)HandSkeletonBone.eBone_IndexFinger3] = "/hand_R_point_base/hand_R_point_mid/hand_R_point_tip_rig";
        boneNamesRight[(int)HandSkeletonBone.eBone_MiddleFinger1] = "/hand_R_midl_base";
        boneNamesRight[(int)HandSkeletonBone.eBone_MiddleFinger2] = "/hand_R_midl_base/hand_R_midl_mid";
        boneNamesRight[(int)HandSkeletonBone.eBone_MiddleFinger3] = "/hand_R_midl_base/hand_R_midl_mid/hand_R_midl_tip_rig";
        boneNamesRight[(int)HandSkeletonBone.eBone_RingFinger1] = "/hand_R_ring_base";
        boneNamesRight[(int)HandSkeletonBone.eBone_RingFinger2] = "/hand_R_ring_base/hand_R_ring_mid";
        boneNamesRight[(int)HandSkeletonBone.eBone_RingFinger3] = "/hand_R_ring_base/hand_R_ring_mid/hand_R_ring_tip_rig";
        boneNamesRight[(int)HandSkeletonBone.eBone_PinkyFinger1] = "/hand_R_pinky_base";
        boneNamesRight[(int)HandSkeletonBone.eBone_PinkyFinger2] = "/hand_R_pinky_base/hand_R_pinky_mid";
        boneNamesRight[(int)HandSkeletonBone.eBone_PinkyFinger3] = "/hand_R_pinky_base/hand_R_pinky_mid/hand_R_pinky_tip_rig";

        minRotation = new Vector3[(int)HandSkeletonBone.eBone_Count];
        for (var i = 0; i < minRotation.Length; i++)
        {
            minRotation[i] = Vector3.zero;
        }

        minRotation[(int)HandSkeletonBone.eBone_Thumb1] = new Vector3(50.2f, 65.0f, 23.1f);
        minRotation[(int)HandSkeletonBone.eBone_Thumb2] = new Vector3(2.7f, -8f, 10f);
        minRotation[(int)HandSkeletonBone.eBone_Thumb3] = new Vector3(0.0f, 0.0f, 2.2f);

        maxRotation = new Vector3[(int)HandSkeletonBone.eBone_Count];
        maxRotation[(int)HandSkeletonBone.eBone_Thumb1] = new Vector3(20.2f, 50.2f, 31.6f);
        maxRotation[(int)HandSkeletonBone.eBone_Thumb2] = new Vector3(37.7f, -8f, 34.0f);
        maxRotation[(int)HandSkeletonBone.eBone_Thumb3] = new Vector3(0.0f, 0.0f, 52.9f);
        maxRotation[(int)HandSkeletonBone.eBone_IndexFinger1] = new Vector3(-10f, -16f, 79.1f);
        maxRotation[(int)HandSkeletonBone.eBone_IndexFinger2] = new Vector3(30.0f, 0.0f, 109.8f);
        maxRotation[(int)HandSkeletonBone.eBone_IndexFinger3] = new Vector3(0.0f, 2.7f, 76.5f);
        maxRotation[(int)HandSkeletonBone.eBone_MiddleFinger1] = new Vector3(-9f, -16f, 77.1f);
        maxRotation[(int)HandSkeletonBone.eBone_MiddleFinger2] = new Vector3(20.0f, 0.0f, 96.8f);
        maxRotation[(int)HandSkeletonBone.eBone_MiddleFinger3] = new Vector3(7.0f, 2.7f, 78.5f);
        maxRotation[(int)HandSkeletonBone.eBone_RingFinger1] = new Vector3(-10f, -20f, 74.1f);
        maxRotation[(int)HandSkeletonBone.eBone_RingFinger2] = new Vector3(15.0f, 0.0f, 94.8f);
        maxRotation[(int)HandSkeletonBone.eBone_RingFinger3] = new Vector3(0.0f, 2.7f, 78.5f);
        maxRotation[(int)HandSkeletonBone.eBone_PinkyFinger1] = new Vector3(-7f, -15f, 71.1f);
        maxRotation[(int)HandSkeletonBone.eBone_PinkyFinger2] = new Vector3(6.0f, 0.0f, 101.8f);
        maxRotation[(int)HandSkeletonBone.eBone_PinkyFinger3] = new Vector3(-8f, 2.7f, 78.5f);

        leftHandFingers = new Transform[(int)HandSkeletonBone.eBone_Count];
        rightHandFingers = new Transform[(int)HandSkeletonBone.eBone_Count];
        var animator = Player.main?.playerAnimator;
        if (animator is Animator anim)
        {
            for (var i = 0; i < boneNamesLeft.Length; i++)
            {
                var boneName = boneNamesLeft[i];
                if (boneName != null)
                {
                    leftHandFingers[i] = anim.transform.Find("export_skeleton/head_rig/neck/chest/clav_L/clav_L_aim/shoulder_L/hand_L" + boneName);
                    if (leftHandFingers[i] == null)
                    {
                        leftHandFingers[i] = anim.transform.Find("export_skeleton/head_rig/neck/chest/clav_L/clav_L_aim/shoulder_L/elbow_L/hand_L" + boneName);
                    }
                }
            }
            for (var i = 0; i < boneNamesRight.Length; i++)
            {
                var boneName = boneNamesRight[i];
                if (boneName != null)
                {
                    rightHandFingers[i] = anim.transform.Find("export_skeleton/head_rig/neck/chest/clav_R/clav_R_aim/shoulder_R/hand_R" + boneName);
                    if (rightHandFingers[i] == null)
                    {
                        rightHandFingers[i] = anim.transform.Find("export_skeleton/head_rig/neck/chest/clav_R/clav_R_aim/shoulder_R/elbow_R/hand_R" + boneName);
                    }
                }
            }
        }

    }


    private void Update()
    {
        if (ik.enabled)
        {
            ik.solver.leftHandEffector.target = leftTarget;
            ik.solver.rightHandEffector.target = rightTarget;
        }
    }

    private void LateUpdate()
    {
        // Hand/controller tracking without IK
        if (this.ik.enabled)
        {
            // TODO: Add back experimental IK behind an option
            return;
        }

        // Move the hands to the targets which are attached to the controllers
#if false
            Transform t = PhysicalPilotingVR.GetCurrentPilotingTarget(PhysicalPilotingVR.PhysicalPilotingHand.Left);
            leftHand.transform.SetPositionAndRotation((t ?? leftTarget).position, (t ?? leftTarget).rotation);
            leftElbow.transform.SetPositionAndRotation(leftHand.position, leftHand.rotation); // Reset Elbows
            t = PhysicalPilotingVR.GetCurrentPilotingTarget(PhysicalPilotingVR.PhysicalPilotingHand.Right);
            rightHand.transform.SetPositionAndRotation((t ?? rightTarget).position, (t ?? rightTarget).rotation);
            rightElbow.transform.SetPositionAndRotation(rightHand.position, rightHand.rotation); // Reset Elbows
#endif

        leftHand.transform.SetPositionAndRotation(leftTarget.position, leftTarget.rotation);
        rightHand.transform.SetPositionAndRotation(rightTarget.position, rightTarget.rotation);

        // Reset Elbows
        leftElbow.transform.SetPositionAndRotation(leftHand.position, leftHand.rotation);
        rightElbow.transform.SetPositionAndRotation(rightHand.position, rightHand.rotation);
        leftElbow.localScale = Vector3.zero;
        rightElbow.localScale = Vector3.zero;

        if (Settings.ArticulatedHands)
        {
            var rightSkeletonAction = SteamVR_Input.GetSkeletonAction("RightHandSkeleton");
            var leftSkeletonAction = SteamVR_Input.GetSkeletonAction("LeftHandSkeleton");

            if (!Player.main.pda.isOpen)
            {
                UpdateFinger(leftHandFingers, (int)HandSkeletonBone.eBone_PinkyFinger1, leftSkeletonAction.pinkyCurl);
                UpdateFinger(leftHandFingers, (int)HandSkeletonBone.eBone_RingFinger1, leftSkeletonAction.ringCurl);
                UpdateFinger(leftHandFingers, (int)HandSkeletonBone.eBone_MiddleFinger1, leftSkeletonAction.middleCurl);
                UpdateFinger(leftHandFingers, (int)HandSkeletonBone.eBone_IndexFinger1, leftSkeletonAction.indexCurl);
                UpdateFinger(leftHandFingers, (int)HandSkeletonBone.eBone_Thumb1, leftSkeletonAction.thumbCurl);
            }

            if (Inventory.main.GetHeld() == null)
            {
                UpdateFinger(rightHandFingers, (int)HandSkeletonBone.eBone_PinkyFinger1, rightSkeletonAction.pinkyCurl);
                UpdateFinger(rightHandFingers, (int)HandSkeletonBone.eBone_RingFinger1, rightSkeletonAction.ringCurl);
                UpdateFinger(rightHandFingers, (int)HandSkeletonBone.eBone_MiddleFinger1, rightSkeletonAction.middleCurl);
                UpdateFinger(rightHandFingers, (int)HandSkeletonBone.eBone_IndexFinger1, rightSkeletonAction.indexCurl);
                UpdateFinger(rightHandFingers, (int)HandSkeletonBone.eBone_Thumb1, rightSkeletonAction.thumbCurl);
            }

        }

    }

    public void UpdateFinger(Transform[] fingers, int fingerID, float percent)
    {
        fingers[fingerID].transform.localRotation = Quaternion.Euler(minRotation[fingerID].x + ((maxRotation[fingerID].x - minRotation[fingerID].x) * percent), minRotation[fingerID].y + ((maxRotation[fingerID].y - minRotation[fingerID].y) * percent), minRotation[fingerID].z + ((maxRotation[fingerID].z - minRotation[fingerID].z) * percent));
        fingers[fingerID + 1].transform.localRotation = Quaternion.Euler(minRotation[fingerID + 1].x + ((maxRotation[fingerID + 1].x - minRotation[fingerID + 1].x) * percent), minRotation[fingerID + 1].y + ((maxRotation[fingerID + 1].y - minRotation[fingerID + 1].y) * percent), minRotation[fingerID + 1].z + ((maxRotation[fingerID + 1].z - minRotation[fingerID + 1].z) * percent));
        fingers[fingerID + 2].transform.localRotation = Quaternion.Euler(minRotation[fingerID + 2].x + ((maxRotation[fingerID + 2].x - minRotation[fingerID + 2].x) * percent), minRotation[fingerID + 2].y + ((maxRotation[fingerID + 2].y - minRotation[fingerID + 2].y) * percent), minRotation[fingerID + 2].z + ((maxRotation[fingerID + 2].z - minRotation[fingerID + 2].z) * percent));
    }

    public void UpdateBody()
    {
        /*
        var bodyRenderers = transform.GetComponentsInChildren<SkinnedMeshRenderer>(includeInactive: true);
        foreach (var bodyRenderer in bodyRenderers)
        {
            Mod.logger.LogInfo($"bodyRenderer {bodyRenderer.name}");
        }
        */

        StartCoroutine(UpdateBodyRendering());
    }

    public void SetHandRendering(bool val)
    {
        var bodyRenderers = transform.GetComponentsInChildren<SkinnedMeshRenderer>().Where(r => r.name.Contains("hand") || r.name.Contains("glove"));
        bodyRenderers.ForEach(r => r.enabled = val);
    }

    public void SetBodyRendering(bool val)
    {
        var bodyRenderers = transform.GetComponentsInChildren<SkinnedMeshRenderer>().Where(r => r.name.Contains("body") || r.name.Contains("vest"));
        bodyRenderers.ForEach(r => r.enabled = val);
    }

    private IEnumerator UpdateBodyRendering()
    {
        while (true)
        {
            //Mod.logger.LogInfo($"UpdateBodyRendering {Settings.FullBody}");
            var bodyRenderers = transform.GetComponentsInChildren<SkinnedMeshRenderer>().Where(r => r.name.Contains("body") || r.name.Contains("vest"));
            foreach (var bodyRenderer in bodyRenderers)
            {
                bodyRenderer.enabled = Settings.FullBody;
            }

            // Fix culling of hands when full body is off
            var handRenderers = transform.GetComponentsInChildren<SkinnedMeshRenderer>(includeInactive: true).Where(m => m.name.Contains("glove") || m.name.Contains("hands"));
            handRenderers.ForEach(mr =>
            {
                // NOTE: This actually fixes the culling, but still not sure why the bbox doesn't work
                mr.updateWhenOffscreen = true;
            });

            yield return new WaitForSeconds(2.0f);
        }

    }

    internal void OnToolEquipped(PlayerTool tool)
    {
        var aimOffset = tool.GetAimOffset();
        VRCameraRig.instance.TargetTransform = aimOffset;
        tool.GetHandOffset().Apply(rightTarget.transform);
    }
}