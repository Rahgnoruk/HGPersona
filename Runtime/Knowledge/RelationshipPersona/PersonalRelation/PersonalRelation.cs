using HyperGnosys.Core;
using System.Collections.Generic;
using System.Linq;

namespace HyperGnosys.PersonaModule
{
    public class PersonalRelation
    {
        private RelationshipPersonaComponent thisPersonaComponent;
        private RelationshipPersona thisPersona;
        
        private RelationshipGroup thisPersonasAllegiances;
        private RelationshipGroup thisPersonasEnmities;
        
        
        
        private RelationshipPersonaComponent otherPersonaComponent;
        private RelationshipPersona otherPersona;

        private LearnedRelationshipHashSetWrapper otherPersonaKnownAllegiances;
        private GameEventListener<LearnedRelationship> onKnownAllegianceAdded 
            = new GameEventListener<LearnedRelationship>();
        private GameEventListener<LearnedRelationship> onKnownAllegianceRemoved
            = new GameEventListener<LearnedRelationship>();

        private LearnedRelationshipHashSetWrapper otherPersonaKnownEnmities;
        private GameEventListener<LearnedRelationship> onKnownEnmityAdded
            = new GameEventListener<LearnedRelationship>();
        private GameEventListener<LearnedRelationship> onKnownEnmityRemoved
            = new GameEventListener<LearnedRelationship>();

        private int allyScore = 0;
        private int enemyScore = 0;
        private RelationType allyRelationType;
        private RelationType enemyRelationType;
        private RelationType currentRelationType;

        
        public PersonalRelation(RelationshipPersonaComponent thisPersonaComponent, RelationshipPersonaComponent otherPersonaComponent,
            LearnedRelationshipHashSetWrapper otherPersonaKnownAllegiances, LearnedRelationshipHashSetWrapper otherPersonaKnownEnmities,
            RelationType allyRelationType, RelationType enemyRelationType)
        {
            this.thisPersonaComponent = thisPersonaComponent;
            this.thisPersona = thisPersonaComponent.Value;
            this.thisPersonasAllegiances = this.thisPersona.Allegiances;
            this.thisPersonasEnmities = this.thisPersona.Enmities;

            this.otherPersonaComponent = otherPersonaComponent;
            this.otherPersona = otherPersonaComponent.Value;
            this.otherPersonaKnownAllegiances.Value.OnItemAdded.AddListener(onKnownAllegianceAdded);

            this.allyRelationType = allyRelationType;
            this.enemyRelationType = enemyRelationType;

            UpdateRelation();
        }

        /// <summary>
        /// Hay que procesar las relaciones detectadas para verificar el learning method. 
        /// Si simplemente se sobreescribieran, los learning methods no servirian.
        /// </summary>
        /// <param name="otherPersonaNewLearnedAllegiances"></param>
        public void ReceiveOtherPersonaDetectedAllegiances(LearnedRelationshipHashSetWrapper otherPersonaNewLearnedAllegiances,
            LearningMethod learningMethod)
        {
            if (ReceiveDetectedRelationships(otherPersonaKnownAllegiances, otherPersonaNewLearnedAllegiances, learningMethod))
            {
                UpdateAllyScore();
            }
        }

        public void ReceiveOtherPersonaDetectedEnmities(LearnedRelationshipHashSetWrapper otherPersonaNewLearnedEnmities,
            LearningMethod learningMethod)
        {
            if (ReceiveDetectedRelationships(otherPersonaKnownEnmities, otherPersonaNewLearnedEnmities, learningMethod))
            {
                UpdateAllyScore();
            }
        }

        private bool ReceiveDetectedRelationships(LearnedRelationshipHashSetWrapper currentlyKnownRelationships, 
            LearnedRelationshipHashSetWrapper newLearnedRelationships, LearningMethod learningMethod)
        {
            ///Si las relaciones de la persona han cambiado, hay que actualizar 
            ///los scores de aliado y enemigo
            int knownRelationshipsCount = currentlyKnownRelationships.Value.Count();

            ///Clona el hashset de las relaciones detectadas. Tienes que clonarlo para no editar directo el set
            HashSet<LearnedRelationship> relationshipsToAdd
                = new HashSet<LearnedRelationship>(newLearnedRelationships.Value.HashSet);
            ///Elimina todas las relaciones del HashSet de las ya conocidas, dejando solo las nuevas
            ///Si el hashset de las conocidas tiene elementos que no estan en las nuevas, no afecta.
            ///Solo se quedan las que SI estan en las nuevas pero no en las viejas
            relationshipsToAdd.ExceptWith(currentlyKnownRelationships.Value.HashSet);

            ///Agrega las relaciones al set de las conocidas
            foreach (LearnedRelationship newRelationshipLearned in relationshipsToAdd)
            {
                currentlyKnownRelationships.Value.Add(newRelationshipLearned);
            }
            ///Ahora clona el set de Relaciones Conocidas PERO ya con las nuevas relaciones agregadas
            HashSet<LearnedRelationship> relationshipsToRemove
                = new HashSet<LearnedRelationship>(currentlyKnownRelationships.Value.HashSet);
            ///Al hacer except con las nuevas relaciones detectadas con todo y las nuevas, lo que quedan son 
            ///las relaciones que si estan entre las Conocidas pero que no se detectaron con el Learning Method 
            ///que se esta procesando en este momento
            relationshipsToRemove.ExceptWith(newLearnedRelationships.Value.HashSet);
            foreach (LearnedRelationship relationshipToRemove in relationshipsToRemove)
            {
                if (learningMethod.Priority >= relationshipToRemove.LearningMethod.Priority)
                {
                    currentlyKnownRelationships.Value.Remove(relationshipToRemove);
                }
            }
            if (knownRelationshipsCount != currentlyKnownRelationships.Value.Count())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void UpdateRelation()
        {
            currentRelationType = DetermineRelationship();

            if (currentRelationType.Equals(allyRelationType))
            {
                this.thisPersona.KnownAllies.Value.Add(otherPersonaComponent);
                this.thisPersona.KnownEnemies.Value.Remove(otherPersonaComponent);

            }else if (currentRelationType.Equals(enemyRelationType))
            {
                this.thisPersona.KnownEnemies.Value.Add(otherPersonaComponent);
                this.thisPersona.KnownAllies.Value.Remove(otherPersonaComponent);
            }
        }
        public RelationType DetermineRelationship()
        {
            if (allyScore > enemyScore)
            {
                return allyRelationType;
            }
            if (allyScore == enemyScore)
            {
                return null;
            }
            else
            {
                return enemyRelationType;
            }
        }

        public void UpdateAllyScore()
        {
            allyScore = 0;
            ///El amigo de mi amigo es mi amigo
            foreach (LearnedRelationship otherPersonasAllegiance in otherPersonaKnownAllegiances.Value.HashSet)
            {
                foreach (ScriptableRelationship thisPersonasAllegiance in thisPersonasAllegiances.Ledger)
                {
                    if (thisPersonasAllegiance.Relationship.Concept.Equals
                        (otherPersonasAllegiance.LearnedScriptableRelationship.Relationship.Concept))
                    {
                        allyScore += thisPersonasAllegiance.Relationship.ResultingAffinity
                            + otherPersonasAllegiance.LearnedScriptableRelationship.Relationship.ResultingAffinity;
                    }
                }
            }
            ///El enemigo de mi enemigo es mi amigo
            foreach (LearnedRelationship otherPersonasEnmity in otherPersonaKnownEnmities.Value.HashSet)
            {
                foreach (ScriptableRelationship thisPersonasEnmity in thisPersonasEnmities.Ledger)
                {
                    if (thisPersonasEnmity.Relationship.Concept.Equals
                        (otherPersonasEnmity.LearnedScriptableRelationship.Relationship.Concept))
                    {
                        allyScore += thisPersonasEnmity.Relationship.ResultingAffinity
                            + otherPersonasEnmity.LearnedScriptableRelationship.Relationship.ResultingAffinity;
                    }
                }
            }

            UpdateRelation();
        }

        public void UpdateEnemyScore()
        {
            enemyScore = 0;
            ///El aliado de mi enemigo es mi enemigo
            foreach (LearnedRelationship otherPersonasAllegiance in otherPersonaKnownAllegiances.Value.HashSet)
            {
                foreach (ScriptableRelationship thisPersonasEnmity in thisPersonasEnmities.Ledger)
                {
                    if (thisPersonasEnmity.Relationship.Concept.Equals
                        (otherPersonasAllegiance.LearnedScriptableRelationship.Relationship.Concept))
                    {
                        enemyScore += thisPersonasEnmity.Relationship.ResultingAffinity
                            + otherPersonasAllegiance.LearnedScriptableRelationship.Relationship.ResultingAffinity;
                    }
                }
            }
            ///El enemigo de mi aliado es mi enemigo
            foreach (LearnedRelationship otherPersonasEnmity in otherPersonaKnownEnmities.Value.HashSet)
            {
                foreach (ScriptableRelationship thisPersonasAllegiance in thisPersonasAllegiances.Ledger)
                {
                    if (thisPersonasAllegiance.Relationship.Concept.Equals
                        (otherPersonasEnmity.LearnedScriptableRelationship.Relationship.Concept))
                    {
                        enemyScore += thisPersonasAllegiance.Relationship.ResultingAffinity
                            + otherPersonasEnmity.LearnedScriptableRelationship.Relationship.ResultingAffinity;
                    }
                }
            }
            UpdateRelation();
        }

        public RelationType CurrentRelationType { get => currentRelationType; set => currentRelationType = value; }
        public LearnedRelationshipHashSetWrapper OtherPersonaKnownAllegiances
        {
            get => otherPersonaKnownAllegiances;
            private set
            {
                otherPersonaKnownAllegiances = value;
            }
        }
        public LearnedRelationshipHashSetWrapper OtherPersonaKnownEnmities
        {
            get => otherPersonaKnownEnmities;
            private set
            {
                otherPersonaKnownEnmities = value;
            }
        }
    }
}