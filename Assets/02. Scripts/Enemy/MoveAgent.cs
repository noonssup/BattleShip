using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class MoveAgent : MonoBehaviour
{

    public List<Transform> wayPoints;
    public int nextIdx;  //���� ���� ������ �迭 �ε���

    NavMeshAgent agent;

    float damping = 1f;  //ȸ�� �ӵ� �����ϴ� ���
    Transform enemyTr;

    //������Ƽ (�Լ����� ������ ���Ǵ� ��)
    readonly float patrolSpeed = 50f;  //�б� ���� ���� �ӵ� ����
    readonly float traceSpeed = 50f;     //���� �ӵ� ����

    bool _patrolling;  //���� ���� �Ǵ� ����

    public  bool patrolling
    {
        get { return _patrolling; }
        set
        {
            //set ���۽� ���޹��� ���� value�� ��
            //value�� �ִ� ���� _patrolling ������ ������ ��
            _patrolling = value;
            if (_patrolling)
            {
                agent.speed = patrolSpeed;
                damping = 1f;
                MoveWayPoint();
            }
        }
    }

    Vector3 _traceTarget;

    public Vector3 traceTarget
    {
        get { return _traceTarget; }
        set
        {
            _traceTarget = value;
            agent.speed = traceSpeed;
            damping = 7f;
            //���� ��� ���� �Լ� ȣ��
            TraceTarget(_traceTarget);
        }
    }

    public float speed
    {
        get { return agent.velocity.magnitude; }  //get�� �����ϹǷ� ���� ���� �Ұ�, ���� ȣ�� ����
    }

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.autoBraking = false;  //�ڵ� ���� ��� ��Ȱ��ȭ
        agent.speed = patrolSpeed;
        agent.updateRotation = false;  //�ڵ� ȸ�� ��� ��Ȱ��ȭ

        enemyTr = GetComponent<Transform>();

        var group = GameObject.Find("WayPointGroup");
        //������Ʈ ������ ������ ��� ����
        if(group != null)
        {
            //WayPointGroup ������ �����ϴ� ��� Transform ������Ʈ ȣ��, wayPoints ������ ���� 
            //�̶� �θ� ������Ʈ�� ���� ȣ���
            group.GetComponentsInChildren<Transform>(wayPoints);
            //�θ� ������Ʈ�� ������� �����Ƿ� �ش� �ε����� ����  (�ε��� 0 �� �Ҵ�Ǿ� ����)
            wayPoints.RemoveAt(0);

            nextIdx = Random.Range(0, wayPoints.Count); //���̾��Ű�� ������ Point ���� ���� �߿��� ������ ��ġ ȣ��
        }

        MoveWayPoint();

    }

    void MoveWayPoint()
    {
        //isPathStale ��� ��� ���� ���� true ������ false ��ȯ
        //�Ÿ� ��� ���� ���� ���� ��� �������� �ʵ��� �ϱ� ���� (���� ���ظ���..)
        if (agent.isPathStale)
            return;

        //�����ص� Point �� �߿��� �� ������ �������� ����
        agent.destination = wayPoints[nextIdx].position;
        //������̼� ��� Ȱ��ȭ�ؼ� �̵� �����ϵ��� ����
        agent.isStopped = false;
    }

    void TraceTarget(Vector3 pos)
    {
        if (agent.isPathStale)
            return;

        agent.destination = pos;  //���� ��� ����
        agent.isStopped = false;  //������̼� ��� Ȱ��ȭ
    }

    public void Stop()
    {
        agent.isStopped = true;
        //�ٷ� �����ϱ� ���� ���� �ӵ� 0���� �ʱ�ȭ
        agent.velocity = Vector3.zero;
        _patrolling = false;
    }

    private void Update()
    {
        if(!agent.isStopped)  //���� �����̴� ���϶�
        {
            //���� �����ؾ� �� ���� ���͸� ���ؼ� ȸ�� ������ ���
            Quaternion rot = Quaternion.LookRotation(agent.desiredVelocity);
            //���� �Լ� ���� ������ ȸ��
            enemyTr.rotation = Quaternion.Slerp(enemyTr.rotation, rot, Time.deltaTime * damping);
        }

        if (!_patrolling)
            return;

        //������ ���� ���� Ȯ��
        //�ӵ��� 0.2 ���� ũ�� ���� �̵��Ÿ��� 10 ������ ���
        //sqrMagnitude �� Magnutude ���� ������ ����
        if(agent.velocity.sqrMagnitude >= 0.2f * 0.2f && agent.remainingDistance <= 10f)
        {
            nextIdx = Random.Range(0, wayPoints.Count);

            MoveWayPoint();
        }
    }

}
