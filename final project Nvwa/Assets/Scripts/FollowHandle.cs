using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FollowHandle : MonoBehaviour
{

    public Image progarssValue;
    public Image disPlayBar, disPlayBarMove;
    public bool follow = false;
    public TextMeshProUGUI content;
    private void Update()
    {

        disPlayBar.gameObject.SetActive(progarssValue.fillAmount != 0);
        disPlayBarMove.gameObject.SetActive(progarssValue.fillAmount != 0);
        content.text = (int)(progarssValue.fillAmount * 100f) + "%";
    
        if (follow)
        {
            transform.eulerAngles = -Vector3.forward * progarssValue.fillAmount * 360f;
        }
    }

}
