
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//base enemy

public class Skeleton : BaseEnemy
{


    public bool canFight;
    public enum SkeletionState
    {
        StandBy,
        Fight,
        Death
    }



    public float StandByMoveSpeed;
    public float moveRangeInStandBy;

    public float minMoveTime;
    public float maxMoveTime;

    public float nextMoveTime;
    //

    public float fightMoveSpeed;


    public float minAttackDistance;
    public float maxAttackDistance;


    public float timer;







    //



    public SkeletionState state;




    public float attackDistance;


    public List<Transform> aList;
 
    // Use this for initialization
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        timer += Time.deltaTime;
        agent.isStopped = false;


     
        switch (state)
        {
            case SkeletionState.StandBy:
                {

                    OnStandByState();
                }
                break;
            case SkeletionState.Fight:
                {
                    OnFightState();
                }
                break;

        }
    }
    //initialization
    public override void Init()
    {
        base.Init();
        SwitchState(SkeletionState.StandBy);
    }
    //standby
    private void OnStandByState()
    {


        if (timer > 4)
        {
            MoveInStandBy();
            timer = 0;
        }


    }
    //fighting stage
    private void OnFightState()
    {

        agent.SetDestination(target.position);
        agent.updatePosition = true;
   

        bool canAttack = false;
        bool attacking = false;


        if (Vector3.Distance(transform.position, target.position) < attackDistance)
        {
            Ray ray = new Ray(transform.position, target.position - transform.position);
            RaycastHit hit;

            canAttack = true;
            transform.LookAt(target);
   
            if (Physics.Raycast(ray, out hit, 100, LayerMask.GetMask("Player", "Building")))
            {
                print("2");
                Debug.DrawRay(transform.position, target.position - transform.position);
 
            }
            else
            {
                canAttack = true;
            }

        }




        if (attacking || canAttack) agent.isStopped = true;
        else agent.isStopped = false;




    }
    //when death
    private void OnStateDeath()
    {
        agent.isStopped = true;

    }

    //switch state of enemy
    public void SwitchState(SkeletionState targetState)
    {
        if (canFight==false) return;
        switch (targetState)
        {
            case SkeletionState.StandBy:
                {
                    state = SkeletionState.StandBy;
                    agent.isStopped = false;
                    agent.speed = StandByMoveSpeed;
                  
                    nextMoveTime = 0;
                }
                break;
            case SkeletionState.Fight:
                {
                    state = SkeletionState.Fight;
                    agent.speed = fightMoveSpeed;
                 
  
                }
                break;

        }

    }

    //check if arrived at destination
    private bool ArriveDestination()
    {
        if (Vector3.Distance(transform.position, agent.destination) < attackDistance)
        {
            return true;
        }
        else
            return false;
    }
    
    //movement when standby
    private void MoveInStandBy()
    {
        //Vector3 moveOffset = Random.insideUnitSphere * moveRangeInStandBy;
        // Vector3 targetPos = transform.position + new Vector3(moveOffset.x, 0, moveOffset.z);
        print("cc");
        int num = Random.Range(0, aList.Count);
        Vector3 targetPos = aList[num].position;


        agent.SetDestination(targetPos);

        nextMoveTime = Random.Range(minMoveTime, maxMoveTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            SwitchState(SkeletionState.Fight);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            SwitchState(SkeletionState.Fight);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            SwitchState(SkeletionState.StandBy);
        }
    }
}
