using HyperGnosys.Core;

namespace HyperGnosys.PersonaModule
{
    public interface IEnemyAwareness
    {
        ListWrapper<BodyTag> PerceivedEnemies { get; }
    }
}