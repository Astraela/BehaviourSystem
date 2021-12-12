using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BlackboardServiceDesk
{
    static Dictionary<object,Blackboard> blackboardList = new Dictionary<object, Blackboard>();

    public static void AddBlackboard(object obj, Blackboard blackboard){
        if(!blackboardList.ContainsKey(obj)){
            blackboardList.Add(obj,blackboard);
        }else{
            blackboardList[obj] = blackboard;
        }
    }

    public static Blackboard GetBlackboard(object obj){
        if(blackboardList.ContainsKey(obj)){
            return blackboardList[obj];
        }
        return null;
    }
}
