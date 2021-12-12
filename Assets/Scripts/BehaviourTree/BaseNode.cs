using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TaskStatus { Success, Failed, Running, Skip }
public abstract class BaseNode
{
    public abstract TaskStatus Run();
}

public class NodeSequence : BaseNode{
        private int currentIndex = 0;
        private BaseNode[] nodes;
        public NodeSequence(params BaseNode[] inputNodes)
        {
            nodes = inputNodes;
        }

        public override TaskStatus Run()
        {
            for(; currentIndex < nodes.Length ; currentIndex++)
            {
                TaskStatus result = nodes[currentIndex].Run();
                switch (result)
                {
                    case TaskStatus.Failed: currentIndex = 0; return TaskStatus.Failed;
                    case TaskStatus.Success: continue;
                    case TaskStatus.Skip: return TaskStatus.Success;
                    case TaskStatus.Running: return TaskStatus.Running;
                }
            }
            currentIndex = 0;
            return TaskStatus.Success;
        }
}