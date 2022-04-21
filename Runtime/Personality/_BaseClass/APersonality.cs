using HyperGnosys.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HyperGnosys.PersonaModule
{
    public abstract class APersonality : MonoBehaviour
    {
        [SerializeField] private bool debugging = false;
        [SerializeField] private float timeBetweenTicks = 0.01f;

        private List<Belief> absoluteBeliefs = new List<Belief>();
        private Attitude currentAttitude;
        private Coroutine personalityCoroutine;
        private bool personalityIsRunning = false;
        protected virtual void Start()
        {
            Init();
        }
        protected abstract void Init();
        public void ChangeAttitude(Attitude newAttitude)
        {
            HGDebug.Log($"Changing attitude to {newAttitude.Activity}", this, debugging);
            StopRunningPersonality();
            if (currentAttitude != null)
            {
                currentAttitude.OnAbandonAttitude();
            }
            if (newAttitude == null) return;
            currentAttitude = newAttitude;
            currentAttitude.OnAdoptAttitude();
            personalityIsRunning = true;
            personalityCoroutine = StartCoroutine(RunAttitude());
        }
        private IEnumerator RunAttitude()
        {
            while (personalityIsRunning)
            {
                if (!IsAttitudeSet)
                {
                    HGDebug.Log("Tried to run an empty Attitude in this personality", this, debugging);
                    StopRunningPersonality();
                }
                Attitude newAttitude = CheckAbsoluteBeliefs();
                if (newAttitude != null)
                {
                    ChangeAttitude(newAttitude);
                }
                currentAttitude.React();
                yield return new WaitForSeconds(timeBetweenTicks);
            }
        }
        private Attitude CheckAbsoluteBeliefs()
        {
            foreach (Belief belief in absoluteBeliefs)
            {
                if (belief.Condition.Evaluate() == belief.DesiredOutcome)
                {
                    HGDebug.Log($"Absolute Belief {belief.Condition} Activated. " +
                        $"Changing to Activity {belief.NewAttitude.Activity}", this, debugging);
                    return belief.NewAttitude;
                }
            }
            return null;
        }
        public void StopRunningPersonality()
        {
            if (!personalityIsRunning || personalityCoroutine == null)
            {
                return;
            }
            StopCoroutine(personalityCoroutine);
            personalityIsRunning = false;
        }
        private bool IsAttitudeSet
        {
            get
            {
                if (currentAttitude == null)
                {
                    return false;
                }
                return true;
            }
        }
        private void OnDisable()
        {
            StopRunningPersonality();
        }

        protected Attitude CurrentAttitude { get => currentAttitude; }
        public List<Belief> AbsoluteBeliefs { get => absoluteBeliefs; set => absoluteBeliefs = value; }
        protected bool Debugging { get => debugging; set => debugging = value; }
    }
}