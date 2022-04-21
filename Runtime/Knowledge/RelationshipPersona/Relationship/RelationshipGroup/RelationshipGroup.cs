using System;
using System.Collections.Generic;
using HyperGnosys.Core;

namespace HyperGnosys.PersonaModule
{
    [Serializable]
    public class RelationshipGroup 
        : Property<HashSetWrapper<ScriptableRelationship>>
    {
        public HashSetWrapper<ScriptableRelationship> HashSet
        {
            ///Value ya hace la evaluacion de si debe usar ScriptableVariable o no
            get => Value;
            set { }
        }
        /// <summary>
        /// List permite editar la lista sin ocasionar eventos
        /// </summary>
        public HashSet<ScriptableRelationship> Ledger
        {
            ///Value ya hace la evaluacion de si debe usar ScriptableVariable o no
            get => Value.HashSet;
            set { }
        }
    }
}