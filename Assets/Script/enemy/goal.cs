using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{



    public List<Skeleton> enemyList;
    public Skeleton temp;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
           
        }
        else if (other.tag == "Player")
        {
            print("gggg");
            float minDis = 100;
     

            foreach (var item in enemyList)
            {
                if(Vector3.Distance(item.transform.position, other.transform.position) < minDis)
                {
                    temp = item;
                }
            }
            temp.SwitchState(Skeleton.SkeletionState.Fight);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Enemy")
        {
            //enemyList.Remove(other.GetComponent<Skeleton>());
        }
    }
}
