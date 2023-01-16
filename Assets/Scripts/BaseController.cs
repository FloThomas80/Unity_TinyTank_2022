using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class BaseController : MonoBehaviour
{

    [SerializeField] protected int LifePoint;
    public float CoolTime = 2f;
    [SerializeField] protected GameObject BulletPrefab;
    [SerializeField] protected GameObject BulletSpawnPosition;

    public int TotalTurret;
    [SerializeField] protected GameObject _head;
    // Start is called before the first frame update


    private void Start()
    {
        GameObject UI = GameObject.Find("UI");
        TinyTank_UI playerScript = UI.GetComponent<TinyTank_UI>();
    }

    protected void fire()
    {

        GameObject BulletOne = Instantiate<GameObject>(BulletPrefab, BulletSpawnPosition.transform.position, BulletSpawnPosition.transform.rotation);
    }
    public void ApplyDamage(int damage)
    {
        LifePoint -= damage;
        if (LifePoint <= 0)
        {
            Destruction();
        }
    }
    protected void RotateHeadTo(Vector3 targetPosition)
    {
        _head.transform.LookAt(targetPosition);
    }

    protected virtual void Destruction()
    {
        //Destroy(gameObject);
    }
}
