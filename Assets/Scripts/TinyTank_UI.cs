using Microsoft.Unity.VisualStudio.Editor;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

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

    [SerializeField]
    private Sprite HeartFull;
    [SerializeField]
    private Sprite HeartEmpty;

    public int Tank_Life = 3;
    public Image[] Hearts;

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

        foreach (Image img in Hearts)
        {
            img.sprite = HeartEmpty;
        }
        for (int i = 0; i< Tank_Life; i++)
        {
            Hearts[i].sprite = HeartFull;
        }
        Loose();
    }


    public void TankCooldown()
    {
        //UI Tank Cooldown jauge on fire, then call the grow coroutine
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
    private void Loose()
    {
        if (Tank_Life == 0)
        {
            WinTxt.text = "YOU LOOSE !";
        }
    }

    IEnumerator StartRoutine()
    {
        //not so pretty routine
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

