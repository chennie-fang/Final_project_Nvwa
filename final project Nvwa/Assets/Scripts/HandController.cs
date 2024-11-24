using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // 检查碰撞的物体是否是配方物品
        Item recipeItem = other.GetComponent<Item>();
        if (recipeItem != null)
        {
            // 处理物品消失
            HandleItemCollision(recipeItem);
        }
    }

    private void HandleItemCollision(Item recipeItem)
    {
        // 这里可以添加逻辑，例如高亮显示或其他效果
        Debug.Log("Touched recipe item: " + recipeItem.itemType);

        // 销毁配方物品
        Destroy(recipeItem.gameObject);
    }
}
