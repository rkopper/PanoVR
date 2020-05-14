using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarGenerator : MonoBehaviour {
    
	public List<GameObject> avatars = new List<GameObject>();

    public int numPlayers = 0;

    public enum AvatarLayout
    {
        LayoutHorizontal,
        LayoutVertical,
        LayoutCircle
    }

    public int numAvatars = 0;
    public GameObject avatar;
    public float horizontalPadding = 10f;
    public float verticalPadding = 10f;

    public AvatarLayout layout = AvatarLayout.LayoutVertical;

    private float canvasHeight;
    private float canvasWidth;
    private float iconSize;
    private float usableCanvasH = .5f; // Proportion of Canvas that's usable.
    private float usableCanvasV = 0.2f;

    public bool debugMode = false; //used so we dont have to use lobby for testing, defaults the avatar number
	private App app;

	void Start()
	{
		app=GameObject.FindObjectOfType<App>(); 
		if (app == null)
			Debug.LogError ("Can't find App script");
	}

    private void Update()
    {
		List<ClientScript> users = app.model.users.userList;
        if (avatars.Count != users.Count)
        {
            //createClones();
            layoutAvatars();
        }
    }

    public void layoutAvatars()
    {
        print("laying out avatars");
		numPlayers = app.model.users.userList.Count;
        numAvatars = numPlayers;
        MakeLayout();
       // SetColor();
    }

    void MakeLayout()
    {
        // Layout: Circular, Line Sides, Line Bottom, Line Top
		Vector2 size = app.view.userInterface.canvas.GetComponent<RectTransform>().sizeDelta;
        canvasHeight = size.x; canvasWidth = size.y;
        iconSize = avatar.GetComponent<RectTransform>().sizeDelta.x;

        switch (layout)
        {
            case AvatarLayout.LayoutCircle:
                layoutCircle();
                break;
            case AvatarLayout.LayoutVertical:
                layoutVertically();
                break;
            case AvatarLayout.LayoutHorizontal:
                layoutHorizontally();
                break;
            default:
                layoutVertically();
                break;
        }
    }

    void layoutCircle()
    {
        float radius = Mathf.Min(usableCanvasV * canvasHeight, usableCanvasH * canvasWidth);

        float theta = Mathf.PI / 2;
        for (int i = 0; i < avatars.Count; i++)
        {
            Vector2 circlePos = new Vector2(radius * Mathf.Cos(theta), radius * Mathf.Sin(theta));
            avatars[i].GetComponent<RectTransform>().localPosition = circlePos;
            theta += 2 * Mathf.PI / numAvatars;
        }
    }

    void layoutHorizontally()
    {
		List<ClientScript> users = app.model.users.userList;
        if ((numAvatars) * iconSize + (numAvatars - 1) * horizontalPadding > canvasWidth * usableCanvasH)
        {
            // We're gonna need 2 rows, distribute evenly
            float xPos = -(((numAvatars - 1) / 2) * iconSize + ((numAvatars - 1) / 2) * horizontalPadding) / 2;
            float yPos = -canvasHeight * usableCanvasV;
            int x = 0;
            print("if");
            for (int row = 1; row <= 2; row++)
            {
                for (int i = 0; i < numAvatars / 2; i++)
                {
                    var pos = new Vector2(xPos, yPos);
                    GameObject clone = Instantiate(Resources.Load("Avatar")) as GameObject;
					clone.transform.parent = app.view.userInterface.canvas.transform;
					clone.transform.position = gameObject.transform.position;
                    clone.transform.localRotation = Quaternion.identity;
                    clone.transform.localScale = new Vector3(1, 1, 1);
                    clone.GetComponent<RectTransform>().localPosition = pos;
                    clone.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", users[x].playerColor);
                    clone.GetComponentInChildren<UnityEngine.UI.Text>().material = clone.GetComponent<MeshRenderer>().materials[0];
                    clone.GetComponentInChildren<UnityEngine.UI.Text>().text = users[x].userName;
                   clone.GetComponentsInChildren<UnityEngine.UI.RawImage>()[1].material = clone.GetComponent<MeshRenderer>().materials[0];
                    avatars.Add(clone);
                    xPos += horizontalPadding + iconSize;
                    x++;
                }
                yPos = -yPos;
                xPos = -(((numAvatars - 1) / 2) * iconSize + ((numAvatars - 1) / 2) * horizontalPadding) / 2;
            }

        }
        else
        {
            // We're gonna need a single row
            float xPos = -((numAvatars - 1) * iconSize + (numAvatars - 1) * horizontalPadding) / 2;
            float yPos = -canvasHeight * usableCanvasV;
            for (int i = 0; i < numAvatars; i++)
            {
                var pos = new Vector2(xPos, yPos);
                GameObject clone = Instantiate(Resources.Load("Avatar")) as GameObject;
				clone.transform.parent = app.view.userInterface.canvas.transform;
                //clone.transform.parent = gameObject.transform;
                clone.transform.position = gameObject.transform.position;
                clone.transform.localRotation = Quaternion.identity;
                clone.transform.localScale = new Vector3(1, 1, 1);
                clone.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", users[i].playerColor);
                clone.GetComponentInChildren<UnityEngine.UI.Text>().material = clone.GetComponent<MeshRenderer>().materials[0];
                clone.GetComponentInChildren<UnityEngine.UI.Text>().text = users[i].userName;
                clone.GetComponentsInChildren<UnityEngine.UI.RawImage>()[1].material = clone.GetComponent<MeshRenderer>().materials[0];
                print(users[i].playerColor);
                clone.GetComponent<RectTransform>().localPosition = pos;
                avatars.Add(clone);
                xPos += horizontalPadding + iconSize;

            }
        }

    }

    void createClones()
    {
		List<ClientScript> users = app.model.users.userList;
        for (int i = 0; i < users.Count; i++)
        {
            GameObject clone = Instantiate(Resources.Load("Avatar")) as GameObject;
			clone.transform.parent = app.view.userInterface.canvas.transform;
            clone.transform.position = gameObject.transform.position;
            clone.transform.localRotation = Quaternion.identity;
            clone.transform.localScale = new Vector3(1, 1, 1);
            //clone.GetComponent<RectTransform>().localPosition = pos;
            clone.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", users[i].playerColor);
            clone.GetComponentsInChildren<UnityEngine.UI.RawImage>()[1].material = clone.GetComponent<MeshRenderer>().materials[0];
            avatars.Add(clone);
        }
    }

    void layoutVertically()
    {
        // We're gonna need 2 rows, distribute evenly
        float yPos = -((numAvatars / 2) * iconSize + (numAvatars / 2 - 1) * verticalPadding) / 2;
        float xPos = -canvasWidth * usableCanvasH;
		List<ClientScript> users = app.model.users.userList;
        int x = 0;
        for (int row = 1; row <= 2; row++)
        {
            for (int i = 0; i < numAvatars / 2; i++)
            {
                var pos = new Vector2(xPos, yPos);
                GameObject clone = (GameObject)Instantiate(avatar);
				clone.transform.parent = app.view.userInterface.canvas.transform;
                clone.transform.position = gameObject.transform.position;
                clone.transform.localRotation = Quaternion.identity;
                clone.transform.localScale = new Vector3(1, 1, 1);
                clone.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", users[x].playerColor);
                clone.GetComponentsInChildren<UnityEngine.UI.RawImage>()[1].material.SetColor("_EmissionColor", users[x].playerColor);
                clone.GetComponent<RectTransform>().localPosition = pos;
                avatars.Add(clone);
                yPos += verticalPadding + iconSize;
                x++;
            }
            xPos = -xPos;
            yPos = -((numAvatars / 2) * iconSize + (numAvatars / 2 - 1) * verticalPadding) / 2;
        }
    }

    void SetColor()
    {
		List<ClientScript> users = app.model.users.userList;
        for (int i = 0; i < users.Count; i++)
        {
            avatars[i].GetComponentsInChildren<UnityEngine.UI.RawImage>()[1].material.SetColor("_EmissionColor", users[i].playerColor);
        }
    }
}
