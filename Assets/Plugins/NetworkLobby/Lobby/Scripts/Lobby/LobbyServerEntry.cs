using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.Networking.Types;
using System.Collections;

namespace Prototype.NetworkLobby
{
    public class LobbyServerEntry : MonoBehaviour 
    {
        public Text serverInfoText;
        public Text slotInfo;
        public Text pwProtected;
        public Button joinButton;

        public string submittedPW;

		public void Populate(MatchInfoSnapshot match, LobbyManager lobbyManager, Color c)
		{
            serverInfoText.text = match.name;
            slotInfo.text = match.currentSize.ToString() + "/" + match.maxSize.ToString();
            pwProtected.text = match.isPrivate ? "\uf023" : "";

            NetworkID networkID = match.networkId;

            joinButton.onClick.RemoveAllListeners();
            joinButton.onClick.AddListener(() => { JoinMatch(match, lobbyManager); });

            GetComponent<Image>().color = c;
        }

        void JoinMatch(MatchInfoSnapshot match, LobbyManager lobbyManager)
        {
            // Fraught w/ Issues, shouldn't use PW rn, todo:
            // If password is incorrect, tell the user.
            // PW length validation.

            // If there's a PW, ask for a PW
            if (!match.isPrivate | !submittedPW.Equals(""))
            {
                NetworkID networkID = match.networkId;
                
                lobbyManager.matchMaker.JoinMatch(networkID, match.isPrivate ? submittedPW : "", "", "", 0, 0, lobbyManager.OnMatchJoined);
                lobbyManager.backDelegate = lobbyManager.StopClientClbk;
                lobbyManager._isMatchmaking = true;
                lobbyManager.DisplayIsConnecting();
                // wait until we get the password to join

            } else
            {
                lobbyManager.DisplayEnterPassword(this);
                StartCoroutine(WaitForPW(match, lobbyManager));
                
            }

        }

        private IEnumerator WaitForPW(MatchInfoSnapshot match, LobbyManager lobbyManager)
        {
            while (submittedPW.Equals(""))
            {
                yield return new WaitForEndOfFrame();
            }
            JoinMatch(match, lobbyManager);
        }
    }
}