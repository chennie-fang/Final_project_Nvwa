using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rokid.UXR.Interaction;
using System;
using Rokid.UXR.Native;

public class FireGesture : GestureBase
{
    private GestureBean leftBean;
    private GestureBean rightBean;

    public Action<GestureBean> onLeftGestureSuccess; //左手手势识别成功
    public Action<GestureBean> onLeftGestureUpdate; //左手手势识别持续检测
    public Action onLeftGestureFail; //左手手势识别失败

    public Action<GestureBean> onRightGestureSuccess; //右手势识别成功
    public Action<GestureBean> onRightGestureUpdate; //右手手势识别持续检测
    public Action onRightGestureFail; //右手手势识别失败

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
            onRightGestureUpdate?.Invoke(rightBean);
        }

        if (handType == HandType.LeftHand)
        {
            leftBean = gestureBean;
            onLeftGestureUpdate?.Invoke(leftBean);
        }

        if (handType == HandType.None)
        {
            rightBean = null;
            leftBean = null;
        }
    }
    
}