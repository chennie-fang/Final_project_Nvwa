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
    private List<RecipeItem> requiredItems;                                     // ��ǰ�䷽�������Ʒ
    //private Dictionary<PointUIPanels, GameObject> PointUIs = new();           // �ɽ������
    private Dictionary<Item, bool> placedItems = new Dictionary<Item, bool>();  // ��¼��Ʒ�Ƿ��ѷ���
    private RecipeData currentDatas;


    //public GameObject generatedObject;        // ��Ҫ�����ֵ�����
    public GameObject PointerUI;
    public GameObject tutorialPanel;            // ������������
    public Button startButton;                  // ��ʼ��ť
    public GameObject completionPanel;          // �䷽���ʱ��ʾ��UI���
    public Button closeButton;                  // �ر����İ�ť
    public GameObject countdownPanel;           // ����ʱ����
    public TMP_Text countdownText;              // ����ʱ�ı�
    public GameObject congratulationsPanel;     // ��ϲ��ɵ� UI ���
    public Button closeCongratulationsButton;   // �رչ�ϲ������İ�ť
    public Button nextSceneButton;              // �л�����һ�������İ�ť
    public GameObject testCompletionPanel;      //�����䷽���ʱ�� UI ���
    public Button proceedToMainButton;          // ������ʽ�䷽�İ�ť
    public GameObject DiePanel;                 //ʧ�ܽ���
    public Button DieBtn;                       //ʧ�ܰ�ť
    public GameObject testfpanel;               //������������
    public Button testfBtn;                     //��������������ť
    public GameObject peifangpanel;

    public GameObject itemPrefab; // �����Ԥ�Ƽ�
    public Transform ringTransform; // Բ����Transform
    public float spawnRadius; // ��������İ뾶


    private List<GameObject> spawnedItems = new List<GameObject>(); // �洢���ɵ�����
    private List<GameObject> distractorObjects = new List<GameObject>(); // �洢���ɵĸ�������
    private bool isTestRecipe = true; // ��ǵ�ǰ�Ƿ�Ϊ�����䷽
    public RecipeDisplayManager recipeDisplayManager; // ���� RecipeDisplayManager
    public List<Transform> Anchors = new List<Transform>();
 
    private Coroutine coolDownIE = null;
    public TMP_Text descriptionText; // ���һ�����������������ı�
    public GameObject description;
    private AudioSource audioSource; // �����ƵԴ
    private void Start()
    {
        //��ʼ��������������
        PointerUI.SetActive(true);
        tutorialPanel.SetActive(false);
        startButton.onClick.AddListener(OnStartButtonClicked);
        //��ɽ���
        completionPanel.SetActive(false);
        countdownPanel.SetActive(false);
        closeButton.onClick.AddListener(OnCloseButtonClicked);
        // ��ʼ����ϲ������
        congratulationsPanel.SetActive(false);
        closeCongratulationsButton.onClick.AddListener(CloseCongratulationsPanel);
        nextSceneButton.onClick.AddListener(SwitchToNextScene);
        // ��ʼ�������䷽������
        StartCoroutine(AllBefore());
        //proceedToMainButton.onClick.AddListener(OnProceedToMainButtonClicked);

        DiePanel.SetActive(false);
        DieBtn.onClick.AddListener(DieBtnclick);
        testfpanel.SetActive(false);
        testfBtn.onClick.AddListener(testfBtnlick);
        // ��ȡ����� AudioSource ���
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

    // ��ʾ��ϲ��ɵ� UI
    public void ShowCongratulations()
    {
        PointerUI.SetActive(true);
        congratulationsPanel.SetActive(true);
       
    }

    // �رչ�ϲ��ɵ� UI
    private void CloseCongratulationsPanel()
    {
        congratulationsPanel.SetActive(false);
        // �����������κ���Ҫ�ڹر�ʱִ�е��߼�
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
        
        // �����������κ���Ҫ�ڹر�ʱִ�е��߼�
    }
    // �л�����һ������
    private void SwitchToNextScene()
    {
        // ������һ������������Ϊ "StartScene"
        SceneManager.LoadScene("StartScene");
    }

    private void OnCloseButtonClicked()
    {
        completionPanel.SetActive(false);
        PointerUI.SetActive(false);
        // recipeDisplayManager.allRecipes[0].winAsset.SetActive(false);
       // isTestRecipe = false; // ����Ϊ�ǲ����䷽

        //currentLevelIndex++; // ���¹ؿ�����
        recipeDisplayManager.ShowNextRecipe();// ��ʾ��һ���䷽
        

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
        // ����������������
        tutorialPanel.SetActive(false);
        PointerUI.SetActive(false);
        tutorialPanel.SetActive(false); // �����ʼ��ť���������ֽ̳����
        // ������ק����
        // EnableItemDragging();
    }

    // ���õ�ǰ�䷽��UIӳ��
    public void SetRecipe(RecipeData recipeData, List<RecipeItemUI> recipeItemUIs)
    {
        requiredItems = recipeData.items;
        //placedItems.Clear();
        currentDatas = recipeData;
        currentRECUI = recipeItemUIs;
        // ���������ı�
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

        // �����µĸ�������
        // GenerateDistractors(recipeData.distractorCount);
        ClearSpawnedItems();

        List<Transform> nachor = new List<Transform>();
        foreach (var item in Anchors)
        {
            nachor.Add(item);
        }
        // �����䷽���������
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

        // ���ɸ�������
        for (int i = 0; i < recipeData.distractorCount; i++)
        {
            int index = Random.Range(0, nachor.Count);
            GameObject distractor = Instantiate(itemPrefab, nachor[index].position, Quaternion.identity);
            nachor.Remove(nachor[index]);
            distractor.GetComponent<Item>().itemType = ItemType.Lens;// GetRandomItemType();
            spawnedItems.Add(distractor);
        }

    }
    // ���������ı��ķ���
    private void UpdateDescriptionText(string newDescription)
    {
        descriptionText.text = newDescription; // ���������ı�
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
            // ����Э��
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
                itemui.HighlightItem();//���ø�����Ʒ
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
    /// ʧ�ܵĽ�������
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
        //ShowFailurePanel(); // ������ʾʧ�ܽ���ķ���
        // ��ʾʧ�ܽ���
        DiePanel.SetActive(true);
        ClearDistractors();
        yield return new WaitForSeconds(1f);
        PlayDestructionSound(DieSound);
        //recipeDisplayManager.ShowNextRecipe();
    }
    //public void ShowFailurePanel()
    //{
    //    // ��ʾʧ�ܽ���
    //    DiePanel.SetActive(true);

    //    // �����Զ��ر�Э��
    //    StartCoroutine(AutoCloseFailurePanel());
    //}

    //private IEnumerator AutoCloseFailurePanel()
    //{
    //    yield return new WaitForSeconds(1f); // �ȴ�һ��
    //    DiePanel.SetActive(false); // �ر�ʧ�ܽ���
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
        // ��ʾ�ɹ����
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
        //��������ʱ
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
        //// ������ս���߼�
        //testCompletionPanel.SetActive(false);
        //PointerUI.SetActive(false);
        //isTestRecipe = false; // ����Ϊ�ǲ����䷽
        //                      // �����������߼�����ʼ�µ���ս
        //                      // ���磬���õ�ǰ�䷽�������һ���䷽
        //recipeDisplayManager.ShowNextRecipe();
    }

    private void ShowCompletionPanel()
    {
        PointerUI.SetActive(true);
        //completionPanel.SetActive(true);

    }
    // ��ȡԲ�����������λ��
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
    // ��ȡ�����ItemType
    private ItemType GetRandomItemType()
    {
        ItemType[] itemTypes = (ItemType[])System.Enum.GetValues(typeof(ItemType));
        return itemTypes[Random.Range(0, itemTypes.Length)];
    }

    //// ����Ʒ������ײ����ʱ����
    //private void OnTriggerEnter(Collider other)
    //{
    //    Item item = other.GetComponent<Item>();
    //    Debug.Log(item.itemType + "Ԫ������");

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
    //                // ������Ӧ�� UI ͼ��
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
    //            // ����䷽�Ƿ����

    //        }
    //    }

    //}
    //private IEnumerator ReturnItemToOriginalPosition(Item item)
    //{
    //    Vector3 originalPosition = item.OriginalPosition; // ���� Item ������һ�� OriginalPosition ����
    //    float duration = 1.0f; // ���صĳ���ʱ��
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
    //    // �л�����һ���䷽���߼�
    //    recipeDisplayManager.ShowNextRecipe();
    //}
    // ����䷽�Ƿ�ȫ�����
    //private void CheckRecipeCompletion()
    //{
    //    foreach (var placed in currentDatas.items)
    //    {
    //        if (!placed.isPress) return;
    //    }
    //    Debug.Log("�䷽����ɣ�");
    //    // �䷽��ɺ���߼�
    //    ShowCompletionPanel();
    //    StartCoroutine(FadeInGeneratedObject(currentDatas.winAsset));
    //    // ֪ͨ RecipeDisplayManager �л�����һ���䷽

    //}

    //private void ShowTestCompletionPanel()
    //{
    //    testCompletionPanel.SetActive(true);
    //}
    //private void OnProceedToMainButtonClicked()
    //{
    //    testCompletionPanel.SetActive(false);
    //    isTestRecipe = false; // ���Ϊ��ʽ�䷽
    //    FindObjectOfType<RecipeDisplayManager>().ShowNextRecipe();
    //}
    //private void CloseFinalPanel()
    //{
    //    finalCompletionPanel.SetActive(false);
    //    // �������и�������
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
        Debug.Log("Playing sound: " + distractorSound); // �����Ƶ������Ϣ
        PlayDestructionSound(distractorSound);
    }
    private IEnumerator FadeInGeneratedObject(GameObject asset, AudioClip distractorSound)
    {
        //float duration = 2.0f; // �����ֵ�ʱ��
        //float elapsedTime = 0f;
        //Vector3 initialScale = Vector3.zero; // ��ʼ����Ϊ0
        // Vector3 targetScale = Vector3.one;   // Ŀ������Ϊ1��ԭʼ��С��
        //generatedObject.transform.localScale = initialScale;
        //generatedObject.SetActive(true);

        //Renderer renderer = asset.GetComponent<Renderer>();
        //Color color = renderer.material.color;
        //color.a = 0;
        //renderer.material.color = color;
        
        yield return new WaitForSeconds(1.5f);
        asset.SetActive(true);
        //UpdateDescriptionText(aa);
        Debug.Log("Playing sound: " + distractorSound); // �����Ƶ������Ϣ
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
            coolDownIE = StartCoroutine(CountdownAndSwitchRecipe()); // �ȴ���Ƶ������Ϻ������һ��
        }
            //description.SetActive(true);
            //while (elapsedTime < duration)
            //{
            //    //elapsedTime += Time.deltaTime;
            //    //float scaleProgress = Mathf.Lerp(0, 1, elapsedTime / duration); // ʹ�����Բ�ֵ
            //    //generatedObject.transform.localScale = Vector3.Lerp(initialScale, targetScale, scaleProgress);
            //    //yield return null;
            //    //���ʲ�͸����
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
            audioSource.PlayOneShot(sound); // ����ָ������Ч
        }
    }
    // �ȴ���Ƶ������Ϻ������һ��
    private IEnumerator WaitForSoundAndProceed(float soundDuration)
    {
        yield return new WaitForSeconds(soundDuration); // �ȴ���Ƶ�������
                                                        // �ȴ��������ٽ�����һ���䷽
        yield return new WaitForSeconds(1f); // ������Ե����ȴ�ʱ��
        //// ������һ�ص��߼�
        //recipeDisplayManager.ShowNextRecipe();

        // ��������ʱ��������һ���䷽
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
    // ������Ʒ���Ϳ������������
}

