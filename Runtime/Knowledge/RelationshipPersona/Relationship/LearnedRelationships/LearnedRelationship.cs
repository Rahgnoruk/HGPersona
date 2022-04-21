using HyperGnosys.Persona;
using UnityEngine;

namespace HyperGnosys.PersonaModule
{
    public class LearnedRelationship
    {
        [SerializeField] private LearningMethod learningMethod;
        [SerializeField] private ScriptableRelationship learnedScriptableRelationship;

        public LearnedRelationship(LearningMethod learningMethod, ScriptableRelationship learnedScriptableRelationship)
        {
            this.learningMethod = learningMethod;
            this.learnedScriptableRelationship = learnedScriptableRelationship;
        }

        public LearningMethod LearningMethod { get => learningMethod; set => learningMethod = value; }
        public ScriptableRelationship LearnedScriptableRelationship { get => learnedScriptableRelationship; set => learnedScriptableRelationship = value; }
    }
}