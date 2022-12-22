using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Timers;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class Tank : BaseController
{
    private Rigidbody _Tank;
    [SerializeField] private float _TankSpeed;
    [SerializeField] private float _TankRotSpeed;
    //[SerializeField] private float _TankTurretRotSpeed;
    //[SerializeField] private float _TankCanonRotSpeed;
    [SerializeField] private Transform _TankTurret;
    [SerializeField] private Transform _TankCanon;
    private Vector3 aimPoint;
    float TurretAngle;

    [SerializeField] 
    protected GameObject BulletSpawnPosition;

    [SerializeField]
    private GameObject m_BarrelPivot = null;

    [SerializeField]
    public float m_HorizontalRotateSpeed = 1.0f; // Speed at which the cannon rotates horizontally
    [SerializeField]
    public float m_VerticalRotateSpeed = 10.0f; // Speed at which the cannon rotates vertically

    [SerializeField]
    public float m_MaxHorizontalAngle = 45.0f; // Maximum angle the cannon can rotate horizontally to the left or right
    [SerializeField]
    public float m_MaxUpAngle = 30.0f; // Maximum angle the cannon can rotate up
    [SerializeField]
    public float m_MaxDownAngle = 5.0f; // Maximum angle the cannon can rotate down

    private float m_HorizontalAngle = 0.0f; // Current angle the cannon is rotated horizontally
    private float m_VerticalAngle = 0.0f; // Current angle the cannon is rotated vertically
    private bool IsCool = true;
    
    

    [Range(0, 90)]
    public float rightRotationLimit = 30;

    [Range(0, 90)]
    public float leftRotationLimit=30;

    
     

    //ask for UI Script so i can call mthod from here
    [SerializeField]
    TinyTank_UI UiScript;
    [SerializeField]
    BaseController BaseController;

    // Start is called before the first frame update
    void Start()
    {
        _Tank = GetComponent<Rigidbody>();

        

        _TankSpeed = 4f;
        _TankRotSpeed = 30f;
        BulletSpeed = 4000;
}


    public void SetAim(Vector3 AimPosition)
    {
        aimPoint = AimPosition;
    }
    // Update is called once per frame
    void Update()
    {
            //watch for moving aiming and fire
        TankFire();
        MouveTank();
        MouveTurret();
    }

    


    void TankFire()  //fire method
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (IsCool == true)
            {
                fire(BulletSpawnPosition);
                //calling method From UI script
                UiScript.TankCooldown();
                StartCoroutine("CoolDown");
            }
        }

    }
    void MouveTank()
    {
        // controls move tank (not the tank turret)
        //i split from getaxis tu transform forward because of the local axis ... can it be improve ?
        if (Input.GetAxis("Vertical") > 0)
        {
            //_Tank.AddForce(0f, 0f, Input.GetAxis("Vertical") * _TankSpeed);
            _Tank.AddForce(transform.forward * _TankSpeed);
        }
        else if (Input.GetAxis("Vertical") < 0)
        {
            _Tank.AddForce(-transform.forward * _TankSpeed);
        }

        if (Input.GetAxis("Horizontal") != 0)
        {
            //_Tank.AddTorque(0f, Input.GetAxis("Horizontal") * _TankRotSpeed, 0f);
            transform.Rotate(0f, Input.GetAxis("Horizontal") * _TankRotSpeed * Time.deltaTime, 0f);
        }
    }

    IEnumerator CoolDown()  //cooldown coroutine
    {
        IsCool = false;
        yield return new WaitForSeconds(CoolTime);
        IsCool = true;

    }

    void MouveTurret()
    {

        m_HorizontalAngle += Input.GetAxis("Mouse X") * m_HorizontalRotateSpeed;

        // Clamp the turret horizontal
        m_HorizontalAngle = Mathf.Clamp(m_HorizontalAngle, -m_MaxHorizontalAngle, m_MaxHorizontalAngle);

        m_VerticalAngle += Input.GetAxis("Mouse ScrollWheel") * m_VerticalRotateSpeed;

        // Clamp the barrel up down
        m_VerticalAngle = Mathf.Clamp(m_VerticalAngle, -m_MaxUpAngle, m_MaxDownAngle);

        // Set the rotation of the turret to the new angle
        _TankTurret.transform.localRotation = Quaternion.Euler(0, m_HorizontalAngle, 0);
        m_BarrelPivot.transform.localRotation = Quaternion.Euler(m_VerticalAngle, 0, 0);
    }
}
