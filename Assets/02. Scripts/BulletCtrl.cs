using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCtrl : MonoBehaviour
{
    public float damage = 20f; //총알 공격력
    public float speed = 1000f; //총알 속도
    Rigidbody rb;
    Transform tr;
    //TrailRenderer trail;

    void Awake()
    {
        tr = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
        //trail = GetComponent<TrailRenderer>();
    }

    private void OnEnable()
    {
        rb.AddForce(transform.forward * speed);
        StartCoroutine(BulletFalse());
    }

    IEnumerator BulletFalse()
    {
        yield return new WaitForSeconds(5f);
        gameObject.SetActive(false);
    }

    private void OnDisable() //비활성화될 때
    {
        //총알에 있는 기능들 전부 비활성화 및 초기화
        //trail.Clear();
        tr.position = Vector3.zero;
        tr.rotation = Quaternion.identity;
        rb.Sleep();
    }
}
