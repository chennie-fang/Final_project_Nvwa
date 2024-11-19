using UnityEngine;
using UnityEngine.UI;

public class RecipeItemUI : MonoBehaviour
{
    public ItemType itemType; // 设置为配方项的物品类型
    private Image itemImage;      // 图标的 Image 组件
    public Sprite highlightedSprite; // 高亮时的图标
    private Sprite originalSprite; // 原始图标

    public bool HightLight = false;
    private void Awake()
    {
        itemImage = GetComponent<Image>();
        originalSprite = itemImage.sprite; // 保存原始图标
    }

    // 高亮物品图标
    public void HighlightItem()
    {
        if (highlightedSprite != null)
        {
            Debug.Log("Switching to highlighted sprite for " + itemType);
            itemImage.sprite = highlightedSprite; // 切换到高亮图标
            HightLight = true;
        }
        else
        {
            Debug.LogWarning("Highlighted sprite is not set for " + itemType);
        }
        //itemImage.sprite = highlightedSprite; // 切换到高亮图标
        //HightLight = true;
    }

    // 恢复原始图标
    public void ResetHighlight()
    {
        itemImage.sprite = originalSprite; // 恢复原始图标
        HightLight = false;
    }
}
