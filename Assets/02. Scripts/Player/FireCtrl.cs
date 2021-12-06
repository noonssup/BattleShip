using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct PlayerSfx
{
    public AudioClip[] fire;
}

public class FireCtrl : MonoBehaviour
{ 
    public GameObject bullet;   //����
    public Transform[] firePos;   //���� �߻� ��ġ

    AudioSource _audio;
    public PlayerSfx playerSfx;

    public Text magazineText;

    public int maxBullet = 12;  //�ִ� ź�� ��
    public int remainingBullet = 12;  //���� �Ѿ� ��
    bool isBulletEmpty = false;   //ź���� 0 �� ��� true

    private void Start()
    {
        _audio = GetComponent<AudioSource>();
    }
    private void Update()
    {
        if (!isBulletEmpty && Input.GetKeyDown(KeyCode.J))
        {
            remainingBullet--;  //ź�� ����

            FireBow();

            if(remainingBullet <= 0)
            {
                isBulletEmpty = true;
                //ź�� ���ٴ� �Ҹ��� ������??... ��� ��
                StartCoroutine(Reload());
            }

        }
        if (!isBulletEmpty && Input.GetKeyDown(KeyCode.K))
        {
            FireSide();

            if (remainingBullet <= 0)
            {
                isBulletEmpty = true;
                //ź�� ���ٴ� �Ҹ��� ������??... ��� ��
                StartCoroutine(Reload());
            }

        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            remainingBullet = maxBullet;
            isBulletEmpty = false;
        }
    }

    void FireBow()
    {
        Instantiate(bullet, firePos[0].position, firePos[0].rotation);
        UpdateBulletText();
        //
        //var _bullet = GameManager.instance.GetBullet();
        //if (_bullet != null)
        //{
        //    _bullet.transform.position = firePos.position;
        //    _bullet.transform.rotation = firePos.rotation;
        //    _bullet.SetActive(true);
        //}
    }

    void FireSide()
    {

        if (remainingBullet >=0)
        {
            for (int i = 1; i <= 4; i++)
            {
                remainingBullet--;
                Instantiate(bullet, firePos[i].position, firePos[i].rotation);
                if (remainingBullet <= 0)
                    break;
            }
        }
        else
        {
            StartCoroutine(Reload());
        }
        UpdateBulletText();
    }

    IEnumerator Reload()
    {
        yield return new WaitForSeconds(2.0f);
        remainingBullet = maxBullet;
        isBulletEmpty = false;
        UpdateBulletText();

    }

    void UpdateBulletText()
    {
        magazineText.text = string.Format("<color=#ff0000>{0}</color>/{1}", remainingBullet, maxBullet);
    }
}
