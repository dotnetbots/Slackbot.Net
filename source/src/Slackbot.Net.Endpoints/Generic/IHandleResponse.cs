using System.Threading.Tasks;
using Microsoft.Extensions.Primitives;

namespace Slackbot.Net.Endpoints.Generic
{
    public interface IHandleAllEvents
    {
        Task Handle(StringValues payload);
    }
}