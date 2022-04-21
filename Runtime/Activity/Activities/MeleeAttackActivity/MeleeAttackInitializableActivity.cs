using HyperGnosys.Core;
using HyperGnosys.PersonaModule;
using System;
using UnityEngine;
using UnityEngine.AI;

namespace HyperGnosys.PersonalityModule
{
    [Serializable]
    public class MeleeAttackInitializableActivity : IActivity
    {
        [SerializeField] private bool debugging = false;
        [SerializeField] private float attackCooldown = 2f;
        [Space, Header("Animation Variables. If the attack doesn't have an animation you can ignore these.")]
        [SerializeField] private Animator animator;
        [SerializeField] private string attackAnimationName;
        /*[SerializeField] private float attackDuration;
        [SerializeField] private float timeBetweenAttacks;*/
        private APersonality personality;
        private NavMeshAgent agent;
        private int animationNameHash;
        public void Init(APersonality personality, NavMeshAgent agent)
        {
            this.personality = personality;
            this.agent = agent;
            animationNameHash = Animator.StringToHash(attackAnimationName);
        }
        public void ActivityStart()
        {
            HGDebug.Log("Starting Melee Attack Activity", personality, debugging);
            animator.SetTrigger(animationNameHash);
        }
        public void ActivityUpdate()
        {
            
        }
        public void ActivityEnd()
        {
        }
    }
}