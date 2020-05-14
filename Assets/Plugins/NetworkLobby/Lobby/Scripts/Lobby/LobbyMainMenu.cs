using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Prototype.NetworkLobby
{
    //Main menu, mainly only a bunch of callback called by the UI (setup throught the Inspector)
    public class LobbyMainMenu : MonoBehaviour 
    {
        public LobbyManager lobbyManager;

        public RectTransform lobbyServerList;
        public RectTransform lobbyPanel;

        // Isaac implementation
        public RectTransform createLobbyPanel;
        public RectTransform settingsPanel;
        public RectTransform advancedPanel;

        public void OnEnable()
        {
            lobbyManager.topPanel.ToggleVisibility(true);
        }

        public void OnClickBack()
        {
            // Fix this....
            // i.e. Make sure that players are kicked, settings are reset, etc.
            lobbyManager.topPanel.GetComponentInChildren<Text>().text = "PanoVR \uF21D";
        }

        public void OnClickSettings()
        {
            lobbyManager.ChangeTo(settingsPanel);
            lobbyManager.topPanel.GetComponentInChildren<Text>().text = "About / Settings";
        }

        public void OnClickHost()
        {
            lobbyManager.StartHost();
        }

        public void OnClickJoin()
        {
            lobbyManager.ChangeTo(lobbyPanel);
            lobbyManager.StartClient();

            lobbyManager.backDelegate = lobbyManager.StopClientClbk;
            lobbyManager.DisplayIsConnecting();

            lobbyManager.SetServerInfo("Connecting...", lobbyManager.networkAddress);
        }

        public void OnClickDedicated()
        {
            lobbyManager.ChangeTo(null);
            lobbyManager.StartServer();

            lobbyManager.backDelegate = lobbyManager.StopServerClbk;

            lobbyManager.SetServerInfo("Dedicated Server", lobbyManager.networkAddress);
        }

        public void OnClickCreateNewLobby()
        {
            // Open create server menu.
            //lobbyManager.topPanel.GetComponentInChildren<Text>().text = "PanoVR \uF21D";
            lobbyManager.topPanel.GetComponentInChildren<Text>().text = "Lobby Settings";
            lobbyManager.backDelegate = lobbyManager.SimpleBackClbk;
            lobbyManager.ChangeTo(createLobbyPanel);
            // TODO: Create a back button, change the top label.

            /*if (VideoInput.text != "")
            {
                errorField.enabled = false;
                lobbyManager.StartMatchMaker();
                lobbyManager.matchMaker.CreateMatch(
                    matchNameInput.text,
                    (uint)lobbyManager.maxPlayers,
                    true,
                    "", "", "", 0, 0,
                    lobbyManager.OnMatchCreate);

                lobbyManager.backDelegate = lobbyManager.StopHost;
                lobbyManager._isMatchmaking = true;
                lobbyManager.DisplayIsConnecting();

                lobbyManager.SetServerInfo("Matchmaker Host", lobbyManager.matchHost);
            }
            else
            {
                errorField.enabled = true;
            }*/
        }

        public void OnClickOpenServerList()
        {
            lobbyManager.topPanel.GetComponentInChildren<Text>().text = "Server List";
            lobbyManager.StartMatchMaker();
            lobbyManager.backDelegate = lobbyManager.SimpleBackClbk;
            lobbyManager.ChangeTo(lobbyServerList);

        }


    }
}
