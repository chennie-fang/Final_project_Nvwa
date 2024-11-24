using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoPlayerController : MonoBehaviour
{
    private VideoPlayer videoPlayer;
    public RawImage rawImage; // 引用 RawImage 组件

    void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();

        // 播放视频
        videoPlayer.Play();

        // 调整 RawImage 大小
        RectTransform rectTransform = rawImage.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(60, 60); // 设置宽度和高度
    }
}
