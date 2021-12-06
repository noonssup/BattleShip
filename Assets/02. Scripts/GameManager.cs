using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("적함 생성 정보")]
    public Transform[] points;
    public GameObject enemy;

    //생성주기, 최대 적함 수, 게임오버 여부
    public float createTime = 2f;
    public int maxEnemy = 10;
    public bool isGameOver = false;

    public static GameManager instance = null;

    public Text textScore;
    public int score = 0;

    [Header("오브젝트 풀 정보")]
    public GameObject bulletPrefab;
    public int maxPool = 12;
    public List<GameObject> bulletPool = new List<GameObject>();  //bullet 을 담을 리스트

    private void Awake()
    {
        //싱글턴이 존재하지 않을 경우
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
            //태그 활용, enemy 숫자 파악
            int enemyCount = (int)GameObject.FindGameObjectsWithTag("ENEMY").Length;

            //enemy의 최대생성 개수보다 작을 때만 리스폰
            if(enemyCount < maxEnemy)
            {
                //적 생성 주기 시간만큼 대기
                yield return new WaitForSeconds(createTime);

                int idx = Random.Range(1, points.Length); //points 로 불러온 Transform 중 부모오브젝트인 0 인덱스는 제외
                Instantiate(enemy, points[idx].position, points[idx].rotation);
            }
            else
            {
                yield return null;
            }
        }
    }

    //오브젝트 풀 중에서 사용 가능한 총알 가져오기
    public GameObject GetBullet()
    {
        for (int i = 0; i < bulletPool.Count; i++)
        {
            //총알이 비활성화 상태일 경우
            if (bulletPool[i].activeSelf == false)
            {
                return bulletPool[i];
            }
        }
        return null;
    }

    public void CreatePooling()
    {
        GameObject objectPools = new GameObject("ObjectPools"); //하이어라키에서 Create Enpty 와 동일한 기능을 스크립트로 구현
        
        //오브젝트 풀 채우기
        for(int i =0; i <maxPool; i++)
        {
            //동적 생성과 동시에 위에서 생성한 ObjectPools의 자식으로 설정
            var obj = Instantiate<GameObject>(bulletPrefab, objectPools.transform);

            obj.name = "Bullet_" + i.ToString("00");
            obj.SetActive(false);  //발사 전 비활성화
            bulletPool.Add(obj);
        }
    }
   
}
