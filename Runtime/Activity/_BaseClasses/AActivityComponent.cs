using HyperGnosys.PersonaModule;
using UnityEngine;

namespace HyperGnosys.PersonalityModule
{
    public abstract class AActivityComponent : MonoBehaviour, IActivity
    {
        public abstract void ActivityStart();
        public abstract void ActivityUpdate();
        public abstract void ActivityEnd();
    }
}