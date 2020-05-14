using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshColor : MonoBehaviour {
//    public App app { get { return GameObject.FindObjectOfType<App>(); } }
	private App app;

	public Material m;
    public GameObject ray;

    // Use this for initialization
    void Start () {
		app=GameObject.FindObjectOfType<App>(); 
		if (app == null)
			Debug.LogError ("Can't find App script");
    }
	
	// Update is called once per frame
	void Update () {
        m.SetColor("_SpecColor", app.model.users.local.playerColor);
        m.SetColor("_Emission", app.model.users.local.playerColor);
        m.SetColor("_Color", app.model.users.local.playerColor);
        ray.GetComponent<MeshRenderer>().material = m;
    }
}
