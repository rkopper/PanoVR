using Prototype.NetworkLobby;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using VoiceChat.Networking;

public class ConfigDropdown : MonoBehaviour
{
    public Dropdown deviceDropdown;

    public string currentDevice = "";
    public string configPath = "";
    List<string> configurations = new List<string>();
    List<string> configFiles = new List<string>();
    public LobbyManager lobbyManager;

    void Start()
    {
        string fp = Application.dataPath+"/MiddleVRConfigs/";

        foreach (string file in System.IO.Directory.GetFiles(fp))
        {
            if (file.EndsWith(".vrx"))
            {
                configFiles.Add(file);
                string filename = Regex.Replace(file, ".*\\/(.*)", "$1");
                configurations.Add(filename);
            }
        }

        deviceDropdown.AddOptions(configurations);

        int index = 0;
        configPath = configFiles[index];
        currentDevice = configurations[index];
    }

    private void Update()
    {

    }

    public void DropdownIndexChange(int index)
    {
        configPath = configFiles[index];
        currentDevice = configurations[index];
        print(configPath + index);
    }


}
