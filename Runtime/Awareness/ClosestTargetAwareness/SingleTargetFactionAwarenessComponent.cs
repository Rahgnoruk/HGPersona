using HyperGnosys.BehaviourLibrary;
using HyperGnosys.Core;
using UnityEngine;

namespace HyperGnosys.PersonaModule
{
    public class SingleTargetFactionAwarenessComponent : MonoBehaviour, ISingleEnemyTargetAwareness
    {
        [SerializeField] private Transform thisAwarenessesBody;
        [SerializeField] private FactionAwarenessComponent factionAwareness;
        [SerializeField] private ISingleTargetSelectionReference targetSelection;

        private BodyTag targetEnemy;

        private void Awake()
        {
            factionAwareness.PerceivedEnemies.OnItemAdded.AddListener(SelectClosestTarget);
            factionAwareness.PerceivedEnemies.OnItemRemoved.AddListener(SelectClosestTarget);
        }
        private void SelectClosestTarget(BodyTag body)
        {
            if (factionAwareness.PerceivedEnemies.Count <= 0)
            {
                targetEnemy = null;
                return;
            }
            targetEnemy = targetSelection.Reference.SelectTarget(thisAwarenessesBody, factionAwareness.PerceivedEnemies);
        }
        public BodyTag TargetEnemy { get => targetEnemy; }
    }
}