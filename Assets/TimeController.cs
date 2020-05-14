using RenderHeads.Media.AVProVideo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TimeController : NetworkBehaviour {

    //public App app { get { return GameObject.FindObjectOfType<App>(); } }

	private App app;

    List<ClientScript> users;
    public MediaPlayer mp;
    float myTime;

    // Use this for initialization
    void Start () {
		app=GameObject.FindObjectOfType<App>(); 
		if (app == null)
			Debug.LogError ("Can't find App script");
	}
	
	// Update is called once per frame
	void Update () {
        users = app.model.users.userList;
        float time = mp.Control.GetCurrentTimeMs();
        myTime = time / mp.Info.GetDurationMs();
        app.model.users.local.time = myTime;
        if(app.model.users.local.userRole == UserRole.UserHost)
        {
            app.model.users.local.refTime = myTime;
            for (int i = 0; i < users.Count; i++)
            {
                users[i].refTime = app.model.users.local.refTime;
            }
        }
    }
}
