using RenderHeads.Media.AVProVideo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;
using PermissionEnum;
using System.IO;

public struct ClientState
{

    public string userId;
    public string userName;
    public UserRole userRole;
    public Color userColor;
    public string userAvatar;
    public Vector3 viewingAngle;
    public bool isPlaying;
    //added for permission feature
    public string fp;
    public string ap;
    public string asp;
    public PermissionEnum.PermissionCategories permissionLevel;
    public bool isVoicePlaying;
    public float time;
    public bool arrow;
    public Vector3 refPos;
    public String configPath;
    public bool rewind;
    public bool newdoc;
    public float refTime;
    public bool playFeat;
    public bool chatFeat;
    public bool drawFeat;
    public bool lookFeat;

}

public class ClientScript : NetworkBehaviour
{
    //public App app { get { return GameObject.FindObjectOfType<App>(); } }
	private App app;

    public vrWand leftwand;
    public vrWand rightwand;
    public LineUpdater lineUpdater;
    public List<GameObject> lineAnnotations = new List<GameObject>();
    //StreamWriter writer = new StreamWriter("Assets/Resources/test.txt",true);
    private const string FILE_NAME = "MyFile.txt";
    public bool savePath = false;
    public string docPath;

    [SyncVar]
    public string userId;
    [SyncVar]
    public string userName;
    [SyncVar]
    public UserRole userRole;
    [SyncVar]
    public string userAvatar;
    [SyncVar]
    public Vector3 viewingAngle;
    [SyncVar]
    public Color playerColor = Color.white;
    [SyncVar]
    public bool isPlaying;
    //added for permission feature
    [SyncVar]
    public bool isVoicePlaying;
    [SyncVar]
    public PermissionCategories permissionLevel;
    //added for fp
    [SyncVar]
    public string filePath;
    
    public string annotationPath;
    [SyncVar]
    public string annotationSavePath;
    [SyncVar]
    public float time;
    [SyncVar]
    public bool arrow;
    [SyncVar]
    public Vector3 refPos;
    // [SyncVar]
    public String configPath;
    [SyncVar]
    public bool rewind;
    [SyncVar]
    public bool newdoc;
    [SyncVar]
    public float refTime;
    [SyncVar]
    public bool playFeat;
    [SyncVar]
    public bool chatFeat;
    [SyncVar]
    public bool drawFeat;
    [SyncVar]
    public bool lookFeat;

    VoiceChat.VoiceChatRecorder vc;

    public void Start()
    {
		app=GameObject.FindObjectOfType<App>(); 
		if (app == null)
			Debug.LogError ("Can't find App script");
		
        name = "other client";
        isPlaying = true;
        arrow = false;
        leftwand = MiddleVR.VRDeviceMgr.GetWand("Wand1");
        rightwand = MiddleVR.VRDeviceMgr.GetWand("Wand0");
        rewind = false;
        print("written");
        newdoc = false;
        if(annotationSavePath != null && annotationSavePath != "")
        {
            savePath = true;
            docPath = annotationSavePath;
        }
        if (isLocalPlayer)
        {
            name = "local client";
            NetworkIdentity nID = gameObject.GetComponent(typeof(NetworkIdentity)) as NetworkIdentity;
            // Add some error checking here.
            // userName = nID.isLocalPlayer ? "localPlayer" : "otherUser";
            userId = nID.netId.ToString();
            userRole = nID.isServer ? UserRole.UserHost : UserRole.UserNormal;
            userAvatar = "Computer 1";
            viewingAngle = new Vector3(0, 0, 0);
            CmdInitializeData(currentState());
            app.model.users.local = this;
            //added for permission feature
            //permissionLevel = dataHolder.GetComponent<DataHolderScript>().RetrievePermission();
            
            vc = GameObject.Find("VoiceChat").GetComponent<VoiceChat.VoiceChatRecorder>();
            time = 0;
            refPos = new Vector3(0, 0, 0);
            configPath = "oculus_touch.vrx";
        }
        else
        {
            app.model.users.userList.Add(this);
        }
        print("Client Instantiated: " + (isLocalPlayer ? "Local Player" : "Other User"));
    }
    [Command]
    public void CmdDelete(GameObject go)
    {
        if (app.model.users.local.isServer)
        {
            GameObject line = lineAnnotations[lineAnnotations.Count - 1];
            lineAnnotations.Remove(line);
            NetworkServer.Destroy(line);
        }
        else
        {
            NetworkServer.Destroy(go);
        }
    }
    [Command]
    public void CmdDraw(GameObject go, Color color)
    {
        ((GameObject)Resources.Load("LineDrawing")).GetComponent<LineRenderer>().startColor = color;
        ((GameObject)Resources.Load("LineDrawing")).GetComponent<LineRenderer>().endColor = color;
        RpcChangeColor(color);
        GameObject line = (GameObject)Instantiate(Resources.Load("LineDrawing"), app.view.userInterface.annotations.GetComponent<Transform>());
        lineAnnotations.Add(line);
        NetworkServer.Spawn(line);
        lineUpdater = line.GetComponent<LineUpdater>();
        CmdUpdateTriggered(true);
        lineUpdater.triggered = true;
        RpcSetMinTime(line);
    }
    [Command]
    public void CmdUpdateTriggered(bool state)
    {
        lineUpdater.triggered = state;
        RpcUpdateTriggered(state);
    }
    [ClientRpc]
    public void RpcUpdateTriggered(bool state)
    {
		if(lineUpdater!=null) //prevent null error
           lineUpdater.triggered = state;
    }
    [ClientRpc]
    public void RpcSetMinTime(GameObject line)
    {
      line.GetComponent<LineUpdater>().minTime=app.model.users.local.refTime;
       
    }
    [Command]
    public void CmdStopDraw()
    {
        lineUpdater.triggered = false;
        if (savePath == true)
        {
            text(new String[1] { "end" }, app.model.users.local.newdoc);
        }
    }

	bool is_url_setup=false;

    public void Update()
    {
		if (is_url_setup == false) {
			if (filePath.ToLower ().Contains ("stereo") == true) {
				app.controller.videoController.mp.m_StereoPacking = StereoPacking.TopBottom;
				//app.controller.videoController.mp.m_DisplayDebugStereoColorTint = true; //for debuging if stereo is working -DJZ
				print ("filename contains stereo tag, so setting up top/bottom stereo settings");
			} else {
				app.controller.videoController.mp.m_StereoPacking = StereoPacking.None;
				print ("filename doesn't contain stereo tag, so setting up no stereo (mono)");
			}
			app.controller.videoController.mp.OpenVideoFromFile(MediaPlayer.FileLocation.AbsolutePathOrURL, filePath, true); //for some reason starting in Start function throws error... -DJZ

			is_url_setup = true;
		}

        if (isLocalPlayer)
        {
            // Update data here, values not passed as arguments end up being assigned the server values.
            CmdUpdateData(app.view.cameras.mainCamera.transform.eulerAngles);
			//print ("Euler: " + app.view.cameras.mainCamera.transform.eulerAngles);
            CmdUpdateVoiceState(vc.isTransmittingAutoDetectedVoice);
        }


    }

    public ClientState currentState()
    {
        ClientState state = new ClientState();
        state.userId = userId;
        state.userName = userName;
        state.userRole = userRole;
        state.userAvatar = userAvatar;
        state.userColor = playerColor;
        state.viewingAngle = viewingAngle;
        state.isPlaying = isPlaying;
        state.isVoicePlaying = isVoicePlaying;
        state.fp = filePath;
        state.ap = annotationPath;
        state.asp = annotationSavePath;
        state.time = time;
        state.arrow = arrow;
        state.refPos = refPos;
        state.configPath = configPath;
        state.rewind = rewind;
        state.newdoc = newdoc;
        state.refTime = refTime;
        state.playFeat = playFeat;
        state.chatFeat = chatFeat;
        state.drawFeat = drawFeat;
        state.lookFeat = lookFeat;

        return state;
    }

    [Command]
    public void CmdUpdateData(Vector3 angles)
    {
        // Update data on:
        // - Where each client is looking
        // - What commands each user inputs
        // 
        viewingAngle = angles;
    }

    [Command]
    public void CmdUpdateVoiceState(bool isVoiceOn)
    {
        isVoicePlaying = isVoiceOn;
    }

    [Command]
    public void CmdUpdateNewDoc(bool state)
    {
        newdoc = state;
        for (int i = 0; i < app.model.users.userList.Count; i++)
        {
            app.model.users.userList[i].newdoc = state;
        }
        RpcUpdateNewDoc(state);
    }
    [ClientRpc]
    public void RpcUpdateNewDoc(bool state)
    {
        app.model.users.local.newdoc = state;
    }

    [Command]
    public void CmdChangeState(bool state)
    {
        isPlaying = state;
        for (int i = 0; i < app.model.users.userList.Count; i++)
        {
            app.model.users.userList[i].isPlaying = state;
        }
        RpcChangeState(state);
    }
    [ClientRpc]
    public void RpcChangeState(bool state)
    {
        app.model.users.local.isPlaying = state;
    }
    [Command]
    public void CmdRewind(bool state)
    {
        rewind = state;
        for (int i = 0; i < app.model.users.userList.Count; i++)
        {
            app.model.users.userList[i].rewind = state;
        }
        RpcRewind(state);

    }
    [ClientRpc]
    public void RpcRewind(bool state)
    {
        app.model.users.local.rewind = state;
    }

    [Command]
    public void CmdInitializeData(ClientState state)
    {
        userId = state.userId;
        userName = state.userName;
        userRole = state.userRole;
        userAvatar = state.userAvatar;
        playerColor = state.userColor;
        viewingAngle = state.viewingAngle;
        isVoicePlaying = state.isVoicePlaying;
        isPlaying = state.isPlaying;
        filePath = state.fp;
        annotationPath = state.ap;
        annotationSavePath = state.asp;
        time = state.time;
        arrow = state.arrow;
        refPos = state.refPos;
        configPath = state.configPath;
        rewind = state.rewind;
        newdoc = state.newdoc;
        refTime = state.refTime;
        playFeat = state.playFeat;
        chatFeat = state.chatFeat;
        drawFeat = state.drawFeat;
        lookFeat = state.lookFeat;

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
            CmdUpdateVertex(hit.point);
        }
    }

    [Command]
    public void CmdUpdateVertex(Vector3 newVert)
    {
        lineUpdater.maxTime = Math.Max(lineUpdater.maxTime, app.model.users.local.refTime);
        lineUpdater.newPos = newVert;
        lineUpdater.RpcAddVertex(newVert);
        String[] values = new String[] { string.Format("{0:N5}", lineUpdater.maxTime), string.Format("{0:N5}", lineUpdater.newPos) };
        if (savePath == true)
        {
            text(values, newdoc);
            CmdUpdateNewDoc(true);
        }
    }
    [ClientRpc]
    void RpcUpdateMinTime()
    {
       
        lineUpdater.minTime = app.model.users.local.refTime;
        print("updatemintime");
    }
    [ClientRpc]
    void RpcChangeColor(Color color)
    {
        ((GameObject)Resources.Load("LineDrawing")).GetComponent<LineRenderer>().startColor = color;
        ((GameObject)Resources.Load("LineDrawing")).GetComponent<LineRenderer>().endColor = color;

    }
    public void text(String[] lines, bool doc)
    {
        // Set a variable to the My Documents path.
        string mydocpath = docPath;

        // Write the string array to a new file named "WriteLines.txt".
        using (StreamWriter outputFile = new StreamWriter(mydocpath, doc))
        {
            doc = true;
            foreach (string line in lines)
                outputFile.WriteLine(line);
        }
    }
    [Command]
    public void CmdArrowon(Vector3 vector)
    {
        for (int i = 0; i < app.model.users.userList.Count; i++)
        {
            app.model.users.userList[i].arrow = true;
            app.model.users.userList[i].refPos = vector;
        }
        RpcArrowon(vector);

    }
    [ClientRpc]
    public void RpcArrowon(Vector3 vector)
    {
        app.model.users.local.arrow = true;
        app.model.users.local.refPos = vector;

    }


    [Command]
    public void CmdArrowOff()
    {
        arrow = false;
    }

    [Command]
    public void CmdDrawFromFile(String[] myArray, float minTime, float maxTime)
    {
        List<Vector3> vertices = new List<Vector3>();
        ((GameObject)Resources.Load("LineDrawing")).GetComponent<LineRenderer>().startColor = Color.green;
        ((GameObject)Resources.Load("LineDrawing")).GetComponent<LineRenderer>().endColor = Color.green;
        GameObject line = (GameObject)Instantiate(Resources.Load("LineDrawing"), app.view.userInterface.annotations.GetComponent<Transform>());
        //lineAnnotations.Add(line);
        NetworkServer.Spawn(line);
        lineUpdater = line.GetComponent<LineUpdater>();
        lineUpdater.minTime = minTime;
        lineUpdater.maxTime = maxTime;
        lineUpdater.triggered = true;
        for (int i = 0; i < myArray.Length; i++)
        {
            Vector3 pos = stringToVec(myArray[i]);
            vertices.Add(pos);
            //vertices.Add(pos);
            //lineUpdater.newPos = pos;
            //lineUpdater.RpcAddVertex(pos);
            CmdUpdateNewVertex(pos);
        }
        lineUpdater.vertices = vertices;
        lineUpdater.triggered = false;
        //lineUpdater.vertices = vertices;
    }

    [Command]
    public void CmdUpdateNewVertex( Vector3 pos)
    {
        lineUpdater.newPos = pos;
        lineUpdater.RpcAddVertex(pos);
    }
    public Vector3 stringToVec(string s)
    {
        string[] temp = s.Substring(1, s.Length - 2).Split(',');
        return new Vector3(float.Parse(temp[0]), float.Parse(temp[1]), float.Parse(temp[2]));
    }

}