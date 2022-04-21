using HyperGnosys.Core;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace HyperGnosys.PersonaModule
{
    public class GuardPersonality : APersonality
    {
        [SerializeField] private AttributesComponent attributes;
        [SerializeField] private AttributeTag healthAttributeTag;
        [SerializeField] private AttributeTag maxHealthAttributeTag;
        private Attribute<float> healthAttribute;
        private Attribute<float> maxHealthAttribute;

        [SerializeField] private FactionAwarenessComponent enemyAwareness;
        [SerializeField] private SingleTargetFactionAwarenessComponent targetAwareness;
        [SerializeField] private NavMeshAgent agent;

        [Space]
        [Space]
        [Header("Behaviour Styles Configuration")]
        [SerializeField] private HasTargetCondition hasTargetCondition;
        [SerializeField] private NavMeshCantReachCondition navMeshCantReachCondition;

        [Space, Header("Guarding Style Configuration")]
        [SerializeField] private ExternalReference<IActivity> guardingBehaviour;
        private Attitude guardingAttitude;

        [Space, Header("Melee Style Configuration")]
        [SerializeField] private ExternalReference<IActivity> meleePursuitBehaviour;
        [SerializeField] private ExternalReference<IActivity> meleeAttackBehaviour;
        [SerializeField] private HealthBelowThresholdInitializableCondition healthTooLowForMeleeCondition;
        [SerializeField] private TargetInRangeInitializableCondition targetInMeleeRangeCondition;
        private Attitude meleePursuitAttitude;
        private Attitude meleeAttackAttitude;

        [Space, Header("Ranged Style Configuration")]
        [SerializeField] private ExternalReference<IActivity> rangedPursuitBehaviour;
        [SerializeField] private ExternalReference<IActivity> rangedAttackBehaviour;
        [SerializeField] private TargetInRangeInitializableCondition targetInRangedRangeCondition;
        private Attitude rangedPursuitAttitude;
        private Attitude rangedAttackAttitude;

        [Space, Header("Fleeing Configuration")]
        [SerializeField] private ExternalReference<IActivity> fleeingBehaviour;
        [SerializeField] private EvaluateHealthAndEnemiesInitializableCondition shouldFleeCondition;
        private Attitude fleeingAttitude;
        private void CreateAttitudes()
        {
            guardingAttitude = new Attitude(this, guardingBehaviour.Reference, Debugging);
            meleePursuitAttitude = new Attitude(this, meleePursuitBehaviour.Reference, Debugging);
            meleeAttackAttitude = new Attitude(this, meleeAttackBehaviour.Reference, Debugging);
            rangedPursuitAttitude = new Attitude(this, rangedPursuitBehaviour.Reference, Debugging);
            rangedAttackAttitude = new Attitude(this, rangedAttackBehaviour.Reference, Debugging);
            fleeingAttitude = new Attitude(this, fleeingBehaviour.Reference, Debugging);
        }
        private void GetHealthAttributes()
        {
            healthAttribute = attributes.GetAttribute<float>(healthAttributeTag);
            if (healthAttribute == null) Debug.Log("health attribute not found");
            maxHealthAttribute = attributes.GetAttribute<float>(maxHealthAttributeTag);
            if (maxHealthAttribute == null) Debug.Log("max health attribute not found");
        }

        protected override void Init()
        {
            CreateAttitudes();
            GetHealthAttributes();

            healthTooLowForMeleeCondition.Init(healthAttribute, maxHealthAttribute);
            shouldFleeCondition.Init(healthAttribute, maxHealthAttribute, enemyAwareness);
            targetInMeleeRangeCondition.Init(targetAwareness, agent.transform);
            targetInRangedRangeCondition.Init(targetAwareness, agent.transform);

            hasTargetCondition.Init(targetAwareness);
            navMeshCantReachCondition.Init(agent);
            Belief noTargetsBelief = new Belief(guardingAttitude, hasTargetCondition, false);
            Belief cantReachTargetBelief = new Belief(guardingAttitude, navMeshCantReachCondition, true);

            Belief absoluteFleeBelief = new Belief(fleeingAttitude, shouldFleeCondition, true);
            AbsoluteBeliefs.Add(absoluteFleeBelief);

            ConfigureGuardingStyle();
            ConfigureMeleeStyle(noTargetsBelief, cantReachTargetBelief);
            ConfigureRangedStyle(noTargetsBelief, cantReachTargetBelief);
            ConfigureFleeingStyle();
        }
        private void ConfigureGuardingStyle()
        {
            List<Belief> leaveGuardingAttitudeBeliefs = new List<Belief>();

            Belief foundTargetBelief = new Belief(meleePursuitAttitude, hasTargetCondition, true);

            leaveGuardingAttitudeBeliefs.Add(foundTargetBelief);

            guardingAttitude.SetBeliefs(leaveGuardingAttitudeBeliefs);
            ChangeAttitude(guardingAttitude);
        }
        private void ConfigureMeleeStyle(Belief noTargetsBelief, Belief cantReachTargetBelief)
        {
            List<Belief> meleeBeliefs = new List<Belief>();
            List<Belief> meleePursuitBeliefs;
            List<Belief> meleeAttackBeliefs;

            Belief healthTooLowBelief = new Belief(rangedPursuitAttitude, healthTooLowForMeleeCondition, true);

            meleeBeliefs.Add(healthTooLowBelief);
            meleeBeliefs.Add(noTargetsBelief);
            meleeBeliefs.Add(cantReachTargetBelief);
            meleePursuitBeliefs = new List<Belief>(meleeBeliefs);
            meleeAttackBeliefs = new List<Belief>(meleeBeliefs);

            Belief targetInRangeBelief = new Belief(meleeAttackAttitude, targetInMeleeRangeCondition, true);
            meleePursuitBeliefs.Add(targetInRangeBelief);
            Belief targetNotInRangeBelief = new Belief(meleePursuitAttitude, targetInMeleeRangeCondition, false);
            meleeAttackBeliefs.Add(targetNotInRangeBelief);

            meleePursuitAttitude.SetBeliefs(meleePursuitBeliefs);
            meleeAttackAttitude.SetBeliefs(meleeAttackBeliefs);
        }
        private void ConfigureRangedStyle(Belief noTargetsBelief, Belief cantReachTargetBelief)
        {
            List<Belief> rangedBeliefs = new List<Belief>();
            List<Belief> rangedPursuitBeliefs;
            List<Belief> rangedAttackBeliefs;

            Belief backToMeleeBelief = new Belief(meleePursuitAttitude, healthTooLowForMeleeCondition, false);

            rangedBeliefs.Add(noTargetsBelief);
            rangedBeliefs.Add(backToMeleeBelief);
            rangedBeliefs.Add(cantReachTargetBelief);
            rangedPursuitBeliefs = new List<Belief>(rangedBeliefs);
            rangedAttackBeliefs = new List<Belief>(rangedBeliefs);

            Belief targetInRangeBelief = new Belief(rangedAttackAttitude, targetInRangedRangeCondition, true);
            rangedPursuitBeliefs.Add(targetInRangeBelief);
            Belief targetNotInRangeBelief = new Belief(rangedPursuitAttitude, targetInRangedRangeCondition, false);
            rangedAttackBeliefs.Add(targetNotInRangeBelief);

            rangedPursuitAttitude.SetBeliefs(rangedPursuitBeliefs);
            rangedAttackAttitude.SetBeliefs(rangedAttackBeliefs);
        }
        private void ConfigureFleeingStyle()
        {
            List<Belief> stopFleeingBeliefs = new List<Belief>();

            Belief canFightAgainBelief = new Belief(rangedAttackAttitude, shouldFleeCondition, false);

            stopFleeingBeliefs.Add(canFightAgainBelief);

            fleeingAttitude.SetBeliefs(stopFleeingBeliefs);
        }
    }
}