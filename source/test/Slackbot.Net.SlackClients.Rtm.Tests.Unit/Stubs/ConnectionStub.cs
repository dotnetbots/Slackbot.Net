using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Slackbot.Net.SlackClients.Rtm.EventHandlers;
using Slackbot.Net.SlackClients.Rtm.Models;

namespace Slackbot.Net.SlackClients.Rtm.Tests.Unit.Stubs
{
    public class ConnectionStub : IConnection
    {
        public string[] Aliases { get; set; }
        public IEnumerable<ChatHub> ConnectedDMs { get; }
        public IEnumerable<ChatHub> ConnectedChannels { get; }
        public IEnumerable<ChatHub> ConnectedGroups { get; }
        public IReadOnlyDictionary<string, User> UserNameCache { get; }
        public bool IsConnected { get; }
        public ContactDetails Team { get; }
        public ContactDetails Self { get; }
        public Task Connect(string slackKey)
        {
            throw new NotImplementedException();
        }

        public void Disconnect()
        {
            throw new NotImplementedException();
        }

        public event DisconnectEventHandler OnDisconnect;
        public void RaiseOnDisconnect()
        {
            OnDisconnect?.Invoke(Team?.Id, Team?.Name);
        }

        public event ReconnectEventHandler OnReconnecting;
        public void RaiseOnReconnecting()
        {
            OnReconnecting?.Invoke(Team?.Id, Team?.Name);
        }

        public event ReconnectEventHandler OnReconnect;
        public void RaiseOnReconnect()
        {
            if (Team?.Name == "always fail")
                OnReconnectFailure?.Invoke("stub", Team?.Id, Team?.Name);
            
            OnReconnect?.Invoke(Team?.Id, Team?.Name);
        }

        public event MessageReceivedEventHandler OnMessageReceived;
        public void RaiseOnMessageReceived()
        {
            OnMessageReceived?.Invoke(null);
        }

        public Task Ping()
        {
            throw new NotImplementedException();
        }

        public string SlackKey { get; }
        
       
        public event ReconnectFailureEventHandler OnReconnectFailure;

        public event PongEventHandler OnPong;

        public void RaiseOnPong()
        {
            OnPong?.Invoke(DateTime.MinValue);
        }

        public Task Close()
        {
            throw new NotImplementedException();
        }
    }
}
