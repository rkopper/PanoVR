using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VoiceChat.Networking;

public class RecorderDropdown : MonoBehaviour {

    public Dropdown deviceDropdown;
    List<string> devices = new List<string>();

    void Start()
    {      
        foreach (string device in VoiceChat.VoiceChatRecorder.Instance.AvailableDevices)
        {
            devices.Add(device);
            if(devices != null)
            {
                VoiceChat.VoiceChatRecorder.Instance.Device = devices[0];
            }
           
        }
        PopulateList();
    }

    private void Update()
    {
        
    }

    void PopulateList()
    {
        List<string> devices = new List<string>();
        foreach(string device in VoiceChat.VoiceChatRecorder.Instance.AvailableDevices)
        {
            devices.Add(device);
        }   
        deviceDropdown.AddOptions(devices);
    }

    public void DropdownIndexChange(int index)
    {
        VoiceChat.VoiceChatRecorder.Instance.Device = devices[index];
        print(VoiceChat.VoiceChatRecorder.Instance.Device);
    }

   
}
