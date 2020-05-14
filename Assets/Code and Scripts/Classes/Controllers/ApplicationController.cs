using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplicationController : MonoBehaviour {
    // Handle game events.
    public VideoController videoController;
    public NetworkController networkController;
    public UserController userController;
    public InterfaceController interfaceController;
    public AnnotationController annotationController;
    public GameObject VideoControl;

	private App app;

    public void Start()
    {
        // Wait until all users are connected, then:

        //Start video
        //Generate interface

		app=GameObject.FindObjectOfType<App>(); 
		if (app == null)
			Debug.LogError ("Can't find App script");

    }

    public void Update()
    {
        // Adjust this for correct number of users.
        VideoControl.SetActive(true);
        if (app.model.users.userList.Count >= 1 && !interfaceController.isRunning)
        {
            print("initializing the view!!");
            interfaceController.initializeView();
        }
        // Update video

        // Update interface
    }
}
