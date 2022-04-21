using HyperGnosys.Core;
using UnityEngine;

namespace HyperGnosys.Persona
{
    [CreateAssetMenu(menuName = "HyperGnosys/Persona Module/Persona Scriptable List")]
    public class PersonaScriptableListWrapper 
        : AScriptableObservableList<RelationshipPersona>
    {
    }
}