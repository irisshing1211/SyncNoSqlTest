using System.Threading.Tasks;

namespace client.Events
{
    public interface IEvents
    {
       Task<bool> Push();
    }
}
