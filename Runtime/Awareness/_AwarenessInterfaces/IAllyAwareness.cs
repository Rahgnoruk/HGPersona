using HyperGnosys.Core;

namespace HyperGnosys.PersonaModule
{
    public interface IAllyAwareness
    {
        ListWrapper<BodyTag> PerceivedAllies { get; }
    }
}