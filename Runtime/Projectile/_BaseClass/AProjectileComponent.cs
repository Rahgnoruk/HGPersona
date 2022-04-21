using System;
using UnityEngine;

namespace HyperGnosys.BehaviourLibrary
{
    [Serializable]
    public abstract class AProjectileComponent : MonoBehaviour
    {
        private Vector3 targetPosition = Vector3.zero;
        public abstract void Init();
        public abstract void Fly();
        public Vector3 TargetPosition { get => targetPosition; set => targetPosition = value; }
    }
}