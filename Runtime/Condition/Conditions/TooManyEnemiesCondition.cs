using HyperGnosys.Core;
using HyperGnosys.Persona;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HyperGnosys.PersonaModule
{
    public class TooManyEnemiesCondition : ICondition
    {
        [SerializeField] private bool debugging = false;
        [SerializeField] private int maxEnemyAmount = 1;
        [SerializeField] private ExternalReference<IEnemyAwareness> enemyAwareness;

        public bool Evaluate()
        {
            return enemyAwareness.Reference.PerceivedEnemies.Count > maxEnemyAmount;
        }
    }
}