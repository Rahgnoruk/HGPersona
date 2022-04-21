namespace HyperGnosys.PersonaModule
{
    public class Belief
    {
        public ICondition Condition { get; }
        public bool DesiredOutcome { get; }
        public Attitude NewAttitude { get; }
        public Belief(Attitude newAttitude, ICondition condition, bool desiredOutcome)
        {
            NewAttitude = newAttitude;
            Condition = condition;
            DesiredOutcome = desiredOutcome;
        }
    }
}