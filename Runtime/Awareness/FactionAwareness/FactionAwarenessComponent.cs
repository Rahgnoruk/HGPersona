using HyperGnosys.Core;
using HyperGnosys.PersonalityModule.Factions;
using HyperGnosys.PersonaModule;
using System.Collections.Generic;
using UnityEngine;

namespace HyperGnosys.Persona
{
    public class FactionAwarenessComponent : MonoBehaviour, IAllyAwareness, IEnemyAwareness
    {
        [SerializeField] private bool debugging = false;
        [SerializeField] private FactionTag thisAwarenessFactionTag;
        [SerializeField] private ObservableList<BodyTag> sensedObjects;
        /// <summary>
        /// Se puede tener una memoria para los Known Allies y Enemies para no volver a hacer el proceso de reconocimiento.
        /// Pero probablemente no vale la pena tener esa memoria en todas las personas, ni para todas las personas.
        /// Asi que se puede tener un estatus de VIP para que solo los VIP tengan memoria y solo recuerden otros VIPs.
        /// </summary>
        [SerializeField] private ObservableList<BodyTag> perceivedAllies = new ObservableList<BodyTag>();
        [SerializeField] private ObservableList<BodyTag> perceivedEnemies = new ObservableList<BodyTag>();
        private void Awake()
        {
            sensedObjects.AddOnItemAddedListener(DetectPersonas);
            sensedObjects.AddOnItemRemovedListener(UnDetectPersonas);
        }
        private void DetectPersonas(BodyTag detectedBody)
        {
            HGDebug.Log($"Receiving detected body {detectedBody.name}", this, debugging);
            List<MonoBehaviour> otherModuleComponents = detectedBody.ModuleComponentList.ModuleComponents;
            foreach (MonoBehaviour otherModuleComponent in otherModuleComponents)
            {
                if (!(otherModuleComponent is FactionTag)) continue;
                FactionTag otherPersonaFactionTag = (FactionTag)otherModuleComponent;
                HGDebug.Log("Detected other Persona", this, debugging);
                foreach (Faction otherPersonaAlly in otherPersonaFactionTag.ResultingAllyFactions.List)
                {
                    if (thisAwarenessFactionTag.ResultingEnemyFactions.Contains(otherPersonaAlly))
                    {
                        perceivedEnemies.Add(detectedBody);
                        perceivedAllies.Remove(detectedBody);
                        return;
                    }
                    if (thisAwarenessFactionTag.ResultingAllyFactions.Contains(otherPersonaAlly))
                    {
                        perceivedAllies.Add(detectedBody);
                    }
                }
                foreach (Faction otherPersonaEnemy in otherPersonaFactionTag.ResultingEnemyFactions.List)
                {
                    if (thisAwarenessFactionTag.ResultingAllyFactions.Contains(otherPersonaEnemy))
                    {
                        perceivedEnemies.Add(detectedBody);
                        perceivedAllies.Remove(detectedBody);
                        return;
                    }
                    if (thisAwarenessFactionTag.ResultingEnemyFactions.Contains(otherPersonaEnemy))
                    {
                        perceivedAllies.Add(detectedBody);
                    }
                }
            }
        }
        private void UnDetectPersonas(BodyTag detectedBody)
        {
            if (detectedBody == null) return;
            HGDebug.Log($"Undetecting detected body {detectedBody.name}", this, debugging);
            List<MonoBehaviour> detectedBodyModules = detectedBody.ModuleComponentList.ModuleComponents;
            foreach (MonoBehaviour moduleComponent in detectedBodyModules)
            {
                if (!(moduleComponent is FactionTag)) continue;
                perceivedAllies.Remove(detectedBody);
                perceivedEnemies.Remove(detectedBody);
                return;
            }
        }
        public ObservableList<BodyTag> PerceivedAllies { get => perceivedAllies; }
        public ObservableList<BodyTag> PerceivedEnemies { get => perceivedEnemies; }
        public ObservableList<BodyTag> SensedObjects { get => sensedObjects; set => sensedObjects = value; }
    }
}