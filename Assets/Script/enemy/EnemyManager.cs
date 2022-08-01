using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{

    // stores all enemy
    [SerializeField]
    public static EnemyAIFighting[] skList;

    public static EnemyManager instance;

    // Start is called before the first frame update
    void Start()
    {
        EnemyManager.skList = GameObject.FindObjectsOfType<EnemyAIFighting>();
        EnemyManager.instance = this;
    }

}
