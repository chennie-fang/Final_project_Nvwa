using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;

public class RecipeDisplayManager : MonoBehaviour
{
    public List<RecipeData> allRecipes;          // �䷽�б�
    public Transform recipeDisplayParent;        // �䷽ UI ������
    public GameObject recipeItemPrefab;          // ��Ʒͼ����λ��ͨ��Ԥ����
    public ItemManager itemManager;              // ���� ItemManager ���ڽ��ȸ���
    public int currentRecipeIndex = 0; // ��ǰ�䷽����
    
    private List<GameObject> currentRecipeItems = new List<GameObject>(); // ��ǰ�䷽����Ʒ��λʵ��
    private List<RecipeItemUI> currentRecipeUIs = new List<RecipeItemUI>(); // ��ǰ�䷽����Ʒ��λ UI �б�
    public Slider progressSlider; // ����չʾ�䷽���ȵĽ�����


    // �����ķ�����������ʾ�䷽�Ľ�����
    public void ShowRecipeWithProgress()
    {
        StartCoroutine(ProgressAndShowRecipe());
    }
    // Э�̣����ؽ�������������ɺ���ʾ�䷽
    private IEnumerator ProgressAndShowRecipe()
    {
        // ��ʼ��������
        progressSlider.value = 0;
        progressSlider.gameObject.SetActive(true);

        // ����������ʱ�䣨�ɸ�����Ҫ����ʱ�䣩
        float loadingTime = 2.0f;
        float elapsedTime = 0;

        // ƽ�����ӽ�������ֵ
        while (elapsedTime < loadingTime)
        {
            elapsedTime += Time.deltaTime;
            progressSlider.value = Mathf.Clamp01(elapsedTime / loadingTime);
            yield return null;
        }

        // ���ؽ���������ʾ�䷽
        progressSlider.gameObject.SetActive(false);
        ShowRecipeByIndex(currentRecipeIndex);
    }
    public void ShowResetRecipe()
    {
       // int rd = 0;
        //currentRecipeIndex++;
       // currentRecipeIndex = currentRecipeIndex % allRecipes.Count;
        //ShowRecipeByIndex(currentRecipeIndex);
        //ShowRecipeByIndex(1);
        ClearCurrentRecipe();
   
        //int randomIndex = Random.Range(1, allRecipes.Count); // �ӵڶ����䷽��ʼ���ѡ��
        //RecipeData selectedRecipe = allRecipes[randomIndex];
        ShowRecipeByIndex(currentRecipeIndex);
    }
    // ��ʾ��һ���䷽
    public void ShowNextRecipe()
    {
        recipeDisplayParent.gameObject.SetActive(true);
        //currentRecipeIndex = (currentRecipeIndex + 1) % allRecipes.Count;
        //ShowRecipeByIndex(currentRecipeIndex);
        ClearCurrentRecipe();
        currentRecipeIndex++;
        
        //itemManager.recipeDisplayManager.ShowNextRecipe(); // ��ʾ��һ���䷽
        //RecipeData selectedRecipe = allRecipes[currentRecipeIndex];
        ShowRecipeByIndex(currentRecipeIndex);
     
       
    }
    public bool isLastPEC()
    {
        return currentRecipeIndex >= allRecipes.Count-1;
 
    }
    public bool isFirstPEC()
    {
        return currentRecipeIndex == 0;
    }
    // ��������ʾ�䷽
    public void ShowRecipeByIndex(int index)
    {
        ClearCurrentRecipe();

        if (index < 0 || index >= allRecipes.Count)
        {
            itemManager.countdownPanel.SetActive(false);
            Debug.LogError("Invalid recipe index");
            return;
        }
        currentRecipeIndex = index;
        RecipeData selectedRecipe = allRecipes[currentRecipeIndex];
        DisplayRecipe(selectedRecipe);
    }
    private void DisplayRecipe(RecipeData recipeData)
    {
        // ��ʾ�䷽��
        foreach (var recipeItem in recipeData.items)
        {
            recipeItem.isPress = false;
            GameObject itemInstance = Instantiate(recipeItemPrefab, recipeDisplayParent);
            RecipeItemUI itemUI = itemInstance.GetComponent<RecipeItemUI>();
            itemUI.itemType = recipeItem.itemType;
            itemUI.highlightedSprite = recipeItem.highlightedIcon; // ��ֵ����ͼ��
            itemInstance.GetComponent<Image>().sprite = recipeItem.icon;
            currentRecipeItems.Add(itemInstance);
            currentRecipeUIs.Add(itemUI);
        }
        // ���µ�ǰ�䷽�� UI �б����ݸ� ItemManager
        itemManager.SetRecipe(recipeData, currentRecipeUIs);
    }
    // �����ǰ��ʾ���䷽��λ
    private void ClearCurrentRecipe()
    {
        for (int i = 0; i < currentRecipeItems.Count; i++)
        {
            Destroy(currentRecipeItems[i]);
        }
        for (int i = 0; i < currentRecipeUIs.Count; i++)
        {
            Destroy(currentRecipeUIs[i]);
        }
        
        currentRecipeItems.Clear();
        currentRecipeUIs.Clear();
    }
    private void Start()
    {
       // ��ʾ��һ�������䷽
        //ShowRandomRecipe(); // ��ʾ��һ���䷽
       // ShowRecipeByIndex(currentRecipeIndex); // ��ʾ��һ���䷽
    }
   public void ShowFirstRecipe()
    {
        ClearCurrentRecipe();

        RecipeData firstRecipe = allRecipes[0]; // ��һ���䷽��Ϊ�����䷽
        DisplayRecipe(firstRecipe);
    }
}



// ����ÿ����Ʒ��λ�����ͺ�ͼ��
[System.Serializable]
public class RecipeItem
{
    public ItemType itemType;  // ��Ʒ���ͣ�ö�٣�
    public Sprite icon;
    public Sprite highlightedIcon; // ����ͼ��
    public bool isPress=false;// ��Ʒͼ��
    
}

// �����䷽���ݽṹ
[System.Serializable]
public class RecipeData
{
    public List<RecipeItem> items;  // �䷽�������Ʒ�б�
    public int distractorCount;// ����������
    public GameObject winAsset;
    //public GameObject windescription;// �䷽�������
    public AudioClip windistractorSound; // ��������������Ч
    //private AudioSource audioSource; // �����ƵԴ
}


