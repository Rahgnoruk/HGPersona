using HyperGnosys.Core;
using UnityEngine;

namespace HyperGnosys.Persona
{
    [CreateAssetMenu(fileName = "New Faction", menuName = "HyperGnosys/Personality Module/Faction")]
    public class Faction : ScriptableObject
    {
        [SerializeField] private ObservableList<Faction> alliedFactions = new ObservableList<Faction>();
        [SerializeField] private ObservableList<Faction> enemyFactions = new ObservableList<Faction>();
        public ObservableList<Faction> EnemyFactions { get => enemyFactions; }
        public ObservableList<Faction> AlliedFactions { get => alliedFactions; }
    }
}