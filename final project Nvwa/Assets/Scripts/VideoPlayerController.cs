using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoPlayerController : MonoBehaviour
{
    private VideoPlayer videoPlayer;
    public RawImage rawImage; // ���� RawImage ���

    void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();

        // ������Ƶ
        videoPlayer.Play();

        // ���� RawImage ��С
        RectTransform rectTransform = rawImage.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(60, 60); // ���ÿ�Ⱥ͸߶�
    }
}
