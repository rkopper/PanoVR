using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MiddleVR_Unity3D;

public class TakeNodePosition : MonoBehaviour {
    public vrNode3D thisNode;
    public Transform thisTransform;
	// Use this for initialization
	void Start () {
        thisNode = MVRNodesMapper.Instance.GetNode(gameObject);
        thisTransform = gameObject.GetComponent<Transform>();
	}
	
	// Update is called once per frame
	void Update () {
        vrQuat v = thisNode.GetOrientationLocal();
        Quaternion v3 = MVRTools.ToUnity(v);
        print(v3.x);
        thisTransform.localRotation = v3;
    }
}
