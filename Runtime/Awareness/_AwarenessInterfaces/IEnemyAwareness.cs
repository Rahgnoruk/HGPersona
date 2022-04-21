using HyperGnosys.Core;

namespace HyperGnosys.Persona
{
    public interface IEnemyAwareness
    {
        ObservableList<BodyTag> PerceivedEnemies { get; }
    }
}