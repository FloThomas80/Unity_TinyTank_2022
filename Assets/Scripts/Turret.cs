using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using static UnityEngine.GraphicsBuffer;

public class Turret : BaseController
{
    [SerializeField] private Material Mat_Alarm;
    [SerializeField] private float DetectRadius;
    [SerializeField] private Collider TankCollider;

    //[SerializeField] protected GameObject BulletSpawnPosition;

    [SerializeField] float TurretRotateSpeed;

    private bool TankOut = true;
    private bool ReloadingTurret = false;

   
    // Start is called before the first frame update
    void Start()
    {
        Mat_Alarm.color = Color.red;
        BulletSpeed = 10;
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit Hit;
        if (Physics.Raycast(BulletSpawnPosition.transform.position, -BulletSpawnPosition.transform.up, out Hit) && (TankOut == false))
        {
            Debug.DrawRay(BulletSpawnPosition.transform.position, BulletSpawnPosition.transform.up * 20f);

            if ((Hit.collider.gameObject == TankCollider.gameObject) && (ReloadingTurret == false) )
            {
                Debug.Log(" you've been locked by the turret !!! ");
                ShootTurret();
            }
        }


    }
    private void OnTriggerEnter(Collider TankCollider) //test if someone enter collider and if is tank
    {
        if (TankCollider.gameObject.name == "TankTrigger")
        {
            TankOut = false;
            StartCoroutine("LookAtTank");
            Mat_Alarm.color = Color.green;
        }
    }

    private void OnTriggerExit(Collider TankCollider) //test if someone exit collider and if is tank
    {
        if (TankCollider.gameObject.name == "TankTrigger")
        { 
            TankOut = true;
            Mat_Alarm.color = Color.red;
        }
    }

    void ShootTurret()
    {
        if (ReloadingTurret == false)
        {
            fire();
            ReloadingTurret = true;
            StartCoroutine("CoolTurret");
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
