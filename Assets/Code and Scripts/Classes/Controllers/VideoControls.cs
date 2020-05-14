using RenderHeads.Media.AVProVideo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class VideoControls : NetworkBehaviour
{
    //public App app { get { return GameObject.FindObjectOfType<App>(); } }
	private App app;

	private IMediaInfo m_info;
    public MediaPlayer mp;
    public IMediaControl m_control;
    public vrWand wand;
    public float myTime, refTime;
    public GameObject restart;
    public bool isSynced = false;
    List<ClientScript> users;
    bool allowFeature = false;

    void Start()
    {
		app=GameObject.FindObjectOfType<App>(); 
		if (app == null)
			Debug.LogError ("Can't find App script");
		
        wand = MiddleVR.VRDeviceMgr.GetWand("Wand0");
    }
    // Update is called once per frame
    void Update()
    {
        users = app.model.users.userList;
        NetworkIdentity nID = gameObject.GetComponent(typeof(NetworkIdentity)) as NetworkIdentity;
        myTime = app.model.users.local.time;
        if(app.model.users.local.permissionLevel == PermissionCategories.Admin || app.model.users.local.playFeat == false)
        {
            allowFeature = true;
        }
        for (int i = 0; i < users.Count; i++)
        {
            if(users[i].userRole == UserRole.UserHost)
            {
                refTime = users[i].time;
            }
        }
        if (app.model.users.local.isPlaying == false)
        {
            mp.Pause();
           
        }
        else
        {
            mp.Play();
        }
   
        if  (allowFeature && wand.IsButtonToggled(4, true) && !wand.IsButtonPressed(2) || MiddleVR.VRDeviceMgr.IsKeyToggled(MiddleVR.VRK_SPACE))
        {
            if (app.model.users.local.isPlaying == true)
            {
                app.model.users.local.CmdChangeState(false);
                app.model.users.local.isPlaying = false;
            }
            else
            {
                app.model.users.local.CmdChangeState(true);
                app.model.users.local.isPlaying = true;
            }
        }
        if (app.model.users.local.rewind == true)
        {
            mp.Rewind(false);
            
            app.model.users.local.CmdRewind(false);
            app.model.users.local.rewind = false;
            StartCoroutine(ColorChange());
        }
        if (wand.IsButtonPressed(2) && allowFeature)
        {
            if (wand.IsButtonToggled(4, true))
            {
                isSynced = true;
                app.model.users.local.CmdRewind(true);
                app.model.users.local.rewind = true;
            }
        }
        if (MiddleVR.VRDeviceMgr.IsKeyToggled(MiddleVR.VRK_R))
        {
            isSynced = true;
            app.model.users.local.CmdRewind(true);
            app.model.users.local.rewind = true;
        }
        if (app.model.users.local.userRole != UserRole.UserHost)
        {
            syncState();
        }
        //if(app.model.users.local.isPlaying == false && app.model.users.local.userRole != UserRole.UserHost)
        //{
        //    syncPauseState();
        //}
    }
    IEnumerator ColorChange()
    {
        while (true)
        {
            restart.GetComponent<SpriteRenderer>().material.color = Color.blue;
            yield return new WaitForSeconds(1.5f);
            restart.GetComponent<SpriteRenderer>().material.color = Color.grey;
            break;
        }
    }
    public void syncState()
    {
            if (myTime >= refTime + .005 || myTime <= refTime - .005)
            {
                mp.Pause();
                float t = refTime * mp.Info.GetDurationMs();
                mp.Control.Seek(t);
                mp.Play();
            }
    }
    public void syncPauseState()
    {
        float t = refTime * mp.Info.GetDurationMs();
        mp.Control.Seek(t);
    }
}