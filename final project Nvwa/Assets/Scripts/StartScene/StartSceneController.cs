using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class StartSceneController : MonoBehaviour
{
    public static bool IsFirstEnter = true;
    public static bool FirstISOver = false;
    public static bool SecondISOver = false;

    public Transform target;
    public GameObject Waves;
    public GameObject Video;

    public ParticleSystem WavesParticleSystem1;
    public ParticleSystem WavesParticleSystem2;
    public Material MirrorMaterial;

    public AudioSource OpeningRemarks;

    public GameObject StartInetrface;               // 开始界面
    public GameObject SkipButton;                   // 跳过按钮
    public GameObject Mirror;
    public GameObject StartUI;

    public Image FirstImage;
    public Image SecondImage;
    public Sprite FirstImageOver;
    public Sprite SecondImageOver;

    private bool StartUIIsOver;
    // Start is called before the first frame update
    void Start()
    {
        if (!IsFirstEnter)
        {
            StartInetrface.SetActive(true);
            Mirror.SetActive(false);
            StartUI.SetActive(false);
            StartUIIsOver = true;
        }
        else
        {
            StartInetrface.SetActive(false);
            StartUI.SetActive(true);
            StartUIIsOver = false;
        }

        if (FirstISOver)
        {
            FirstImage.sprite = FirstImageOver;
        }
        if (SecondISOver)
        {
            SecondImage.sprite = SecondImageOver;
        }
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(transform.position, target.position);
        if(distance <= 4.5f && IsFirstEnter && StartUIIsOver)
        {
            Waves.SetActive(false);
            Video.SetActive(true);
            MirrorMaterial.DOFade(0f, 4f);
            // 获取粒子系统的Emission模块
            ParticleSystem.EmissionModule emission1 = WavesParticleSystem1.emission;
            emission1.rateOverTime = 0f;
            ParticleSystem.EmissionModule emission2 = WavesParticleSystem2.emission;
            emission2.rateOverTime = 0f;
            IsFirstEnter = false;
            StartCoroutine(OpeningRemarksPlay());
        }

    }

    private void OnDisable()
    {
        MirrorMaterial.color = new Color(MirrorMaterial.color.r, MirrorMaterial.color.g, MirrorMaterial.color.b, 1f);
    }

    public void EnterSceneFirst()
    {
        SceneManager.LoadScene("SceneFirst");
        FirstISOver = true;
    }
    public void EnterSceneSecond()
    {
        SceneManager.LoadScene("SceneSecond");
        SecondISOver = true;
    }

    public void EnterSceneThird()
    {
        //SceneManager.LoadScene("SceneThird");
    }
    public void SkipAnimation()
    {
        Mirror.SetActive(false);
        StartInetrface.SetActive(true);
        SkipButton.SetActive(false);
    }
    public void StartUIOver()
    {
        //SceneManager.LoadScene("SceneThird");
        StartUIIsOver = true;
        Mirror.SetActive(true);
        SkipButton.SetActive(false);
        StartCoroutine(AudioPlay());
    }
    private IEnumerator AudioPlay()
    {
        yield return new WaitForSeconds(3f);
        Mirror.GetComponent<AudioSource>().Play();
    }
    private IEnumerator OpeningRemarksPlay()
    {
        // 等待一秒钟
        yield return new WaitForSeconds(2f);
        OpeningRemarks.Play();
        //SkipButton.SetActive(true);
        StartCoroutine(FirstEnterScene1());
    }
    private IEnumerator FirstEnterScene1()
    {
        yield return new WaitForSeconds(16f);
        //EnterSceneFirst();
        SkipButton.SetActive(false);
        StartInetrface.SetActive(true);
        Mirror.SetActive(false);
    }
}
