using HyperGnosys.Core;
using System;
using UnityEngine;

namespace HyperGnosys.PersonaModule
{
    [Serializable]
    public class TargetInRangeInitializableCondition : ICondition
    {
        [SerializeField] private bool debugging = false;
        [SerializeField] private float range = 2;
        private ISingleEnemyTargetAwareness targetAwareness;
        private Transform agentsBody;
        public void Init(ISingleEnemyTargetAwareness targetAwareness, Transform agentsBody)
        {
            this.targetAwareness = targetAwareness;
            this.agentsBody = agentsBody;
        }
        public bool Evaluate()
        {
            Vector3 distanceVector = targetAwareness.TargetEnemy.transform.position - agentsBody.position;
            HGDebug.Log($"Target is at {distanceVector.magnitude} units", debugging);
            return distanceVector.magnitude <= range;
        }
    }
}