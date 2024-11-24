using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rokid.UXR.Interaction;

public class GestureBase : MonoBehaviour
{
    public void BindEvent()
    {
        if (!GestureControlMgr.Instance.managers.Contains(this))
        {
            GestureControlMgr.Instance.managers.Add(this);
        }
    }

    public void UnBindEvent()
    {
        if (GestureControlMgr.Instance.managers.Contains(this))
        {
            GestureControlMgr.Instance.managers.Remove(this);
        }
    }


    public virtual void OnTrackedSuccess(HandType handType)
    {

    }

    public virtual void OnTrackedFailed(HandType handType)
    {

    }

    public virtual void OnRenderHand(HandType handType, GestureBean gestureBean)
    {

    }


    #region 内部处理方法

    /// <summary>
    /// 获取骨骼点信息
    /// </summary>
    /// <param name="index">骨骼点序号</param>
    /// <param name="hand">左手或右手</param>
    /// <returns></returns>
    public Pose GetSkeletonPose(SkeletonIndexFlag index, HandType hand)
    {
        return GesEventInput.Instance.GetSkeletonPose(index, hand);
    }

    #endregion

    #region 外部调用方法

    /// <summary>
    /// 获取单手方向
    /// </summary>
    /// <param name="handType">左手或右手</param>
    /// <returns></returns>
    public Vector3 GetHandForward(HandType handType)
    {
        //手方向
        Vector3 handForward = (GetSkeletonPose(SkeletonIndexFlag.MIDDLE_FINGER_MCP, handType).position -
                               GetSkeletonPose(SkeletonIndexFlag.WRIST, handType).position);
        return handForward;
    }

    #endregion
}
