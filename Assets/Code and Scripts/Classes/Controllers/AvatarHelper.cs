using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AvatarHelper : MonoBehaviour {

    public Canvas c;
	private App app;

	// Use this for initialization
	void Start() {
		app=GameObject.FindObjectOfType<App>(); 
		if (app == null)
			Debug.LogError ("Can't find App script");
		
        c.worldCamera = app.view.cameras.mainCamera;
	}
	
	// Update is called once per frame
	void Update () {
        if(c.worldCamera == null)
        {
			c.worldCamera = app.view.cameras.mainCamera;
			if (c.worldCamera == null) {
				print ("camera not initialized yet"); //prevent throwing error
				return;
			}
			gameObject.transform.parent = app.view.cameras.mainCamera.transform;
            gameObject.transform.localPosition = new Vector3(0.0f, 0.0f, 1.5f);
            gameObject.GetComponent<RectTransform>().localEulerAngles = new Vector3(0, 0, 0);
        }
       // c.transform.eulerAngles = c.transform.eulerAngles;
	}
}
