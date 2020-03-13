using client.Entities;

namespace client.Events
{
    public class BaseEvent
    {
        internal readonly ClientServerContext Ctx;
        protected BaseEvent(ClientServerContext ctx) { Ctx = ctx; }
    }
}
