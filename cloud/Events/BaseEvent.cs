using cloud.Entities;

namespace cloud.Events
{
    public class BaseEvent
    {
        internal readonly CloudServerContext Ctx;
        protected BaseEvent(CloudServerContext ctx) { Ctx = ctx; }
    }
}
