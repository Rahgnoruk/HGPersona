using UnityEngine;
namespace HyperGnosys.Persona
{
    [CreateAssetMenu(menuName = "HyperGnosys/Persona Module/Concept")]
    public class ScriptableConcept : ScriptableObject
    {
        [SerializeField] private Concept concept;

        public Concept Concept { get => concept; set => concept = value; }
    }
}