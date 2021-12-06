using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerDamage : MonoBehaviour
{
    const string bulletTag = "EBULLET";
    public float iniHp = 100;
    public float currHp;

    public GameObject expEffect;  //플레이어 전함 파괴 이펙트

    //델리게이트 선언
    public delegate void PlayerDieHandler();
    //델리게이트를 활용한 이벤트 선언
    public static event PlayerDieHandler OnPlayerDieEvent;

    public Image hpBar;
    readonly Color iniColor = new Vector4(0, 1f, 0, 1f);
    Color currColor;

    private void Start()
    {
        currHp = iniHp;
        hpBar.color = iniColor;
        currColor = iniColor;
    }

    void Update()
    {

    }

    void Display()
    {
        if((currHp / iniHp) > 0.5f) //현재 체력이 50% 이상일때
        {
            //녹색 >> 노란색
            currColor.r = (1 - (currHp / iniHp)) * 2.0f;
        }
        else  //50% 이하일때 노란색 >> 빨간색
        {
            currColor.g = (currHp / iniHp) * 2f;
        }
        hpBar.color = currColor;  //체력게이지 색상 적용
        hpBar.fillAmount = (currHp / iniHp);  //체력바 조절
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == bulletTag)
        {
            //Debug.Log("플레이어 피격");
            Destroy(other.gameObject);

            currHp -= 15;


            Display();

            if (currHp <= 0f)
            {
                PlayerDie();
            }
        }
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.collider.tag == bulletTag)
    //    {
    //        Destroy(collision.collider.gameObject);

    //        currHp -= 15;
    //        Debug.Log("플레이어 피격");

    //        Display();

    //        if (currHp <= 0f)
    //        {
    //            PlayerDie();
    //        }
    //    }
    //}

    void PlayerDie()
    {
        OnPlayerDieEvent();
        GameObject effect = Instantiate(expEffect, transform.position, Quaternion.identity);
        //3초후 폭발 이펙트 삭제
        gameObject.SetActive(false);
        Image gameOver = GameObject.Find("ImgGameOver").GetComponent<Image>();
        Text retry = GameObject.Find("Retry").GetComponent<Text>();
        gameOver.enabled = true;
        retry.enabled = true;

        Destroy(effect, 3f);

        GameManager.instance.isGameOver = true;
    }
}
