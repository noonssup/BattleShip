using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadarCam : MonoBehaviour
{
    public Transform target;

    private void Update()
    {
        transform.position = target.transform.position + new Vector3(0, 1500,0);
    }
}
