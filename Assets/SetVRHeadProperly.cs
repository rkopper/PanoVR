using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetVRHeadProperly : MonoBehaviour {
	bool isSetup=false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (isSetup == false) {
			GameObject head=GameObject.Find ("HeadNode"); 
			if(head==null) return;

			Camera c = head.AddComponent<Camera>();
			c.name = "DaveReferenceCamera";

			if (c == null)	return;

			this.GetComponent<RenderHeads.Media.AVProVideo.UpdateStereoMaterial> ()._camera = c;
			print ("setup fake camera for shader to determine Left vs Right eye");
			isSetup = true;
		}
	}
}
