using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PauseIcon : NetworkBehaviour {

    public GameObject Pause;
    public GameObject Play;
    public GameObject restart;
    //public App app { get { return GameObject.FindObjectOfType<App>(); } }

	private App app;
	bool icons_setup=false;

    public List<ClientScript> users;

    // Use this for initialization
    void Start () {
		app=GameObject.FindObjectOfType<App>(); 
		if (app == null)
			Debug.LogError ("Can't find App script");
		
        restart.GetComponent<SpriteRenderer>().material.color = Color.grey;
    }
	
	// Update is called once per frame
	void Update () {
        
        //users = app.model.users.userList; //seems to be un-needed....-DJZ
       
		if (icons_setup == false && app.view.cameras.mainCamera != null) { //lets just do this once. instead of every frame. -DJZ
			restart.transform.parent = app.view.cameras.mainCamera.transform;
			Pause.transform.parent = app.view.cameras.mainCamera.transform;
			Play.transform.parent = app.view.cameras.mainCamera.transform;

			Play.transform.localEulerAngles = new Vector3(0, 0, 0);
			Pause.transform.localEulerAngles = new Vector3(0, 0, 0);
			restart.transform.localEulerAngles = new Vector3(0, 0, 0);

			restart.transform.localPosition = new Vector3(0.15f, -0.75f, 1.5f);
			Pause.transform.localPosition = new Vector3(0.0f, -0.75f, 1.5f);
			Play.transform.localPosition = new Vector3(-0.15f, -0.75f, 1.5f);

			restart.SetActive(true);
			Pause.SetActive(true);
			Play.SetActive(true);

			icons_setup = true;
		}
       
        if (app.model.users.local.rewind == true)
        {
            StartCoroutine(ColorChange()); //is this really correct? might be spawning too many... -DJZ
        }
        if (app.model.users.local.isPlaying == false)
        {
            Pause.GetComponent<SpriteRenderer>().material.color = Color.red;
            Play.GetComponent<SpriteRenderer>().material.color = Color.grey;
        }
        else
        {
            Pause.GetComponent<SpriteRenderer>().material.color = Color.grey;
            Play.GetComponent<SpriteRenderer>().material.color = Color.green;
        }
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
}
