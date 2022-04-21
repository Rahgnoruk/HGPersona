using HyperGnosys.Core;
using System;
using UnityEngine;
using UnityEngine.AI;

namespace HyperGnosys.PersonaModule
{
    [Serializable]
    public class MoveInitializableActivity : IActivity
    {
        [SerializeField] private bool debugging = false;
        [Space, Header("Animation Variables. If the agent's movement doesn't have an animation you can ignore these.")]
        [SerializeField] private Animator animator;
        [SerializeField] private string moveAnimationName;
        private ISingleEnemyTargetAwareness targetAwareness;
        private APersonality personality;
        private NavMeshAgent agent;
        private int moveAnimationNameHash;

        private Vector3 destination = Vector3.zero;
        public void Init(APersonality personality, NavMeshAgent agent, ISingleEnemyTargetAwareness targetAwareness)
        {
            this.personality = personality;
            this.agent = agent;
            this.targetAwareness = targetAwareness;
            moveAnimationNameHash = Animator.StringToHash(moveAnimationName);
        }
        public void ActivityStart()
        {
            if (agent == null)
            {
                HGDebug.LogError($"NavMeshAgent hasn't been assigned", personality, debugging);
                return;
            }
            animator.SetBool(moveAnimationNameHash, true);
            destination = targetAwareness.TargetEnemy.transform.position;
            agent.SetDestination(destination);
        }
        public void ActivityUpdate()
        {
            if (destination != targetAwareness.TargetEnemy.transform.position)
            {
                destination = targetAwareness.TargetEnemy.transform.position;
                agent.SetDestination(destination);
            }
            bool hasReachedDestination = agent.remainingDistance <= agent.stoppingDistance;
            if (!hasReachedDestination)
            {
                return;
            }
            animator.SetBool(moveAnimationNameHash, false);
            HGDebug.Log($"{agent.name} reached it's destination: {agent.destination}", agent, debugging);
        }

        public void ActivityEnd()
        {
            animator.SetBool(moveAnimationNameHash, false);
        }
    }
}