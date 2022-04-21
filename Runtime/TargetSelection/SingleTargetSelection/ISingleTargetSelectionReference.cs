using HyperGnosys.Core;
using System;
using UnityEngine;

namespace HyperGnosys.BehaviourLibrary
{
    [Serializable]
    public class ISingleTargetSelectionReference : ExternalReference<ISingleTargetSelection>, ISingleTargetSelection
    {
        public BodyTag SelectTarget(Transform origin, ObservableList<BodyTag> perceivedEnemies)
        {
            return Reference.SelectTarget(origin, perceivedEnemies);
        }
    }
}