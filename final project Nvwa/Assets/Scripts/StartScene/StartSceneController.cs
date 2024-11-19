using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSceneController : MonoBehaviour
{
    public Transform target;
    public GameObject Waves;
    public GameObject Video;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(transform.position, target.position);
        if(distance <=11f)
        {
            Waves.SetActive(false);
            Video.SetActive(true);
        }
    }
}
