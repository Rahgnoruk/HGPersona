using UnityEngine;

namespace HyperGnosys.BehaviourLibrary
{
    public class NoGravityStraightProjectile : AProjectileComponent
    {
        [SerializeField] float forceMagnitude = 5f;
        Rigidbody projectileRigidbody;
        Vector3 directionVector = Vector3.zero;
        Vector3 forceVector = Vector3.zero;
        public override void Init()
        {
            projectileRigidbody.GetComponent<Rigidbody>();
            directionVector = this.TargetPosition - this.transform.position;
            forceVector = directionVector * forceMagnitude;
        }
        public override void Fly()
        {
            projectileRigidbody.AddForce(forceVector, ForceMode.VelocityChange);
        }
    }
}