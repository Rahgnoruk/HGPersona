using HyperGnosys.Core;
using System;
using System.Collections.Generic;
using UnityEngine;
namespace HyperGnosys.Persona
{
    [Serializable]
    public class RelationshipPersona
    {
        [SerializeField] private RelationshipGroup likes;
        [SerializeField] private RelationshipGroup dislikes;
        [SerializeField] private RelationshipGroup allegiances;
        [SerializeField] private RelationshipGroup enmities;

        private Dictionary<RelationshipPersonaComponent, PersonalRelation> personalRelations =
            new Dictionary<RelationshipPersonaComponent, PersonalRelation>();

        private AObservableListComponent<RelationshipPersonaComponent> knownAllies;
        private AObservableListComponent<RelationshipPersonaComponent> knownEnemies;

        public void Init()
        {
            likes.AddOnItemAddedListener(OnRelationshipsChanged);

            dislikes.AddOnItemAddedListener(OnRelationshipsChanged);

            allegiances.AddOnItemAddedListener(OnRelationshipsChanged);

            enmities.AddOnItemAddedListener(OnRelationshipsChanged);
        }

        public void OnRelationshipsChanged(ScriptableRelationship item)
        {
            foreach (RelationshipPersonaComponent otherPersona in personalRelations.Keys)
            {
                personalRelations[otherPersona].UpdateRelation();
            }
        }
        public RelationshipGroup Likes { get => likes; set => likes = value; }
        public RelationshipGroup Dislikes { get => dislikes; set => dislikes = value; }
        public RelationshipGroup Allegiances { get => allegiances; set => allegiances = value; }
        public RelationshipGroup Enmities { get => enmities; set => enmities = value; }
        public Dictionary<RelationshipPersonaComponent, PersonalRelation> KnownPersonas { get => personalRelations; set => personalRelations = value; }
        public AObservableListComponent<RelationshipPersonaComponent> KnownAllies { get => knownAllies; set => knownAllies = value; }
        public AObservableListComponent<RelationshipPersonaComponent> KnownEnemies { get => knownEnemies; set => knownEnemies = value; }
    }
}