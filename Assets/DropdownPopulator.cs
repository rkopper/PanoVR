using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.IO;
using UnityEngine.UI;

public class DropdownPopulator : MonoBehaviour {

    public Dropdown videoDropdown;
    public Dropdown annotationDropdown;
	public Dropdown remoteDropdown; 
	public CreateServerManager ourServerManager;

    public List<string> videoFileNameList = new List<string>() { "" };
    public List<string> annotationFileNameList = new List<string>() { "" };

    public string filePath;
    public string annotationPath;

    string urlFetch = "ftp://rapid-702.vm.duke.edu/html/";
    string urlAccess = "http://rapid-702.vm.duke.edu/";
    public string username = "rayan";
    public string password = "1234567";

	void restore_remote_values()
	{
		if (videoDropdown.value < videoDropdown.options.Count) 
			Dropdown_IndexChangedVideo (videoDropdown.value);
		if(annotationDropdown.value<annotationDropdown.options.Count)
			Dropdown_IndexChangedAnnotations (annotationDropdown.value);
	}


    // Use this for initialization
    void Start () {

		GrabFilesFTP();

		videoDropdown.RefreshShownValue ();
		annotationDropdown.RefreshShownValue ();
		remoteDropdown.RefreshShownValue ();       

		restore_remote_values ();
    }

	public void Dropdown_IndexRemoteOrLocal(int index)
	{
		print ("user has changed remote or local to: " + index);

		if (index == 1) {

			print ("turning off the remote dropdowns");
			videoDropdown.gameObject.SetActive (false);
			annotationDropdown.gameObject.SetActive (false);

			videoDropdown.gameObject.transform.parent.Find ("BrowseButton").gameObject.SetActive (true);
			videoDropdown.gameObject.transform.parent.Find ("InputFile").gameObject.SetActive (true);
			annotationDropdown.gameObject.transform.parent.Find ("BrowseButton").gameObject.SetActive (true);
			annotationDropdown.gameObject.transform.parent.Find ("InputFile").gameObject.SetActive (true);

			ourServerManager.restoreVideoPath ();
			ourServerManager.restoreAnnotationPath ();

		} else {
			videoDropdown.gameObject.transform.parent.Find ("BrowseButton").gameObject.SetActive (false);
			videoDropdown.gameObject.transform.parent.Find ("InputFile").gameObject.SetActive (false);
			annotationDropdown.gameObject.transform.parent.Find ("BrowseButton").gameObject.SetActive (false);
			annotationDropdown.gameObject.transform.parent.Find ("InputFile").gameObject.SetActive (false);

			videoDropdown.gameObject.SetActive (true);
			annotationDropdown.gameObject.SetActive (true);

			restore_remote_values ();
		}
	}

    public void Dropdown_IndexChangedVideo(int index)
    {
        //change video file path
        filePath = urlAccess + videoFileNameList[index];
        print("setting video path to: " + filePath);
    }

    public void Dropdown_IndexChangedAnnotations(int index)
    {
        //change annotation file path
        annotationPath = urlAccess + annotationFileNameList[index];
		print ("setting annotation path to: " + annotationPath);
    }

    // Update is called once per frame
    void Update () {
		
        /*if(Input.GetKey(KeyCode.Space)) //buggy - just keeps adding to the list -DJZ
        {
            GrabFilesFTP();
        }*/
	}
    
    void GrabFilesFTP()
    {
        videoFileNameList.Clear();
        annotationFileNameList.Clear();

        FtpWebRequest request = (FtpWebRequest)WebRequest.Create(urlFetch);
        request.Method = WebRequestMethods.Ftp.ListDirectory;

        request.Credentials = new NetworkCredential(username, password);

        FtpWebResponse response = (FtpWebResponse)request.GetResponse();

        Stream responseStream = response.GetResponseStream();
        StreamReader reader = new StreamReader(responseStream);

        string line = reader.ReadLine();
        while (!string.IsNullOrEmpty(line))
        {
            if (line.Length > 3)
            {
                string extension = line.Substring(line.Length - 4, 4);
                if (extension == ".mp4")
                {
                    videoFileNameList.Add(line);
                    print(urlFetch + line);
                }

                if (extension == ".txt")
                {
                    annotationFileNameList.Add(line);
                    print(urlFetch + line);

                }

            }
            line = reader.ReadLine();
        }

        reader.Close();
        response.Close();

		videoDropdown.ClearOptions ();
        videoDropdown.AddOptions(videoFileNameList);
		videoDropdown.RefreshShownValue ();

		annotationDropdown.ClearOptions ();
        annotationDropdown.AddOptions(annotationFileNameList);
		annotationDropdown.RefreshShownValue ();
    }
}
