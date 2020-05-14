using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigHook : MonoBehaviour
{

    public string config;
    GameObject about;
    ConfigDropdown configdropdown;
    bool isNull = true;

    // Use this for initialization
    void Start()
    {
        config = "oculus_touch.vrx";
    }

    // Update is called once per frame
    void Update()
    {
        about = GameObject.Find("AboutPanel");
      //  configdropdown = about.GetComponent<ConfigDropdown>();
       // config = configdropdown.currentDevice;
    }
}
