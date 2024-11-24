using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarController : MonoBehaviour
{
    public Slider progressBar; // 引用 Slider 组件
    public float duration = 5f; // 进度条持续时间

    public void StartProgressBar()
    {
        StartCoroutine(ProgressBarCoroutine());
    }

    private IEnumerator ProgressBarCoroutine()
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            progressBar.value = elapsedTime / duration; // 更新进度条值
            yield return null; // 等待下一帧
        }

        progressBar.value = 1f; // 确保进度条填满
        // 进度条完成后可以调用其他方法，例如刷新配方
        RefreshRecipe();
    }

    private void RefreshRecipe()
    {
        // 在这里添加刷新配方的逻辑
        Debug.Log("Refreshing recipe...");
        // 例如，调用 RecipeDisplayManager 的 ShowNextRecipe 方法
        FindObjectOfType<RecipeDisplayManager>().ShowNextRecipe();
    }
}
