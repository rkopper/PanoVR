using RenderHeads.Media.AVProVideo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    public vrWand rightWand;
    public vrWand leftWand;



    // Use this for initialization
    void Start()
    {
        // Retrieve input devices
        rightWand = MiddleVR.VRDeviceMgr.GetWand("Wand0");
        leftWand = MiddleVR.VRDeviceMgr.GetWand("Wand1");
    }

    // Update is called once per frame
    void Update()
    {
        // Delegate input to various other scripts.

    }
}

