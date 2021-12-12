using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

namespace EnemyNodes{

public class FindWeapon : BaseNode
{
    Blackboard blackboard = BlackboardServiceDesk.GetBlackboard("Enemy");
    Transform weapon;
    GameObject enemy;
    public FindWeapon(){
        weapon = blackboard.GetVariable<Transform>("weapon");
        enemy = blackboard.GetVariable<GameObject>("self");
    }
    public override TaskStatus Run()
    {
        enemy.GetComponentInChildren<Text>().text = "FindWeapon";
        if(blackboard.GetVariable<bool>("hasWeapon") || (!blackboard.GetVariable<bool>("hasWeapon") && !blackboard.GetVariable<bool>("playerSpotted"))){
            return TaskStatus.Success;
        }else{
            NavMeshAgent agent = enemy.GetComponent<NavMeshAgent>();
            agent.isStopped = false;
            if(Vector3.Distance(agent.destination, weapon.position)>1){
                Debug.Log("SetDestination");
                agent.SetDestination(weapon.position);
            }
            if(agent.remainingDistance <= agent.stoppingDistance){
                weapon.gameObject.SetActive(false);
                blackboard.SetVariable("hasWeapon",true);
                return TaskStatus.Success;
            }
            return TaskStatus.Running;
        }
    }
}

public class PlayerDetection : BaseNode
{
    Blackboard blackboard = BlackboardServiceDesk.GetBlackboard("Enemy");
    Blackboard playerboard = BlackboardServiceDesk.GetBlackboard("Player");
    GameObject player;
    GameObject enemy;

    public PlayerDetection(){
        enemy = blackboard.GetVariable<GameObject>("self");
        player = playerboard.GetVariable<GameObject>("self");
    }

    bool isInSmoke(GameObject obj){
        var smoke = GameObject.FindGameObjectWithTag("Smoke");
        if(smoke == null) return false;
        return smoke.GetComponent<Renderer>().bounds.Contains(obj.transform.position);
    }

    public override TaskStatus Run()
    {
        enemy.GetComponentInChildren<Text>().text = "PlayerDetection";
        float maxRange = 5;
        RaycastHit hit;
        if(Physics.Raycast(enemy.transform.position, (player.transform.position - enemy.transform.position),out hit, maxRange)){
            if(hit.transform == player.transform){
                blackboard.SetVariable("playerSpotted",true);
                playerboard.SetVariable("targetted",true);

                if(isInSmoke(player) || isInSmoke(enemy))
                    return TaskStatus.Skip;
                return TaskStatus.Success;
            }
        }
        playerboard.SetVariable("targetted",false);
        return TaskStatus.Skip;
    }
}

public class GoToPlayer : BaseNode
{
    Blackboard blackboard = BlackboardServiceDesk.GetBlackboard("Enemy");
    GameObject player = BlackboardServiceDesk.GetBlackboard("Player").GetVariable<GameObject>("self");
    GameObject enemy;

    public GoToPlayer(){
        enemy = blackboard.GetVariable<GameObject>("self");
    }

    bool isInSmoke(GameObject obj){
        var smoke = GameObject.FindGameObjectWithTag("Smoke");
        if(smoke == null) return false;
        return smoke.GetComponent<Renderer>().bounds.Contains(obj.transform.position);
    }

    public override TaskStatus Run()
    {
        enemy.GetComponentInChildren<Text>().text = "GoToPlayer";
        if(!blackboard.GetVariable<bool>("hasWeapon")){
            return TaskStatus.Failed;
        }else{
            NavMeshAgent agent = enemy.GetComponent<NavMeshAgent>();

            if(isInSmoke(player) || isInSmoke(enemy))
                return TaskStatus.Failed;

            if(Vector3.Distance(enemy.transform.position,player.transform.position) < 1.5f){
                agent.SetDestination(enemy.transform.position);
                return TaskStatus.Success;
            }

            if(Vector3.Distance(enemy.transform.position,player.transform.position) > 5){
                return TaskStatus.Failed;
            }

            if(Vector3.Distance(agent.destination, player.transform.position)>1){
                agent.SetDestination(player.transform.position);
            }
            return TaskStatus.Running;
        }
    }
}

public class Attack : BaseNode
{
    GameObject enemy = BlackboardServiceDesk.GetBlackboard("Enemy").GetVariable<GameObject>("self");
    GameObject player = BlackboardServiceDesk.GetBlackboard("Player").GetVariable<GameObject>("self");
    float cd = 0;
    float maxCD = 4;
    public Attack(){}

    public override TaskStatus Run()
    {
        enemy.GetComponentInChildren<Text>().text = "Attack";
        if(cd == 0){
            //do an animation!!!
            player.GetComponent<Player>().health -= 10;
            cd++;
            return TaskStatus.Running;
        }else if(cd < maxCD){
            cd += Time.deltaTime;
            return TaskStatus.Running;
        }else{
            cd = 0;
            return TaskStatus.Success;
        }
    }
}

public class Patrol : BaseNode
{
    GameObject enemy = BlackboardServiceDesk.GetBlackboard("Enemy").GetVariable<GameObject>("self");
    Blackboard blackboard = BlackboardServiceDesk.GetBlackboard("Enemy");
    public Patrol(){}
    public override TaskStatus Run()
    {
        enemy.GetComponentInChildren<Text>().text = "Patrol";
        Transform points = blackboard.GetVariable<Transform>("points");
        int point = blackboard.GetVariable<int>("point");
        NavMeshAgent agent = enemy.GetComponent<NavMeshAgent>();

        if(Vector3.Distance(agent.destination, points.GetChild(point).position) > 1){
            agent.SetDestination(points.GetChild(point).position);
        }

        if(agent.remainingDistance <= agent.stoppingDistance){
            point = (point + 1)%4;
            agent.SetDestination(points.GetChild(point).position);
            blackboard.SetVariable("point",point);
        }

        return TaskStatus.Success;
    }
}
    
}