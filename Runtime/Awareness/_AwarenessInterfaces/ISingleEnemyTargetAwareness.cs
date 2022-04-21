using HyperGnosys.Core;

namespace HyperGnosys.PersonaModule
{
    public interface ISingleEnemyTargetAwareness
    {
        BodyTag TargetEnemy { get; }
    }
}