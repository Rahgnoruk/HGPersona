using System;
using HyperGnosys.Core;
using UnityEngine;

namespace HyperGnosys.Persona
{
    [Serializable]
    public class Relationship
    {
        [SerializeField] private ScriptableConcept concept;
        [SerializeField] private LearningMethod learningMethod;
        [SerializeField] private ObservableProperty<int> affinity;
        [SerializeField] private ObservableProperty<int> aversion;

        public ScriptableConcept Concept { get => concept; set => concept = value; }
        public LearningMethod LearningMethod { get => learningMethod; set => learningMethod = value; }
        public ObservableProperty<int> Affinity { get => affinity; set => affinity = value; }
        public ObservableProperty<int> Aversion { get => aversion; set => aversion = value; }
        public int ResultingAffinity { get => affinity.Value - aversion.Value; }
    }
}