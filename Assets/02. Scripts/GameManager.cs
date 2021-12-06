using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("���� ���� ����")]
    public Transform[] points;
    public GameObject enemy;

    //�����ֱ�, �ִ� ���� ��, ���ӿ��� ����
    public float createTime = 2f;
    public int maxEnemy = 10;
    public bool isGameOver = false;

    public static GameManager instance = null;

    public Text textScore;
    public int score = 0;

    [Header("������Ʈ Ǯ ����")]
    public GameObject bulletPrefab;
    public int maxPool = 12;
    public List<GameObject> bulletPool = new List<GameObject>();  //bullet �� ���� ����Ʈ

    private void Awake()
    {
        //�̱����� �������� ���� ���
        if(instance == null)
        {
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(this.gameObject);
        }

        //DontDestroyOnLoad(this.gameObject);
        CreatePooling();
    }

    private void Start()
    {

        points = GameObject.Find("SpawnPointGroup").GetComponentsInChildren<Transform>();

        if (points.Length > 0)
        {
            StartCoroutine(CreateEnemy());
        }
    }



    void Update()
    {
        textScore.text = "" + score.ToString("00");
        if (Input.GetKeyDown(KeyCode.Z))
        {
            SceneManager.LoadScene("BattleScene");
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    IEnumerator CreateEnemy()
    {
        while (!isGameOver)
        {
            //�±� Ȱ��, enemy ���� �ľ�
            int enemyCount = (int)GameObject.FindGameObjectsWithTag("ENEMY").Length;

            //enemy�� �ִ���� �������� ���� ���� ������
            if(enemyCount < maxEnemy)
            {
                //�� ���� �ֱ� �ð���ŭ ���
                yield return new WaitForSeconds(createTime);

                int idx = Random.Range(1, points.Length); //points �� �ҷ��� Transform �� �θ������Ʈ�� 0 �ε����� ����
                Instantiate(enemy, points[idx].position, points[idx].rotation);
            }
            else
            {
                yield return null;
            }
        }
    }

    //������Ʈ Ǯ �߿��� ��� ������ �Ѿ� ��������
    public GameObject GetBullet()
    {
        for (int i = 0; i < bulletPool.Count; i++)
        {
            //�Ѿ��� ��Ȱ��ȭ ������ ���
            if (bulletPool[i].activeSelf == false)
            {
                return bulletPool[i];
            }
        }
        return null;
    }

    public void CreatePooling()
    {
        GameObject objectPools = new GameObject("ObjectPools"); //���̾��Ű���� Create Enpty �� ������ ����� ��ũ��Ʈ�� ����
        
        //������Ʈ Ǯ ä���
        for(int i =0; i <maxPool; i++)
        {
            //���� ������ ���ÿ� ������ ������ ObjectPools�� �ڽ����� ����
            var obj = Instantiate<GameObject>(bulletPrefab, objectPools.transform);

            obj.name = "Bullet_" + i.ToString("00");
            obj.SetActive(false);  //�߻� �� ��Ȱ��ȭ
            bulletPool.Add(obj);
        }
    }
   
}
