using HyperGnosys.Core;
using System.Collections.Generic;

namespace HyperGnosys.PersonaModule
{
    public class Attitude
    {
        private APersonality personality;
        private IActivity activity;
        private bool debugging;
        private List<Belief> beliefs = new List<Belief>();

        public Attitude(APersonality personality, IActivity activity, bool debugging)
        {
            this.personality = personality;
            this.activity = activity;
            this.debugging = debugging;
        }

        public void SetBeliefs(List<Belief> beliefs)
        {
            this.beliefs = beliefs;
        }
        public virtual void OnAdoptAttitude()
        {
            HGDebug.Log($"Running activity {activity}", personality, debugging);
            activity.ActivityStart();
        }
        public virtual void React()
        {
            Attitude newAttitude = CheckForNewAttitude();
            if (newAttitude != null)
            {
                personality.ChangeAttitude(newAttitude);
                return;
            }
            activity.ActivityUpdate();
        }
        private Attitude CheckForNewAttitude()
        {
            foreach (Belief belief in beliefs)
            {
                if (belief.Condition.Evaluate() == belief.DesiredOutcome)
                {
                    HGDebug.Log($"Condition {belief.Condition} activated.\n" +
                        $"Switching to attitude with activity {belief.NewAttitude.Activity} " +
                        $"and {belief.NewAttitude.Beliefs.Count} beliefs", personality, debugging);
                    return belief.NewAttitude;
                }
            }
            return null;
        }
        public virtual void OnAbandonAttitude()
        {
            activity.ActivityEnd();
        }
        protected List<Belief> Beliefs { get => beliefs; set => beliefs = value; }
        public IActivity Activity { get => activity; protected set => activity = value; }
        public APersonality Personality { get => personality; set => personality = value; }
    }
}