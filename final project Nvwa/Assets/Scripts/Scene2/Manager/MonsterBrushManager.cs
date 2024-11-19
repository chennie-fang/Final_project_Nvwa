using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;

public class MonsterBrushManager : MonoBehaviour
{
    public GameObject[] monsterPrefabs;     // ����Ԥ��������
    public Transform[] spawnPoints;         // ��������λ������

    public float spawnInterval = 2f;        // ˢ�ּ��ʱ��
    public float wavesInterval;             // ���μ��ʱ�� 
    public static bool isCanBrushMonster;   // �Ƿ����ˢ��

    private float nextSpawnTime;            // ��һ��ˢ�ֵ�ʱ��
    private int currentWavesNumber;         // ��ǰ���γ��ֹ�������
    
    private int currentWaves;               // ��ǰ����
    private int lastSpawnPointIndex = -1;   // ��ֹ����һ��ˢ��λ��һ��

    private bool FirstHint;
    private bool SecondHint;
    private bool ThirdHint;
    private bool FourthHint;
    private int CurrentWavesKilled;
    private int CurrentWavesNumberText;

    void OnEnable()
    {
        nextSpawnTime = Time.time + spawnInterval;
        isCanBrushMonster = true;
        currentWaves = 0;
        currentWavesNumber = 0;
        FirstHint = false;
        SecondHint = false;
        ThirdHint = false;
        FourthHint = false;
    }

    void Update()
    {
        // ˢ���ı�
        string killedNumberText = (ProcessControl.Instance.KillsNumber - CurrentWavesKilled) + " / " + CurrentWavesNumberText;
        ProcessControl.Instance.KilledNumberText.text = killedNumberText;
        // ����Ƿ񵽴���һ��ˢ�ֵ�ʱ��
        if (Time.time >= nextSpawnTime && currentWaves < ProcessControl.Instance.WavesNumber.Length)
        {
            if (isCanBrushMonster)
            {
                if(currentWavesNumber >= ProcessControl.Instance.WavesNumber[currentWaves])
                {
                    currentWavesNumber = 0;
                    currentWaves += 1;
                    if(currentWaves >= ProcessControl.Instance.WavesNumber.Length)
                    {
                        ProcessControl.Instance.EndBattle();
                    }
                    else
                    {
                        ProcessControl.Instance.CurrentWaves = currentWaves;
                    }
                    nextSpawnTime = Time.time + wavesInterval;
                    isCanBrushMonster = false;
                    /*if(currentWaves > 2)
                    {
                        CurrentWavesKilled = SumOfFirstIElements(ProcessControl.Instance.WavesNumber, currentWaves);
                    }*/
                    return;
                }
                else
                {
                    SpawnMonster();
                    // ������һ��ˢ�ֵ�ʱ��
                    nextSpawnTime = Time.time + spawnInterval;
                    
                }
            }
            else
            {
                int sum = SumOfFirstIElements(ProcessControl.Instance.WavesNumber, currentWaves);
                if (ProcessControl.Instance.KillsNumber >= sum)
                {
                    isCanBrushMonster = true;
                }  
            }
        }
    }

    /// <summary>
    /// ˢ���߼�
    /// </summary>
    void SpawnMonster()
    {
        CurrentWavesKilled = SumOfFirstIElements(ProcessControl.Instance.WavesNumber, currentWaves);
        CurrentWavesNumberText = ProcessControl.Instance.WavesNumber[currentWaves];
        GameObject monsterPrefab;
        if(currentWaves < 3)
        {
            if (currentWaves == 0 && !FirstHint)
            {
                Scene2VoiceManager.Instance.BattleFirstHintPlay();
                FirstHint = true;
                return;
            }
            else if (currentWaves == 1 && !SecondHint)
            {
                Scene2VoiceManager.Instance.BattleSecondHintPlay();
                SecondHint = true;
                return;
            }
            else if (currentWaves == 2 && !ThirdHint)
            {
                Scene2VoiceManager.Instance.BattleThirdHintPlay();
                ThirdHint = true;
                return;
            }
            monsterPrefab = monsterPrefabs[currentWaves]; 
        }
        else
        {
            int prefabIndex = Random.Range(0, monsterPrefabs.Length);
            monsterPrefab = monsterPrefabs[prefabIndex];
            if (!FourthHint)
            {
                Scene2VoiceManager.Instance.BattleFourthHintPlay();
                FourthHint = true;
                return;
            }
        }

        // ���ѡ��һ������λ��
        int spawnPointIndex = Random.Range(0, spawnPoints.Length);  
        // ������ɵ�λ���Ƿ�����һ����ͬ�������ͬ����������
        while (spawnPointIndex == lastSpawnPointIndex)
        {
            spawnPointIndex = Random.Range(0, spawnPoints.Length);
        }

        // ������һ�ε�λ��
        lastSpawnPointIndex = spawnPointIndex;

        Transform spawnPoint = spawnPoints[spawnPointIndex];

        // ��ѡ���λ�����ɹ���
        Instantiate(monsterPrefab, spawnPoint.position, spawnPoint.rotation);
        currentWavesNumber += 1;
    }


    // ��������ǰi������֮�͵ĺ���
    public static int SumOfFirstIElements(int[] array, int i)
    {
        int sum = 0; // ��ʼ����Ϊ0

        // ȷ��i���ᳬ�����鳤��
        i = Mathf.Min(i, array.Length);

        // ѭ���ۼ�ǰi��Ԫ��
        for (int j = 0; j < i; j++)
        {
            sum += array[j];
        }

        return sum; // ���ؼ���ĺ�
    }

    private void OnDisable()
    {
        currentWaves = 0;
        ProcessControl.Instance.CurrentWaves = 0;
        ProcessControl.Instance.KillsNumber = 0;
        CurrentWavesKilled = 0;
        ProcessControl.Instance.KilledNumberText.text = "0 / 0";
        ProcessControl.Instance.Fire.GetComponent<SkinnedMeshRenderer>().material = ProcessControl.Instance.Fire1;
    }
}
