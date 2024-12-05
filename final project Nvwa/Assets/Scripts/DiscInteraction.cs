using UnityEngine;
using UnityEngine.Video;

public class DiscInteraction : MonoBehaviour
{
    public Animator tvAnimator;
    public GameObject screen; 
    public GameObject defaults;
    public VideoPlayer videoPlayer;
    public GameObject discObject; // 光盘对象
    public GameObject targetTV; 

    private Vector3 initialDiscPosition; // 光盘初始位置
    private Quaternion initialDiscRotation; // 光盘初始旋转

    private bool isVideoPlaying = false;
    private void Start()
    {
        // 保存光盘的初始位置和旋转
        initialDiscPosition = discObject.transform.position;
        initialDiscRotation = discObject.transform.rotation;
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name + "??????????????????" + gameObject.name);
        if (gameObject == targetTV && !isVideoPlaying)
        {
            // 播放插入动画
            tvAnimator.Play("CDin");

            // 隐藏光盘
            discObject.SetActive(false);

            // 延迟播放视频
            Invoke(nameof(PlayVideo), 2.0f); // 动画播放完成后开始视频播放
        }
    }
   

    private void PlayVideo()
    {
        defaults.SetActive(false);
        screen.SetActive(true);
        videoPlayer.Play();
        isVideoPlaying = true;
        Invoke(nameof(ResetScreen), (float)videoPlayer.clip.length);
    }

    private void ResetScreen()
    {
        videoPlayer.Stop();
        defaults.SetActive(true);
        screen.SetActive(false);
        ResetDisc();
        isVideoPlaying = false;

    }
    private void ResetDisc()
    {
        // 重置光盘位置和旋转

        // discObject.transform.position = initialDiscPosition;
        //discObject.transform.rotation = initialDiscRotation;
        tvAnimator.Play("CDout");

        // 显示光盘
        //discObject.SetActive(true);
    }
}
