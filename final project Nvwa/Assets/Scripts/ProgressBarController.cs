using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarController : MonoBehaviour
{
    public Slider progressBar; // ���� Slider ���
    public float duration = 5f; // ����������ʱ��

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
            progressBar.value = elapsedTime / duration; // ���½�����ֵ
            yield return null; // �ȴ���һ֡
        }

        progressBar.value = 1f; // ȷ������������
        // ��������ɺ���Ե�����������������ˢ���䷽
        RefreshRecipe();
    }

    private void RefreshRecipe()
    {
        // ���������ˢ���䷽���߼�
        Debug.Log("Refreshing recipe...");
        // ���磬���� RecipeDisplayManager �� ShowNextRecipe ����
        FindObjectOfType<RecipeDisplayManager>().ShowNextRecipe();
    }
}
