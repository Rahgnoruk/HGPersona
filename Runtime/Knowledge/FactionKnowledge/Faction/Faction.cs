using HyperGnosys.Core;
using UnityEngine;

namespace HyperGnosys.PersonaModule
{
    [CreateAssetMenu(fileName = "New Faction", menuName = "HyperGnosys/Personality Module/Faction")]
    public class Faction : ScriptableObject
    {
        [SerializeField] private ListWrapper<Faction> alliedFactions = new ListWrapper<Faction>();
        [SerializeField] private ListWrapper<Faction> enemyFactions = new ListWrapper<Faction>();
        public ListWrapper<Faction> EnemyFactions { get => enemyFactions; }
        public ListWrapper<Faction> AlliedFactions { get => alliedFactions; }
    }
}