using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttackType
{
    AttackType1,
    AttackType2,
    AttackType3
}

public class MonsterController : MonoBehaviour
{
    #region �ֶ�
    [SerializeField, Header("��������")]
    /// <summary>
    /// Ѫ��
    /// </summary>
    public int BloodVolume;
    /// <summary>
    /// �˺�ֵ
    /// </summary>
    public int HurtVolume;
    /// <summary>
    /// �����ٶ�
    /// </summary>
    public float MonsterSpeed;
    /// <summary>
    /// ���޽��ܹ�������
    /// </summary>
    public AttackType AcceptAttackType;
    #endregion



    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * (MonsterSpeed * Time.deltaTime);
    }
    private void OnDestroy()
    {
        ProcessControl.Instance.KillsNumber += 1;
    }
}
