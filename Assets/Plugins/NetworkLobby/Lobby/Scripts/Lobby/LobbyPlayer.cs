using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PermissionEnum;

namespace Prototype.NetworkLobby
{
    //Player entry in the lobby. Handle selecting color/setting name & getting ready for the game
    //Any LobbyHook can then grab it and pass those value to the game player prefab (see the Pong Example in the Samples Scenes)
    public class LobbyPlayer : NetworkLobbyPlayer
    {
        static Color[] Colors = new Color[] { Color.magenta, Color.red, Color.cyan, Color.blue, Color.green, Color.yellow };
        //used on server to avoid assigning the same color to two player
        static List<int> _colorInUse = new List<int>();
        public GameObject dataHolder;

        public Button colorButton;
        public InputField nameInput;
        public Button readyButton;
        public Button permissionButton;
        public Button waitingPlayerButton;
        public Button removePlayerButton;

        public GameObject localIcone;
        public GameObject remoteIcone;

        public ConfigHook configdropdown;
        GameObject about;

        public MasterFeaturesHook mh;

        //OnMyName function will be invoked on clients when server change the value of playerName
        [SyncVar(hook = "OnMyName")]
        public string playerName = "";
        [SyncVar(hook = "OnMyColor")]
        public Color playerColor = Color.white;
        [SyncVar(hook = "OnMyPermission")]
        public PermissionCategories permissionStatus;
        [SyncVar(hook = "OnMyFilePath")]
        public string filePath;
        //[SyncVar(hook = "OnMyAnnotationPath")]
        public string annotationPath;
        [SyncVar(hook = "OnMyAnnotationSavePath")]
        public string annotationSavePath;
        [SyncVar(hook = "OnMyPlayFeature")]
        public bool playFeature;
        [SyncVar(hook = "OnMyChatFeature")]
        public bool chatFeature;
        [SyncVar(hook = "OnMyDrawFeature")]
        public bool drawFeature;
        [SyncVar(hook = "OnMyLookFeature")]
        public bool lookFeature;

        //[SyncVar(hook = "OnMyConfigPath")]
        public string configPath;

        public Color OddRowColor = new Color(250.0f / 255.0f, 250.0f / 255.0f, 250.0f / 255.0f, 1.0f);
        public Color EvenRowColor = new Color(180.0f / 255.0f, 180.0f / 255.0f, 180.0f / 255.0f, 1.0f);

        static Color JoinColor = new Color(255.0f / 255.0f, 0.0f, 101.0f / 255.0f, 1.0f);
        static Color NotReadyColor = new Color(34.0f / 255.0f, 44 / 255.0f, 55.0f / 255.0f, 1.0f);
        static Color ReadyColor = new Color(0.0f, 204.0f / 255.0f, 204.0f / 255.0f, 1.0f);
        static Color TransparentColor = new Color(0, 0, 0, 0);

        //static Color OddRowColor = new Color(250.0f / 255.0f, 250.0f / 255.0f, 250.0f / 255.0f, 1.0f);
        //static Color EvenRowColor = new Color(180.0f / 255.0f, 180.0f / 255.0f, 180.0f / 255.0f, 1.0f);


        public override void OnClientEnterLobby()
        {
            base.OnClientEnterLobby();

            dataHolder = GameObject.Find("DataHolder");

            if (LobbyManager.s_Singleton != null) LobbyManager.s_Singleton.OnPlayersNumberModified(1);

            LobbyPlayerList._instance.AddPlayer(this);
            LobbyPlayerList._instance.DisplayDirectServerWarning(isServer && LobbyManager.s_Singleton.matchMaker == null);

            if (isLocalPlayer)
            {
                SetupLocalPlayer();
            }
            else
            {
                SetupOtherPlayer();
            }

            //setup the player data on UI. The value are SyncVar so the player
            //will be created with the right value currently on server
            OnMyName(playerName);
            OnMyColor(playerColor);
            OnMyPermission(permissionStatus);
            about = GameObject.Find("LobbyManager");
            configdropdown = about.GetComponent<ConfigHook>();
            configPath = configdropdown.config;
            OnMyConfigPath(configPath);
            mh = about.GetComponent<MasterFeaturesHook>();
            playFeature = mh.getPlay();
            chatFeature = mh.getChat();
            lookFeature = mh.getLook();
            drawFeature = mh.getDraw();
            OnMyPlayFeature(playFeature);
            OnMyChatFeature(chatFeature);
            OnMyLookFeature(lookFeature);
            OnMyDrawFeature(drawFeature);



        }

        public override void OnStartAuthority()
        {
            base.OnStartAuthority();

            //if we return from a game, color of text can still be the one for "Ready"
            readyButton.transform.GetChild(0).GetComponent<Text>().color = Color.white;

            SetupLocalPlayer();
        }

        void ChangeReadyButtonColor(Color c)
        {
            ColorBlock b = readyButton.colors;
            b.normalColor = c;
            b.pressedColor = c;
            b.highlightedColor = c;
            b.disabledColor = c;
            readyButton.colors = b;
        }

        void ChangePermissionButtonColor(Color c)
        {
            ColorBlock b = permissionButton.colors;
            b.normalColor = c;
            b.pressedColor = c;
            b.highlightedColor = c;
            b.disabledColor = c;
            permissionButton.colors = b;
        }

        void SetupOtherPlayer()
        {
            nameInput.interactable = false;
            removePlayerButton.interactable = NetworkServer.active;

            if (!isServer)
            {
                ChangePermissionButtonColor(Color.gray);
            }

            permissionButton.onClick.RemoveAllListeners();
            permissionButton.onClick.AddListener(OnPermissionChanged);

            ChangeReadyButtonColor(NotReadyColor);

            readyButton.transform.GetChild(0).GetComponent<Text>().text = "...";
            readyButton.interactable = false;

            OnClientReady(false);
        }

        void SetupLocalPlayer()
        {
            nameInput.interactable = true;
            remoteIcone.gameObject.SetActive(false);
            localIcone.gameObject.SetActive(true);

            CheckRemoveButton();

            if (playerColor == Color.white)
                CmdColorChange();

            ChangeReadyButtonColor(JoinColor);

            readyButton.transform.GetChild(0).GetComponent<Text>().text = "JOIN";
            readyButton.interactable = true;

            //have to use child count of player prefab already setup as "this.slot" is not set yet
            if (playerName == "")
                CmdNameChanged("Player" + (LobbyPlayerList._instance.playerListContentTransform.childCount - 1));

            //we switch from simple name display to name input
            colorButton.interactable = true;
            nameInput.interactable = true;

            nameInput.onEndEdit.RemoveAllListeners();
            nameInput.onEndEdit.AddListener(OnNameChanged);

            colorButton.onClick.RemoveAllListeners();
            colorButton.onClick.AddListener(OnColorClicked);

            permissionButton.onClick.RemoveAllListeners();
            permissionButton.onClick.AddListener(OnPermissionChanged);



            if (isServer)
            {
                print("happeningServer");
                print("before on my permission: " + permissionStatus);
                OnMyPermission(PermissionCategories.Master);
                if (isLocalPlayer)
                {
                    OnPermissionChanged();
                    print("true");
                }
                print("after on my permission: " + permissionStatus);
            }
            else if (isClient)
            {
                print("happeningClient");
                print("client: " + permissionStatus);
                OnMyPermission(PermissionCategories.User);
                print("client afer on my permisssion " + permissionStatus);
                if (isLocalPlayer)
                {
                    OnPermissionChanged();
                }
                ChangePermissionButtonColor(Color.gray);
                permissionButton.interactable = false;

            }

            readyButton.onClick.RemoveAllListeners();
            readyButton.onClick.AddListener(OnReadyClicked);

            //when OnClientEnterLobby is called, the loval PlayerController is not yet created, so we need to redo that here to disable
            //the add button if we reach maxLocalPlayer. We pass 0, as it was already counted on OnClientEnterLobby
            if (LobbyManager.s_Singleton != null) LobbyManager.s_Singleton.OnPlayersNumberModified(0);
        }

        //This enable/disable the remove button depending on if that is the only local player or not
        public void CheckRemoveButton()
        {
            if (!isLocalPlayer)
                return;

            int localPlayerCount = 0;
            foreach (PlayerController p in ClientScene.localPlayers)
                localPlayerCount += (p == null || p.playerControllerId == -1) ? 0 : 1;

            removePlayerButton.interactable = localPlayerCount > 1;
        }

        public override void OnClientReady(bool readyState)
        {
            if (readyState)
            {
                ChangeReadyButtonColor(TransparentColor);
                if (isLocalPlayer)
                {
                    //dataHolder.GetComponent<DataHolderScript>().SavePermission(permissionStatus);
                }
                Text textComponent = readyButton.transform.GetChild(0).GetComponent<Text>();
                textComponent.text = "READY";
                textComponent.color = ReadyColor;
                readyButton.interactable = false;
                colorButton.interactable = false;
                nameInput.interactable = false;
            }
            else
            {
                ChangeReadyButtonColor(isLocalPlayer ? JoinColor : NotReadyColor);

                Text textComponent = readyButton.transform.GetChild(0).GetComponent<Text>();
                textComponent.text = isLocalPlayer ? "JOIN" : "...";
                textComponent.color = Color.white;
                readyButton.interactable = isLocalPlayer;
                colorButton.interactable = isLocalPlayer;
                nameInput.interactable = isLocalPlayer;
            }
        }

        public void OnPlayerListChanged(int idx)
        {
            GetComponent<Image>().color = (idx % 2 == 0) ? EvenRowColor : OddRowColor;
        }

        ///===== callback from sync var

        public void OnMyName(string newName)
        {
            playerName = newName;
            nameInput.text = playerName;
        }

        public void OnMyColor(Color newColor)
        {
            playerColor = newColor;
            colorButton.GetComponent<Image>().color = newColor;
        }

        public void OnMyPermission(PermissionCategories permission)
        {
            permissionStatus = permission;
            permissionButton.GetComponentInChildren<Text>().text = permission.ToString();

        }

        public void OnMyFilePath(string fp)
        {
            filePath = fp;
        }

        public void OnMyPlayFeature(bool val)
        {
            playFeature = val;
        }
        public void OnMyChatFeature(bool val)
        {
            chatFeature = val;
        }
        public void OnMyDrawFeature(bool val)
        {
            drawFeature = val;
        }
        public void OnMyLookFeature(bool val)
        {
            lookFeature = val;
        }

        public void OnMyAnnotationPath(string ap)
        {
            annotationPath = ap;
        }

        public void OnMyAnnotationSavePath(string asp)
        {
            annotationSavePath = asp;
        }

        public void OnMyConfigPath(string cp)
        {
            configPath = cp;
            print("onmyconfigpath " + cp);
        }

        //===== UI Handler

        //Note that those handler use Command function, as we need to change the value on the server not locally
        //so that all client get the new value throught syncvar
        public void OnColorClicked()
        {
            CmdColorChange();
        }

        public void OnPermissionChanged()
        {
            CmdPermissionChange();
            print(permissionStatus);
        }

        public void OnReadyClicked()
        {
            SendReadyToBeginMessage();
        }

        public void OnNameChanged(string str)
        {
            CmdNameChanged(str);
        }

        public void OnRemovePlayerClick()
        {
            if (isLocalPlayer)
            {
                RemovePlayer();
            }
            else if (isServer)
                LobbyManager.s_Singleton.KickPlayer(connectionToClient);

        }

        public void ToggleJoinButton(bool enabled)
        {
            readyButton.gameObject.SetActive(enabled);
            waitingPlayerButton.gameObject.SetActive(!enabled);
        }

        [ClientRpc]
        public void RpcUpdateCountdown(int countdown)
        {
            LobbyManager.s_Singleton.countdownPanel.UIText.text = "Match Starting in " + countdown;
            LobbyManager.s_Singleton.countdownPanel.gameObject.SetActive(countdown != 0);
        }

        [ClientRpc]
        public void RpcUpdateRemoveButton()
        {
            CheckRemoveButton();
        }

        //====== Server Command

        [Command]
        public void CmdColorChange()
        {
            int idx = System.Array.IndexOf(Colors, playerColor);

            int inUseIdx = _colorInUse.IndexOf(idx);

            if (idx < 0) idx = 0;

            idx = (idx + 1) % Colors.Length;

            bool alreadyInUse = false;

            do
            {
                alreadyInUse = false;
                for (int i = 0; i < _colorInUse.Count; ++i)
                {
                    if (_colorInUse[i] == idx)
                    {//that color is already in use
                        alreadyInUse = true;
                        idx = (idx + 1) % Colors.Length;
                    }
                }
            }
            while (alreadyInUse);

            if (inUseIdx >= 0)
            {//if we already add an entry in the colorTabs, we change it
                _colorInUse[inUseIdx] = idx;
            }
            else
            {//else we add it
                _colorInUse.Add(idx);
            }

            playerColor = Colors[idx];
        }

        [Command]
        public void CmdPermissionChange()
        {
            //int permissionTypeLength = System.Enum.GetValues(typeof(PermissionCategories)).Length;
            if (permissionStatus == PermissionCategories.Master)
            {
                permissionStatus = PermissionCategories.User;
                permissionButton.GetComponentInChildren<Text>().text = permissionStatus.ToString();
            }
            else if(permissionStatus == PermissionCategories.User)
            {
                permissionStatus = PermissionCategories.Audience;
                permissionButton.GetComponentInChildren<Text>().text = permissionStatus.ToString();
            }
            else
            {
                permissionStatus = PermissionCategories.Master;
                permissionButton.GetComponentInChildren<Text>().text = permissionStatus.ToString();
            }
        }

        [Command]
        public void CmdNameChanged(string name)
        {
            playerName = name;
        }

        [Command]
        public void CmdChangeFilePath(string fp)
        {
            filePath = fp;
            if (!fp.Equals(""))
            {
               // filePath = fp;
            }
            
        }

        [Command]
        public void CmdChangeAnnotationPath(string ap)
        {
            annotationPath = ap;
        }

        [Command]
        public void CmdChangeAnnotationSavePath(string asp)
        {
            annotationSavePath = asp;
        }

        [Command]
        public void CmdChangeConfigPath(string fp)
        {
            configPath = fp;
            print("changeconfigpath");
        }

        //Cleanup thing when get destroy (which happen when client kick or disconnect)
        public void OnDestroy()
        {
            LobbyPlayerList._instance.RemovePlayer(this);
            if (LobbyManager.s_Singleton != null) LobbyManager.s_Singleton.OnPlayersNumberModified(-1);

            int idx = System.Array.IndexOf(Colors, playerColor);

            if (idx < 0)
                return;

            for (int i = 0; i < _colorInUse.Count; ++i)
            {
                if (_colorInUse[i] == idx)
                {//that color is already in use
                    _colorInUse.RemoveAt(i);
                    break;
                }
            }
        }
    }
}