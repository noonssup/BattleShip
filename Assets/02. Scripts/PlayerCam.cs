using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    public Transform target;

    private void Update()
    {
        transform.position = target.transform.position + new Vector3(0, 10, -5);
    }
}
