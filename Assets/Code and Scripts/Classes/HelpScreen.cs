using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpScreen : MonoBehaviour
{

    public GameObject help, keyboard, Aline, Bline, Tline, HTline, Sline, playWords, helpWords, pushWords, shiftWords, drawWords, rewindWords, callWords, deleteWords;
    public vrWand rightwand;
    public bool screenOn = true;
    public bool keyboardOn = false;
    public bool shift = false;
    //public App app { get { return GameObject.FindObjectOfType<App>(); } }

	bool help_setup=false;

	private App app;

    // Use this for initialization
    void Start()
    {
		app=GameObject.FindObjectOfType<App>(); 
		if (app == null)
			Debug.LogError ("Can't find App script");
		
        rightwand = MiddleVR.VRDeviceMgr.GetWand("Wand0");
        updateScreen();
    }

    // Update is called once per frame
    void Update()
    {
		if (help_setup == false && app.view.cameras.mainCamera != null) {
			help.transform.localEulerAngles = new Vector3 (0, 0, 0);
			help.transform.parent = app.view.cameras.mainCamera.transform;
			help.transform.localPosition = new Vector3 (0.0f, 0f, 1.5f);
			keyboard.transform.localEulerAngles = new Vector3 (0, 0, 0);
			keyboard.transform.parent = app.view.cameras.mainCamera.transform;
			keyboard.transform.localPosition = new Vector3 (0.0f, 0f, 1.5f);

			help_setup = true;
		}

        if (rightwand.IsButtonPressed(2))
        {
            shift = true;
        }
        else
        {
            shift = false;
        }

        if (rightwand.IsButtonToggled(3, true))
        {
            updateScreen();
        }
        if (screenOn)
        {
            shiftCommands();
        }
        if (MiddleVR.VRDeviceMgr.IsKeyToggled(MiddleVR.VRK_H))
        {
            updateKeyboard();
        }
    }
    void updateScreen()
    {
        if (!screenOn)
        {
            Aline.SetActive(true);
            Bline.SetActive(true);
            Tline.SetActive(true);
            HTline.SetActive(true);
            Sline.SetActive(true);
            playWords.SetActive(true);
            helpWords.SetActive(true);
            pushWords.SetActive(true);
            shiftWords.SetActive(true);
            drawWords.SetActive(true);
            rewindWords.SetActive(true);
            callWords.SetActive(true);
            deleteWords.SetActive(true);
            screenOn = true;
        }
        else
        {
            Aline.SetActive(false);
            Bline.SetActive(false);
            Tline.SetActive(false);
            HTline.SetActive(false);
            Sline.SetActive(false);
            playWords.SetActive(false);
            helpWords.SetActive(false);
            pushWords.SetActive(false);
            shiftWords.SetActive(false);
            drawWords.SetActive(false);
            rewindWords.SetActive(false);
            callWords.SetActive(false);
            deleteWords.SetActive(false);
            screenOn = false;
        }
    }
    void shiftCommands()
    {
        if (shift)
        {
            HTline.SetActive(false);
            playWords.SetActive(false);
            pushWords.SetActive(false);
            shiftWords.SetActive(false);
            drawWords.SetActive(false);
            rewindWords.SetActive(true);
            callWords.SetActive(true);
            deleteWords.SetActive(true);
        }
        else
        {
            HTline.SetActive(true);
            playWords.SetActive(true);
            pushWords.SetActive(true);
            shiftWords.SetActive(true);
            drawWords.SetActive(true);
            rewindWords.SetActive(false);
            callWords.SetActive(false);
            deleteWords.SetActive(false);
        }
    }
    void updateKeyboard()
    {
        if (!keyboardOn)
        {
            keyboard.SetActive(true);
            keyboardOn = true;
        }
        else
        {
            keyboard.SetActive(false);
            keyboardOn = false;
        }
    }
}
