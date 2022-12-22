using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class BaseController : MonoBehaviour
{


    public float CoolTime = 2f;
    [SerializeField] protected GameObject BulletPrefab;
    [SerializeField] protected GameObject BulletSpawnPosition;
    [SerializeField] protected int BulletSpeed;
    //[SerializeField] protected GameObject LocalCanon;
    //private bool IsCool = true;
    private TextMeshProUGUI CoolText;
    // Start is called before the first frame update


    private void Start()
    {
        GameObject UI = GameObject.Find("UI");
        TinyTank_UI playerScript = UI.GetComponent<TinyTank_UI>();
        CoolText = playerScript.CoolText;
    }
    protected void fire()
    {

        GameObject BulletOne = Instantiate<GameObject>(BulletPrefab, BulletSpawnPosition.transform.position, BulletSpawnPosition.transform.rotation);
        BulletOne.GetComponent<Rigidbody>().AddForce(BulletSpawnPosition.transform.up * BulletSpeed);
        Destroy(BulletOne, 2);
    }


    //IEnumerator CanonShootShock()  //recul du canon coroutine
    //{
    //    Vector3 OriginalPos = new Vector3(0, 0, 0);
    //    Vector3 endPos = new Vector3(0, 0, -5f);
    //    LocalCanon.transform.localPosition = OriginalPos;
    //    LocalCanon.transform.localPosition = Vector3.MoveTowards(OriginalPos, endPos, 20 * Time.deltaTime);
    //    yield return new WaitForSeconds(0.5f);
    //    LocalCanon.transform.localPosition = Vector3.Slerp(endPos, OriginalPos, 20 * Time.deltaTime);
    //}
    // Update is called once per frame

    protected void Update()
    {

        //pour voir le raycat ne pas oublier de se mettre en mode scene pas en game dans unity
        RaycastHit Hit;
        if (Physics.Raycast(BulletSpawnPosition.transform.position, BulletSpawnPosition.transform.up, out Hit))
        {
            Debug.DrawRay(BulletSpawnPosition.transform.position, BulletSpawnPosition.transform.up * 20f);
        }
    }

}
