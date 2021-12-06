using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{ 
    const string bulletTag = "BULLET";

    public GameObject expEffect;
    public float hp = 100f;


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == bulletTag)
        {
            //Debug.Log("적함 피격");
            other.gameObject.SetActive(false);
            hp -= other.gameObject.GetComponent<BulletCtrl>().damage;

            if (hp <= 0)
            {
                GetComponent<CruiserCtrl>().state = CruiserCtrl.State.DIE;
                StartCoroutine(EnemyGone());
            }
        }
    }

    IEnumerator EnemyGone()
    {
        gameObject.transform.position += new Vector3(0, -1f, 0);
        GameObject effect = Instantiate(expEffect, transform.position, Quaternion.identity);
        Destroy(effect, 1.5f);
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }

    void Update()
    {
        
    }
}
