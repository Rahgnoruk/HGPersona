using HyperGnosys.Core;
using UnityEngine;

namespace HyperGnosys.BehaviourLibrary
{
    public interface ISingleTargetSelection
    {
        BodyTag SelectTarget(Transform origin, ObservableList<BodyTag> perceivedEnemies);
    }
}