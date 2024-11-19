using System;
using System.Collections;
using System.Collections.Generic;
using Rokid.UXR.Interaction;
using UnityEngine;

public class MakeAFist : GestureBase
{
    private GestureBean rightBean;

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
            rightBean = null;
        }

        if (handType == HandType.RightHand)
        {
            rightBean = null;
        }
    }

    public override void OnRenderHand(HandType handType, GestureBean gestureBean)
    {
        base.OnRenderHand(handType, gestureBean);
        if (handType == HandType.RightHand)
        {
            rightBean = gestureBean;
        }
        if (handType == HandType.None)
        {
            rightBean = null;
        }
    }


    #region 获取手势骨骼点信息

    private Pose GetSkeletonPose(SkeletonIndexFlag index, HandType hand)
    {
        return GesEventInput.Instance.GetSkeletonPose(index, hand);
    }

    #endregion

    #region 获取手方向

    public Vector3 GetHandForward(HandType handType)
    {
        //手方向
        Vector3 handForward = (GetSkeletonPose(SkeletonIndexFlag.MIDDLE_FINGER_MCP, handType).position -
                                   GetSkeletonPose(SkeletonIndexFlag.WRIST, handType).position);
        return handForward;
    }

    #endregion


    private void FixedUpdate()
    {
        Pose rightHandPose = Pose.identity;
        if (rightBean != null && (GestureType)rightBean.gesture_type == GestureType.Grip)
        {
            rightHandPose = GesEventInput.Instance.GetHandPose(HandType.RightHand);
            onRightGestureSuccess?.Invoke(rightHandPose, rightBean);
        }
        else if(rightBean != null && (GestureType)rightBean.gesture_type == GestureType.Palm)
        {
            onRightGestureFail?.Invoke();
        }
    }
}
