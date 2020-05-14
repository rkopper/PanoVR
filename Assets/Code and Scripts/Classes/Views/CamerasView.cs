using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamerasView : MonoBehaviour {
    public Camera mainCamera;
    public Camera referenceCam;
    public GameObject vrCenterNode;
    public GameObject headnode, tester;
    public vrMouse mouse;

    public int x, y, z;


    public void Start()
    {
        x = 0;
        y = 0;
        z = 0;
        //tester.SetActive(true);
        mouse = MiddleVR.VRDeviceMgr.GetMouse();
    }
    public void Update()
    {
        if (mainCamera == null)
		{   
			//finding the camera (previously) gives us static angle of viewing (as the camera doesn't change position), let's find the reference camera we created to solve the stereo issue -DJZ
			mainCamera = GameObject.Find ("DaveReferenceCamera").GetComponent<Camera>(); 
			if (mainCamera == null) {
				print ("camera not ready yet!");
				return;
			}
            print("camera initialized");

            // Change some of the settings, there should be a better way to do this.
            mainCamera.fieldOfView = referenceCam.fieldOfView; //DJZ - should we really be messing with the FOV?
           
        }
		/*
        if (MiddleVR.VRDeviceMgr.IsKeyPressed(MiddleVR.VRK_RIGHT))
        {
            y = y + 1;
        }
        if (MiddleVR.VRDeviceMgr.IsKeyPressed(MiddleVR.VRK_LEFT))
        {
            y = y - 1;
        }
        if (MiddleVR.VRDeviceMgr.IsKeyPressed(MiddleVR.VRK_UP))
        {
            x = x - 1;
        }
        if (MiddleVR.VRDeviceMgr.IsKeyPressed(MiddleVR.VRK_DOWN))
        {
            x = x + 1;
        }
        //if (MiddleVR.VRDeviceMgr.IsMouseButtonPressed(0))
        //{
        //    mouse.SetCursorPosition(new vrVec2(0,0));
        //}
        //vrVec2 vec = new vrVec2(mouse.GetCursorPosition());
        //Vector3 pz = mainCamera.ScreenToWorldPoint(new Vector3(vec.x(), vec.y(), 1.5f));
        //print("mouse location: " + pz);
        //float a = MiddleVR.VRDeviceMgr.GetMouseAxisValue(0);
        //float b = MiddleVR.VRDeviceMgr.GetMouseAxisValue(1);
        //float c = MiddleVR.VRDeviceMgr.GetMouseAxisValue(2);
        //tester.transform.localEulerAngles = new Vector3(0, 0, 0);
        //tester.transform.parent = app.view.cameras.mainCamera.transform;
        //tester.transform.localPosition = new Vector3(pz.x-1.2f,-pz.y+1.4f,1.5f);
        mainCamera.transform.localEulerAngles = new Vector3(x, y, z);
        */
    }
}
