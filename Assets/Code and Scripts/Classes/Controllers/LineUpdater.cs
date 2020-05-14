using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using MiddleVR_Unity3D;
using System;

public class LineUpdater : NetworkBehaviour
{
    //public App app { get { return GameObject.FindObjectOfType<App>(); } }
	private App app;

	public Camera mainCamera;

    float lastTime = 0.0f;

    public LineRenderer lr;

    float timeBeforeFade = 3.0f;
    float fadeTime = 3.0f;

    float ogMultiplier;
    float fadePercent;
    float timeElapsed;


    [SyncVar]
    public float minTime;
    [SyncVar]
    public float maxTime;

    public List<Vector3> vertices;
    [SyncVar]
    public bool triggered = false;

    public vrWand rightWand;
    // Use this for initialization

    [SyncVar]
    public Vector3 newPos;

    void Start()
    {
		app=GameObject.FindObjectOfType<App>(); 
		if (app == null)
			Debug.LogError ("Can't find App script");
		
        lr = gameObject.GetComponent<LineRenderer>();
        ogMultiplier = lr.widthMultiplier;
        vertices = new List<Vector3>();
        rightWand = MiddleVR.VRDeviceMgr.GetWand("Wand0");
        fadePercent = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (mainCamera == null)
        {
            mainCamera = app.view.cameras.mainCamera;
        }

        if (triggered)
        {
            if (mainCamera == null) return;
        }

        if (!triggered && vertices.Count > 0)
        {
            float t = app.model.users.local.refTime;
            if (t < minTime)
            {
                showMaxVertex(0);
                fadePercent = 1;
                lr.widthMultiplier = ogMultiplier * fadePercent;
            }
            else if (t > maxTime)
            {
                showMaxVertex(vertices.Count);
                timeElapsed = (t - maxTime) * app.controller.videoController.mp.Info.GetDurationMs() / 1000;

                fadePercent = 1 - (timeElapsed - timeBeforeFade) / fadeTime;
                if (fadePercent > 1) fadePercent = 1;
                if (fadePercent < 0) fadePercent = 0;

                lr.widthMultiplier = ogMultiplier * fadePercent;


            }
            else
            {
                int frame = (int)((vertices.Count * (t - minTime)) / (maxTime - minTime));
                fadePercent = 1;
                lr.widthMultiplier = ogMultiplier * fadePercent;
                showMaxVertex(frame);
                
            }
        }
    }

    public void showMaxVertex(int n)
    {
        n = Math.Min(n, vertices.Count - 1);
		if (n < 0) {
			//Debug.LogError ("Annotation Fix: don't allow negative numbers");
			n = 0; 
		}
		if (n > 0) 
		{	
			Vector3[] destfoo = new Vector3[n];
			List<Vector3> newVerts = vertices.GetRange (0, n);
			lr.SetPositions (newVerts.ToArray ());
			//Debug.LogError ("Annotation Fix: don't allow size 0 list of positions");
		}
		lr.positionCount = n;
    }

    public void addVertexWand()
    {


        //vrNode3D node = MiddleVR.VRDisplayMgr.GetNode("LeftHandNode");
        GameObject n1 = GameObject.Find("HandNode");

        Transform t = n1.GetComponent<Transform>();

        Vector3 fwd = t.forward;
        Vector3 pos = t.position;


        Ray r = new Ray(pos, fwd);

        Vector3 o = r.GetPoint(100);

        Debug.DrawRay(o, -fwd, Color.green);

        RaycastHit hit;
        if (Physics.Raycast(o, -fwd, out hit))
        {
            CmdUpdateVertex(hit.point);
        }
    }

    void addVertexSphere()
    {
        Vector3 angles = mainCamera.GetComponent<Transform>().eulerAngles;
        float phi = Mathf.Deg2Rad * ((90 + angles.x) % 360.0f);
        float theta = Mathf.Deg2Rad * angles.y; // reference angle between z axis and angle

        float radius = 9;
        float x = radius * Mathf.Sin(phi) * Mathf.Sin(theta);
        float z = radius * Mathf.Sin(phi) * Mathf.Cos(theta);
        float y = radius * Mathf.Cos(phi);

        Vector3 loc = new Vector3(x, y, z);

        // Check that it's not too close to the previous vertex
        Vector3 lastLoc = lr.GetPosition(lr.positionCount - 1);

        print(Vector3.Distance(loc, lastLoc));
        if (Vector3.Distance(loc, lastLoc) > .08)
        {
            CmdUpdateVertex(loc);
        }
    }

    [Command]
    void CmdUpdateVertex(Vector3 newVert)
    {
        if (lr.positionCount == 0)
        {
            minTime = app.model.users.local.refTime;
        }
        maxTime = Math.Max(maxTime, app.model.users.local.refTime);
        newPos = newVert;
        RpcAddVertex(newVert);
    }

    [ClientRpc]
    public void RpcAddVertex(Vector3 pos)
    {
        
        addVertex(pos);
    }

    public void addVertex(Vector3 pos)
    {
        if (lr.positionCount == 0)
        {
            lr.positionCount++;
            lr.SetPosition(lr.positionCount - 1, pos);
            vertices.Add(pos);
        }
        else
        {
            Vector3 lastLoc = lr.GetPosition(lr.positionCount - 1);

            if (Vector3.Distance(pos, lastLoc) > .05)
            {
                lr.positionCount++;
                lr.SetPosition(lr.positionCount - 1, pos);
                vertices.Add(pos);
            }
        }

    }
}