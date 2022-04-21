using HyperGnosys.Core;
using HyperGnosys.PersonaModule;
using System.Collections.Generic;
using System.Linq;

namespace HyperGnosys.Persona
{
    public class PersonalRelation
    {
        private RelationshipPersonaComponent thisPersonaComponent;
        private RelationshipPersona thisPersona;

        private RelationshipGroup thisPersonasAllegiances;
        private RelationshipGroup thisPersonasEnmities;



        private RelationshipPersonaComponent otherPersonaComponent;
        private RelationshipPersona otherPersona;

        private LearnedRelationships otherPersonaKnownAllegiances;

        private LearnedRelationships otherPersonaKnownEnmities;

        private int allyScore = 0;
        private int enemyScore = 0;
        private RelationType allyRelationType;
        private RelationType enemyRelationType;
        private RelationType currentRelationType;

        public PersonalRelation(RelationshipPersonaComponent thisPersonaComponent, RelationshipPersonaComponent otherPersonaComponent,
            LearnedRelationships otherPersonaKnownAllegiances, LearnedRelationships otherPersonaKnownEnmities,
            RelationType allyRelationType, RelationType enemyRelationType)
        {
            this.thisPersonaComponent = thisPersonaComponent;
            thisPersona = thisPersonaComponent.Value;
            thisPersonasAllegiances = thisPersona.Allegiances;
            thisPersonasEnmities = thisPersona.Enmities;

            this.otherPersonaComponent = otherPersonaComponent;
            otherPersona = otherPersonaComponent.Value;
            this.otherPersonaKnownAllegiances.AddOnItemAddedListener(OnKnownAllegianceAdded);

            this.allyRelationType = allyRelationType;
            this.enemyRelationType = enemyRelationType;

            UpdateRelation();
        }

        private void OnKnownAllegianceAdded(LearnedRelationship learnedRelationship)
        {

        }
        private void OnKnownAllegianceRemoved(LearnedRelationship learnedRelationship)
        {

        }
        private void OnKnownEnmityAdded(LearnedRelationship learnedRelationship)
        {

        }
        private void OnKnownEnmityRemoved(LearnedRelationship learnedRelationship)
        {

        }
        /// <summary>
        /// Hay que procesar las relaciones detectadas para verificar el learning method. 
        /// Si simplemente se sobreescribieran, los learning methods no servirian.
        /// </summary>
        /// <param name="otherPersonaNewLearnedAllegiances"></param>
        public void ReceiveOtherPersonaDetectedAllegiances(LearnedRelationships otherPersonaNewLearnedAllegiances,
            LearningMethod learningMethod)
        {
            if (ReceiveDetectedRelationships(otherPersonaKnownAllegiances, otherPersonaNewLearnedAllegiances, learningMethod))
            {
                UpdateAllyScore();
            }
        }

        public void ReceiveOtherPersonaDetectedEnmities(LearnedRelationships otherPersonaNewLearnedEnmities,
            LearningMethod learningMethod)
        {
            if (ReceiveDetectedRelationships(otherPersonaKnownEnmities, otherPersonaNewLearnedEnmities, learningMethod))
            {
                UpdateAllyScore();
            }
        }

        private bool ReceiveDetectedRelationships(LearnedRelationships currentlyKnownRelationships,
            LearnedRelationships newLearnedRelationships, LearningMethod learningMethod)
        {
            ///Si las relaciones de la persona han cambiado, hay que actualizar 
            ///los scores de aliado y enemigo
            int knownRelationshipsCount = currentlyKnownRelationships.Count;

            ///Clona el hashset de las relaciones detectadas. Tienes que clonarlo para no editar directo el set
            List<LearnedRelationship> relationshipsToAdd
                = new List<LearnedRelationship>(newLearnedRelationships.List);
            ///Elimina todas las relaciones del HashSet de las ya conocidas, dejando solo las nuevas
            ///Si el hashset de las conocidas tiene elementos que no estan en las nuevas, no afecta.
            ///Solo se quedan las que SI estan en las nuevas pero no en las viejas
            //relationshipsToAdd.ExceptWith(currentlyKnownRelationships.List);

            ///Agrega las relaciones al set de las conocidas
            foreach (LearnedRelationship newRelationshipLearned in relationshipsToAdd)
            {
                currentlyKnownRelationships.Add(newRelationshipLearned);
            }
            ///Ahora clona el set de Relaciones Conocidas PERO ya con las nuevas relaciones agregadas
            List<LearnedRelationship> relationshipsToRemove
                = new List<LearnedRelationship>(currentlyKnownRelationships.List);
            ///Al hacer except con las nuevas relaciones detectadas con todo y las nuevas, lo que quedan son 
            ///las relaciones que si estan entre las Conocidas pero que no se detectaron con el Learning Method 
            ///que se esta procesando en este momento
            //relationshipsToRemove.ExceptWith(newLearnedRelationships.List);
            foreach (LearnedRelationship relationshipToRemove in relationshipsToRemove)
            {
                if (learningMethod.Priority >= relationshipToRemove.LearningMethod.Priority)
                {
                    currentlyKnownRelationships.Remove(relationshipToRemove);
                }
            }
            if (knownRelationshipsCount != currentlyKnownRelationships.Count)
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
                thisPersona.KnownAllies.Add(otherPersonaComponent);
                thisPersona.KnownEnemies.Remove(otherPersonaComponent);

            }
            else if (currentRelationType.Equals(enemyRelationType))
            {
                thisPersona.KnownEnemies.Add(otherPersonaComponent);
                thisPersona.KnownAllies.Remove(otherPersonaComponent);
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
            foreach (LearnedRelationship otherPersonasAllegiance in otherPersonaKnownAllegiances.List)
            {
                foreach (ScriptableRelationship thisPersonasAllegiance in thisPersonasAllegiances.List)
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
            foreach (LearnedRelationship otherPersonasEnmity in otherPersonaKnownEnmities.List)
            {
                foreach (ScriptableRelationship thisPersonasEnmity in thisPersonasEnmities.List)
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
            foreach (LearnedRelationship otherPersonasAllegiance in otherPersonaKnownAllegiances.List)
            {
                foreach (ScriptableRelationship thisPersonasEnmity in thisPersonasEnmities.List)
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
            foreach (LearnedRelationship otherPersonasEnmity in otherPersonaKnownEnmities.List)
            {
                foreach (ScriptableRelationship thisPersonasAllegiance in thisPersonasAllegiances.List)
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
        public LearnedRelationships OtherPersonaKnownAllegiances
        {
            get => otherPersonaKnownAllegiances;
            private set
            {
                otherPersonaKnownAllegiances = value;
            }
        }
        public LearnedRelationships OtherPersonaKnownEnmities
        {
            get => otherPersonaKnownEnmities;
            private set
            {
                otherPersonaKnownEnmities = value;
            }
        }
    }
}