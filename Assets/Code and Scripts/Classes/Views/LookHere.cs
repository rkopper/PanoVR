using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LookHere : NetworkBehaviour
{

    //public App app { get { return GameObject.FindObjectOfType<App>(); } }
	private App app;

    public GameObject arrowSprite;
    public GameObject words;

    public vrWand wand;

    bool clicked = false;

    public List<ClientScript> users;

    Vector3 refPos;
    public SpriteRenderer sr;

    // Use this for initialization
    void Start()
    {
		app=GameObject.FindObjectOfType<App>(); 
		if (app == null)
			Debug.LogError ("Can't find App script");
		
        wand = MiddleVR.VRDeviceMgr.GetWand("Wand0");
        arrowSprite.SetActive(false);
        users = app.model.users.userList;
    }

    // Update is called once per frame (test)
    void Update()
    {
        if (app.model.users.local.permissionLevel == PermissionCategories.Admin)
        {
            if (wand.IsButtonPressed(2))
            {
                if (wand.IsButtonToggled(5, true))
                {
                    app.model.users.local.CmdArrowon(app.model.users.local.viewingAngle);
                    clicked = true;
                }
            }
        }

        if (app.model.users.local.arrow == true)
        {
            for (int i = 0; i < users.Count; i++)
            {
                if (users[i].arrow == false)
                {
                    sr.material.SetColor("_Color", users[i].playerColor);
                }
            }
            if (!clicked)
            {
                arrowSprite.SetActive(true);
                words.SetActive(true);
            }
            refPos = app.model.users.local.refPos;
            onClick();
        }
        else
        {
            arrowSprite.SetActive(false);
            words.SetActive(false);
            app.model.users.local.refPos = new Vector3(0, 0, 0);
        }
    }

    void onClick()
    {
        clicked = false;
        SpriteRenderer spr = arrowSprite.GetComponent<SpriteRenderer>();
        users = app.model.users.userList;
        Vector3 myPos = app.model.users.local.viewingAngle;
        Vector3 angles = myPos;
        arrowSprite.transform.parent = app.view.cameras.mainCamera.transform;
        arrowSprite.transform.localPosition = new Vector3(0.0f, 0.0f, 1.5f);
        words.transform.parent = app.view.cameras.mainCamera.transform;
        words.transform.localPosition = new Vector3(0.0f, 0.0f, 1.49f);
        float ydiff = myPos.y - refPos.y;
        float xdiff = refPos.x - myPos.x;
        float zdiff = refPos.z - myPos.z;
        if (ydiff <= 0 && ydiff >= -180|| ydiff>=180 && ydiff <=360)
        {
            spr.flipX = false;
            arrowSprite.transform.localPosition = new Vector3(0f, 0.0f, 1.5f);
            words.transform.localPosition = new Vector3(0f, 0.0f, 1.49f);
            arrowSprite.transform.localEulerAngles = new Vector3(0, 0, -xdiff);
            words.transform.localEulerAngles = new Vector3(0, 0, -xdiff);
        }
        else
        {
            spr.flipX = true;
            arrowSprite.transform.localPosition = new Vector3(0f, 0.0f, 1.5f);
            words.transform.localPosition = new Vector3(0f, 0.0f, 1.49f);
            arrowSprite.transform.localEulerAngles = new Vector3(0, 0, xdiff);
            words.transform.localEulerAngles = new Vector3(0, 0, xdiff);
        }
        if (ydiff >= -20 && ydiff <= 20 && xdiff >= -20 && xdiff <= 20)
        {
            ClientScript me = app.model.users.local;
            me.CmdArrowOff();
        }
    }
}
