using HyperGnosys.Core;
using UnityEngine;

namespace HyperGnosys.BehaviourLibrary
{
    [CreateAssetMenu(fileName ="Closest Single Target Selection", menuName ="HyperGnosys/Behaviour Library/Closest Single Target Selection")]
    public class ClosestTargetSelection : ScriptableObject, ISingleTargetSelection
    {
        public BodyTag SelectTarget(Transform origin, ObservableList<BodyTag> perceivedEnemies)
        {
            BodyTag closestEnemy = null;
            float closestDistance = int.MaxValue;
            foreach (BodyTag bodyTag in perceivedEnemies.List)
            {
                foreach (MonoBehaviour moduleComponent in bodyTag.ModuleComponents)
                {
                    Vector3 distanceVector = origin.position - moduleComponent.transform.position;
                    float distance = distanceVector.magnitude;
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestEnemy = bodyTag;
                    }
                }
            }
            return closestEnemy;
        }
    }
}