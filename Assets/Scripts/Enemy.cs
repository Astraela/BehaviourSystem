using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using EnemyNodes;

public class Enemy : MonoBehaviour
{
    public Transform points;
    public Transform weapon;

    int point = 0;
    NavMeshAgent agent;
    NodeSequence tree;
    Blackboard blackboard = new Blackboard();

    void Start(){
        BlackboardServiceDesk.AddBlackboard("Enemy", blackboard);
        blackboard.SetVariable("points",points);
        blackboard.SetVariable("point",point);
        blackboard.SetVariable("self",gameObject);
        blackboard.SetVariable("weapon",weapon);
        blackboard.SetVariable("hasWeapon",false);
        blackboard.SetVariable("playerSpotted",false);

        tree = new NodeSequence(
            new FindWeapon(),
            new NodeSequence(
                new PlayerDetection(),
                new GoToPlayer(),
                new Attack()
            ),
            new Patrol()
        );
    }
    
    void Update()
    {
        tree?.Run();

    }
}
