using System.Collections.Generic;
using UnityEngine;

namespace HyperGnosys.Persona
{
    public class Concept
    {
        [SerializeField] private int completixy = 0;
        [SerializeField] private HashSet<ScriptableConcept> precedingConcepts = new HashSet<ScriptableConcept>();
        [SerializeField] private HashSet<ScriptableConcept> succeedingConcepts = new HashSet<ScriptableConcept>();

        public int Completixy { get => completixy; set => completixy = value; }
        public HashSet<ScriptableConcept> PrecedingConcepts { get => precedingConcepts; set => precedingConcepts = value; }
        public HashSet<ScriptableConcept> SucceedingConcepts { get => succeedingConcepts; set => succeedingConcepts = value; }
    }
}