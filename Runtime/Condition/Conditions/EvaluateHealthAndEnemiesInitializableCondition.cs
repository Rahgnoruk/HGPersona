using HyperGnosys.Core;
using HyperGnosys.MathUtilities;
using HyperGnosys.PersonaModule;
using System;
using UnityEngine;

namespace HyperGnosys.Persona
{
    [Serializable]
    public class EvaluateHealthAndEnemiesInitializableCondition : ICondition
    {
        [SerializeField] private float hurtHealthPercentage = 0.5f;
        [SerializeField] private float criticalHealthPercentage = 0.1f;
        [SerializeField] private int overwhelmingEnemiesAmount = 2;
        private ExternalizableLabeledProperty<float> health;
        private ExternalizableLabeledProperty<float> maxHealth;
        private IEnemyAwareness enemyAwareness;

        public void Init(ExternalizableLabeledProperty<float> health, ExternalizableLabeledProperty<float> maxHealth, IEnemyAwareness enemyAwareness)
        {
            this.health = health;
            this.maxHealth = maxHealth;
            this.enemyAwareness = enemyAwareness;
        }

        public bool Evaluate()
        {
            bool isHealthBelowHalf = FloatOperations.GetPercentage(health.Value, maxHealth.Value) < hurtHealthPercentage;
            bool isTooRisky = isHealthBelowHalf && enemyAwareness.PerceivedEnemies.Count >= overwhelmingEnemiesAmount;
            bool isHealthCritical = FloatOperations.GetPercentage(health.Value, maxHealth.Value) < criticalHealthPercentage;
            return isTooRisky || isHealthCritical;
        }
    }
}