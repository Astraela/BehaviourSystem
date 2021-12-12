using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blackboard
{
    Dictionary<object,object> variables = new Dictionary<object, object>();

    public T GetVariable<T>(object obj){
        if(variables.ContainsKey(obj)){
            return (T)variables[obj];
        }
        return default;
    }

    public void SetVariable(object obj, object variable){
        if(!variables.ContainsKey(obj)){
            variables.Add(obj,variable);
            return;
        }
        variables[obj] = variable;
    }

    public Blackboard(){}
}
