using RenderHeads.Media.AVProVideo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;

namespace RenderHeads.Media.AVProVideo
{
    public class VideoLocation : NetworkBehaviour
    {
        //public App app { get { return GameObject.FindObjectOfType<App>(); } }

		private App app;

        private IMediaInfo m_info;
        public MediaPlayer PlayingPlayer;
        public vrWand wand;
        public float d;
        private bool isPlaying = true;
        // Use this for initialization
        void Start()
        {
			app=GameObject.FindObjectOfType<App>(); 
			if (app == null)
				Debug.LogError ("Can't find App script");
			
            wand = MiddleVR.VRDeviceMgr.GetWand("Wand1");
        }

        // Update is called once per frame
        void Update()
        {
            if (PlayingPlayer == null) return;
            List<ClientScript> users = app.model.users.userList;
            NetworkIdentity nID = gameObject.GetComponent(typeof(NetworkIdentity)) as NetworkIdentity;
            float time = PlayingPlayer.Control.GetCurrentTimeMs();
            d = time / PlayingPlayer.Info.GetDurationMs();


            if (app.model.users.local.userRole == UserRole.UserHost)
            {
                app.model.users.local.time = d;

                for (int i = 0; i < users.Count; i++)
                {
                    users[i].time = d;
                }
                if (wand.IsButtonToggled(2, true))
                {
                    print("here");
                    PlayingPlayer.Rewind(false);
                }
                if (wand.IsButtonToggled(5, true))
                {
                    if (isPlaying == true)
                    {
                        isPlaying = false;
                        app.model.users.local.isPlaying = false;
                        PlayingPlayer.Pause();
                        for (int i = 0; i < users.Count; i++)
                        {
                            users[i].isPlaying = false;
                        }

                    }
                    else
                    {
                        isPlaying = true;
                        app.model.users.local.isPlaying = true;
                        PlayingPlayer.Play();
                        for (int i = 0; i < users.Count; i++)
                        {
                            users[i].isPlaying = true;
                        }
                    }
                }


            }
            else
            {
                if (d >= app.model.users.local.time + .01 || d <= app.model.users.local.time - .01)
                {
                    PlayingPlayer.Pause();
                    float t = app.model.users.local.time * PlayingPlayer.Info.GetDurationMs();
                    PlayingPlayer.Control.Seek(t);
                    PlayingPlayer.Play();
                }
                if (app.model.users.local.isPlaying == false)
                {
                    float t = app.model.users.local.time * PlayingPlayer.Info.GetDurationMs();
                    PlayingPlayer.Control.Seek(t);
                    PlayingPlayer.Pause();
                }
                else
                {
                    PlayingPlayer.Play();
                }
            }
        }
    }
}