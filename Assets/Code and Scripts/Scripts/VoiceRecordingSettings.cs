using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VoiceRecordingSettings : MonoBehaviour {


    void Start()
    {

    }

    public void PushToTalkToggle(bool toggle)
    {
        VoiceChat.VoiceChatRecorder.Instance.AutoDetectSpeech = toggle;
        
    }
    
}
