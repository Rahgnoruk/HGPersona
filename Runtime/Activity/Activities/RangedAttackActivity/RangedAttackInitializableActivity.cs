using HyperGnosys.BehaviourLibrary;
using HyperGnosys.PersonaModule;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Object = UnityEngine.Object;
namespace HyperGnosys.PersonalityModule
{
    [Serializable]
    public class RangedAttackInitializableActivity : IActivity
    {
        [SerializeField] private AProjectileComponent projectilePrefab;
        [SerializeField] private float timeBetweenShots;
        [SerializeField] private Transform projectileSpawnPoint;
        [Space, Header("Animation Variables. If the attack doesn't have an animation you can ignore these.")]
        [SerializeField] private bool hasAnimation = false;
        [SerializeField] private Animator animator;
        [SerializeField] private string attackAnimationName;
        [Tooltip("If false, the projectile will be spawned using a Coroutine Countdown\n" +
            "If true, you have to manually hook SpawnProjectileOnAnimationFrame to your attack animation")]
        [SerializeField] private bool spawnWithAnimationEvent = false;
        [SerializeField] private float spawnProjectileSecond;
        private APersonality personality;
        private ISingleEnemyTargetAwareness persona;
        private NavMeshAgent agent;
        private int animationNameHash;
        public void Init(APersonality personality, ISingleEnemyTargetAwareness persona, NavMeshAgent agent)
        {
            this.personality = personality;
            this.persona = persona;
            this.agent = agent;
            animationNameHash = Animator.StringToHash(attackAnimationName);
        }
        public void ActivityStart()
        {
            animator.SetBool(animationNameHash, true);
            personality.StartCoroutine(SpawnProjectileOnFrame());
        }
        private IEnumerator SpawnProjectileOnFrame()
        {
            yield return new WaitForSeconds(spawnProjectileSecond);
            SpawnProjectileOnAnimationFrame();
        }
        public void SpawnProjectileOnAnimationFrame()
        {
            AProjectileComponent projectileInstance = Object.Instantiate(projectilePrefab, projectileSpawnPoint.position, projectileSpawnPoint.rotation);
            projectileInstance.TargetPosition = persona.TargetEnemy.ModuleComponents[0].transform.position;
            projectileInstance.Init();
            projectileInstance.Fly();
        }
        public void ActivityUpdate()
        {
        }
        public void ActivityEnd()
        {
            animator.SetBool(animationNameHash, false);
        }
    }
}