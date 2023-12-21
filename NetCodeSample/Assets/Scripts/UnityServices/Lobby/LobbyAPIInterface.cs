using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;

public class LobbyAPIInterface
{
    public async Task<Lobby> CreateLobbyAsync(string lobbyName, 
                                              int maxPlayer,
                                              bool isPrivate,
                                              Dictionary<string, DataObject> lobbyData,
                                              string hostID,
                                              Dictionary<string, PlayerDataObject> hostData)
    {
        CreateLobbyOptions options = new CreateLobbyOptions
        {
            Data = lobbyData,
            IsLocked = true,
            IsPrivate = isPrivate,
            Player = new Player(id: hostID, data: hostData),
        };

        return await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayer, options);
    }



}