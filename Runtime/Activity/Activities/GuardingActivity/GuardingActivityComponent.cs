using HyperGnosys.PersonaModule;
using UnityEngine;
using UnityEngine.AI;

namespace HyperGnosys.PersonalityModule
{
    public class GuardingActivityComponent : AActivityComponent
    {
        [SerializeField] private APersonality personality;
        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private GuardingInitializableActivity guardingActivity;
        private void Awake()
        {
            guardingActivity.Init(personality, agent);
        }
        public override void ActivityStart()
        {
            guardingActivity.ActivityStart();
        }
        public override void ActivityUpdate()
        {
            guardingActivity.ActivityUpdate();
        }
        public override void ActivityEnd()
        {
            guardingActivity.ActivityEnd();
        }
    }
}