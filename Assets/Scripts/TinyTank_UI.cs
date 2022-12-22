using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class TinyTank_UI : MonoBehaviour
{
    [SerializeField]
    private UnityEngine.UI.Image ImageCoolBar;

    [SerializeField]
    public TextMeshProUGUI CoolText;

    private float CoolTime;
    private bool IsCool = true;


    // Start is called before the first frame update
    void Start()
    {
        GameObject Tank = GameObject.Find("Tank");
        Tank playerScript = Tank.GetComponent<Tank>();
        CoolTime = playerScript.CoolTime;
        CoolText.text = "";
    }


    public void TankCooldown()
    {
        //Debug.Log("CoolTime = " + CoolTime);
        if(IsCool == true)
        {
            IsCool = false;
        ImageCoolBar.fillAmount = 0;
        CoolText.text = "RELOADING...";
        StartCoroutine(CoolDownGrow());
        }
    }
    IEnumerator CoolDownGrow()
    {
        float time = 0.0f;
        while (time < CoolTime)
        {
            ImageCoolBar.fillAmount = time / Mathf.Max(CoolTime, 0);
            time += Time.deltaTime;
            yield return new WaitForSecondsRealtime(0);
        }
        CoolText.text = "";
        IsCool = true;
    }
}

