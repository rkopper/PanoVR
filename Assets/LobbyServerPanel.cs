using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Prototype.NetworkLobby
{
    public class LobbyServerPanel : MonoBehaviour
    {

        public LobbyManager lobbyManager;
        public RectTransform lobbyServerPanel;
        public RectTransform advancedPanel;

        public void OnEnable()
        {
            lobbyManager.topPanel.ToggleVisibility(true);
        }

        public void OnClickSettings()
        {
            lobbyManager.ChangeTo(advancedPanel);
            //lobbyManager.backServerDelegate = lobbyManager.SimpleBackClbkServer;
            lobbyManager.topPanel.GetComponentInChildren<Text>().text = "Permission Settings";
        }
        public void OnClickBack()
        {

            // Fix this....
            // i.e. Make sure that players are kicked, settings are reset, etc.
            lobbyManager.topPanel.GetComponentInChildren<Text>().text = "Lobby Settings";
        }
    }
}
