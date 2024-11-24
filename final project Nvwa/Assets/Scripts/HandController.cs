using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // �����ײ�������Ƿ����䷽��Ʒ
        Item recipeItem = other.GetComponent<Item>();
        if (recipeItem != null)
        {
            // ������Ʒ��ʧ
            HandleItemCollision(recipeItem);
        }
    }

    private void HandleItemCollision(Item recipeItem)
    {
        // �����������߼������������ʾ������Ч��
        Debug.Log("Touched recipe item: " + recipeItem.itemType);

        // �����䷽��Ʒ
        Destroy(recipeItem.gameObject);
    }
}
