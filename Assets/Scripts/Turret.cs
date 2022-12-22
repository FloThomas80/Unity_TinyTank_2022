using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.SocialPlatforms.Impl;
using static UnityEngine.GraphicsBuffer;

public class Turret : BaseController
{
    [SerializeField] private Material Mat_Alarm;
    [SerializeField] private float DetectRadius;
    [SerializeField] private Collider TankCollider;
    [SerializeField] private GameObject AnimTur;

    [SerializeField]
    TinyTank_UI UiScript;

    [SerializeField]
    protected GameObject BulletSpawnPosition;

    //[SerializeField] protected GameObject BulletSpawnPosition;

    [SerializeField] float TurretRotateSpeed;

    private bool TankOut = true;
    private bool ReloadingTurret = false;
    private bool AlreadyDead = false;
    //private Animation DeadTurret;

    // Start is called before the first frame update
    void Start()
    {
        Mat_Alarm.color = Color.red;
        BulletSpeed = 4000;
        //DeadTurret = GetComponent<Animation>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(BulletSpawnPosition.transform.position, BulletSpawnPosition.transform.up * 20f,Color.red);
        ShootTurret();
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Bullet" && AlreadyDead == false)
        {
            AlreadyDead = true;
            UiScript.ScoreUi = UiScript.ScoreUi+1;
            //DeadTurret.Play("Dead_Turret");
            this.GetComponent<Turret>().enabled = false;
        }
    }


    private void OnTriggerEnter(Collider TankCollider) //test if someone enter collider and if is tank
    {
        if (TankCollider.gameObject.name == "Tank" && AlreadyDead == false)
        {
            TankOut = false;
            StartCoroutine("LookAtTank");
            Mat_Alarm.color = Color.green;
        }
    }

    private void OnTriggerExit(Collider TankCollider) //test if someone exit collider and if is tank
    {
        if (TankCollider.gameObject.name == "Tank")
        { 
            TankOut = true;
            Mat_Alarm.color = Color.red;
        }
    }

    void ShootTurret()
    {
        RaycastHit Hit;

        if (Physics.Raycast(BulletSpawnPosition.transform.position, BulletSpawnPosition.transform.up, out Hit) && (TankOut == false))
        {
            Debug.Log(" you've been locked by the turret !!! ");

            //Debug.Log(" the turret see :  "+ Hit.collider.gameObject.name);
            //if (Hit.transform.tag == "Tank" && (ReloadingTurret == false))
            if (Hit.collider.gameObject.CompareTag("Tank") && ReloadingTurret == false)
                {
            Debug.Log(" you've been locked by the turret !!! ");
            fire(BulletSpawnPosition);
            ReloadingTurret = true;
            StartCoroutine("CoolTurret");
            }
        }
    }
        IEnumerator LookAtTank()  //look at tank coroutine
        {
       
            while (TankOut == false)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(TankCollider.transform.position - transform.position), TurretRotateSpeed * Time.deltaTime);
                yield return null;
            }
        }

        IEnumerator CoolTurret()  //look at tank coroutine
        {
            yield return new WaitForSeconds(2);
            ReloadingTurret = false;
        }
    }
