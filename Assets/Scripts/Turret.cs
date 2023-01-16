using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;


public class Turret : BaseController
{
    [SerializeField] private Material Mat_Alarm;
    [SerializeField] private float DetectRadius = 10f;
    [SerializeField] private Collider TankCollider;
    [SerializeField] private GameObject _target;

    [SerializeField]
    TinyTank_UI UiScript;


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
        ReloadingTurret= false;  
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.DrawRay(BulletSpawnPosition.transform.position, BulletSpawnPosition.transform.up * 20f,Color.red);
        //_head.transform.LookAt(AimToTarget());
        //RotateHeadTo(AimToTarget());

        //if (ReloadingTurret)
        //{
        //    fire();
        //    //StartCoroutine(CoolTurret());
        //}
        ShootTurret();
    }

    private Vector3 AimToTarget()
    {
        RaycastHit hit;
        Vector3 targetPos = new Vector3(_target.transform.position.x, _head.transform.position.y, _target.transform.position.z); //position du tank

        Vector3 direction = targetPos - _head.transform.position;

        Debug.DrawRay(_head.transform.position, direction*2, Color.red);

        if (Physics.Raycast(_head.transform.position, direction, out hit, DetectRadius))
        {
            if (hit.collider.gameObject.GetComponentInParent<Tank>() != null)
            {
                ReloadingTurret = true;
                return hit.point;
            }
        }
        ReloadingTurret = false;

        return Vector3.zero;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bullet" && AlreadyDead == false)
        {
            AlreadyDead = true;
            UiScript.ScoreUi++;
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
        //if (TankCollider.gameObject.GetComponent<BaseController>() != null)

        if (TankCollider.gameObject.name == "Tank")
        {
            TankOut = true;
            Mat_Alarm.color = Color.red;
        }
    }

    void ShootTurret()
    {
        RaycastHit Hit;
        Debug.DrawRay(BulletSpawnPosition.transform.position, BulletSpawnPosition.transform.up * 200, Color.red);

        if (Physics.Raycast(BulletSpawnPosition.transform.position, BulletSpawnPosition.transform.up*200, out Hit) && (TankOut == false))
        {
            //Debug.Log(" the turret see :  "+ Hit.collider.gameObject.name);
            //if (Hit.transform.tag == "Tank" && (ReloadingTurret == false))
            if (Hit.collider.gameObject.CompareTag("Tank") && ReloadingTurret == false)
            {
                Debug.Log(" you've been locked by the turret !!! ");
                

                fire();
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
