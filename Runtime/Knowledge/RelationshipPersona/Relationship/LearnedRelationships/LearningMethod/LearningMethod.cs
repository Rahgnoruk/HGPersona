using UnityEngine;

namespace HyperGnosys.Persona
{
    [CreateAssetMenu(menuName = "HyperGnosys/Persona Module/Learning Method")]
    public class LearningMethod : ScriptableObject
    {
        [Range(0, 100)]
        private int priority = 0;

        public int Priority { get => priority; set => priority = value; }
    }
}