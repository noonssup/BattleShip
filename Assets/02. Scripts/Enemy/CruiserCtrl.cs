using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CruiserCtrl : MonoBehaviour
{
    public enum State
    {
        PATROL, TRACE, ATTACK, DIE
    }
    public State state = State.PATROL;  //�ʱ� ���� ����

    Transform playerTr;
    Transform enemyTr;

    public float attackDist = 800f; //���� ��Ÿ�
    public float traceDist = 1500f;  //���� ��Ÿ�
    public bool isDie = false;     //��� ���� �Ǵ� ����

    WaitForSeconds ws;   //�ð� ���� ����

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
                yield break; //�ڷ�ƾ �Լ� ����
            }

            float dist = Vector3.Distance(playerTr.position, enemyTr.position);
            if (dist <= attackDist)     //���� ���� �Ÿ� �̳��� ����
            {
                state = State.ATTACK;
            }
            else if (dist <= traceDist) //���� ���� �Ÿ� �̳��� ����
            {
                state = State.TRACE;
            }
            else                       //����, ���� ���� �Ÿ��� �ƴ� ��� ����
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
                    enemyFire.isFire = false;     //�������
                    moveAgent.patrolling = true;  //����
                    break;
                case State.TRACE:
                    enemyFire.isFire = false;     //�������
                    moveAgent.traceTarget = playerTr.position;  //�÷��̾�����
                    break;
                case State.ATTACK:
                    moveAgent.Stop();
                    if (enemyFire.isFire == false)  //������� �����ϰ�� ��ݻ��·� ����
                    {
                        enemyFire.isFire = true;
                    }
                    break;
                case State.DIE:

                    gameObject.tag = "Untagged";   //�±׻���
                    isDie = true;                  //�������·� ����
                    enemyFire.isFire = false;      //�������
                    GameManager.instance.score += 1; //�ǰݽ� score 1��
                    moveAgent.Stop();                //moveAgent ����


                    break;

            }
        }
    }
    public void OnPlayerDie()
    {
        moveAgent.Stop();
        enemyFire.isFire = false;

        StopAllCoroutines(); //��� �ڷ�ƾ �Լ� ����
    }
}
