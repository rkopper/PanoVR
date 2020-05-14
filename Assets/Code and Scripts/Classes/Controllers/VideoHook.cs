using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using PermissionEnum;

namespace Prototype.NetworkLobby
{
    // Subclass this and redefine the function you want
    // then add it to the lobby prefab
    public class VideoHook: LobbyHook
    {
        public override void OnLobbyServerSceneLoadedForPlayer(NetworkManager manager, GameObject lobbyPlayer, GameObject gamePlayer)
        {
            gamePlayer.GetComponent<ClientScript>().playerColor = lobbyPlayer.GetComponent<LobbyPlayer>().playerColor;
            gamePlayer.GetComponent<ClientScript>().userName = lobbyPlayer.GetComponent<LobbyPlayer>().playerName;
            gamePlayer.GetComponent<ClientScript>().filePath = lobbyPlayer.GetComponent<LobbyPlayer>().filePath;
            gamePlayer.GetComponent<ClientScript>().annotationPath = lobbyPlayer.GetComponent<LobbyPlayer>().annotationPath;
            gamePlayer.GetComponent<ClientScript>().annotationSavePath = lobbyPlayer.GetComponent<LobbyPlayer>().annotationSavePath;
            gamePlayer.GetComponent<ClientScript>().permissionLevel = (PermissionCategories) lobbyPlayer.GetComponent<LobbyPlayer>().permissionStatus;
            gamePlayer.GetComponent<ClientScript>().configPath = lobbyPlayer.GetComponent<LobbyPlayer>().configPath;
            gamePlayer.GetComponent<ClientScript>().playFeat = lobbyPlayer.GetComponent<LobbyPlayer>().playFeature;
            gamePlayer.GetComponent<ClientScript>().chatFeat = lobbyPlayer.GetComponent<LobbyPlayer>().chatFeature;
            gamePlayer.GetComponent<ClientScript>().lookFeat = lobbyPlayer.GetComponent<LobbyPlayer>().lookFeature;
            gamePlayer.GetComponent<ClientScript>().drawFeat = lobbyPlayer.GetComponent<LobbyPlayer>().drawFeature;
        }
    }

}
