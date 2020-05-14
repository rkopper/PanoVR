using RenderHeads.Media.AVProVideo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;

public class StateController : NetworkBehaviour
{

    //public App app { get { return GameObject.FindObjectOfType<App>(); } }

	private App app;

    public MediaPlayer mp;
    private bool isPlaying;

    public vrWand wand;


    // Use this for initialization
    void Start()
	{   
		app=GameObject.FindObjectOfType<App>(); 
		if (app == null)
			Debug.LogError ("Can't find App script");
		
        isPlaying = true;
        wand = MiddleVR.VRDeviceMgr.GetWand("Wand1");

    }

    // Update is called once per frame
    void Update()
    {
        onChangeState();
        List<ClientScript> users = app.model.users.userList;

        if (wand.IsButtonToggled(5, true))
        {
            if (isPlaying == true)
            {
                isPlaying = false;
                app.model.users.local.isPlaying = false;
                for (int i = 0; i < users.Count; i++)
                {
                    users[i].isPlaying = false;
                }
                //mp.Pause();

            }
            else
            {
                isPlaying = true;
                app.model.users.local.isPlaying = true;
                for (int i = 0; i < users.Count; i++)
                {
                    users[i].isPlaying = true;
                }
                //mp.Play();
            }
        }
        
    }

    private void onChangeState()
    {
        if (app.model.users.local.isPlaying == true)
        {
            mp.Play();
        }
        if (app.model.users.local.isPlaying == false)
        {
            mp.Pause();
        }
    }
}

