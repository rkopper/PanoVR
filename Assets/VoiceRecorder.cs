using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoiceRecorder : MonoBehaviour {

    //public App app { get { return GameObject.FindObjectOfType<App>(); } }

	private App app;

    public List<ClientScript> users;
    AudioSource audio;
    bool checker = false;
    public VoiceChat.VoiceChatRecorder vc;

    // Use this for initialization
    void Start () {
		app=GameObject.FindObjectOfType<App>(); 
		if (app == null)
			Debug.LogError ("Can't find App script");
		
        users = app.model.users.userList;
        audio = GetComponent<AudioSource>();
        vc = GameObject.Find("VoiceChat").GetComponent<VoiceChat.VoiceChatRecorder>();
    }
	
	// Update is called once per frame
	void Update () {
        if (vc.transmitToggled)
        {
            //print("hi");
            checker = true;
        }
        if(!vc.transmitToggled && checker)
        {
            audio.Play();
            checker = false;
        }
	}
}
