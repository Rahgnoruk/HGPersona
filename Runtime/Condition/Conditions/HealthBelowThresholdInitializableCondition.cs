using HyperGnosys.Core;
using HyperGnosys.MathUtilities;
using HyperGnosys.PersonaModule;
using System;
using UnityEngine;

namespace HyperGnosys.Persona
{
    [Serializable]
    public class HealthBelowThresholdInitializableCondition : ICondition
    {
        [SerializeField, Range(0, 1)] private float healthThreshold = 0.7f;

        private ExternalizableLabeledProperty<float> health;
        private ExternalizableLabeledProperty<float> maxHealth;

        public void Init(ExternalizableLabeledProperty<float> health,
            ExternalizableLabeledProperty<float> maxHealth)
        {
            this.health = health;
            this.maxHealth = maxHealth;
        }

        public bool Evaluate()
        {
            bool isHealthBelowThreshold = FloatOperations.GetPercentage(health.Value, maxHealth.Value) < healthThreshold;
            return isHealthBelowThreshold;
        }
    }
}