using System;
using Darkhorizon.Shared.Lobby.Protocol.Commands;
using Darkhorizon.Shared.Lobby.Protocol.Events;
using Darkhorizon.Shared.Party.Dtos;
using Darkhorizon.Shared.Party.Protocol.Commands;
using Darkhorizon.Shared.Party.Protocol.Events;
using Gateways.Interfaces;

namespace UseCases.Menu
{
    public class SearchGameUseCase
    {
        private readonly ILobbyGateway _lobbyGateway;

        public SearchGameUseCase(ILobbyGateway lobbyGateway)
        {
            _lobbyGateway = lobbyGateway;
        }

        public void JoinParty(string serverUri, PartyTicketDto ticket, Action<JoinPartySucceedEvent> onSucceed = null)
        {
            _lobbyGateway.Send(new JoinPartyCommand {Ticket = ticket});
            _lobbyGateway.EventHub.Subscribe<JoinPartySucceedEvent>(OnSucceed);

            void OnSucceed(JoinPartySucceedEvent @event)
            {
                _lobbyGateway.EventHub.Unsubscribe<JoinPartySucceedEvent>(OnSucceed);
                onSucceed?.Invoke(@event);
            }
        }

        public void StartMatchmaking(Action<PartyInviteReceivedEvent> onSucceed = null)
        {
            //TODO: Deck ID.
            _lobbyGateway.Send(new StartMatchmakingCommand {DeckId = 0});
            _lobbyGateway.EventHub.Subscribe<PartyInviteReceivedEvent>(OnSucceed);

            void OnSucceed(PartyInviteReceivedEvent @event)
            {
                _lobbyGateway.EventHub.Unsubscribe<PartyInviteReceivedEvent>(OnSucceed);
                onSucceed?.Invoke(@event);
            }
        }
    }
}