using HyperGnosys.Core;
using HyperGnosys.PersonaModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace HyperGnosys.PersonalityModule
{
    public class MoveActivityComponent : AActivityComponent
    {
        [SerializeField] private APersonality personality;
        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private ExternalReference<ISingleEnemyTargetAwareness> targetAwareness;
        [SerializeField] private MoveInitializableActivity moveActivity;
        private void Awake()
        {
            moveActivity.Init(personality, agent, targetAwareness.Reference);
        }
        public override void ActivityStart()
        {
            moveActivity.ActivityStart();
        }
        public override void ActivityUpdate()
        {
            moveActivity.ActivityUpdate();
        }
        public override void ActivityEnd()
        {
            moveActivity.ActivityEnd();
        }
    }
}