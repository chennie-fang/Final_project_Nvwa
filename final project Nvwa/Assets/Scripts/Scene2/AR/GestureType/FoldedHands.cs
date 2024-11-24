using System;
using System.Collections;
using System.Collections.Generic;
using Rokid.UXR.Interaction;
using UnityEngine;

public class FoldedHands : GestureBase
{
    private GestureBean leftBean;
    private GestureBean rightBean;

    public Action onGestureSuccess; //手势识别成功
    public Action onGestureFail; //手势识别失败


    private bool state;

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
            rightBean = null;
            leftBean = null;
        }
    }

    private void FixedUpdate()
    {
        if (LeftGestureDiscern() && RightGestureDiscern())
        {
            onGestureSuccess?.Invoke();
        }
    }


    private bool LeftGestureDiscern()
    {
        if (leftBean != null && (GestureType)leftBean.gesture_type == GestureType.Palm &&
            (HandOrientation)leftBean.hand_orientation == HandOrientation.Palm)
        {
            return true;
        }

        return false;
    }


    private bool RightGestureDiscern()
    {
        if (rightBean != null && (GestureType)rightBean.gesture_type == GestureType.Palm &&
            (HandOrientation)rightBean.hand_orientation == HandOrientation.Palm)
        {
            return true;
        }

        return false;
    }
}