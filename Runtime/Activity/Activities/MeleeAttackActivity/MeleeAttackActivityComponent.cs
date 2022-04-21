using HyperGnosys.PersonaModule;
using UnityEngine;
using UnityEngine.AI;

namespace HyperGnosys.PersonalityModule
{
    public class MeleeAttackActivityComponent : AActivityComponent
    {
        [SerializeField] private APersonality personality;
        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private MeleeAttackInitializableActivity meleeAttackActivity;
        private void Awake()
        {
            meleeAttackActivity.Init(personality, agent);
        }
        public override void ActivityStart()
        {
            meleeAttackActivity.ActivityStart();
        }
        public override void ActivityUpdate()
        {
            meleeAttackActivity.ActivityUpdate();
        }
        public override void ActivityEnd()
        {
            meleeAttackActivity.ActivityEnd();
        }
    }
}