using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MiddleVR_Unity3D;
using UnityEngine.Networking;
using System.IO;
using System;
using System.Globalization;

public class AnnotationController : NetworkBehaviour
{
    //public App app { get { return GameObject.FindObjectOfType<App>(); } }

	private App app;

    public float textRadius = 9.0f;
    public List<TextAnnotationPrefab> textAnnotations = new List<TextAnnotationPrefab>();
    public List<GameObject> lineAnnotations = new List<GameObject>();
    public TextAnnotationPrefab activePrefab;
    public string activeText;

    Vector3 init;

    public GameObject circlesprite;
    public GameObject handnode;

    public Camera mainCamera;
    public vrWand rightwand;

    public bool hasFile = false;
    bool allowFeature = false;

    // Use this for initialization
    void Start()
    {
        rightwand = MiddleVR.VRDeviceMgr.GetWand("Wand0");
        handnode = GameObject.Find("HandNode");

		app=GameObject.FindObjectOfType<App>(); 
		if (app == null)
			Debug.LogError ("Can't find App script");
		
		if (app.model.users.local.annotationPath != null && app.model.users.local.annotationPath != "")
        {
            hasFile = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
		if (app.model.users.local.permissionLevel == PermissionCategories.Admin || app.model.users.local.drawFeat == false)
        {
            allowFeature = true;
        }
        if (hasFile)
        {
            fileUpdater();
        }
        if (rightwand.IsButtonToggled(4, true))
        {
            circlesprite.SetActive(false);
        }
        if(rightwand.IsButtonPressed(4))
        {
            GameObject n1 = GameObject.Find("HandNode");
            Transform t = n1.GetComponent<Transform>();
            Vector3 fwd = t.forward;
            Vector3 pos = t.position;
            Ray r = new Ray(pos, fwd);
            Vector3 o = r.GetPoint(100);
            Debug.DrawRay(o, -fwd, Color.green);
            RaycastHit hit;
            Physics.Raycast(o, -fwd, out hit);
            o = hit.point;
            spawncircle(o);
        }
        updateWand();
        updateText();
    }

    void spawncircle(Vector3 o)
    {
        if (!circlesprite.active)
        {
            init = handnode.transform.position;
            circlesprite.transform.localScale = new Vector3(.3f, .3f, 1);
            circlesprite.transform.position = o;
            circlesprite.transform.rotation = handnode.transform.rotation;
            circlesprite.SetActive(true);
        }
        double diff = Math.Sqrt(init.x * init.x + init.z*init.z);
        diff = diff - Math.Sqrt(handnode.transform.position.x * handnode.transform.position.x + handnode.transform.position.z * handnode.transform.position.z);
        circlesprite.transform.localScale = new Vector3((float) Math.Abs(diff)*10, (float) Math.Abs(diff)*10, 0);
        
    }

    void updateWand()
    {
        if (mainCamera == null)
        {
			mainCamera = app.view.cameras.mainCamera;
        }


        {

            if (rightwand.IsButtonToggled(0, true) && !rightwand.IsButtonPressed(2) && allowFeature)
            {

				if (app.model.users.local.isServer)
                {
                    GameObject go = null;
					app.model.users.local.CmdDraw(go, app.model.users.local.playerColor);
                }
                else
                {
					GameObject line = (GameObject)Instantiate(Resources.Load("LineDrawing"), app.view.userInterface.annotations.GetComponent<Transform>());
					app.model.users.local.lineUpdater = line.GetComponent<LineUpdater>();
					app.model.users.local.lineUpdater.triggered = true;
                    lineAnnotations.Add(line);
					app.model.users.local.CmdDraw(line, app.model.users.local.playerColor);
                }
            }
            if (rightwand.IsButtonPressed(2))
            {
                if (rightwand.IsButtonToggled(0, true))
                {

					if (app.model.users.local.lineUpdater.triggered == false)
                    {
                        //Client keeps track of lineAnnotations in AnnotationController, Server keeps track
                        //of lineAnnotations in clientScript
						if (app.model.users.local.isServer && app.model.users.local.lineAnnotations.Count > 0)
                        {
                            GameObject go = null;

							app.model.users.local.CmdDelete(go);

                        }
						if (!app.model.users.local.isServer && lineAnnotations.Count > 0)
                        {
                            GameObject go = lineAnnotations[lineAnnotations.Count - 1];
                            lineAnnotations.Remove(go);
							app.model.users.local.CmdDelete(go);
                        }
                    }
                }
            }
            if (rightwand.IsButtonToggled(0, false))
            {
				app.model.users.local.lineUpdater.triggered = false;
				app.model.users.local.CmdStopDraw();
                //app.model.users.local.text(new String[] { "end" }, app.model.users.local.newdoc);
            }
			if (app.model.users.local.lineUpdater != null)
            {
				if (app.model.users.local.lineUpdater.triggered)
                {
                    addVertexWand();
                }
            }
        }
    }

    public void addVertexWand()
    {
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
			app.model.users.local.CmdUpdateVertex(hit.point);
        }
    }



    void updateText()
    {
        if (Input.GetKeyDown("-"))
        {
            activeText = "";
            activePrefab = null;
        }
        // Read text input for typing
        if (activePrefab != null)
        {
            foreach (char c in Input.inputString)
            {
                if (c == '\b' & activeText.Length >= 1)
                {
                    activeText = activeText.Substring(0, activeText.Length - 1);
                    activePrefab.setText(activeText);
                }
                else
                {
                    activeText = activeText + c;
                    activePrefab.setText(activeText);
                }

            }
        }

        if (Input.GetKeyDown("="))
        {
            activeText = "";
            //createTextAnnotation();
        }
    }

    void createTextAnnotation(Vector3 location)
    {
		GameObject o = (GameObject)Instantiate(Resources.Load("TextAnnotationPrefab"), app.view.userInterface.annotations.GetComponent<Transform>());
        o.GetComponent<Transform>().position = location;
        TextAnnotationPrefab t = o.GetComponent<TextAnnotationPrefab>();
        t.setText("");
        t.name = "annotation: " + (textAnnotations.Count + 1);
        textAnnotations.Add(t);
        activePrefab = t;
    }

    void createTextAnnotation()
    {
        // By default, create an annotation where the camera is looking.

		Vector3 angles = app.view.cameras.mainCamera.GetComponent<Transform>().localEulerAngles;
        float phi = Mathf.Deg2Rad * ((90 + angles.x) % 360.0f);
        float theta = Mathf.Deg2Rad * angles.y; // reference angle between z axis and angle

        float radius = textRadius;
        float x = radius * Mathf.Sin(phi) * Mathf.Sin(theta);
        float z = radius * Mathf.Sin(phi) * Mathf.Cos(theta);
        float y = radius * Mathf.Cos(phi);

        Vector3 loc = new Vector3(x, y, z);
        createTextAnnotation(loc);

    }

    public void fileUpdater()
    {
		string filepath = app.model.users.local.annotationPath;
         // Open the text file using a stream reader.
        using (StreamReader sr = new StreamReader(filepath))
        {
            string line;
            // Read and display lines from the file until the end of 
            // the file is reached.
            while ((line = sr.ReadLine()) != null)
            {
                ArrayList times = new ArrayList();
                ArrayList lines = new ArrayList();
                while ((line = sr.ReadLine()) != "end")
                {
                    if (line.Contains("("))
                    {
                        lines.Add(line);
                    }
                    else
                    {
                        times.Add(line);
                    }
                }
                hasFile = false;
                string[] timeArray = (string[])times.ToArray(typeof(string));
                drawFileLines(lines, timeArray[0], timeArray[timeArray.Length - 1]);
            }
        }
    }
    public Vector3 stringToVec(string s)
    {
        string[] temp = s.Substring(1, s.Length - 2).Split(',');
        return new Vector3(float.Parse(temp[0]), float.Parse(temp[1]), float.Parse(temp[2]));
    }
    public void drawFileLines(ArrayList points, string minTime, string maxTime)
    {
        string[] myArray = (string[])points.ToArray(typeof(string));
        Debug.LogError(myArray);
		app.model.users.local.CmdDrawFromFile(myArray, float.Parse(minTime, CultureInfo.InvariantCulture.NumberFormat), float.Parse(maxTime, CultureInfo.InvariantCulture.NumberFormat));
    }
}
