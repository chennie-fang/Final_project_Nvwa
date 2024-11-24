using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Scene2VoiceManager : MonoSingleton<Scene2VoiceManager>
{
    public AudioClip EnterScene;
    public AudioClip BattleStart;
    public AudioClip BattleFail;
    public AudioClip BattleSucceed;
    public AudioClip FirstAttackHint;
    public AudioClip SecondAttackHint;
    public AudioClip ThirdAttackHint;
    public AudioClip FourthAttackHint;

    public AudioClip InjuredEffect;
    public AudioClip FirstEffect;
    public AudioClip SecondEffect;
    public AudioClip ThirdEffect;
    public AudioClip FailEffect;

    public GameObject FirstHintImage;
    public GameObject SecondHintImage;
    public GameObject ThirdHintImage;

    public GameObject SmallCardImage1;
    public GameObject SmallCardImage2;
    public GameObject SmallCardImage3;

    private float FirstHintImageTime;
    private float SecondHintImageTime;
    private float ThirdHintImageTime;

    private bool ToCard1;
    private bool ToCard2;
    private bool ToCard3;

    private AudioSource audioSource;
    public AudioSource audioEffectSource;

    private Vector3[] initialPositions;
    private Quaternion[] initialRotations;
    private Vector3[] initialScales;
    private GameObject[] objectsToReset;
    // Start is called before the first frame update
    void Start()
    {
        objectsToReset = new GameObject[] { FirstHintImage, SecondHintImage, ThirdHintImage};
        audioSource = GetComponent<AudioSource>();
        
        // 记录初始卡牌位置信息
        initialPositions = new Vector3[objectsToReset.Length];
        initialRotations = new Quaternion[objectsToReset.Length];
        initialScales = new Vector3[objectsToReset.Length];

        for (int i = 0; i < objectsToReset.Length; i++)
        {
            initialPositions[i] = objectsToReset[i].transform.position;
            initialRotations[i] = objectsToReset[i].transform.rotation;
            initialScales[i] = objectsToReset[i].transform.localScale;
        }
    }
    private void OnEnable()
    {
        FirstHintImage.SetActive(false);
        SecondHintImage.SetActive(false);
        ThirdHintImage.SetActive(false);

        ToCard1 = false;
        ToCard2 = false;
        ToCard3 = false;

        SmallCardImage1.SetActive(false);
        SmallCardImage2.SetActive(false);
        SmallCardImage3.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (ToCard1)
        {
            if(Time.time > FirstHintImageTime)
            {
                ToCard1 = false;
                FirstHintImage.transform.DOMove(SmallCardImage1.transform.position, 1f).OnComplete(() => 
                {
                    SmallCardImage1.SetActive(true);
                    FirstHintImage.SetActive(false);
                });
                FirstHintImage.transform.DORotate(SmallCardImage1.transform.eulerAngles, 1f);
                FirstHintImage.transform.DOScale(SmallCardImage1.transform.localScale, 1f);
            }
        }
        else if (ToCard2)
        {
           
            if (Time.time > SecondHintImageTime)
            {
                ToCard2 = false;
                SecondHintImage.transform.DOMove(SmallCardImage2.transform.position, 1f).OnComplete(() =>
                {
                    SmallCardImage2.SetActive(true);
                    SecondHintImage.SetActive(false);
                });
                SecondHintImage.transform.DORotate(SmallCardImage2.transform.eulerAngles, 1f);
                SecondHintImage.transform.DOScale(SmallCardImage2.transform.localScale, 1f);
            }
        }
        else if (ToCard3)
        {
            
            if (Time.time > ThirdHintImageTime)
            {
                ToCard3 = false;
                ThirdHintImage.transform.DOMove(SmallCardImage3.transform.position, 1f).OnComplete(() =>
                {
                    SmallCardImage3.SetActive(true);
                    ThirdHintImage.SetActive(false);
                });
                ThirdHintImage.transform.DORotate(SmallCardImage3.transform.eulerAngles, 1f);
                ThirdHintImage.transform.DOScale(SmallCardImage3.transform.localScale, 1f);
            }
        }
    }

    /// <summary>
    /// 战斗开始
    /// </summary>
    public void BattleStartPlay()
    {
        audioSource.Stop();
        audioSource.clip = BattleStart;
        audioSource.Play();
    }

    /// <summary>
    /// 战斗失败
    /// </summary>
    public void BattleFailPlay()
    {
        audioSource.Stop();
        audioSource.clip = BattleFail;
        audioSource.Play();
    }

    /// <summary>
    /// 战斗胜利
    /// </summary>
    public void BattleSucceedPlay()
    {
        audioSource.Stop();
        audioSource.clip = BattleSucceed;
        audioSource.Play();
    }
    /// <summary>
    /// 第一种进攻方式提示
    /// </summary>
    public void BattleFirstHintPlay()
    {
        audioSource.Stop();
        audioSource.clip = FirstAttackHint;
        audioSource.Play();
        FirstHintImageTime = Time.time + FirstAttackHint.length + 0.5f;
        FirstHintImage.SetActive(true);
        ResetAllObjects();
        ToCard1 = true;
        ProcessControl.Instance.Fire.GetComponent<SkinnedMeshRenderer>().material = ProcessControl.Instance.Fire2;
    }
    /// <summary>
    /// 第二种进攻方式提示
    /// </summary>
    public void BattleSecondHintPlay()
    {
        audioSource.Stop();
        audioSource.clip = SecondAttackHint;
        audioSource.Play();
        SecondHintImageTime = Time.time + SecondAttackHint.length + 0.5f;
        SecondHintImage.SetActive(true);
        ResetAllObjects();
        ToCard2 = true;
        ProcessControl.Instance.Fire.GetComponent<SkinnedMeshRenderer>().material = ProcessControl.Instance.Fire1;
    }
    /// <summary>
    /// 第三种进攻方式提示
    /// </summary>
    public void BattleThirdHintPlay()
    {
        audioSource.Stop();
        audioSource.clip = ThirdAttackHint;
        audioSource.Play();
        ThirdHintImageTime = Time.time + ThirdAttackHint.length + 0.5f;
        ThirdHintImage.SetActive(true);
        ResetAllObjects();
        ToCard3 = true;
        ProcessControl.Instance.Fire.GetComponent<SkinnedMeshRenderer>().material = ProcessControl.Instance.Fire3;
    }
    /// <summary>
    /// 最后一波进攻提示
    /// </summary>
    public void BattleFourthHintPlay()
    {
        audioSource.Stop();
        audioSource.clip = FourthAttackHint;
        audioSource.Play();
        ProcessControl.Instance.Fire.GetComponent<SkinnedMeshRenderer>().material = ProcessControl.Instance.Fire1;
    }

    /// <summary>
    /// 玩家受伤音效
    /// </summary>
    public void InjuredEffectPlay()
    {
        audioEffectSource.Stop();
        audioEffectSource.clip = InjuredEffect;
        audioEffectSource.Play();
    }
    /// <summary>
    /// 第一种攻击音效
    /// </summary>
    public void FirstAttackEffectPlay()
    {
        audioEffectSource.Stop();
        audioEffectSource.clip = FirstEffect;
        audioEffectSource.Play();
    }

    /// <summary>
    /// 第二种攻击音效
    /// </summary>
    public void SecondAttackEffectPlay()
    {
        audioEffectSource.Stop();
        audioEffectSource.clip = SecondEffect;
        audioEffectSource.Play();
    }

    /// <summary>
    /// 第三种攻击音效
    /// </summary>
    public void ThirdAttackEffectPlay()
    {
        audioEffectSource.Stop();
        audioEffectSource.clip = ThirdEffect;
        audioEffectSource.Play();
    }
    
    /// <summary>
    /// 失败音效
    /// </summary>
    public void FailEffectPlay()
    {
        audioEffectSource.Stop();
        audioEffectSource.clip = FailEffect;
        audioEffectSource.Play();
    }

    /// <summary>
    /// 回到初始信息
    /// </summary>
    public void ResetAllObjects()
    {
        for (int i = 0; i < objectsToReset.Length; i++)
        {
            objectsToReset[i].transform.position = initialPositions[i];
            objectsToReset[i].transform.rotation = initialRotations[i];
            objectsToReset[i].transform.localScale = initialScales[i];
        }
    }
}
