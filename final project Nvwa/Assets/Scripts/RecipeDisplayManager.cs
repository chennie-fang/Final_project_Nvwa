using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;

public class RecipeDisplayManager : MonoBehaviour
{
    public List<RecipeData> allRecipes;          // 配方列表
    public Transform recipeDisplayParent;        // 配方 UI 父物体
    public GameObject recipeItemPrefab;          // 物品图标栏位的通用预制体
    public ItemManager itemManager;              // 引用 ItemManager 用于进度跟踪
    public int currentRecipeIndex = 0; // 当前配方索引
    
    private List<GameObject> currentRecipeItems = new List<GameObject>(); // 当前配方的物品栏位实例
    private List<RecipeItemUI> currentRecipeUIs = new List<RecipeItemUI>(); // 当前配方的物品栏位 UI 列表
    public Slider progressSlider; // 用于展示配方进度的进度条


    // 新增的方法，启动显示配方的进度条
    public void ShowRecipeWithProgress()
    {
        StartCoroutine(ProgressAndShowRecipe());
    }
    // 协程：加载进度条，并在完成后显示配方
    private IEnumerator ProgressAndShowRecipe()
    {
        // 初始化进度条
        progressSlider.value = 0;
        progressSlider.gameObject.SetActive(true);

        // 进度条加载时间（可根据需要调整时间）
        float loadingTime = 2.0f;
        float elapsedTime = 0;

        // 平滑增加进度条的值
        while (elapsedTime < loadingTime)
        {
            elapsedTime += Time.deltaTime;
            progressSlider.value = Mathf.Clamp01(elapsedTime / loadingTime);
            yield return null;
        }

        // 隐藏进度条并显示配方
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
   
        //int randomIndex = Random.Range(1, allRecipes.Count); // 从第二个配方开始随机选择
        //RecipeData selectedRecipe = allRecipes[randomIndex];
        ShowRecipeByIndex(currentRecipeIndex);
    }
    // 显示下一个配方
    public void ShowNextRecipe()
    {
        recipeDisplayParent.gameObject.SetActive(true);
        //currentRecipeIndex = (currentRecipeIndex + 1) % allRecipes.Count;
        //ShowRecipeByIndex(currentRecipeIndex);
        ClearCurrentRecipe();
        currentRecipeIndex++;
        
        //itemManager.recipeDisplayManager.ShowNextRecipe(); // 显示下一个配方
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
    // 按索引显示配方
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
        // 显示配方项
        foreach (var recipeItem in recipeData.items)
        {
            recipeItem.isPress = false;
            GameObject itemInstance = Instantiate(recipeItemPrefab, recipeDisplayParent);
            RecipeItemUI itemUI = itemInstance.GetComponent<RecipeItemUI>();
            itemUI.itemType = recipeItem.itemType;
            itemUI.highlightedSprite = recipeItem.highlightedIcon; // 赋值高亮图标
            itemInstance.GetComponent<Image>().sprite = recipeItem.icon;
            currentRecipeItems.Add(itemInstance);
            currentRecipeUIs.Add(itemUI);
        }
        // 更新当前配方的 UI 列表并传递给 ItemManager
        itemManager.SetRecipe(recipeData, currentRecipeUIs);
    }
    // 清除当前显示的配方栏位
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
       // 显示第一个测试配方
        //ShowRandomRecipe(); // 显示第一个配方
       // ShowRecipeByIndex(currentRecipeIndex); // 显示第一个配方
    }
   public void ShowFirstRecipe()
    {
        ClearCurrentRecipe();

        RecipeData firstRecipe = allRecipes[0]; // 第一个配方作为测试配方
        DisplayRecipe(firstRecipe);
    }
}



// 定义每个物品栏位的类型和图标
[System.Serializable]
public class RecipeItem
{
    public ItemType itemType;  // 物品类型（枚举）
    public Sprite icon;
    public Sprite highlightedIcon; // 高亮图标
    public bool isPress=false;// 物品图标
    
}

// 定义配方数据结构
[System.Serializable]
public class RecipeData
{
    public List<RecipeItem> items;  // 配方所需的物品列表
    public int distractorCount;// 干扰项数量
    public GameObject winAsset;
    //public GameObject windescription;// 配方完成描述
    public AudioClip windistractorSound; // 干扰物体销毁音效
    //private AudioSource audioSource; // 添加音频源
}


