using HyperGnosys.Core;
using System;
using UnityEngine;

namespace HyperGnosys.PersonaModule
{
    [Serializable]
    public class HealthBelowThresholdInitializableCondition : ICondition
    {
        [SerializeField, Range(0, 1)] private float healthThreshold = 0.7f;

        private Attribute<float> health;
        private Attribute<float> maxHealth;

        public void Init(Attribute<float> health, Attribute<float> maxHealth)
        {
            this.health = health;
            this.maxHealth = maxHealth;
        }

        public bool Evaluate()
        {
            bool isHealthBelowThreshold = AttributeUtilities.GetHealthPercentage(health, maxHealth) < healthThreshold;
            return isHealthBelowThreshold;
        }
    }
}