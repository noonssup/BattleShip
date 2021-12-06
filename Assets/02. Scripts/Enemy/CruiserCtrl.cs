using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CruiserCtrl : MonoBehaviour
{
    public enum State
    {
        PATROL, TRACE, ATTACK, DIE
    }
    public State state = State.PATROL;  //초기 상태 지정

    Transform playerTr;
    Transform enemyTr;

    public float attackDist = 800f; //공격 사거리
    public float traceDist = 1500f;  //추적 사거리
    public bool isDie = false;     //사망 여부 판단 변수

    WaitForSeconds ws;   //시간 지연 변수

    MoveAgent moveAgent;
    EnemyFire enemyFire;

    private void Awake()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        if(player != null)
        {
            playerTr = player.GetComponent<Transform>();
        }
        enemyTr = GetComponent<Transform>();
        moveAgent = GetComponent<MoveAgent>();
        enemyFire = GetComponent<EnemyFire>();

        ws = new WaitForSeconds(0.3f);
    }


    private void OnEnable()
    {
        StartCoroutine(CheckState());
        StartCoroutine(Action());

        PlayerDamage.OnPlayerDieEvent += this.OnPlayerDie;
    }

    private void OnDisable()
    {
        PlayerDamage.OnPlayerDieEvent -= this.OnPlayerDie;
    }
    
    IEnumerator CheckState()
    {
        while (!isDie)
        {
            if (state == State.DIE)
            {
                yield break; //코루틴 함수 정지
            }

            float dist = Vector3.Distance(playerTr.position, enemyTr.position);
            if (dist <= attackDist)     //공격 가능 거리 이내면 공격
            {
                state = State.ATTACK;
            }
            else if (dist <= traceDist) //추적 가능 거리 이내면 추적
            {
                state = State.TRACE;
            }
            else                       //공격, 추적 가능 거리가 아닐 경우 순찰
            {
                state = State.PATROL;
            }
            yield return ws;
        }
    }

    IEnumerator Action()
    {
        while (!isDie)
        {
            yield return ws;

            switch (state)
            {
                case State.PATROL:
                    enemyFire.isFire = false;     //사격중지
                    moveAgent.patrolling = true;  //순찰
                    break;
                case State.TRACE:
                    enemyFire.isFire = false;     //사격중지
                    moveAgent.traceTarget = playerTr.position;  //플레이어추적
                    break;
                case State.ATTACK:
                    moveAgent.Stop();
                    if (enemyFire.isFire == false)  //사격중지 상태일경우 사격상태로 변경
                    {
                        enemyFire.isFire = true;
                    }
                    break;
                case State.DIE:

                    gameObject.tag = "Untagged";   //태그삭제
                    isDie = true;                  //죽음상태로 변경
                    enemyFire.isFire = false;      //사격중지
                    GameManager.instance.score += 1; //피격시 score 1점
                    moveAgent.Stop();                //moveAgent 중지


                    break;

            }
        }
    }
    public void OnPlayerDie()
    {
        moveAgent.Stop();
        enemyFire.isFire = false;

        StopAllCoroutines(); //모든 코루틴 함수 종료
    }
}
