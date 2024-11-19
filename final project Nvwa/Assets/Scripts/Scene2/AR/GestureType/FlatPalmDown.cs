using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rokid.UXR.Interaction;
using System;

public class FlatPalmDown : GestureBase
{
    private GestureBean leftBean;
    private GestureBean rightBean;

    public Action<Pose, GestureBean> onLeftGestureSuccess; //左手手势识别成功
    public Action<Pose, GestureBean> onLeftGestureUpdate; //左手手势识别持续检测
    public Action onLeftGestureFail;                      //左手手势识别失败
    
    public Action<Pose, GestureBean> onRightGestureSuccess; //右手势识别成功
    public Action<Pose, GestureBean> onRightGestureUpdate; //右手手势识别持续检测
    public Action onRightGestureFail;                      //右手手势识别失败

    private bool rightState;
    private bool leftState;

    private void Awake()
    {
        BindEvent();
    }

    private void OnDestroy()
    {
        UnBindEvent();
    }

    public override void OnTrackedFailed(HandType handType)
    {
        base.OnTrackedFailed(handType);
        if (handType == HandType.None)
        {
            leftBean = null;
            rightBean = null;
        }

        if (handType == HandType.RightHand)
        {
            rightBean = null;
        }

        if (handType == HandType.LeftHand)
        {
            leftBean = null;
        }
    }

    public override void OnRenderHand(HandType handType, GestureBean gestureBean)
    {
        base.OnRenderHand(handType, gestureBean);
        if (handType == HandType.RightHand)
        {
            rightBean = gestureBean;
        }

        if (handType == HandType.LeftHand)
        {
            leftBean = gestureBean;
        }
        
        if (handType == HandType.None)
        {
            leftBean = null;
            rightBean = null;
        }
    }

    private void Update()
    {
        Pose leftHandPose = Pose.identity;
        if (leftBean != null && (GestureType)leftBean.gesture_type == GestureType.Palm &&(HandOrientation)leftBean.hand_orientation == HandOrientation.Back)
        {
            leftHandPose = GesEventInput.Instance.GetHandPose(HandType.LeftHand);

            if (!leftState)
            {
                leftState = true;
                onLeftGestureSuccess?.Invoke(leftHandPose, leftBean);
            }

            onLeftGestureUpdate?.Invoke(leftHandPose, leftBean);
        }
        else
        {
            if (leftState)
            {
                leftState = false;
                onLeftGestureFail?.Invoke();
            }
        }
        
        Pose rightHandPose = Pose.identity;
        if (rightBean != null && (GestureType)rightBean.gesture_type == GestureType.Palm &&(HandOrientation)rightBean.hand_orientation == HandOrientation.Back)
        {
            rightHandPose = GesEventInput.Instance.GetHandPose(HandType.RightHand);

            if (!rightState)
            {
                rightState = true;
                onRightGestureSuccess?.Invoke(rightHandPose, rightBean);
            }

            onRightGestureUpdate?.Invoke(rightHandPose, rightBean);
        }
        else
        {
            if (rightState)
            {
                rightState = false;
                onRightGestureFail?.Invoke();
            }
        }
    }
}
