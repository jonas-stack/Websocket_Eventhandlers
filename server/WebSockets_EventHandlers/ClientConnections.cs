using Fleck;

namespace WebSockets_EventHandlers;

public class ClientConnectionsState
{
    public List<IWebSocketConnection> ClientConnections { get; set; } = new List<IWebSocketConnection>() { };
}