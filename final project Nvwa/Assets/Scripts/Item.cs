using System.Collections;
using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemType itemType; // ��Ʒ������
    public Vector3 OriginalPosition { get; private set; } // ԭʼλ��

    public GameObject[] display;
    public GameObject destructionEffect; // �����Ч�ֶ�
    public AudioClip recipeDestructionSound; // �䷽����������Ч
    public AudioClip distractorDestructionSound; // ��������������Ч
    private AudioSource audioSource; // �����ƵԴ
    private void Start()
    {
        DisPlay_Created();
        // ����Ʒ��ʼ��ʱ��¼��ԭʼλ��
        OriginalPosition = transform.position;
        // ��ȡ����� AudioSource ���
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

            Debug.Log("�Ƕ���" + (int)itemType);
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
        // ����������Ч
       // PlayDestructionEffect();
        // ������Ʒ���Ͳ��Ų�ͬ��������Ч
        if (itemType == ItemType.Lens) // ���� ItemType.Recipe ���䷽����
        {
            PlayDestructionSound(distractorDestructionSound);
        }
        else // ��������Ϊ�䷽
        {
            // ����������Ч
            PlayDestructionEffect();
            PlayDestructionSound(recipeDestructionSound);
        }

        yield return new WaitForSeconds(0.25f); // �ȴ���Ч������ϣ�������Чʱ��������
        //yield return null;
        ItemManager.itemManager.StartIE(this);
        //ItemManager.itemManager.OnTriggerEnterdown(this);

    }
    private void PlayDestructionEffect()
    {
        if (destructionEffect != null)
        {
            // ʵ������Ч������λ��
            //GameObject effectInstance = Instantiate(destructionEffect, transform.position, Quaternion.identity);
            //Destroy(effectInstance, 2f); // 2���������Чʵ��
        }
    }
    private void PlayDestructionSound(AudioClip sound)
    {
        if (sound != null)
        {
            audioSource.PlayOneShot(sound); // ����ָ������Ч
        }
    }
}
