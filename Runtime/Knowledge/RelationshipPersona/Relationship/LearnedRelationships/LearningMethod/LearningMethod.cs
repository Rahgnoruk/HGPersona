using UnityEngine;

namespace HyperGnosys.PersonaModule
{
    [CreateAssetMenu(fileName = "New Learning Method", menuName = "HyperGnosys/Persona Module/Learning Method")]
    public class LearningMethod : ScriptableObject
    {
        [Range(0,100)]
        private int priority = 0;

        public int Priority { get => priority; set => priority = value; }
    }
}