using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterfaceController : MonoBehaviour {
    public List<ClientScript> users;
    public List<GameObject> frustums = new List<GameObject>();
    public List<GameObject> avatars = new List<GameObject>();
    // Use this for initialization
    public bool isRunning = false;
	private App app;

	void Start()
	{
		app=GameObject.FindObjectOfType<App>(); 
		if (app == null)
			Debug.LogError ("Can't find App script");
	}

    public void initializeView()
    {
        isRunning = true;
        print("initializing view (but only once!");

    }
        // Update is called once per frame
    void Update () {
        List<ClientScript> users = app.model.users.userList;
        if (frustums.Count != users.Count)
        {
            clearFrustoms();
            generateFrustums();
        }
        if (avatars.Count != users.Count)
        {
            clearAvatars();
            generateInterface();
        }
        if (isRunning)
        {
              updateInterface();
              updateFrustums();
        }
	}

    public void clearFrustoms()
    {
        for (int i = 0; i < frustums.Count; i++)
        {
            Destroy(frustums[i]);
        }
        frustums.Clear();
    }
    public void clearAvatars()
    {
        for (int i = 0; i < avatars.Count; i++)
        {
            Destroy(avatars[i]);
        }
        avatars.Clear();
    }

    public void generateInterface()
    {
        print("generatingInterface");
        gameObject.GetComponent<AvatarGenerator>().layoutAvatars();
        avatars = gameObject.GetComponent<AvatarGenerator>().avatars;
    }

    public void generateFrustums()
    {
        users = app.model.users.userList;
        foreach (ClientScript user in users)
        {
                    GameObject frustum = Instantiate(Resources.Load("FrustumObject")) as GameObject;
                    frustum.name = user.name + " frustum";
                    frustum.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", user.playerColor);
					frustum.transform.parent = app.view.userInterface.GetComponent<Transform>();
                    frustums.Add(frustum);
        }
    }

    public void updateFrustums()
    {
        for (int i = 0; i < users.Count; i++)
        {
            ClientScript user = users[i];
            GameObject frustum = frustums[i];

            frustum.transform.eulerAngles = user.viewingAngle;
        }
    }
    
    public void updateInterface()
    {

        for(int i = 0; i < users.Count; i++)
        {
			avatars[i].gameObject.GetComponentsInChildren<RectTransform>()[1].localEulerAngles = new Vector3(0,0,(app.model.users.local.viewingAngle.y-app.model.users.userList[i].viewingAngle.y));
        }
    }
}
