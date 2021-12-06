using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battleship : MonoBehaviour
{

    Transform tr;

    float h = 0f;
    float v = 0f;

    public float moveSpeed = 30f; //전함의 좌우회전 속도
    public float rotSpeed = 10f;  //전함의 전후진 이동 속도

    private void Start()
    {
        tr = GetComponent<Transform>();
    }

    private void Update()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");

        Vector3 moveDir = (Vector3.forward * v);// + (Vector3.right * h);

        moveDir = moveDir.normalized;

        tr.Translate(moveDir * moveSpeed * Time.deltaTime, Space.Self);
        tr.Rotate(Vector3.up * rotSpeed * Time.deltaTime * h);
    }

    void OnTriggerEnter(Collider coll)
    {
        if(coll.name == "Cube_Bottom")
        {
            this.transform.position += new Vector3(0, 0, 5000);
        }
        else if (coll.name == "Cube_Top")
        {
            this.transform.position += new Vector3(0, 0, -5000);
        }
        else if (coll.name == "Cube_Left")
        {
            this.transform.position += new Vector3(4300, 0,0 );
        }
        else if (coll.name == "Cube_Right")
        {
            this.transform.position += new Vector3(-4300, 0, 0);
        }
    }
}
