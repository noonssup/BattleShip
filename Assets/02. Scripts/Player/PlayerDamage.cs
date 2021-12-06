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

    public GameObject expEffect;  //�÷��̾� ���� �ı� ����Ʈ

    //��������Ʈ ����
    public delegate void PlayerDieHandler();
    //��������Ʈ�� Ȱ���� �̺�Ʈ ����
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
        if((currHp / iniHp) > 0.5f) //���� ü���� 50% �̻��϶�
        {
            //��� >> �����
            currColor.r = (1 - (currHp / iniHp)) * 2.0f;
        }
        else  //50% �����϶� ����� >> ������
        {
            currColor.g = (currHp / iniHp) * 2f;
        }
        hpBar.color = currColor;  //ü�°����� ���� ����
        hpBar.fillAmount = (currHp / iniHp);  //ü�¹� ����
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == bulletTag)
        {
            //Debug.Log("�÷��̾� �ǰ�");
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
    //        Debug.Log("�÷��̾� �ǰ�");

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
        //3���� ���� ����Ʈ ����
        gameObject.SetActive(false);
        Image gameOver = GameObject.Find("ImgGameOver").GetComponent<Image>();
        Text retry = GameObject.Find("Retry").GetComponent<Text>();
        gameOver.enabled = true;
        retry.enabled = true;

        Destroy(effect, 3f);

        GameManager.instance.isGameOver = true;
    }
}
