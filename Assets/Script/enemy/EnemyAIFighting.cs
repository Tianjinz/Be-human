
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//enemy ai when lured

public class EnemyAIFighting : BaseEnemy
{


    public bool canFight;
    public enum SkeletionState
    {
        StandBy,
        BackToPatrol,
        Fight,
        Death
    }



    public float StandByMoveSpeed;
    public float moveRangeInStandBy;

    public float minMoveTime;
    public float maxMoveTime;

    public float nextMoveTime;

    public float fightStateTime;
    //

    public float fightMoveSpeed;


    public float minAttackDistance;
    public float maxAttackDistance;


    public float timer;

    private EnemyAIPatroling enemyPatrol;

    private float fightTimerCountDown;

    // original position of enemy

    private Vector3 ogPos;

    private Vector3 ogPosWorld;



    //



    public SkeletionState state;




    public float attackDistance;


    public List<Transform> aList;
 
    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        agent.enabled = false;
        enemyPatrol = this.gameObject.GetComponent<EnemyAIPatroling>();
        ogPos = this.gameObject.transform.localPosition;
        ogPosWorld = this.gameObject.transform.position;
    }

    // Update is called once per frame
    protected override void Update()
    {

        timer += Time.deltaTime;
        if(agent.enabled){
            agent.isStopped = false;
        }

     
        switch (state)
        {
            case SkeletionState.StandBy:
                {

                    // OnStandByState();
                }
                break;
            case SkeletionState.BackToPatrol:
                {
                    OnBackToPatrol();
                }
                break;
            case SkeletionState.Fight:
                {
                    fightTimerCountDown -= Time.deltaTime;
                    if(fightTimerCountDown < 0){
                        SwitchState(SkeletionState.BackToPatrol);
                    }
                    OnFightState();
                }
                break;

        }
    }
    //initialization
    public override void Init()
    {
        base.Init();
        // SwitchState(SkeletionState.StandBy);
    }

    private void OnBackToPatrol()
    {
        // this fixes a bug that change the z position of object when destination changes
        Vector3 localPos = this.gameObject.transform.localPosition;
        this.gameObject.transform.localPosition = new Vector3 (localPos.x, ogPos.y, localPos.z);

        // check if the entity got back to its original spot
        if(ArriveDestination()){
            SwitchState(SkeletionState.StandBy);
        }
    }

    //action when in fight state
    private void OnFightState()
    {
        // this fixes a bug that change the z position of object when destination changes
        Vector3 localPos = this.gameObject.transform.localPosition;
        this.gameObject.transform.localPosition = new Vector3 (localPos.x, ogPos.y, localPos.z);
        // if the distance between ai and player less than attack distance, set navigation is off
        if (Vector3.Distance(transform.position, target.position) < attackDistance)
            {
                agent.isStopped = true;
            }
    }

    //when enemy is dead
    private void OnStateDeath()
    {
        agent.isStopped = true;

    }

    //switch the states enemy is in
    public void SwitchState(SkeletionState targetState)
    {
        if (canFight==false) return;
        switch (targetState)
        {
            // Ai enter the patrol state
            case SkeletionState.StandBy:
                {
                    state = SkeletionState.StandBy;
                    agent.enabled = false;
                    enemyPatrol.startPatroling();
                }
                break;
            // Ai stop looking for player and go back to patrol
            case SkeletionState.BackToPatrol:
                {
                    state = SkeletionState.BackToPatrol;
                    agent.isStopped = false;
                    agent.speed = StandByMoveSpeed;
                    agent.SetDestination(ogPosWorld);
                }
                break;
            // Ai starts looking for player
            case SkeletionState.Fight:
                {
                    agent.enabled = true;
                    agent.SetDestination(target.position);
                    enemyPatrol.stopPatroling();
                    state = SkeletionState.Fight;
                    agent.enabled = true;
                    agent.speed = fightMoveSpeed;
                    fightTimerCountDown = fightStateTime;
                }
                break;

        }

    }

    // check if enemy has arrived at the destination
    private bool ArriveDestination()
    {
        return (Vector3.Distance(transform.position, agent.destination) < attackDistance);
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
