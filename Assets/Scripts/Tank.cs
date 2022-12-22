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
                fire();
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


        //Vector3 MousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,Input.mousePosition.y,Input.mousePosition.z));

        //Quaternion targetRot = Quaternion.LookRotation(MousePos - _TankTurret.position);

        //_TankTurret.rotation = Quaternion.Slerp(_TankTurret.rotation, targetRot, _TankTurretRotSpeed * Time.deltaTime);
        //float CanonIncrRot = Input.mouseScrollDelta.y * _TankCanonRotSpeed;

        //float CanonRot = _TankCanon.localEulerAngles.x;

        //int vCanonRot = Convert.ToInt32(CanonRot);

        //vCanonRot = Math.Clamp(vCanonRot,0,6);
        //vCanonRot = Math.Clamp(vCanonRot, 300, 355);

        //Debug.Log("CanonIncrRot : " + CanonIncrRot + "CanonIncrRot : " + CanonRot + "vCanonIncrRot : " + vCanonRot);

        //if (vCanonRot >= 300 || vCanonRot <= 6)
        //    _TankCanon.Rotate(CanonIncrRot, 0, 0);




        //Vector3 currentRotation = _TankTurret.transform.localEulerAngles; // get local euler angles
        //float gunRotation = Input.GetAxis("Mouse X") * _TankTurretRotSpeed;
        //gunRotation *= Time.deltaTime;
        //currentRotation.y += gunRotation; // adds gunRotation to the x axis of the currentRotation
        //                                  // limits currentRotation.x and applies it to the transform
        //float minRotation = -15;
        //float maxRotation = 15;
        //currentRotation.y = Mathf.Clamp(currentRotation.y, minRotation, maxRotation);
        //_TankTurret.transform.localEulerAngles = currentRotation; // you can just use transform.localEulerAngles


        //float minRotation = -45;
        //float maxRotation = 45;
        //Vector3 currentRotation = _TankTurret.transform.localRotation.eulerAngles;

        //currentRotation.y = Mathf.Clamp(currentRotation.y, minRotation, maxRotation);


        //float rotationX = Input.GetAxis("Mouse X");
        //float rotationY = Input.GetAxis("Mouse Y");
        //Debug.Log("euler ") ;
        ////left and right
        //if (Mathf.Abs(rotationX) > Mathf.Abs(rotationY))
        //{
        //    if (rotationX > 0)
        //        _TankTurret.transform.Rotate(Vector3.up, _TankTurretRotSpeed * Time.deltaTime);
        //    else
        //        _TankTurret.transform.Rotate(Vector3.up, -_TankTurretRotSpeed * Time.deltaTime);
        //}
        ////up and down
        //if (Mathf.Abs(rotationX) < Mathf.Abs(rotationY))
        //{
        //    if (rotationY < 0)
        //    {
        //        _TankCanon.transform.Rotate(Vector3.right, -_TankCanonRotSpeed * Time.deltaTime);
        //    }
        //    else
        //    {
        //        _TankCanon.transform.Rotate(Vector3.right, _TankCanonRotSpeed * Time.deltaTime);
        //    }
        //}






        //// tank turret controls
        //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        //// Copy the ray's direction
        //Vector3 mouseDirection = ray.direction;
        //Vector3 mouseCanon = ray.direction;
        //// Constraint it to stay in the X/Z plane
        //mouseDirection.y = 0;
        //mouseCanon.x = 0;


        ////Look for the constraint direction
        //Quaternion currentTankRotation = _Tank.transform.localRotation;

        //Quaternion currentRotation = _TankTurret.transform.localRotation;

        //Quaternion targetRotation = Quaternion.LookRotation(mouseDirection);

        //Quaternion currentCanRotation = _TankCanon.transform.localRotation;
        //Quaternion targetCanRotation = Quaternion.LookRotation(mouseCanon);

        //float minRotation = -45;
        //float maxRotation = 45;

        //float angle = Quaternion.Angle(currentRotation, currentTankRotation);

        //targetCanRotation = Mathf.Clamp(angle, minRotation, maxRotation);





        ////_TankTurret.transform.localEulerAngles.Normalize();
        ////_TankCanon.transform.localEulerAngles.Normalize();

        //Debug.Log("euler " + _TankCanon.transform.localEulerAngles.x);



        //int Iangle = (int)angle;




        //_TankTurret.transform.localRotation = Quaternion.Slerp(currentRotation, targetRotation,_TankTurretRotSpeed * Time.deltaTime );


        //Vector3 currentRotationE = _TankTurret.transform.localRotation;

        //currentRotationE.y = Mathf.Clamp(angle, minRotation, maxRotation);
        //Debug.Log("angle " + angle + currentRotationE.y);




        //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        ////// Copy the ray's direction
        //Vector3 mouseDirection = ray.direction;


        //// Get aim position in parent gameobject local space in relation to aim position world space 
        //Vector3 targetPositionInLocalSpace = _TankTurret.transform.InverseTransformPoint(mouseDirection);

        //// Set "aimPoint" Y position to zero, since this is horizontal rotation n because we dont need it
        //targetPositionInLocalSpace.y = 0.0f;
        //Debug.Log("targetPositionInLocalSpace " + Input.mousePosition + mouseDirection + targetPositionInLocalSpace);
        //// Store limit value of the rotation
        //Vector3 limitedRotation = targetPositionInLocalSpace;

        //// limit turret horizontal rotation according to its rotation limit
        //if (targetPositionInLocalSpace.x >= 0.0f)
        //    limitedRotation = Vector3.RotateTowards(Vector3.forward, targetPositionInLocalSpace, Mathf.Deg2Rad * rightRotationLimit, float.MaxValue);
        //else
        //    limitedRotation = Vector3.RotateTowards(Vector3.forward, targetPositionInLocalSpace, Mathf.Deg2Rad * leftRotationLimit, float.MaxValue);

        ////Get direction
        //Quaternion whereToRotate = Quaternion.LookRotation(limitedRotation);
        //// Rotate the turret
        //_TankTurret.transform.localRotation = Quaternion.RotateTowards(_TankTurret.transform.localRotation, whereToRotate, _TankTurretRotSpeed * Time.deltaTime);
    }


    //private void CanonLimitRot()
    //{
    //    Vector3 TurretEulerAngles = _TankTurret.rotation.eulerAngles;
    //    TurretEulerAngles.x = (TurretEulerAngles.x > 180) ? TurretEulerAngles.y - 360 : TurretEulerAngles.x;
    //    TurretEulerAngles.x = Math.Clamp(TurretEulerAngles.x, -5, 20);
    //    _TankTurret.rotation = Quaternion.Euler(TurretEulerAngles);
    //}
}
