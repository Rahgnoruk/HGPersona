using HyperGnosys.Persona;
using HyperGnosys.PersonaModule;
using UnityEngine;
using UnityEngine.AI;

namespace HyperGnosys.PersonalityModule
{
    public class RangedAttackActivityComponent : AActivityComponent
    {
        [SerializeField] private APersonality personality;
        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private ISingleEnemyTargetAwareness persona;
        [SerializeField] private RangedAttackInitializableActivity rangedAttackActivity;
        private void Awake()
        {
            rangedAttackActivity.Init(personality, persona, agent);
        }
        public override void ActivityStart()
        {
            rangedAttackActivity.ActivityStart();
        }
        public override void ActivityUpdate()
        {
            rangedAttackActivity.ActivityUpdate();
        }
        public override void ActivityEnd()
        {
            rangedAttackActivity.ActivityEnd();
        }
    }
}