using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleToTalk : MonoBehaviour {

    public vrWand wand;
    //public App app { get { return GameObject.FindObjectOfType<App>(); } }
	private App app;

    List<ClientScript> users;
    bool allowFeature = false;

    // Use this for initialization
    void Start () {
		app=GameObject.FindObjectOfType<App>(); 
		if (app == null)
			Debug.LogError ("Can't find App script");
		
        wand = MiddleVR.VRDeviceMgr.GetWand("Wand0");
    }
	
	// Update is called once per frame
	void Update () {
        users = app.model.users.userList;
        if (app.model.users.local.permissionLevel == PermissionCategories.Admin || app.model.users.local.chatFeat == false)
        {
            allowFeature = true;
        }
        if (wand.IsButtonPressed(5) && allowFeature)
        {
            GameObject.Find("VoiceChat").GetComponent<VoiceChat.VoiceChatRecorder>().ClickTransmit(true);
        }
        else
        {
            GameObject.Find("VoiceChat").GetComponent<VoiceChat.VoiceChatRecorder>().ClickTransmit(false);
        }
	}
}
