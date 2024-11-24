using System;
using System.Collections;
using System.Collections.Generic;
using Rokid.UXR.Interaction;
using UnityEngine;

public class MakeAFist : GestureBase
{
    private GestureBean rightBean;

    public Action<Pose, GestureBean> onRightGestureSuccess; //������ʶ��ɹ�
    public Action<Pose, GestureBean> onRightGestureUpdate; //��������ʶ��������
    public Action onRightGestureFail;                      //��������ʶ��ʧ��

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


    #region ��ȡ���ƹ�������Ϣ

    private Pose GetSkeletonPose(SkeletonIndexFlag index, HandType hand)
    {
        return GesEventInput.Instance.GetSkeletonPose(index, hand);
    }

    #endregion

    #region ��ȡ�ַ���

    public Vector3 GetHandForward(HandType handType)
    {
        //�ַ���
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
