using HyperGnosys.Core;
using System;
using UnityEngine;

namespace HyperGnosys.PersonaModule
{
    [Serializable]
    public class HasTargetCondition : ICondition
    {
        [SerializeField] private bool debugging = false;
        private ISingleEnemyTargetAwareness singleTargetAwareness;

        public void Init(ISingleEnemyTargetAwareness awareness)
        {
            singleTargetAwareness = awareness;
        }

        public bool Evaluate()
        {
            HGDebug.Log("Evaluating if there's a target", debugging);
            return singleTargetAwareness.TargetEnemy != null;
        }
    }
}