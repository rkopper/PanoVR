using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPress : MonoBehaviour {

    public vrWand wand;
    GameObject Abutton;
    GameObject Bbutton;
    GameObject HandTrigger;
    GameObject Trigger;
    GameObject stick;
    float xcoord;
    float ycoord;
    float zcoord;
    float Bxcoord;
    float Bycoord;
    float Bzcoord;
    float Txcoord;
    float Tycoord;
    float Tzcoord;
    float sxcoord;
    float sycoord;
    float szcoord;

    // Use this for initialization
    void Start () {
        wand = MiddleVR.VRDeviceMgr.GetWand("Wand0");
        Abutton = GameObject.Find("button_1");
        Bbutton = GameObject.Find("button_2");
        HandTrigger = GameObject.Find("hand_trigger");
        Trigger = GameObject.Find("primary_trigger");
        stick = GameObject.Find("stick");
        xcoord = Abutton.transform.localPosition.x;
        ycoord = Abutton.transform.localPosition.y;
        zcoord = Abutton.transform.localPosition.z;
        Bxcoord = Bbutton.transform.localPosition.x;
        Bycoord = Bbutton.transform.localPosition.y;
        Bzcoord = Bbutton.transform.localPosition.z;
        Txcoord = HandTrigger.transform.localPosition.x;
        Tycoord = HandTrigger.transform.localPosition.y;
        Tzcoord = HandTrigger.transform.localPosition.z;
        sxcoord = stick.transform.localPosition.x;
        sycoord = stick.transform.localPosition.y;
        szcoord = stick.transform.localPosition.z;
    }
	
	// Update is called once per frame
	void Update () {
        if (wand.IsButtonPressed(4))
        {
            Abutton.transform.localPosition = new Vector3(xcoord, -.007f, zcoord);
        }
        else
        {
            Abutton.transform.localPosition = new Vector3(xcoord, ycoord, zcoord);
        }
        if (wand.IsButtonPressed(5))
        {
            Bbutton.transform.localPosition = new Vector3(Bxcoord, -.007f, Bzcoord);
        }
        else
        {
            Bbutton.transform.localPosition = new Vector3(Bxcoord, Bycoord, Bzcoord);
        }
        if (wand.IsButtonPressed(2))
        {
            HandTrigger.transform.localPosition = new Vector3(.005f, Tycoord, Tzcoord);
        }
        else
        {
            HandTrigger.transform.localPosition = new Vector3(Txcoord, Tycoord, Tzcoord);
        }
        if (wand.IsButtonPressed(0))
        {
            Trigger.transform.localEulerAngles = new Vector3(20, 0, 0);
        }
        else
        {
            Trigger.transform.localEulerAngles = new Vector3(0, 0, 0);
        }
        if (wand.IsButtonPressed(3))
        {
            stick.transform.localPosition = new Vector3(sxcoord, .001f, szcoord);
        }
        else
        {
            stick.transform.localPosition = new Vector3(sxcoord, sycoord, szcoord);
        }

    }
}
