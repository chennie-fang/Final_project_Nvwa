using UnityEngine;
using UnityEngine.UI;

public class RecipeItemUI : MonoBehaviour
{
    public ItemType itemType; // ����Ϊ�䷽�����Ʒ����
    private Image itemImage;      // ͼ��� Image ���
    public Sprite highlightedSprite; // ����ʱ��ͼ��
    private Sprite originalSprite; // ԭʼͼ��

    public bool HightLight = false;
    private void Awake()
    {
        itemImage = GetComponent<Image>();
        originalSprite = itemImage.sprite; // ����ԭʼͼ��
    }

    // ������Ʒͼ��
    public void HighlightItem()
    {
        if (highlightedSprite != null)
        {
            Debug.Log("Switching to highlighted sprite for " + itemType);
            itemImage.sprite = highlightedSprite; // �л�������ͼ��
            HightLight = true;
        }
        else
        {
            Debug.LogWarning("Highlighted sprite is not set for " + itemType);
        }
        //itemImage.sprite = highlightedSprite; // �л�������ͼ��
        //HightLight = true;
    }

    // �ָ�ԭʼͼ��
    public void ResetHighlight()
    {
        itemImage.sprite = originalSprite; // �ָ�ԭʼͼ��
        HightLight = false;
    }
}
