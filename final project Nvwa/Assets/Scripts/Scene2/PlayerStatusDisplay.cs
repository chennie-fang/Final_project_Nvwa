using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatusDisplay : MonoBehaviour
{

    public GameObject[] mImage;
    public int currentBlood;
    void OnEnable()
    {
        currentBlood = mImage.Length;
        foreach(GameObject gameObject in mImage)
        {
            gameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
    /// <summary>
    /// ÕÊº“ ‹…À
    /// </summary>
    public void PlayerInjured()
    {
        Scene2VoiceManager.Instance.InjuredEffectPlay();
        currentBlood -= 1;
        if (currentBlood >= 0) 
        {
            mImage[currentBlood].SetActive(false);
            if (currentBlood == 0)
            {
                ProcessControl.Instance.FailBattle();
            }
        }
        else
        {
            ProcessControl.Instance.FailBattle();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        int layerIndex = collision.gameObject.layer;
        string layerName = LayerMask.LayerToName(layerIndex);
        if (layerName == "Monster")
        {
            PlayerInjured();
            Destroy(collision.gameObject);
        }
    }

    private void OnDisable()
    {
        currentBlood = mImage.Length;
        foreach (GameObject gameObject in mImage)
        {
            gameObject.SetActive(true);
        }
    }
}
