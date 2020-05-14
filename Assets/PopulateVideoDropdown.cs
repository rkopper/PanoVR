using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.UI;
using Renci.SshNet.Common;
using Renci.SshNet.Sftp;
using Renci.SshNet;

public class PopulateVideoDropdown : MonoBehaviour
{
    //populate the dropdown with all the videos present on the server   

    public Dropdown videoDropdown;
    public Dropdown annotationDropdown;

    public List<string> videoFileNameList = new List<string>() { "Select video file" };
    public List<string> annotationFileNameList = new List<string>() { "Select Annotation file" };

    public string filePath;
    public string annotationPath;

    string host = "rapid-702.vm.duke.edu";
    string temp = "http://rapid-702.vm.duke.edu/";
    int port = 22;
    string username = "rayan";
    string password = "1234567";
    string remoteDirectory = "/DATA/";

    bool t = true;

    public void Dropdown_IndexChangedVideo(int index)
    {
        //videoName.text = fileNameList[index];
        //change video file path
        filePath = temp + videoFileNameList[index];
    }

    public void Dropdown_IndexChangedAnnotation(int index)
    {
        //videoName.text = fileNameList[index];
        //change video file path
        annotationPath = host + remoteDirectory + annotationFileNameList[index];
    }

    void Start()
    {
        videoDropdown.AddOptions(videoFileNameList);
        annotationDropdown.AddOptions(annotationFileNameList);
    }

    void Update()
    {
        if (t == true)
        {
            PopulateLists();
            t = false;
        }
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        // }
    }

    void PopulateLists()
    {
        using (var sftp = new SftpClient(host, username, password))
        {
            sftp.Connect();
            var files = sftp.ListDirectory(remoteDirectory);

            foreach (var file in files)
            {
                string fileName = file.Name;
                if (fileName.Length > 3)
                {
                    string extension = fileName.Substring(fileName.Length - 4, 4);
                    if (extension == ".mp4")
                    {
                        videoFileNameList.Add(fileName);
                        print(host + remoteDirectory + fileName);
                    }

                    if (extension == ".txt")
                    {
                        annotationFileNameList.Add(fileName);
                        print(host + remoteDirectory + fileName);
                    }

                }

            }
        }

        videoDropdown.ClearOptions();
        annotationDropdown.ClearOptions();
        videoDropdown.AddOptions(videoFileNameList);
        annotationDropdown.AddOptions(annotationFileNameList);

    }


}
