using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFire : MonoBehaviour
{
    //AudioSource audio;

    Transform playerTr;
    Transform enemyTr;

    float nextFire = 0f;
    readonly float fireRate = 0.8f;  //�߻� ����
    readonly float damping = 10;     //ȸ�� �ӵ� ���??

    public bool isFire = false;
    //public AudioClip fireSfx;

    //������
    readonly float reloadTime = 2f;
    readonly int maxBullet = 6;
    int currBullet = 6;
    bool isReload;
    WaitForSeconds wsReload;
    //public AudioClip reloadSfx;

    public GameObject Bullet;
    public Transform firePos;

    //public MeshRenderer muzzleFlash;   //���� �߻� �� ����Ʈ ȿ��

    private void Start()
    {
        playerTr = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        enemyTr = GetComponent<Transform>();
        //AudioClip = GetComponent<AudioSource>();

        wsReload = new WaitForSeconds(reloadTime);

        //muzzleFlash.enabled = false;
    }

    private void Update()
    {
        if(!isReload && isFire)
        {
            if(Time.time >= nextFire)
            {
                Fire();
                nextFire = Time.time + fireRate + Random.Range(0f, 0.3f);
            }
            //�÷��̾ �ִ� ��ġ�� ȸ������ ���
            //A���� - B���� = B���� A������ ����� �Ÿ�
            //B���� - A���� = A���� B������ ����� �Ÿ�
            Quaternion rot = Quaternion.LookRotation(playerTr.position - enemyTr.position);
            enemyTr.rotation = Quaternion.Slerp(enemyTr.rotation, rot, Time.deltaTime * damping);
        }
    }

    void Fire()
    {
        //audio.PlayOneShot(fireSfx, 1f);

        //StartCoroutine(ShowMuzzleFlash());

        GameObject _bullet = Instantiate(Bullet, firePos.position, firePos.rotation);
        Destroy(_bullet, 5f);

        currBullet--;
        isReload = (currBullet % maxBullet == 0);
        if (isReload)
        {
            StartCoroutine(Reloading());
        }
    }

    IEnumerator Reloading()
    {
        //muzzleFlash.enabled = false;
        //audio.PlayOneShot(reloadSfx, 1f);  //���� �߻� ȿ�� �̹���, ���� Ȯ�� �� �߰�


        yield return wsReload;
        currBullet = maxBullet;
        isReload = false;
    }

    //IEnumerator ShowMuzzleFlash()
    //{
    //    muzzleFlash.enabled = true;
    //    yield return new WaitForSeconds(Random.Range(0.05f, 0.2f));
    //    muzzleFlash.enabled = false;
    //}

}
