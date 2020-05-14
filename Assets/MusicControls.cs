using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicControls : MonoBehaviour {

    //public App app { get { return GameObject.FindObjectOfType<App>(); } }
	private App app;

    public HajiyevMusicManager mm;
    bool paused = false;

    // Use this for initialization
    void Start () {
		app=GameObject.FindObjectOfType<App>(); 
		if (app == null)
			Debug.LogError ("Can't find App script");
		
       // mm = new HajiyevMusicManager();
	}
	
	// Update is called once per frame
	void Update () {
        if (app.model.users.local.isPlaying == false)
        {
            mm.Pause();
            paused = true;
        }
        if(app.model.users.local.isPlaying == true && paused == true)
        {
            mm.Play();
        }
        if (app.model.users.local.rewind)
        {
            mm.Rewind();
        }
	}
}
