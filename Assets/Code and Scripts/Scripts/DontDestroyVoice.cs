using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyVoice : MonoBehaviour {

	// Use this for initialization
	void Start () {

        GameObject.DontDestroyOnLoad(this);

	}
	
	// Update is called once per frame
	void Update () {
        //print(VoiceChat.VoiceChatRecorder.Instance.Device);
    }
}
