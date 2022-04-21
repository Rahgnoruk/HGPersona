using HyperGnosys.Core;
using HyperGnosys.PersonaModule;
using System.Collections.Generic;
using UnityEngine;

namespace HyperGnosys.PersonalityModule.Factions
{
    public class FactionTag : MonoBehaviour, IModuleComponent
    {
        [SerializeField] private ListWrapper<Faction> factionAllegiances = new ListWrapper<Faction>();
        private ListWrapper<Faction> resultingAllyFactions = new ListWrapper<Faction>();
        private ListWrapper<Faction> resultingEnemyFactions = new ListWrapper<Faction>();
        private void OnValidate()
        {
            CalculateAllegiancesAndEnmities();
        }
        private void Awake()
        {
            factionAllegiances.OnItemAdded.AddListener(CalculateNewFactionAllegiance);
            factionAllegiances.OnItemRemoved.AddListener(CalculateAbandonedFaction);
        }
        private void CalculateNewFactionAllegiance(Faction newFaction)
        {
            if (IsContradictoryAllegiance(newFaction))
            {
                RemoveContradictoryAllegiance(newFaction);
            }
            AdoptAllegiances(newFaction);
        }
        private void CalculateAbandonedFaction(Faction abandonedFaction)
        {
            CalculateAllegiancesAndEnmities();
        }
        private void CalculateAllegiancesAndEnmities()
        {
            resultingAllyFactions.Clear();
            resultingEnemyFactions.Clear();
            List<Faction> contradictoryAllegiances = new List<Faction>();
            foreach (Faction factionAllegiance in factionAllegiances.List)
            {
                if (IsContradictoryAllegiance(factionAllegiance))
                {
                    contradictoryAllegiances.Add(factionAllegiance);
                    continue;
                }
                AdoptAllegiances(factionAllegiance);
            }
            foreach(Faction contradictoryAllegiance in contradictoryAllegiances)
            {
                RemoveContradictoryAllegiance(contradictoryAllegiance);
            }
        }
        private void AdoptAllegiances(Faction newFaction)
        {
            foreach (Faction alliedFaction in newFaction.AlliedFactions.List)
            {
                resultingAllyFactions.Add(alliedFaction);
            }
            foreach (Faction enemyFaction in newFaction.EnemyFactions.List)
            {
                resultingEnemyFactions.Add(enemyFaction);
            }
        }
        private bool IsContradictoryAllegiance(Faction factionAllegiance)
        {
            foreach (Faction alliedFaction in factionAllegiance.AlliedFactions.List)
            {
                if (resultingEnemyFactions.Contains(alliedFaction))
                {
                    return true;
                }
            }
            foreach(Faction enemyFaction in factionAllegiance.EnemyFactions.List)
            {
                if (resultingAllyFactions.Contains(enemyFaction))
                {
                    return true;
                }
            }
            return false;
        }
        private void RemoveContradictoryAllegiance(Faction contradictoryAllegiance)
        {
            Debug.LogWarning($"{contradictoryAllegiance.name} can't be added to this faction tag because " +
                    $"it's an enemy of one or more of the other Factions.", this);
            factionAllegiances.Remove(contradictoryAllegiance);
        }
        private void OnDisable()
        {
            factionAllegiances.OnItemAdded.RemoveListener(CalculateNewFactionAllegiance);
            factionAllegiances.OnItemRemoved.RemoveListener(CalculateAbandonedFaction);
        }
        public ListWrapper<Faction> FactionAllegiances { get => factionAllegiances; set => factionAllegiances = value; }
        public ListWrapper<Faction> ResultingAllyFactions { get => resultingAllyFactions; set => resultingAllyFactions = value; }
        public ListWrapper<Faction> ResultingEnemyFactions { get => resultingEnemyFactions; set => resultingEnemyFactions = value; }
    }
}