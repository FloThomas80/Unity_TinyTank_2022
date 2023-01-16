using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private int _damage = 1;
    [SerializeField] private float _lifeDuration = 4f;
    [SerializeField] protected int BulletSpeed = 4000;
    void Start()
    {
        //keeping my old system with add force
        // GetComponent<Rigidbody>().velocity = transform.up * _speed;
        GetComponent<Rigidbody>().AddForce(transform.up * BulletSpeed);
        Destroy(gameObject, _lifeDuration);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponentInParent<BaseController>() != null)
        {
            collision.gameObject.GetComponentInParent<BaseController>().ApplyDamage(_damage);
        }
        Destroy(gameObject, _lifeDuration);
    }
}
