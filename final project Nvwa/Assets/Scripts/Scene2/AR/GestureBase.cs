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


    #region �ڲ�������

    /// <summary>
    /// ��ȡ��������Ϣ
    /// </summary>
    /// <param name="index">���������</param>
    /// <param name="hand">���ֻ�����</param>
    /// <returns></returns>
    public Pose GetSkeletonPose(SkeletonIndexFlag index, HandType hand)
    {
        return GesEventInput.Instance.GetSkeletonPose(index, hand);
    }

    #endregion

    #region �ⲿ���÷���

    /// <summary>
    /// ��ȡ���ַ���
    /// </summary>
    /// <param name="handType">���ֻ�����</param>
    /// <returns></returns>
    public Vector3 GetHandForward(HandType handType)
    {
        //�ַ���
        Vector3 handForward = (GetSkeletonPose(SkeletonIndexFlag.MIDDLE_FINGER_MCP, handType).position -
                               GetSkeletonPose(SkeletonIndexFlag.WRIST, handType).position);
        return handForward;
    }

    #endregion
}
