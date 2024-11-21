using System.Collections;
using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemType itemType; // 物品的类型
    public Vector3 OriginalPosition { get; private set; } // 原始位置

    public GameObject[] display;
    public GameObject destructionEffect; // 添加特效字段
    public AudioClip recipeDestructionSound; // 配方物体销毁音效
    public AudioClip distractorDestructionSound; // 干扰物体销毁音效
    private AudioSource audioSource; // 添加音频源
    private void Start()
    {
        DisPlay_Created();
        // 在物品初始化时记录其原始位置
        OriginalPosition = transform.position;
        // 获取或添加 AudioSource 组件
        audioSource = gameObject.AddComponent<AudioSource>();
    }
    private void DisPlay_Created()
    {


        foreach (var item in display)
        {
            item.gameObject.SetActive(false);
        }
        try
        {
            display[(int)itemType].SetActive(true);
        }
        catch (System.Exception e)
        {

            Debug.Log("是多少" + (int)itemType);
        }
        

 
    }
    public void StartIEn()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        StartCoroutine(destorymyslef());
        //destorymyslef();
    }

   
    public IEnumerator destorymyslef ()
    {
        // 播放销毁特效
       // PlayDestructionEffect();
        // 根据物品类型播放不同的销毁音效
        if (itemType == ItemType.Lens) // 假设 ItemType.Recipe 是配方物体
        {
            PlayDestructionSound(distractorDestructionSound);
        }
        else // 其他类型为配方
        {
            // 播放销毁特效
            PlayDestructionEffect();
            PlayDestructionSound(recipeDestructionSound);
        }

        yield return new WaitForSeconds(0.25f); // 等待特效播放完毕（根据特效时长调整）
        //yield return null;
        ItemManager.itemManager.StartIE(this);
        //ItemManager.itemManager.OnTriggerEnterdown(this);

    }
    private void PlayDestructionEffect()
    {
        if (destructionEffect != null)
        {
            // 实例化特效并设置位置
            //GameObject effectInstance = Instantiate(destructionEffect, transform.position, Quaternion.identity);
            //Destroy(effectInstance, 2f); // 2秒后销毁特效实例
        }
    }
    private void PlayDestructionSound(AudioClip sound)
    {
        if (sound != null)
        {
            audioSource.PlayOneShot(sound); // 播放指定的音效
        }
    }
}
