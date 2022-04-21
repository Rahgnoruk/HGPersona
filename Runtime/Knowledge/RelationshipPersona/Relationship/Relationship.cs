using System;
using HyperGnosys.Core;
using UnityEngine;

namespace HyperGnosys.PersonaModule
{
    [Serializable]
    public class Relationship
    {
        [SerializeField] private ScriptableConcept concept;
        [SerializeField] private LearningMethod learningMethod;
        [SerializeField] private Property<int> affinity;
        [SerializeField] private Property<int> aversion;

        public ScriptableConcept Concept { get => concept; set => concept = value; }
        public LearningMethod LearningMethod { get => learningMethod; set => learningMethod = value; }
        public Property<int> Affinity { get => affinity; set => affinity = value; }
        public Property<int> Aversion { get => aversion; set => aversion = value; }
        public int ResultingAffinity { get => affinity.Value - aversion.Value; }
    }
}