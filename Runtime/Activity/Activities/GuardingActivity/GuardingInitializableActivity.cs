using HyperGnosys.Core;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;
namespace HyperGnosys.PersonaModule
{
    [Serializable]
    public class GuardingInitializableActivity : IActivity
    {
        [SerializeField] private bool debugging = true;
        [SerializeField] private Vector3 guardedAreaCenter = Vector3.zero;
        [SerializeField] private float guardedAreaRadius = 20;
        [SerializeField] private float maxWaitTime = 7;
        [SerializeField] private float minWaitTime = 3;
        [Space, Header("Animation Variables. If the agent doesn't have animations you can ignore these.")]
        [SerializeField] private Animator animator;
        [SerializeField] private string moveAnimationName;
        private int moveAnimationNameHash;

        private APersonality personality;
        private NavMeshAgent agent;

        private Vector3 currentDestination = Vector3.zero;
        private Coroutine surveilPosition;
        private bool isSurveilingPosition = false;
        public void Init(APersonality personality, NavMeshAgent agent)
        {
            this.personality = personality;
            this.agent = agent;
            moveAnimationNameHash = Animator.StringToHash(moveAnimationName);
        }
        public void ActivityStart()
        {
            if (agent == null)
            {
                HGDebug.LogError($"NavMeshAgent hasn't been assigned", personality, debugging);
                return;
            }
            SetNewGuardingDestination();
        }
        public void ActivityUpdate()
        {
            bool hasReachedDestination = agent.remainingDistance <= agent.stoppingDistance;
            if (!hasReachedDestination || isSurveilingPosition)
            {
                return;
            }
            animator.SetBool(moveAnimationNameHash, false);
            HGDebug.Log($"{agent.name} reached it's destination: {agent.destination}", agent, debugging);
            isSurveilingPosition = true;
            personality.StartCoroutine(SurveilPosition());
        }

        public void ActivityEnd()
        {
            animator.SetBool(moveAnimationNameHash, false);
            if (surveilPosition != null) personality.StopCoroutine(surveilPosition);
            isSurveilingPosition = false;
        }

        private IEnumerator SurveilPosition()
        {
            yield return new WaitForSeconds(Random.Range(minWaitTime, maxWaitTime));
            SetNewGuardingDestination();
            isSurveilingPosition = false;
        }
        private void SetNewGuardingDestination()
        {
            currentDestination = GetNewDestinationWithinGuardedArea();
            agent?.SetDestination(currentDestination);
            animator.SetBool(moveAnimationNameHash, true);
            HGDebug.Log($"{agent.name} has new destination: {agent.destination}", agent, debugging);
        }
        private Vector3 GetNewDestinationWithinGuardedArea()
        {
            Vector3 direction = VectorOperations.GetRandomXZDir();
            ///El punto maximo al que puede caminar en esa dirección
            ///determinado por el área de roaming
            Vector3 maxRoamingRange = guardedAreaCenter + direction * guardedAreaRadius;
            ///Desde donde está actualmente parado, qué tanta distancia puede caminar para llegar
            ///a ese punto
            Vector3 currentRoamingRange = maxRoamingRange - agent.transform.position;
            ///Crea un destino dentro de esa área
            Vector3 newDestination = guardedAreaCenter + direction * Random.Range(0, currentRoamingRange.magnitude);
            HGDebug.Log($"{personality.name} calculated new destination: {newDestination}", personality, debugging);
            return newDestination;
        }
    }
}