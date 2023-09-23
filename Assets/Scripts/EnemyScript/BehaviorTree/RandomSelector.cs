using System.Collections.Generic;
using Unity.Mathematics;

namespace BehaviorTree
{
    public class RandomSelector : Node
    {
        private int curNodeIndex;
        private Random rnd;

        public RandomSelector() : base() { 
            curNodeIndex = -1;
            rnd = new Random();
        }

        public RandomSelector(List<Node> children) : base(children)
        {
            curNodeIndex = -1;
            rnd = new Random();
        }
        
        public override NodeState Evaluate()
        {
            if (curNodeIndex < 0)
                curNodeIndex = GetRandomIndex();
            
            state = children[curNodeIndex].Evaluate();
            if (state != NodeState.FAILURE)
            {
                if (state == NodeState.SUCCESS)
                    curNodeIndex = GetRandomIndex();
                return state;
            }
            
            List<int> randomShuffle = GetRandomShuffle();
            int failedIndex = curNodeIndex;
            foreach (int index in randomShuffle)
            {
                if (index == failedIndex)
                    continue;
                curNodeIndex = index;
                switch (children[curNodeIndex].Evaluate())
                {
                    case NodeState.FAILURE:
                        break;
                    case NodeState.SUCCESS:
                        state = NodeState.SUCCESS;
                        curNodeIndex = GetRandomIndex();
                        return state;
                    case NodeState.RUNNING:
                        state = NodeState.RUNNING;
                        return state;
                    default:
                        break;
                }
            }

            state = NodeState.FAILURE;
            return state;
        }

        private int GetRandomIndex()
        {
            return rnd.NextInt(children.Count);
        }

        private List<int> GetRandomShuffle()
        {
            List<int> result = new List<int>();
            for (int i = 0; i < children.Count; i++)
                result.Add(i);
            int n = result.Count;
            while (n > 1)
            {
                n--;  
                int k = rnd.NextInt(n + 1);
                (result[k], result[n]) = (result[n], result[k]);
            }
            return result;
        }

    }

}
