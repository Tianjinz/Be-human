using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
//base type enemies
public class BaseEnemy : MonoBehaviour
{

    public  Transform target;

  
    public NavMeshAgent agent;


    // Use this for initialization
    protected virtual void Start()
    {

    }
    //initialization
    public virtual void Init()
    {
 
        agent = GetComponent<NavMeshAgent>();
        target = GameObject.FindGameObjectWithTag("Player").transform;

    }

    // Update is called once per frame
    protected virtual void Update()
    {
 
    }
    //attack
    protected virtual void Attack()
    {

    }
    //damaged
    public virtual void Damage(int damage)
    {
  



    }
    //death
    protected virtual void Dead()
    {

    }


}
