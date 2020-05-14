using Prototype.NetworkLobby;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvancedPanel : MonoBehaviour {

    public LobbyManager lobbyManager;
    public RectTransform createServerPanel;
    public static AdvancedPanel aPanel;

    public bool playToggle = true;
    public bool laToggle = true;
    public bool lhToggle = true;
    public bool chatToggle = true;
    public static ArrayList masterFeatures = new ArrayList();

    // Use this for initialization
    private void Awake()
    {
        if (aPanel == null)
        {
            DontDestroyOnLoad(gameObject);
            aPanel = this;
        }
        else
        {
            if (aPanel != this)
            {
                Destroy(gameObject);
            }
        }
    }
    public void OnEnable()
    {
        lobbyManager.topPanel.ToggleVisibility(true);
    }

    public void OnClickBack()
    {
        lobbyManager.ChangeTo(createServerPanel);
        //lobbyManager.backServerDelegate = lobbyManager.SimpleBackClbkServer;
        //lobbyManager.topPanel.GetComponentInChildren<Text>().text = "Permission Settings";
    }
    public void onPlayToggle()
    {
        playToggle = !playToggle;
    }
    public void onlaToggle()
    {
        laToggle = !laToggle;
    }
    public void onlhToggle()
    {
        lhToggle = !lhToggle;
    }
    public void onchatToggle()
    {
        chatToggle = !chatToggle;
    }
    private void Update()
    {
        //if (playToggle)
        //{
        //    masterFeatures.Add("play");
        //}
        //else
        //{
        //    if (masterFeatures.Contains("play"))
        //    {
        //        masterFeatures.Remove("play");
        //    }
        //}
        //if (laToggle)
        //{
        //    masterFeatures.Add("line annotations");
        //}
        //else
        //{
        //    if (masterFeatures.Contains("line annotations"))
        //    {
        //        masterFeatures.Remove("line annotations");
        //    }
        //}
        //if (lhToggle)
        //{
        //    masterFeatures.Add("look here");
        //}
        //else
        //{
        //    if (masterFeatures.Contains("look here"))
        //    {
        //        masterFeatures.Remove("look here");
        //    }
        //}
        //if (chatToggle)
        //{
        //    masterFeatures.Add("chat");
        //}
        //else
        //{
        //    if (masterFeatures.Contains("chat"))
        //    {
        //        masterFeatures.Remove("chat");
        //    }
        //}
    }
    public bool getPlay()
    {
        return playToggle;
    }
    public bool getChat()
    {
        return chatToggle;
    }
    public bool getDraw()
    {
        return laToggle;
    }
    public bool getLook()
    {
        return lhToggle;
    }
}
