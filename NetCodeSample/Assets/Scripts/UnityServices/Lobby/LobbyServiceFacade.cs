using System.Threading.Tasks;
using System;
using Unity.Services.Authentication;
using Unity.Services.Lobbies.Models;
using System.Collections.Generic;
using UnityEngine;

public class LobbyServiceFacade
{
    LobbyServiceFacade(LobbyAPIInterface lobbyAPIInterface)
    {
        _interface = lobbyAPIInterface;
    }

    LobbyAPIInterface _interface;
    LocalLobbyUser _localUser;

    public event Action<Lobby> onLobbyCreated;

    public void CreateLobby(string lobbyName, int maxPlayer, bool isPrivate)
    {
        Task<Lobby>.Run(async () =>
        {
            Lobby lobby = null;
            try
            {
                lobby = await _interface.CreateLobbyAsync(lobbyName, maxPlayer, isPrivate, null,
                                        AuthenticationService.Instance.PlayerId,
                                        new Dictionary<string, PlayerDataObject>()
                                        {
                                            { "DisplayName", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, _localUser.displayName) }
                                        });
            }
            catch (Exception e)
            {
                Debug.LogWarning(e);
            }
            finally
            {
                if (lobby != null)
                {
                    onLobbyCreated?.Invoke(lobby);
                }
            }
        });
    }
}
