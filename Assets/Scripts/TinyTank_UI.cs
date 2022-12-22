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

    [SerializeField]
    private TextMeshProUGUI WinTxt;

    [SerializeField]
    public TextMeshProUGUI ScoreInt;

    [SerializeField]
    BaseController CtrlerScript;

    private float CoolTime;
    private int TurretToWin;
    private bool IsCool = true;
    public int ScoreUi = 0;
    //private String Win = "";


    // Start is called before the first frame update
    void Start()
    {
        TurretToWin = GameObject.FindGameObjectsWithTag("Turret").Length;
        GameObject Tank = GameObject.Find("Tank");
        Tank playerScript = Tank.GetComponent<Tank>();
        CoolTime = playerScript.CoolTime;
        CoolText.text = "READY !";
        
        StartCoroutine(StartRoutine());
    }

    public void Update()
    {
        ScoreInt.text = ScoreUi.ToString();
        Win();
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

    private void Win()
    {
        if(ScoreUi == TurretToWin)
        {
            WinTxt.text = "YOU WIN !";
        }
    }
 
    IEnumerator StartRoutine()
    {
        WinTxt.text = "START";
        yield return new WaitForSecondsRealtime(0.5f);
        WinTxt.text = "";
        yield return new WaitForSecondsRealtime(0.5f);
        WinTxt.text = "START";
        yield return new WaitForSecondsRealtime(0.5f);
        WinTxt.text = "";
        yield return new WaitForSecondsRealtime(0.5f);
        WinTxt.text = "START";
        yield return new WaitForSecondsRealtime(0.5f);
        WinTxt.text = "";
        yield return new WaitForSecondsRealtime(0.5f);
        WinTxt.text = "START";
        yield return new WaitForSecondsRealtime(2);
        WinTxt.text = "";
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
        CoolText.text = "READY !";
        IsCool = true;
    }
}

