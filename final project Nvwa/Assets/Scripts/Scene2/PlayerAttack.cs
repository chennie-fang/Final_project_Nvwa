using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rokid.UXR.Module;
using UnityEngine.Android;
using Rokid.UXR.Interaction;

public class PlayerAttack : MonoBehaviour
{
    public GameObject VoicAttack;
    public GameObject OneHandedAttack;
    public GameObject TwoHandedAttack;
    public GameObject LeftHanded;
    public GameObject RightHanded;
    public GameObject LeftHandedMesh;
    public GameObject RightHandedMesh;
    public GameObject Eye;
    private string command = "进攻";
    private float TimeControl;

    private FireGesture fireGesture; //攻击手势
    private float coldTime = 1f;
    private float leftFireCountdown;
    private float rightFireCountdown;

    private void Awake()
    {
        if (!Permission.HasUserAuthorizedPermission("android.permission.RECORD_AUDIO"))
        {
            Permission.RequestUserPermission("android.permission.RECORD_AUDIO");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // 语言确定
        ModuleManager.Instance.RegistModule("com.rokid.voicecommand.VoiceCommandHelper", false);
        OfflineVoiceModule.Instance.ChangeVoiceCommandLanguage(LANGUAGE.CHINESE);

        OfflineVoiceModule.Instance.AddInstruct(LANGUAGE.CHINESE, "进攻", "jin gong", this.gameObject.name, "OnReceive");
        OfflineVoiceModule.Instance.Commit();
        TimeControl = 0;

        //攻击手势
        fireGesture = GestureControlMgr.Instance.FindGestureType<FireGesture>();

        //fireGesture.onLeftGestureUpdate += LeftHandAttack;
        fireGesture.onRightGestureUpdate += RightHandAttack;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Instantiate(VoicAttack, Camera.main.transform.position, Camera.main.transform.rotation);
        }
        if (LeftHandedMesh.activeSelf)
        {
            LeftHandedMesh.SetActive(false);
        }
        if (RightHandedMesh.activeSelf)
        {
            RightHandedMesh.SetActive(false);
        }
    }

    public void OnReceive(string msg)
    {
        // && ProcessControl.Instance.CurrentWaves >= 2
        if (string.Equals(command, msg))
        {
            Instantiate(VoicAttack, Eye.transform.position, Eye.transform.rotation);
            Scene2VoiceManager.Instance.FirstAttackEffectPlay();
        }
        
    }

    private void OnDestroy()
    {
        OfflineVoiceModule.Instance.ClearAllInstruct();
        OfflineVoiceModule.Instance.Commit();
    }

    void LeftHandAttack(GestureBean leftBean)
    {
        if (leftFireCountdown <= 0f)
        {
            //GesEventInput.Instance.GetGestureType(HandType.LeftHand) == GestureType.Palm && GesEventInput.Instance.GetHandOrientation(HandType.LeftHand) == HandOrientation.Back
            //GesEventInput.Instance.GetGestureType(HandType.LeftHand) == GestureType.Grip && GesEventInput.Instance.GetHandOrientation(HandType.LeftHand) == HandOrientation.Back
            if (leftBean.gesture_type == 2)
            {
                if(ProcessControl.Instance.MonsterBrush.activeSelf)
                {
                    Instantiate(OneHandedAttack, LeftHanded.transform.position, LeftHanded.transform.rotation);
                    Scene2VoiceManager.Instance.SecondAttackEffectPlay();
                }
            }
            else if (leftBean.gesture_type == 1)
            {
                if (ProcessControl.Instance.CurrentWaves >= 1)
                {
                    Instantiate(TwoHandedAttack, LeftHanded.transform.position, LeftHanded.transform.rotation);
                    Scene2VoiceManager.Instance.ThirdAttackEffectPlay();
                }     
            }
            leftFireCountdown = 0;
            leftFireCountdown += coldTime;
        }
        leftFireCountdown -= Time.deltaTime;
    }

    void RightHandAttack(GestureBean RightBean)
    {
        if (rightFireCountdown <= 0f)
        {
            //GesEventInput.Instance.GetGestureType(HandType.RightHand) == GestureType.Palm && GesEventInput.Instance.GetHandOrientation(HandType.RightHand) == HandOrientation.Back
            //RightBean.hand_orientation == 1
            //GesEventInput.Instance.GetGestureType(HandType.RightHand) == GestureType.Grip && GesEventInput.Instance.GetHandOrientation(HandType.RightHand) == HandOrientation.Back
            if (RightBean.gesture_type == 2)
            {
                if (ProcessControl.Instance.MonsterBrush.activeSelf)
                {
                    Instantiate(OneHandedAttack, RightHanded.transform.position, RightHanded.transform.rotation);
                    Scene2VoiceManager.Instance.SecondAttackEffectPlay();
                }   
            }
            else if (RightBean.gesture_type == 1)
            {
                if (ProcessControl.Instance.CurrentWaves >= 1)
                {
                    Instantiate(TwoHandedAttack, RightHanded.transform.position, RightHanded.transform.rotation);
                    Scene2VoiceManager.Instance.ThirdAttackEffectPlay();
                }   
            }
            rightFireCountdown = 0;
            rightFireCountdown += coldTime;
        }
        rightFireCountdown -= Time.deltaTime;
    }
}
