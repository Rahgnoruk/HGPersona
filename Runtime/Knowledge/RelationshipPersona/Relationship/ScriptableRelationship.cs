using UnityEngine;
namespace HyperGnosys.Persona
{
    [CreateAssetMenu(fileName = "New Relationship", menuName = "HyperGnosys/Persona Module/Relationship")]
    public class ScriptableRelationship : ScriptableObject
    {
        [SerializeField] private Relationship relationship;

        public Relationship Relationship { get => relationship; set => relationship = value; }
    }
}