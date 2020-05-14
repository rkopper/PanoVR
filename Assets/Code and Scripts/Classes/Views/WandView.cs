using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WandView : MonoBehaviour {
    public GameObject wand;
	private App app;

	// Use this for initialization
	void Start () {
		app=GameObject.FindObjectOfType<App>(); 
		if (app == null)
			Debug.LogError ("Can't find App script");
	}
	
	// Update is called once per frame
	void Update () {
        if (wand == null)
        {
            wand = app.view.cameras.vrCenterNode.GetComponentInChildren<VRWand>().gameObject;
        }
		
	}
}
