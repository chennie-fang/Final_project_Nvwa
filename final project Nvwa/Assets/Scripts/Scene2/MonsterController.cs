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
    #region 字段
    [SerializeField, Header("怪物数据")]
    /// <summary>
    /// 血量
    /// </summary>
    public int BloodVolume;
    /// <summary>
    /// 伤害值
    /// </summary>
    public int HurtVolume;
    /// <summary>
    /// 怪物速度
    /// </summary>
    public float MonsterSpeed;
    /// <summary>
    /// 怪兽接受攻击类型
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
