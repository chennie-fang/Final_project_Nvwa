using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class animationeventCallback : MonoBehaviour
{
    public UnityEvent animationCallBack = new UnityEvent();
    // Start is called before the first frame update
   
    public void CDoutBallBack()
    {

        if (GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("CDout"))
        {
            animationCallBack?.Invoke();
        }
        
    }
    public void NomalCallBack()
    {

    
            animationCallBack?.Invoke();
       

    }
}
