using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace HyperGnosys.PersonalityModule
{
    public abstract class ACondition
    {
        public abstract bool Evaluate();
        public UnityEvent<bool> ConditionResult = new UnityEvent<bool>();
    }
}