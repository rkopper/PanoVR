using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterFeaturesHook : MonoBehaviour {

    public AdvancedPanel ap;
    public bool chat = true;
    public bool play = true;
    public bool look = true;
    public bool draw = true;

    // Use this for initialization
    void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        play = ap.getPlay();
        chat = ap.getChat();
        look = ap.getLook();
        draw = ap.getDraw();
	}
    public bool getPlay()
    {
        return play;
    }
    public bool getChat()
    {
        return chat;
    }
    public bool getDraw()
    {
        return draw;
    }
    public bool getLook()
    {
        return look;
    }
}
