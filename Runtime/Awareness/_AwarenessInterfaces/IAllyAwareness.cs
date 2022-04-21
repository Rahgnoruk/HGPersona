using HyperGnosys.Core;

namespace HyperGnosys.PersonaModule
{
    public interface IAllyAwareness
    {
        ObservableList<BodyTag> PerceivedAllies { get; }
    }
}