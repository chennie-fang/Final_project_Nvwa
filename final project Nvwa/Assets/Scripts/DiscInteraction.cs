using UnityEngine;
using UnityEngine.Video;

public class DiscInteraction : MonoBehaviour
{
    public Animator tvAnimator;
    public GameObject screen; 
    public GameObject defaults;
    public VideoPlayer videoPlayer;
    public GameObject discObject; // ���̶���
    public GameObject targetTV; 

    private Vector3 initialDiscPosition; // ���̳�ʼλ��
    private Quaternion initialDiscRotation; // ���̳�ʼ��ת

    private bool isVideoPlaying = false;
    private void Start()
    {
        // ������̵ĳ�ʼλ�ú���ת
        initialDiscPosition = discObject.transform.position;
        initialDiscRotation = discObject.transform.rotation;
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name + "??????????????????" + gameObject.name);
        if (gameObject == targetTV && !isVideoPlaying)
        {
            // ���Ų��붯��
            tvAnimator.Play("CDin");

            // ���ع���
            discObject.SetActive(false);

            // �ӳٲ�����Ƶ
            Invoke(nameof(PlayVideo), 2.0f); // ����������ɺ�ʼ��Ƶ����
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
        // ���ù���λ�ú���ת

        // discObject.transform.position = initialDiscPosition;
        //discObject.transform.rotation = initialDiscRotation;
        tvAnimator.Play("CDout");

        // ��ʾ����
        //discObject.SetActive(true);
    }
}
