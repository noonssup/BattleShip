using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCtrl : MonoBehaviour
{
    public float damage = 20f; //�Ѿ� ���ݷ�
    public float speed = 1000f; //�Ѿ� �ӵ�
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

    private void OnDisable() //��Ȱ��ȭ�� ��
    {
        //�Ѿ˿� �ִ� ��ɵ� ���� ��Ȱ��ȭ �� �ʱ�ȭ
        //trail.Clear();
        tr.position = Vector3.zero;
        tr.rotation = Quaternion.identity;
        rb.Sleep();
    }
}