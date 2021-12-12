using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using AllyNodes;
public class Ally : MonoBehaviour
{
    public Transform HidingPlace;
    public GameObject SmokeBomb;

    int point = 0;
    NavMeshAgent agent;
    NodeSequence tree;
    Blackboard blackboard = new Blackboard();

    void Start(){
        BlackboardServiceDesk.AddBlackboard("Ally", blackboard);
        blackboard.SetVariable("self",gameObject);
        blackboard.SetVariable("playerSpotted",false);
        blackboard.SetVariable("smokeBomb",SmokeBomb);
        blackboard.SetVariable("hidingPlace",HidingPlace);

        tree = new NodeSequence(
            new NodeSequence(
                new Hide(),
                new ThrowBomb()
            ),
            new DetectPlayer(),
            new FollowPlayer()
        );
    }
    
    void Update()
    {
        tree?.Run();

    }
}
