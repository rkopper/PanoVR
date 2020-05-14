using Prototype.NetworkLobby;
using RenderHeads.Media.AVProVideo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VoiceChat;
using VoiceChat.Networking;

public class CreateServerManager : MonoBehaviour {

    public LobbyManager lobbyManager;
    public RectTransform lobbyPanel;

    // State Variables
    public string serverName = "";
    public string filePath = "";
    public string annotationPath = "";
    public string annotationSavePath = "";
    public int numClients = 2;
    public bool usePassword = false;
    public bool saveAnnotations = false;
    public bool loadAnnotations = false;
    public string password = "";
    public string annotationsFile = "";


    public GameObject dropdownManager;

    // Helper Variables
    public bool validFilePath = false;
    public bool validAnnotationPath = false;
    public bool validAnnotationSavePath = false;

    // File Browser Variables
    public string _sStartPath; // The path of the folder to display when opening the le browser
    public FileBrowser_UI.FetchMode _eMode; // The mode to use : select le, select folder, or save le
    public FileBrowser_UI.ToDisplay _eDisplay; // Display only les, only folders, or both
    public string _sDefaultExtension; // The extension applied when saving a le. Empty means any extension can be provided

    public GameObject pwInput, annotationsInput, browserButton, browseButton, loadInput, errorText;
	public InputField videoFileInput, annotationInput, annotationSaveInput;
    public MediaPlayer mp; 

    // Outputs
    public UnityEngine.UI.Text maxClientsLabel;


    // Use this for initialization
    void Start() {
        _sStartPath = "D:/";
        _eMode = FileBrowser_UI.FetchMode.SelectFile;
        _eDisplay = FileBrowser_UI.ToDisplay.Both;
        _sDefaultExtension = ".txt";
    }

    // Update is called once per frame
    void Update() {

    }

    public void BrowseVideoClicked()
    {
        if (!FileBrowser_UI.Instance._bIsOpen)
        {
            StartCoroutine(WaitForResult());
        }
    }

    public void BrowseAnnotationClicked()
    {
        if (!FileBrowser_UI.Instance._bIsOpen)
        {
            StartCoroutine(WaitForAnnotationResult());
        }
    }

    public void BrowseSaveClicked()
    {
        if (!FileBrowser_UI.Instance._bIsOpen)
        {
            StartCoroutine(WaitForSaveResult());
        }
    }

    IEnumerator WaitForResult() {
        FileBrowser_UI.Instance.ShowWindow(_sStartPath, _eDisplay, _eMode, _sDefaultExtension);

		while (FileBrowser_UI.Instance._bIsOpen) {
			
			yield return null;
		}

		videoFileInput.text = FileBrowser_UI.Instance._sResult;
		newVideoPathLocal(videoFileInput.textComponent);
    }

	public void restoreVideoPath()
	{
		newVideoPathLocal(videoFileInput.textComponent);
	}

    IEnumerator WaitForAnnotationResult()
    {
        FileBrowser_UI.Instance.ShowWindow(_sStartPath, _eDisplay, _eMode, _sDefaultExtension);
        while (FileBrowser_UI.Instance._bIsOpen)
            yield return null;
        annotationInput.text = FileBrowser_UI.Instance._sResult;
        newAnnotationPath(annotationInput.textComponent);
    }

	public void restoreAnnotationPath()
	{
		newAnnotationPath(annotationInput.textComponent);
	}

    IEnumerator WaitForSaveResult()
    {
        FileBrowser_UI.Instance.ShowWindow(_sStartPath, _eDisplay, FileBrowser_UI.FetchMode.SaveFile, _sDefaultExtension);
        while (FileBrowser_UI.Instance._bIsOpen)
            yield return null;
        annotationSaveInput.text = FileBrowser_UI.Instance._sResult;
        newSavePath(annotationSaveInput.textComponent);
    }

    public void newServerName(UnityEngine.UI.Text newValue)
    {
        print("new server name called: " + newValue.text);
        serverName = newValue.text;
    }

    public void newVideoFilePath()
    {

        string newVideoPath = dropdownManager.GetComponent<DropdownPopulator>().filePath;
        //mp.OpenVideoFromFile(MediaPlayer.FileLocation.AbsolutePathOrURL, newVideoPath, false);
        lobbyManager.filePath = newVideoPath;
        validFilePath = true;

        /*filePath = newValue.text;
        // Verify file path.
        if ( filePath.EndsWith(".mp4", true, System.Globalization.CultureInfo.CurrentCulture))
        {
            validFilePath = true;
            mp.OpenVideoFromFile(MediaPlayer.FileLocation.AbsolutePathOrURL, filePath, false);
            lobbyManager.filePath = filePath;
            errorText.SetActive(false);
        }
        else {
            errorText.SetActive(true);
            lobbyManager.filePath = filePath;
            validFilePath = false;
            mp.CloseVideo();
        }*/
    }

	public void newVideoPathLocal(UnityEngine.UI.Text newValue)
	{
		string newVideoPath = newValue.text;
		// Verify file path.
		if (System.IO.File.Exists(newVideoPath) && newVideoPath.EndsWith(".mp4", true, System.Globalization.CultureInfo.CurrentCulture) || newVideoPath.Equals(null))
		{
			//mp.OpenVideoFromFile(MediaPlayer.FileLocation.AbsolutePathOrURL, newVideoPath, false);
			lobbyManager.filePath = newVideoPath;
			dropdownManager.GetComponent<DropdownPopulator> ().filePath = newVideoPath;
			validFilePath = true;
			print ("succesfully set video path to local video: " + newVideoPath);
		}
		else
		{
			validFilePath = false;
			mp.CloseVideo();
			print ("unable to set video path to local video");

			lobbyManager.filePath = "";
			dropdownManager.GetComponent<DropdownPopulator> ().filePath = "";
		}
	}


    public void newAnnotationPath(UnityEngine.UI.Text newValue)
    {
        annotationPath = newValue.text;
        // Verify file path.
        if (System.IO.File.Exists(annotationPath) && annotationPath.EndsWith(".txt", true, System.Globalization.CultureInfo.CurrentCulture) || annotationPath.Equals(null))
        {
            validAnnotationPath = true;
            lobbyManager.annotationPath = annotationPath;
			dropdownManager.GetComponent<DropdownPopulator> ().annotationPath = annotationPath;
        }
        else
        {
            validAnnotationPath = false;
            mp.CloseVideo();

			lobbyManager.annotationPath = "";
			dropdownManager.GetComponent<DropdownPopulator> ().annotationPath = "";
        }
    }

    public void newSavePath(UnityEngine.UI.Text newValue)
    {
        annotationSavePath = newValue.text;
        // Verify file path.
        if (System.IO.File.Exists(annotationSavePath) && annotationSavePath.EndsWith(".txt", true, System.Globalization.CultureInfo.CurrentCulture) || annotationPath.Equals(null))
        {
            validAnnotationSavePath = true;
            lobbyManager.annotationSavePath = annotationSavePath;
        }
        else
        {
            validAnnotationPath = false;
            mp.CloseVideo();
        }
    }

    public void newNumClients(UnityEngine.UI.Slider newValue)
    {
        maxClientsLabel.text = newValue.value + "  \uf0c0";
        numClients = (int) newValue.value;
    }

    public void newUsePassword(UnityEngine.UI.Toggle newValue)
    {
        usePassword = newValue.isOn;
        pwInput.SetActive(usePassword);
    }

    public void newSaveAnnotations(UnityEngine.UI.Toggle newValue)
    {
        saveAnnotations = newValue.isOn;
        annotationsInput.SetActive(saveAnnotations);
        browserButton.SetActive(saveAnnotations);
    }

    public void newLoadAnnotations(UnityEngine.UI.Toggle newValue)
    {
        loadAnnotations = newValue.isOn;
        loadInput.SetActive(loadAnnotations);
        browseButton.SetActive(loadAnnotations);
    }

    public void newPassword(UnityEngine.UI.Text newValue)
    {
        password = newValue.text;
    }

    public void newAnnotationFile(UnityEngine.UI.Text newValue)
    {
        annotationsFile = newValue.text;
    }

    public void createServer()
    {
        print(validFilePath);
        newVideoFilePath();
        // Make sure everything is filled out, otherwise, show some errors (Red highlight, etc.)
        if (validFilePath)
        {
            lobbyManager.StartMatchMaker();
            lobbyManager.matchMaker.CreateMatch(
                serverName, // Name
                (uint)numClients, // Number of clients
                true, // Advertise Match
                usePassword ? password : "", // Password
                "", // Public Client Address??
                "", // Private Client Address??
                0, // ELO for Match
                0, // Request Domain??
                lobbyManager.OnMatchCreate);

            lobbyManager.backDelegate = lobbyManager.StopHost;
            lobbyManager._isMatchmaking = true;
            lobbyManager.DisplayIsConnecting();

            lobbyManager.SetServerInfo("Matchmaker Host", lobbyManager.matchHost);
            lobbyManager.ChangeTo(lobbyPanel);
        }
        else
        {
            errorText.SetActive(true);
        }

    }

    VoiceChatNetworkProxy proxy;

    void OnConnectedToServer() { proxy = VoiceChatUtils.CreateProxy(); }

    void OnDisconnectedFromServer(NetworkDisconnection info) { GameObject.Destroy(proxy.gameObject); }

}
