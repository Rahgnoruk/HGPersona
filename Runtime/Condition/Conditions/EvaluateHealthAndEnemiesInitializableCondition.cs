using HyperGnosys.Core;
using System;
using UnityEngine;

namespace HyperGnosys.PersonaModule
{
    [Serializable]
    public class EvaluateHealthAndEnemiesInitializableCondition : ICondition
    {
        [SerializeField] private float hurtHealthPercentage = 0.5f;
        [SerializeField] private float criticalHealthPercentage = 0.1f;
        [SerializeField] private int overwhelmingEnemiesAmount = 2;
        private Attribute<float> health;
        private Attribute<float> maxHealth;
        private IEnemyAwareness enemyAwareness;

        public void Init(Attribute<float> health, Attribute<float> maxHealth, IEnemyAwareness enemyAwareness)
        {
            this.health = health;
            this.maxHealth = maxHealth;
            this.enemyAwareness = enemyAwareness;
        }

        public bool Evaluate()
        {
            bool isHealthBelowHalf = AttributeUtilities.GetHealthPercentage(health, maxHealth) < hurtHealthPercentage;
            bool isTooRisky = isHealthBelowHalf && enemyAwareness.PerceivedEnemies.Count >= overwhelmingEnemiesAmount;
            bool isHealthCritical = AttributeUtilities.GetHealthPercentage(health, maxHealth) < criticalHealthPercentage;
            return isTooRisky || isHealthCritical;
        }
    }
}