using HyperGnosys.Core;
using HyperGnosys.PersonaModule;
using System;
using System.Collections.Generic;
using UnityEngine;
namespace HyperGnosys.Persona
{
    public class ProcessSensedPersonas : MonoBehaviour
    {
        [SerializeField] private RelationshipPersonaComponent thisAgentsPersonaComponent;
        [SerializeField] private LearningMethod sensoryLearningMethod;
        [SerializeField] private RelationType allyRelationType;
        [SerializeField] private RelationType enemyRelationType;
        [SerializeField] private ObservableList<GameObject> sensedAllies;
        [SerializeField] private ObservableList<GameObject> sensedEnemies;


        private RelationshipPersona thisAgentsPersona;




        private void OnEnable()
        {
            thisAgentsPersona = thisAgentsPersonaComponent.Value;

        }

        public void OnAddedInteractedTarget(GameObject target)
        {
            ///Verifica si el target si es una Persona
            RelationshipPersonaComponent otherPersonaComponent = target.transform.root.GetComponentInChildren<RelationshipPersonaComponent>();
            ///Si no es una persona, deja de procesarlo
            if (otherPersonaComponent == null)
            {
                return;
            }

            RelationshipPersona otherPersona = otherPersonaComponent.Value;
            LearnedRelationships otherPersonasDetectedAllegiances
                = FindRelationshipsThroughLearningMethod(otherPersona.Allegiances);

            LearnedRelationships otherPersonasDetectedEnmities
                = FindRelationshipsThroughLearningMethod(otherPersona.Enmities);

            PersonalRelation personalRelation;
            ///Si la persona no es conocida, crea la relacion
            if (!thisAgentsPersona.KnownPersonas.ContainsKey(otherPersonaComponent))
            {
                personalRelation = new PersonalRelation(otherPersonaComponent, thisAgentsPersonaComponent,
                    otherPersonasDetectedAllegiances, otherPersonasDetectedEnmities,
                    allyRelationType, enemyRelationType);
                thisAgentsPersona.KnownPersonas.Add(otherPersonaComponent, personalRelation);
            }
            ///Si la persona ya es conocida, obtenla de la lista
            else
            {
                personalRelation = thisAgentsPersona.KnownPersonas[otherPersonaComponent];
                ///Revisa si las lealtades y enemistades percibidas son las mismas que ya se conocian
                ///y corrige en caso de que no.
                personalRelation.ReceiveOtherPersonaDetectedAllegiances(otherPersonasDetectedAllegiances,
                    sensoryLearningMethod);
                personalRelation.ReceiveOtherPersonaDetectedEnmities(otherPersonasDetectedEnmities,
                    sensoryLearningMethod);
            }
            ///Revisa si la persona que estas viendo es aliado o enemigo e indicalo
            if (personalRelation.CurrentRelationType == allyRelationType)
            {
                sensedAllies.Add(otherPersonaComponent.gameObject);
            }
            else if (personalRelation.CurrentRelationType == enemyRelationType)
            {
                sensedEnemies.Add(otherPersonaComponent.gameObject);
            }
        }

        private LearnedRelationships FindRelationshipsThroughLearningMethod(RelationshipGroup otherPersonRelationships)
        {
            LearnedRelationships visibleRelationships = new LearnedRelationships();
            foreach (ScriptableRelationship relationship in otherPersonRelationships.List)
            {
                if (relationship.Relationship.LearningMethod.Equals(sensoryLearningMethod))
                {
                    visibleRelationships.Add(new LearnedRelationship(sensoryLearningMethod, relationship));
                }
            }
            return visibleRelationships;
        }

        public void OnRemovedInteractedTarget(GameObject target)
        {
            RelationshipPersonaComponent otherPersonaComponent = target.GetComponentInChildren<RelationshipPersonaComponent>();
            if (otherPersonaComponent == null)
            {
                return;
            }
            PersonalRelation personalRelation = thisAgentsPersona.KnownPersonas[otherPersonaComponent];
            ///Indica que el aliado o enemigo se esta dejando de percibir
            if (personalRelation.CurrentRelationType == allyRelationType)
            {
                sensedAllies.Remove(otherPersonaComponent.gameObject);
            }
            else if (personalRelation.CurrentRelationType == enemyRelationType)
            {
                sensedEnemies.Remove(otherPersonaComponent.gameObject);
            }
        }
        public RelationshipPersonaComponent ThisAgentsPersonaComponent { get => thisAgentsPersonaComponent; set => thisAgentsPersonaComponent = value; }
        public RelationshipPersona ThisAgentsPersona { get => thisAgentsPersona; set => thisAgentsPersona = value; }
        public RelationType AllyRelationType { get => allyRelationType; set => allyRelationType = value; }
        public RelationType EnemyRelationType { get => enemyRelationType; set => enemyRelationType = value; }
        public ObservableList<GameObject> SensedAllies { get => sensedAllies; set => sensedAllies = value; }
        public ObservableList<GameObject> SensedEnemies { get => sensedEnemies; set => sensedEnemies = value; }
        public LearningMethod SensoryLearningMethod { get => sensoryLearningMethod; set => sensoryLearningMethod = value; }
    }
}