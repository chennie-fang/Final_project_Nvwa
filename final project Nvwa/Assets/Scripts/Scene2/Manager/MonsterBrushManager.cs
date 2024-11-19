using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;

public class MonsterBrushManager : MonoBehaviour
{
    public GameObject[] monsterPrefabs;     // 怪物预制体数组
    public Transform[] spawnPoints;         // 怪物生成位置数组

    public float spawnInterval = 2f;        // 刷怪间隔时间
    public float wavesInterval;             // 波次间隔时间 
    public static bool isCanBrushMonster;   // 是否可以刷怪

    private float nextSpawnTime;            // 下一次刷怪的时间
    private int currentWavesNumber;         // 当前波次出现怪物数量
    
    private int currentWaves;               // 当前波次
    private int lastSpawnPointIndex = -1;   // 防止和上一次刷新位置一样

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
        // 刷新文本
        string killedNumberText = (ProcessControl.Instance.KillsNumber - CurrentWavesKilled) + " / " + CurrentWavesNumberText;
        ProcessControl.Instance.KilledNumberText.text = killedNumberText;
        // 检查是否到达下一次刷怪的时间
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
                    // 更新下一次刷怪的时间
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
    /// 刷怪逻辑
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

        // 随机选择一个生成位置
        int spawnPointIndex = Random.Range(0, spawnPoints.Length);  
        // 检查生成的位置是否与上一次相同，如果相同则重新生成
        while (spawnPointIndex == lastSpawnPointIndex)
        {
            spawnPointIndex = Random.Range(0, spawnPoints.Length);
        }

        // 更新上一次的位置
        lastSpawnPointIndex = spawnPointIndex;

        Transform spawnPoint = spawnPoints[spawnPointIndex];

        // 在选择的位置生成怪物
        Instantiate(monsterPrefab, spawnPoint.position, spawnPoint.rotation);
        currentWavesNumber += 1;
    }


    // 计算数组前i个数字之和的函数
    public static int SumOfFirstIElements(int[] array, int i)
    {
        int sum = 0; // 初始化和为0

        // 确保i不会超出数组长度
        i = Mathf.Min(i, array.Length);

        // 循环累加前i个元素
        for (int j = 0; j < i; j++)
        {
            sum += array[j];
        }

        return sum; // 返回计算的和
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
