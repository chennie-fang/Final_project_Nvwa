using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ProcessControl : MonoSingleton<ProcessControl>
{

    public int[] WavesNumber;
    public GameObject StartPanel;
    public GameObject EndPanel;
    public GameObject StatusPanel;
    public GameObject FailurePanel;
    public GameObject MonsterBrush;
    public int CurrentWaves;
    public int KillsNumber = 0;

    public Animator[] StartAnimator;
    public Animator[] EndAnimator;
    public Animator HandAnimator;
    public string HandStartAnimationName;
    public string HandBackAnimationName;
    private bool isNeedBrush;
    private bool isNeedHandBack;
    private bool isEndOrFailure;

    public Text KilledNumberText;

    public GameObject Fire;
    public Material Fire1;
    public Material Fire2;
    public Material Fire3;

    public GameObject Card1;
    public GameObject Card2;
    public GameObject Card3;

    public int PlayerBlood;

    /*void Awake()
    {
        Application.targetFrameRate = 60;
    }*/
    private void Start()
    {
        StartCoroutine(AllBefore());
    }
    void Update()
    {
        //KilledNumberText.text = KillsNumber.ToString();
        /*if (IsAnimationFinished(HandStartAnimationName) && isNeedBrush)
        {
            MonsterBrush.SetActive(true);
            isNeedBrush = false;
        }*/

        //Debug.Log(KillsNumber);
        /*if (IsAnimationFinished(HandBackAnimationName) && isNeedHandBack)
        {
            foreach (Animator ani in StartAnimator)
            {
                ani.enabled = false;
            }
            isNeedHandBack = false;
            if (isEndOrFailure)
            {
                EndPanel.SetActive(true);
            }
            else
            {
                FailurePanel.SetActive(true);
            }
        }*/
    }
    private void OnEnable()
    {
        EndPanel.SetActive(false);
        StatusPanel.SetActive(false);
        MonsterBrush.SetActive(false);
       /* foreach (Animator ani in StartAnimator)
        {
            ani.enabled = false;
        }
        foreach (Animator ani in EndAnimator)
        {
            ani.enabled = false;
        }*/
    }
    /// <summary>
    /// 开始战斗
    /// </summary>
    public void StartBattle()
    {
        StartPanel.SetActive(false);
        EndPanel.SetActive(false);
        StatusPanel.SetActive(true);
        FailurePanel.SetActive(false);
        KillsNumber = 0;
        CurrentWaves = 0;
        //MonsterBrush.SetActive(true);
        foreach(Animator ani in StartAnimator)
        {
            ani.enabled = true;
            
        }
        foreach (Animator ani in EndAnimator)
        {
            ani.enabled = false;
        }
        HandAnimator.Play(HandStartAnimationName);
        isNeedBrush = true;
        StartCoroutine(GameStart());
    }

    /// <summary>
    /// 结束战斗
    /// </summary>
    public void EndBattle()
    {
        ClearMonster();
        StartPanel.SetActive(false);
        StatusPanel.SetActive(false);
        FailurePanel.SetActive(false);
        KillsNumber = 0;
        CurrentWaves = 0;
        MonsterBrush.SetActive(false);
        foreach (Animator ani in EndAnimator)
        {
            ani.enabled = true;
        }
        HandAnimator.Play(HandBackAnimationName);
        isNeedHandBack = true;
        isEndOrFailure = true;
        Scene2VoiceManager.Instance.BattleSucceedPlay();
        StartCoroutine(GameOver());
    }

    /// <summary>
    /// 战斗失败
    /// </summary>
    public void FailBattle()
    {
        ClearMonster();
        StartPanel.SetActive(false);
        EndPanel.SetActive(false);
        StatusPanel.SetActive(false);
        KillsNumber = 0;
        CurrentWaves = 0;
        Scene2VoiceManager.Instance.BattleFailPlay();
        Scene2VoiceManager.Instance.FailEffectPlay();
        MonsterBrush.SetActive(false);
        Card1.SetActive(false);
        Card2.SetActive(false);
        Card3.SetActive(false);

        foreach (Animator ani in EndAnimator)
        {
            ani.enabled = true;
        }
        //HandAnimator.Play(HandBackAnimationName);
        isNeedHandBack = true;
        isEndOrFailure = false;
        StartCoroutine(GameOver());
    }

    public void ClearMonster()
    {
        string tag = "Monster";
        GameObject[] dynamicObjects = GameObject.FindGameObjectsWithTag(tag);
        foreach (GameObject obj in dynamicObjects)
        {
            Destroy(obj);
        }
    }

    bool IsAnimationFinished(string stateName)
    {
        AnimatorStateInfo stateInfo = HandAnimator.GetCurrentAnimatorStateInfo(0);
        return stateInfo.IsName(stateName) && stateInfo.normalizedTime >= 1f;
    }

    public void ReturnToSatrtScene()
    {
        SceneManager.LoadScene("StartScene");
    }

    /// <summary>
    /// 游戏开始时调用
    /// </summary>
    /// <returns></returns>
    private IEnumerator GameStart()
    {
        yield return new WaitForSeconds(4f);
        MonsterBrush.SetActive(true);
        isNeedBrush = false;
    }
    private IEnumerator GameOver()
    {
        if (isEndOrFailure)
        {
            yield return new WaitForSeconds(4f);
        }
        else
        {
            yield return new WaitForSeconds(1f);
        }

        isNeedHandBack = false;
        if (isEndOrFailure)
        {
            EndPanel.SetActive(true);
        }
        else
        {
            FailurePanel.SetActive(true);
        }
    }

    private IEnumerator AllBefore()
    {
        yield return new WaitForSeconds(17f);
        //testCompletionPanel.SetActive(true);
        //StartPanel.SetActive(true);
        StartBattle();
        Scene2VoiceManager.Instance.BattleStartPlay();
    }
}
