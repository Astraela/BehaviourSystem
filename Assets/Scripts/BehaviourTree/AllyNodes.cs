using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

namespace AllyNodes{

    public class Hide : BaseNode{
        Blackboard blackboard = BlackboardServiceDesk.GetBlackboard("Ally");
        Blackboard playerBlackboard = BlackboardServiceDesk.GetBlackboard("Player");
        Transform hidingPlace;
        GameObject ally;

        public Hide(){
            ally = blackboard.GetVariable<GameObject>("self");
            hidingPlace = blackboard.GetVariable<Transform>("hidingPlace");
        }

        public override TaskStatus Run()
        {
            ally.GetComponentInChildren<Text>().text = "Hide";
            NavMeshAgent agent = ally.GetComponent<NavMeshAgent>();
            if(!playerBlackboard.GetVariable<bool>("targetted")){
                return TaskStatus.Skip;
            }
            
            if(Vector3.Distance(hidingPlace.position,ally.transform.position) > 1){
                agent.SetDestination(hidingPlace.position);
                return TaskStatus.Failed;
            }else{
                return TaskStatus.Success;
            }
        }
    }

    public class ThrowBomb : BaseNode
    {
        GameObject smokeBomb = BlackboardServiceDesk.GetBlackboard("Ally").GetVariable<GameObject>("smokeBomb");
        GameObject player = BlackboardServiceDesk.GetBlackboard("Player").GetVariable<GameObject>("self");
        float cd = 0;
        float maxCD = 1000;
        GameObject ally = BlackboardServiceDesk.GetBlackboard("Ally").GetVariable<GameObject>("self");
        public ThrowBomb(){}

        public override TaskStatus Run()
        {
            ally.GetComponentInChildren<Text>().text = "ThrowBomb";
            NavMeshAgent agent = ally.GetComponent<NavMeshAgent>();
            agent.SetDestination(ally.transform.position);
            if(cd == 0){
                //do an animation!!!
                GameObject newSmokeBomb = GameObject.Instantiate(smokeBomb);
                newSmokeBomb.transform.position = player.transform.position;
                GameObject.Destroy(newSmokeBomb,3);
                cd++;
                return TaskStatus.Running;
            }else if(cd < maxCD){
                cd++;
                return TaskStatus.Running;
            }else{
                cd = 0;
                return TaskStatus.Success;
            }
        }
    }

    public class DetectPlayer : BaseNode
    {
        Blackboard blackboard = BlackboardServiceDesk.GetBlackboard("Ally");
        GameObject player = BlackboardServiceDesk.GetBlackboard("Player").GetVariable<GameObject>("self");
        GameObject ally;

        public DetectPlayer(){
            ally = blackboard.GetVariable<GameObject>("self");
        }

        public override TaskStatus Run()
        {
            ally.GetComponentInChildren<Text>().text = "DetectPlayer";
            if(blackboard.GetVariable<bool>("playerSpotted")){
                return TaskStatus.Success;
            }
            float maxRange = 5;
            RaycastHit hit;
            if(Physics.Raycast(ally.transform.position, (player.transform.position - ally.transform.position),out hit, maxRange)){
                if(hit.transform == player.transform){
                    blackboard.SetVariable("playerSpotted",true);
                    return TaskStatus.Success;
                }
            }
            return TaskStatus.Failed;
        }
    }

    public class FollowPlayer : BaseNode
    {
        Blackboard blackboard = BlackboardServiceDesk.GetBlackboard("Ally");
        GameObject player = BlackboardServiceDesk.GetBlackboard("Player").GetVariable<GameObject>("self");
        GameObject ally;
        public FollowPlayer(){
            ally = blackboard.GetVariable<GameObject>("self");
        }
        public override TaskStatus Run()
        {
            ally.GetComponentInChildren<Text>().text = "FollowPlayer";
            NavMeshAgent agent = ally.GetComponent<NavMeshAgent>();
            if(Vector3.Distance(player.transform.position,ally.transform.position) > 3){
                agent.SetDestination(player.transform.position);
            }else{
                agent.SetDestination(ally.transform.position);
            }
            return TaskStatus.Success;
        }
    }


}
