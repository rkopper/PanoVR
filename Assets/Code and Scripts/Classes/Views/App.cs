using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// BounceApplication.cs

// Base class for all elements in this application.


/*public class ApplicationElement : MonoBehaviour //pulling this so we get cached app only from our static class -DJZ
{
	private App cachedApp = null; //DJZ - lets avoid doing findobject on every frame

    // Gives access to the application and all instances.
	public App app 
	{ 
		get { 
			if(cachedApp==null) //DJZ - only do the search if we don't have it. 
			    cachedApp=GameObject.FindObjectOfType<App>(); 
			return cachedApp;		
		} 
	}
} */

public class App : MonoBehaviour
{
    // Reference to the root instances of the MVC.
    public ApplicationModel model;
    public ApplicationView view;
    public ApplicationController controller;
}