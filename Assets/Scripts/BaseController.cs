using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class BaseController : MonoBehaviour
{


    public float CoolTime = 2f;
    [SerializeField] protected GameObject BulletPrefab;
    [SerializeField] protected int BulletSpeed;
    public int TotalTurret;
    // Start is called before the first frame update


    private void Start()
    {
        GameObject UI = GameObject.Find("UI");
        TinyTank_UI playerScript = UI.GetComponent<TinyTank_UI>();
    }
    protected void fire(GameObject BulletSpawnPosition)
    {

        GameObject BulletOne = Instantiate<GameObject>(BulletPrefab, BulletSpawnPosition.transform.position, BulletSpawnPosition.transform.rotation);
        BulletOne.GetComponent<Rigidbody>().AddForce(BulletSpawnPosition.transform.up * BulletSpeed);
        Destroy(BulletOne, 2);
    }
}
