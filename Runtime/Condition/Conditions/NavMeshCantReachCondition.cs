using System;
using UnityEngine;
using UnityEngine.AI;

namespace HyperGnosys.PersonaModule
{
    [Serializable]
    public class NavMeshCantReachCondition : ICondition
    {
        [SerializeField] private bool debugging = false;
        private NavMeshAgent agent;

        public void Init(NavMeshAgent agent)
        {
            this.agent = agent;
        }

        public bool Evaluate()
        {
            return agent.path.status == NavMeshPathStatus.PathPartial;
        }
    }
}