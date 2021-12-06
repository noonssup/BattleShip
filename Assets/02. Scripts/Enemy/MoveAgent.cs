using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class MoveAgent : MonoBehaviour
{

    public List<Transform> wayPoints;
    public int nextIdx;  //다음 순찰 지점의 배열 인덱스

    NavMeshAgent agent;

    float damping = 1f;  //회전 속도 조절하는 계수
    Transform enemyTr;

    //프로퍼티 (함수지만 변수로 사용되는 것)
    readonly float patrolSpeed = 50f;  //읽기 전용 순찰 속도 변수
    readonly float traceSpeed = 50f;     //추적 속도 변수

    bool _patrolling;  //순찰 여부 판단 변수

    public  bool patrolling
    {
        get { return _patrolling; }
        set
        {
            //set 동작시 전달받은 값은 value에 들어감
            //value에 있는 값을 _patrolling 변수에 전달해 줌
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
            //추적 대상 지정 함수 호출
            TraceTarget(_traceTarget);
        }
    }

    public float speed
    {
        get { return agent.velocity.magnitude; }  //get만 존재하므로 따로 설정 불가, 값만 호출 가능
    }

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.autoBraking = false;  //자동 감속 기능 비활성화
        agent.speed = patrolSpeed;
        agent.updateRotation = false;  //자동 회전 기능 비활성화

        enemyTr = GetComponent<Transform>();

        var group = GameObject.Find("WayPointGroup");
        //오브젝트 정보가 존재할 경우 실행
        if(group != null)
        {
            //WayPointGroup 하위에 존재하는 모든 Transform 컴포넌트 호출, wayPoints 변수에 대입 
            //이때 부모 오브젝트도 같이 호출됨
            group.GetComponentsInChildren<Transform>(wayPoints);
            //부모 오브젝트는 사용하지 않으므로 해당 인덱스는 삭제  (인덱스 0 에 할당되어 있음)
            wayPoints.RemoveAt(0);

            nextIdx = Random.Range(0, wayPoints.Count); //하이어라키에 생성된 Point 들의 갯수 중에서 랜덤한 위치 호출
        }

        MoveWayPoint();

    }

    void MoveWayPoint()
    {
        //isPathStale 경로 계산 중일 때는 true 끝나면 false 반환
        //거리 계산 중일 때는 순찰 경로 변경하지 않도록 하기 위함 (아직 이해못함..)
        if (agent.isPathStale)
            return;

        //생성해둔 Point 들 중에서 한 곳으로 목적지를 설정
        agent.destination = wayPoints[nextIdx].position;
        //내비게이션 기능 활성화해서 이동 시작하도록 변경
        agent.isStopped = false;
    }

    void TraceTarget(Vector3 pos)
    {
        if (agent.isPathStale)
            return;

        agent.destination = pos;  //추적 대상 지정
        agent.isStopped = false;  //내비게이션 기능 활성화
    }

    public void Stop()
    {
        agent.isStopped = true;
        //바로 정지하기 위해 남은 속도 0으로 초기화
        agent.velocity = Vector3.zero;
        _patrolling = false;
    }

    private void Update()
    {
        if(!agent.isStopped)  //적이 움직이는 중일때
        {
            //적이 진행해야 될 방향 벡터를 통해서 회전 각도를 계산
            Quaternion rot = Quaternion.LookRotation(agent.desiredVelocity);
            //보간 함수 적용 점진적 회전
            enemyTr.rotation = Quaternion.Slerp(enemyTr.rotation, rot, Time.deltaTime * damping);
        }

        if (!_patrolling)
            return;

        //목적지 도착 여부 확인
        //속도가 0.2 보다 크고 남은 이동거리가 10 이하일 경우
        //sqrMagnitude 가 Magnutude 보다 성능이 좋음
        if(agent.velocity.sqrMagnitude >= 0.2f * 0.2f && agent.remainingDistance <= 10f)
        {
            nextIdx = Random.Range(0, wayPoints.Count);

            MoveWayPoint();
        }
    }

}
