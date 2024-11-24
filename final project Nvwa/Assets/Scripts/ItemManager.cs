using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ItemManager : MonoBehaviour
{
    public static ItemManager itemManager = null;
    private List<RecipeItemUI> currentRECUI = new List<RecipeItemUI>();
    private List<RecipeItem> requiredItems;                                     // 当前配方所需的物品
    //private Dictionary<PointUIPanels, GameObject> PointUIs = new();           // 可交互面板
    private Dictionary<Item, bool> placedItems = new Dictionary<Item, bool>();  // 记录物品是否已放置
    private RecipeData currentDatas;


    //public GameObject generatedObject;        // 需要逐渐显现的物体
    public GameObject PointerUI;
    public GameObject tutorialPanel;            // 新手引导界面
    public Button startButton;                  // 开始按钮
    public GameObject completionPanel;          // 配方完成时显示的UI面板
    public Button closeButton;                  // 关闭面板的按钮
    public GameObject countdownPanel;           // 倒计时界面
    public TMP_Text countdownText;              // 倒计时文本
    public GameObject congratulationsPanel;     // 恭喜完成的 UI 面板
    public Button closeCongratulationsButton;   // 关闭恭喜完成面板的按钮
    public Button nextSceneButton;              // 切换到下一个场景的按钮
    public GameObject testCompletionPanel;      //测试配方完成时的 UI 面板
    public Button proceedToMainButton;          // 进入正式配方的按钮
    public GameObject DiePanel;                 //失败界面
    public Button DieBtn;                       //失败按钮
    public GameObject testfpanel;               //新手引导结束
    public Button testfBtn;                     //新手引导结束按钮
    public GameObject peifangpanel;

    public GameObject itemPrefab; // 物体的预制件
    public Transform ringTransform; // 圆环的Transform
    public float spawnRadius; // 生成物体的半径


    private List<GameObject> spawnedItems = new List<GameObject>(); // 存储生成的物体
    private List<GameObject> distractorObjects = new List<GameObject>(); // 存储生成的干扰物体
    private bool isTestRecipe = true; // 标记当前是否为测试配方
    public RecipeDisplayManager recipeDisplayManager; // 引用 RecipeDisplayManager
    public List<Transform> Anchors = new List<Transform>();
 
    private Coroutine coolDownIE = null;
    public TMP_Text descriptionText; // 添加一个变量来引用描述文本
    public GameObject description;
    private AudioSource audioSource; // 添加音频源
    private void Start()
    {
        //初始化新手引导界面
        PointerUI.SetActive(true);
        tutorialPanel.SetActive(false);
        startButton.onClick.AddListener(OnStartButtonClicked);
        //完成界面
        completionPanel.SetActive(false);
        countdownPanel.SetActive(false);
        closeButton.onClick.AddListener(OnCloseButtonClicked);
        // 初始化恭喜完成面板
        congratulationsPanel.SetActive(false);
        closeCongratulationsButton.onClick.AddListener(CloseCongratulationsPanel);
        nextSceneButton.onClick.AddListener(SwitchToNextScene);
        // 初始化测试配方完成面板
        StartCoroutine(AllBefore());
        //proceedToMainButton.onClick.AddListener(OnProceedToMainButtonClicked);

        DiePanel.SetActive(false);
        DieBtn.onClick.AddListener(DieBtnclick);
        testfpanel.SetActive(false);
        testfBtn.onClick.AddListener(testfBtnlick);
        // 获取或添加 AudioSource 组件
        audioSource = gameObject.AddComponent<AudioSource>();
    }
    public void Awake()
    {
        itemManager = this;

    }
    public void OnDestroy()
    {
        itemManager = null;
    }
    public void DieBtnclick()
    {
        PointerUI.SetActive(false);
        DiePanel.SetActive(false);
        recipeDisplayManager.ShowResetRecipe();
        if (coolDownIE != null)
        {
            StopCoroutine(coolDownIE);
            coolDownIE = null;
            countdownPanel.SetActive(false);
        }
        coolDownIE = StartCoroutine(CountdownAndDie());
    }

    // 显示恭喜完成的 UI
    public void ShowCongratulations()
    {
        PointerUI.SetActive(true);
        congratulationsPanel.SetActive(true);
       
    }

    // 关闭恭喜完成的 UI
    private void CloseCongratulationsPanel()
    {
        congratulationsPanel.SetActive(false);
        // 这里可以添加任何需要在关闭时执行的逻辑
        peifangpanel.SetActive(false);
        countdownPanel.SetActive(false);
    }
    private void testfBtnlick()
    {
        PointerUI.SetActive(false);
        testfpanel.SetActive(false);
        // 
        recipeDisplayManager.allRecipes[0].winAsset.SetActive(false);
        recipeDisplayManager.ShowNextRecipe();
     
            if (coolDownIE != null)
            {
                StopCoroutine(coolDownIE);
                coolDownIE = null;
                countdownPanel.SetActive(false);
            }
            coolDownIE = StartCoroutine(CountdownAndDie());
        
        // 这里可以添加任何需要在关闭时执行的逻辑
    }
    // 切换到下一个场景
    private void SwitchToNextScene()
    {
        // 假设下一个场景的名称为 "StartScene"
        SceneManager.LoadScene("StartScene");
    }

    private void OnCloseButtonClicked()
    {
        completionPanel.SetActive(false);
        PointerUI.SetActive(false);
        // recipeDisplayManager.allRecipes[0].winAsset.SetActive(false);
       // isTestRecipe = false; // 设置为非测试配方

        //currentLevelIndex++; // 更新关卡索引
        recipeDisplayManager.ShowNextRecipe();// 显示下一个配方
        

            if (coolDownIE != null)
            {
                StopCoroutine(coolDownIE);
                coolDownIE = null;
                countdownPanel.SetActive(false);

            }
            coolDownIE = StartCoroutine(CountdownAndDie());
        
    }

    private void OnStartButtonClicked()
    {
        // 隐藏新手引导界面
        tutorialPanel.SetActive(false);
        PointerUI.SetActive(false);
        tutorialPanel.SetActive(false); // 点击开始按钮后隐藏新手教程面板
        // 启用拖拽功能
        // EnableItemDragging();
    }

    // 设置当前配方和UI映射
    public void SetRecipe(RecipeData recipeData, List<RecipeItemUI> recipeItemUIs)
    {
        requiredItems = recipeData.items;
        //placedItems.Clear();
        currentDatas = recipeData;
        currentRECUI = recipeItemUIs;
        // 更新描述文本
       // UpdateDescriptionText(recipeData.windescription);
        //ClearDistractors();
        //ClearSpawnedItems();
        //foreach (var item in requiredItems)
        //{
        //    placedItems[item.itemType] = false;
        //}
        //foreach (var ui in recipeItemUIs)
        //{
        //    //itemUIs[ui.itemType] = ui;
        //}
        ClearDistractors();

        // 生成新的干扰物体
        // GenerateDistractors(recipeData.distractorCount);
        ClearSpawnedItems();

        List<Transform> nachor = new List<Transform>();
        foreach (var item in Anchors)
        {
            nachor.Add(item);
        }
        // 生成配方所需的物体
        foreach (var item in recipeData.items)
        {
            int index = Random.Range(0, nachor.Count);
            GameObject newItem = Instantiate(itemPrefab, nachor[index].position, Quaternion.identity);
            nachor.Remove(nachor[index]);
            var itemty = newItem.GetComponent<Item>();
            itemty.itemType = item.itemType;
            spawnedItems.Add(newItem);
            //  placedItems.Add(itemty,false);
        }

        // 生成干扰物体
        for (int i = 0; i < recipeData.distractorCount; i++)
        {
            int index = Random.Range(0, nachor.Count);
            GameObject distractor = Instantiate(itemPrefab, nachor[index].position, Quaternion.identity);
            nachor.Remove(nachor[index]);
            distractor.GetComponent<Item>().itemType = ItemType.Lens;// GetRandomItemType();
            spawnedItems.Add(distractor);
        }

    }
    // 更新描述文本的方法
    private void UpdateDescriptionText(string newDescription)
    {
        descriptionText.text = newDescription; // 更新描述文本
    }
    private void ClearDistractors()
    {
        foreach (var obj in spawnedItems)
        {
            obj.SetActive(false);
        }
        spawnedItems.Clear();
    }
    private void ClearSpawnedItems()
    {
        foreach (var item in spawnedItems)
        {
            Destroy(item);
        }
        spawnedItems.Clear();
    }

    //private void GenerateDistractors(int count)
    //{
    //    for (int i = 0; i < count; i++)
    //    {
    //        GameObject distractor = Instantiate(itemPrefab, GetRandomPosition(), Quaternion.identity);
    //        distractorObjects.Add(distractor);
    //    }
    //}
    //private Vector3 GetRandomPosition()
    //{
    //    Vector2 randomCircle = Random.insideUnitCircle * spawnRadius;
    //    return new Vector3(
    //        ringTransform.position.x + randomCircle.x,
    //        ringTransform.position.y + randomCircle.y,
    //        ringTransform.position.z
    //    );
    //}
    public void StartIE(Item other)
    {
        if (itemManager != null)
        {
            // 启动协程
            StartCoroutine(OnTriggerEnterdown(other));
        }
    }
    public IEnumerator OnTriggerEnterdown(Item other)
    {
        Item item = other.GetComponent<Item>();
        if (item != null) 

        Debug.Log(item.itemType + " entered");
        
        foreach (var item2 in currentDatas.items)
        {
            if (item2.isPress) continue;

            if (item2.itemType == item.itemType)
            {
                item2.isPress = true;
                other.transform.parent = null;
                yield return null;
              ////  Destroy(other.gameObject);
              other.gameObject.SetActive(false);
                CheckRecipeCompletion();
                break;
            }
            else
            {
                StartCoroutine(ReturnItemToOriginalPosition(item));
            }
        }

        foreach (var itemui in currentRECUI)
        {
            if (itemui.HightLight) continue;

            if (itemui.itemType == item.itemType)
            {
                itemui.HighlightItem();//调用高亮物品
                break;
            }
        }
    }

    private IEnumerator ReturnItemToOriginalPosition(Item item)
    {
        Vector3 originalPosition = item.OriginalPosition;
        float duration = 1.0f;
        float elapsedTime = 0f;

        Vector3 startPosition = item.transform.position;

        while (elapsedTime < duration)
        {
            item.transform.position = Vector3.Lerp(startPosition, originalPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        item.transform.position = originalPosition;
    }

    public Image PorgassBar;
    private IEnumerator CountdownAndSwitchRecipe()
    {
        //countdownPanel.SetActive(true);
        float countdown = 6f;
        PorgassBar.gameObject.SetActive(true);
        while (countdown > 0)
        {
            countdownText.text = countdown.ToString();
            PorgassBar.transform.GetChild(0).GetComponent<Image>().fillAmount=1f-( countdown / 6f); 
            yield return null;
            countdown-=Time.deltaTime;
        }
        PorgassBar.gameObject.SetActive(false);
        countdownPanel.SetActive(false);
        recipeDisplayManager.ShowNextRecipe();
        if (coolDownIE!=null)
        {
            StopCoroutine(coolDownIE);
            coolDownIE = null;
            countdownPanel.SetActive(false);
        }
        coolDownIE = StartCoroutine(CountdownAndDie());
    }
    /// <summary>
    /// 失败的结束界面
    /// </summary>
    /// <returns></returns>

    public AudioClip DieSound;
    private IEnumerator CountdownAndDie()
    {
        countdownPanel.SetActive(true);
        int countdown = 30;
        while (countdown > 0)
        {
            countdownText.text = countdown.ToString();
            yield return new WaitForSeconds(1);
            countdown--;
        }
        countdownPanel.SetActive(false);
        PointerUI.SetActive(true);
        //DiePanel.SetActive(true);
        //ShowFailurePanel(); // 调用显示失败界面的方法
        // 显示失败界面
        DiePanel.SetActive(true);
        ClearDistractors();
        yield return new WaitForSeconds(1f);
        PlayDestructionSound(DieSound);
        //recipeDisplayManager.ShowNextRecipe();
    }
    //public void ShowFailurePanel()
    //{
    //    // 显示失败界面
    //    DiePanel.SetActive(true);

    //    // 启动自动关闭协程
    //    StartCoroutine(AutoCloseFailurePanel());
    //}

    //private IEnumerator AutoCloseFailurePanel()
    //{
    //    yield return new WaitForSeconds(1f); // 等待一秒
    //    DiePanel.SetActive(false); // 关闭失败界面
    //}

    private void CheckRecipeCompletion()
    {
         foreach (var placed in currentDatas.items)
       {
           if (!placed.isPress) return;
         }

        Debug.Log("Recipe completed!");
        ClearDistractors();
        recipeDisplayManager.recipeDisplayParent.gameObject.SetActive(false);
        // 显示成功面板
        if (recipeDisplayManager.isLastPEC())
        {
            //ShowCongratulations();
            StartCoroutine(FadeInGeneratedObject(currentDatas.winAsset, currentDatas.windistractorSound));
            //StartCoroutine(FadeInGeneratedObject(currentDatas.winAsset));
            //countdownPanel.SetActive(false);
            return;
        }
        else if (recipeDisplayManager.isFirstPEC())
        {
            PointerUI.SetActive(true);
            tutorialPanel.SetActive(false);
            StartCoroutine(FadeInGeneratedObject(currentDatas.winAsset, currentDatas.windistractorSound));
            // StartCoroutine(FadeInGeneratedObject(currentDatas.winAsset));
        }
        else
        {
            ShowCompletionPanel();
            StartCoroutine(FadeInGeneratedObject(currentDatas.winAsset, currentDatas.windistractorSound));
        }
        //启动倒计时
        if (coolDownIE != null)
        {
            StopCoroutine(coolDownIE);
            coolDownIE = null;
            countdownPanel.SetActive(false);
        }

    }


    private void OnProceedToMainButtonClicked()
    {
        testCompletionPanel.SetActive(false);
        // PointerUI.SetActive(false);
        peifangpanel.SetActive(true);
        isTestRecipe = false;
        Debug.Log("diaoquchenggongmei" + tutorialPanel);
        tutorialPanel.SetActive(true);
        recipeDisplayManager.ShowFirstRecipe();
       // if (coolDownIE!=null)
       // {
       //     StopCoroutine(coolDownIE);
       //     coolDownIE = null;
       //     countdownPanel.SetActive(false);
       // }
       //coolDownIE= StartCoroutine(CountdownAndDie());
        //// 继续挑战的逻辑
        //testCompletionPanel.SetActive(false);
        //PointerUI.SetActive(false);
        //isTestRecipe = false; // 设置为非测试配方
        //                      // 这里可以添加逻辑来开始新的挑战
        //                      // 例如，重置当前配方或加载下一个配方
        //recipeDisplayManager.ShowNextRecipe();
    }

    private void ShowCompletionPanel()
    {
        PointerUI.SetActive(true);
        //completionPanel.SetActive(true);

    }
    // 获取圆环附近的随机位置
    //private Vector3 GetRandomPositionNearRing()
    //{
    //    Vector2 randomCircle = Random.insideUnitCircle * spawnRadius;

    //    if (randomCircle.x>0)
    //    {
    //      randomCircle=new Vector2(  randomCircle.x+0.5f,randomCircle.y);
    //    }
    //    if (randomCircle.x<0)
    //    {
    //        randomCircle = new Vector2(randomCircle.x- 0.5f, randomCircle.y);
    //    }
    //    if (randomCircle.y > 0)
    //    {
    //        randomCircle = new Vector2(randomCircle.x, randomCircle.y + 0.5f);
    //    }
    //    if (randomCircle.y < 0)
    //    {
    //        randomCircle = new Vector2(randomCircle.x , randomCircle.y- 0.5f);
    //    }
    //    Vector3 randomPosition = new Vector3(
    //        ringTransform.position.x+ randomCircle.x,
    //        ringTransform.position.y + randomCircle.y,
    //        ringTransform.position.z
    //    );
    //    Debug.Log(randomPosition.x+":" + randomPosition.y + ":" + randomPosition.z + "wtffffffff");
    //    return randomPosition;
    //}

    //private void GenerateDistractors(int count)
    //{
    //    for (int i = 0; i < count; i++)
    //    {
    //        GameObject distractor = Instantiate(generatedObject, GetRandomPosition(), Quaternion.identity);
    //        distractorObjects.Add(distractor);
    //    }
    //}
    // 获取随机的ItemType
    private ItemType GetRandomItemType()
    {
        ItemType[] itemTypes = (ItemType[])System.Enum.GetValues(typeof(ItemType));
        return itemTypes[Random.Range(0, itemTypes.Length)];
    }

    //// 当物品进入碰撞区域时调用
    //private void OnTriggerEnter(Collider other)
    //{
    //    Item item = other.GetComponent<Item>();
    //    Debug.Log(item.itemType + "元素类型");

    //    if (item!=null)
    //    {
    //        foreach (var item2 in currentDatas.items)
    //        {
    //            if (item2.isPress)
    //            {
    //                continue;
    //            }
    //            if (item2.itemType == item.itemType)
    //            {
    //                item2.isPress = true;                  
    //                // 更新相应的 UI 图标
    //                //Destroy(other.gameObject);
    //                other.gameObject.SetActive(false);
    //                CheckRecipeCompletion();
    //                break;
    //            }
    //            else
    //            {
    //                StartCoroutine(ReturnItemToOriginalPosition(item));
    //            }

    //        }
    //    }
    //    foreach (var itemui in currentRECUI)
    //    {
    //        if (itemui.HightLight)
    //        {
    //            continue;
    //        }
    //        if (itemui.itemType == item.itemType)
    //        {
    //            itemui.HighlightItem();
    //            break;
    //            // 检查配方是否完成

    //        }
    //    }

    //}
    //private IEnumerator ReturnItemToOriginalPosition(Item item)
    //{
    //    Vector3 originalPosition = item.OriginalPosition; // 假设 Item 类中有一个 OriginalPosition 属性
    //    float duration = 1.0f; // 返回的持续时间
    //    float elapsedTime = 0f;

    //    Vector3 startPosition = item.transform.position;

    //    while (elapsedTime < duration)
    //    {
    //        item.transform.position = Vector3.Lerp(startPosition, originalPosition, elapsedTime / duration);
    //        elapsedTime += Time.deltaTime;
    //        yield return null;
    //    }

    //    item.transform.position = originalPosition;
    //}

    //private IEnumerator CountdownAndSwitchRecipe()
    //{
    //    countdownPanel.SetActive(true);
    //    int countdown = 10;
    //    while (countdown > 0)
    //    {
    //        countdownText.text = countdown.ToString();
    //        yield return new WaitForSeconds(1);
    //        countdown--;
    //    }
    //    countdownPanel.SetActive(false);
    //    // 切换到下一个配方的逻辑
    //    recipeDisplayManager.ShowNextRecipe();
    //}
    // 检查配方是否全部完成
    //private void CheckRecipeCompletion()
    //{
    //    foreach (var placed in currentDatas.items)
    //    {
    //        if (!placed.isPress) return;
    //    }
    //    Debug.Log("配方已完成！");
    //    // 配方完成后的逻辑
    //    ShowCompletionPanel();
    //    StartCoroutine(FadeInGeneratedObject(currentDatas.winAsset));
    //    // 通知 RecipeDisplayManager 切换到下一个配方

    //}

    //private void ShowTestCompletionPanel()
    //{
    //    testCompletionPanel.SetActive(true);
    //}
    //private void OnProceedToMainButtonClicked()
    //{
    //    testCompletionPanel.SetActive(false);
    //    isTestRecipe = false; // 标记为正式配方
    //    FindObjectOfType<RecipeDisplayManager>().ShowNextRecipe();
    //}
    //private void CloseFinalPanel()
    //{
    //    finalCompletionPanel.SetActive(false);
    //    // 销毁所有干扰物体
    //    foreach (var obj in distractorObjects)
    //    {
    //        Destroy(obj);
    //    }
    //    distractorObjects.Clear();
    //}
    private IEnumerator FadeGeneratedObject(GameObject asset, AudioClip distractorSound)
    {
        yield return new WaitForSeconds(1.5f);
        ShowCongratulations();
        asset.SetActive(true);
        Debug.Log("Playing sound: " + distractorSound); // 输出音频剪辑信息
        PlayDestructionSound(distractorSound);
    }
    private IEnumerator FadeInGeneratedObject(GameObject asset, AudioClip distractorSound)
    {
        //float duration = 2.0f; // 逐渐显现的时间
        //float elapsedTime = 0f;
        //Vector3 initialScale = Vector3.zero; // 初始缩放为0
        // Vector3 targetScale = Vector3.one;   // 目标缩放为1（原始大小）
        //generatedObject.transform.localScale = initialScale;
        //generatedObject.SetActive(true);

        //Renderer renderer = asset.GetComponent<Renderer>();
        //Color color = renderer.material.color;
        //color.a = 0;
        //renderer.material.color = color;
        
        yield return new WaitForSeconds(1.5f);
        asset.SetActive(true);
        //UpdateDescriptionText(aa);
        Debug.Log("Playing sound: " + distractorSound); // 输出音频剪辑信息
        PlayDestructionSound(distractorSound);
        
        if (recipeDisplayManager.isLastPEC())
        {
            peifangpanel.SetActive(false);
            
            ShowCongratulations();
            if (coolDownIE != null)
            {
                StopCoroutine(coolDownIE);
                coolDownIE = null;
                countdownPanel.SetActive(false);
            }
            yield break;
        }
        else if (recipeDisplayManager.isFirstPEC())
        {
            yield break;
        }
        else
        {
            if (coolDownIE != null)
            {
                StopCoroutine(coolDownIE);
                coolDownIE = null;
                countdownPanel.SetActive(false);
            }
           // coolDownIE = StartCoroutine(CountdownAndDie());
            coolDownIE = StartCoroutine(CountdownAndSwitchRecipe()); // 等待音频播放完毕后进入下一关
        }
            //description.SetActive(true);
            //while (elapsedTime < duration)
            //{
            //    //elapsedTime += Time.deltaTime;
            //    //float scaleProgress = Mathf.Lerp(0, 1, elapsedTime / duration); // 使用线性插值
            //    //generatedObject.transform.localScale = Vector3.Lerp(initialScale, targetScale, scaleProgress);
            //    //yield return null;
            //    //材质不透明度
            //    elapsedTime += Time.deltaTime;
            //    color.a = Mathf.Clamp01(elapsedTime / duration);
            //    float alpha = Mathf.Lerp(0, 1, elapsedTime / duration);
            //    color.a = alpha;
            //    renderer.material.color = color;
            //    yield return null;
            //}
        }
    public  void PlayDestructionSound(AudioClip sound)
    {
        if (sound != null)
        {
            audioSource.PlayOneShot(sound); // 播放指定的音效
        }
    }
    // 等待音频播放完毕后进入下一关
    private IEnumerator WaitForSoundAndProceed(float soundDuration)
    {
        yield return new WaitForSeconds(soundDuration); // 等待音频播放完毕
                                                        // 等待几秒钟再进入下一个配方
        yield return new WaitForSeconds(1f); // 这里可以调整等待时间
        //// 进入下一关的逻辑
        //recipeDisplayManager.ShowNextRecipe();

        // 启动倒计时并进入下一个配方
        coolDownIE = StartCoroutine(CountdownAndSwitchRecipe());
    }

    private IEnumerator AllBefore()
    {
        yield return new WaitForSeconds(2f);
        OnProceedToMainButtonClicked();
        //testCompletionPanel.SetActive(true);
    }
}


public enum ItemType
{
    Water,
    Earth,
    Leaf,
    Lens,
    // 其他物品类型可以在这里添加
}

