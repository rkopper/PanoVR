using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PermissionEnum;

public class DataHolderScript : MonoBehaviour {

    public Text textField;

    public Text errorField;

    public string filePath;

    public Text videoInfo;

    private PermissionCategories permissionLevelLocalHolder;

    private void Awake()
    {
        GameObject.DontDestroyOnLoad(this);
    }

    // Use this for initialization
    

	// Use this for initialization
	void Start () {
    }
	
	// Update is called once per frame
	void Update () {
	}

    public void SavePath()
    {
        if (System.IO.File.Exists(textField.text) && (textField.text.Contains(".mp4") || textField.text.Contains(".MP4")))
        {
            filePath = textField.text;
            errorField.enabled = false;
            videoInfo.text = filePath;
        }
        else
        {
            errorField.enabled = true;
        }
    }

    public void SavePermission(PermissionCategories permission)
    {
        permissionLevelLocalHolder = permission;
        print("data saved " + permissionLevelLocalHolder);
    }

    public PermissionCategories RetrievePermission()
    {
        return permissionLevelLocalHolder;
    }
}
