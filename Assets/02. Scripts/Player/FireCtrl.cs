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
    public GameObject bullet;   //함포
    public Transform[] firePos;   //함포 발사 위치

    AudioSource _audio;
    public PlayerSfx playerSfx;

    public Text magazineText;

    public int maxBullet = 12;  //최대 탄알 수
    public int remainingBullet = 12;  //남은 총알 수
    bool isBulletEmpty = false;   //탄알이 0 일 경우 true

    private void Start()
    {
        _audio = GetComponent<AudioSource>();
    }
    private void Update()
    {
        if (!isBulletEmpty && Input.GetKeyDown(KeyCode.J))
        {
            remainingBullet--;  //탄알 감소

            FireBow();

            if(remainingBullet <= 0)
            {
                isBulletEmpty = true;
                //탄이 없다는 소리를 넣을까??... 고민 중
                StartCoroutine(Reload());
            }

        }
        if (!isBulletEmpty && Input.GetKeyDown(KeyCode.K))
        {
            FireSide();

            if (remainingBullet <= 0)
            {
                isBulletEmpty = true;
                //탄이 없다는 소리를 넣을까??... 고민 중
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
