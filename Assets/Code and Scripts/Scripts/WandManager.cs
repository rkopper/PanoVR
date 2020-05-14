using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WandManager : NetworkBehaviour {

    public GameObject wand;
    public GameObject wand1;

    //public App app { get { return GameObject.FindObjectOfType<App>(); } }
	private App app;

    public List<ClientScript> users;

    bool hasTouch;
	bool is_setup=false;

    // Use this for initialization
	
	// Update is called once per frame
	void Start () {
		app = GameObject.FindObjectOfType<App> (); 
		if (app == null)
			Debug.LogError ("Can't find App script");
	}

	void Update()
	{
		if (is_setup == false) { //lets only do the setup once, and wait for things to be ready, so we don't get errors... -DJZ
			users = app.model.users.userList;

			if (users == null)
				return;
			if (app.model.users.local == null)
				return;
			
			if (app.model.users.local.configPath == "oculus_touch.vrx") { //doesn't seem to be using this for MiddleVR though... -DJZ
				hasTouch = true;
			} else {
				hasTouch = false;
			}
        
			if (hasTouch == false) {
				wand.SetActive (false);
				wand1.SetActive (false);
			}

			is_setup = true;
		}

	}
}
