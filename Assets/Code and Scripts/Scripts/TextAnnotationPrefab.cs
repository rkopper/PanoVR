using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextAnnotationPrefab : MonoBehaviour {
   // public App app { get { return GameObject.FindObjectOfType<App>(); } }
	private App app;

    public ClientScript owner;
    public float openAnimDuration, closeAnimDuration;

    // Spherical Indicator
    public GameObject iconSphere, rippleSphere, ringSphere;
    
    // Canvas elements
    public GameObject textCanvas;

    public bool openOnView = true;

    private bool prefabOpen = false;
    private Transform cameraTransform;



    void Start () {
		app=GameObject.FindObjectOfType<App>(); 
		if (app == null)
			Debug.LogError ("Can't find App script");
		
        cameraTransform = app.view.cameras.mainCamera.GetComponent<Transform>();
        // Let's orient the prefab towards the camera, i.e. origin
        gameObject.GetComponent<Transform>().LookAt(cameraTransform);
        // Open the annotation
        openAnim();
    }
	
	// Update is called once per frame
	void Update () {
        /* Raycast code, maybe useful?
        
        float length = 100.0f;
        RaycastHit hit;
        Vector3 rayDirection = cameraTransform.TransformDirection(Vector3.forward);
        Vector3 rayStart = cameraTransform.position + rayDirection;      // Start the ray away from the player to avoid hitting itself
        Debug.DrawRay(rayStart, rayDirection * length, Color.green);
        if (Physics.Raycast(rayStart, rayDirection, out hit, length))
        {
            if (hit.collider.tag == "TextAnnotation")
            {
                if (!prefabOpen && openOnView) {
                    openAnim();
                }
            }
        } else
        {
            if (prefabOpen && openOnView)
            {
                closeAnim();
            }
        }*/
    }

    public void setText(string newText)
    {
        UnityEngine.UI.Text textBox =  textCanvas.GetComponentInChildren<UnityEngine.UI.Text>();
        //print("old Text: " + textBox.text);
        textBox.text = newText; 
    }

    public void openAnim()
    {

        // Open the center icon
        Transform iconT = iconSphere.GetComponent<Transform>();
        StartCoroutine(ScaleBy(iconT, .35f, openAnimDuration*.8f, ScaleBy(iconT, -0.1f, openAnimDuration*.2f,end())));

        // Open the ring icon
        Transform ringT = ringSphere.GetComponent<Transform>();
        StartCoroutine(ScaleBy(ringT, .6f, openAnimDuration * .8f, end()));

        // Create a ripple
        Transform rippleT = rippleSphere.GetComponent<Transform>();
        Material rippleM = rippleSphere.GetComponent<MeshRenderer>().material;
        StartCoroutine(ScaleBy(rippleT, 2.0f, openAnimDuration, end()));
        StartCoroutine(toggleActive(rippleSphere, ScaleMaterialComponentBy(rippleM, "_Outline", -.1f, openAnimDuration * .5f, toggleActive(rippleSphere, end()))));

        // Open the canvas
        RectTransform canvasT = textCanvas.GetComponent<RectTransform>();
        StartCoroutine(ScaleByX(canvasT, 0.02f, openAnimDuration*.2f, ScaleByY(canvasT, 0.02f, openAnimDuration*.4f,end())));

        prefabOpen = true;
    }

    public void closeAnim()
    {
        // Close the center icon
        Transform iconT = iconSphere.GetComponent<Transform>();
        StartCoroutine(ScaleBy(iconT, -.25f, closeAnimDuration, end()));

        // Close the ring icon
        Transform ringT = ringSphere.GetComponent<Transform>();
        StartCoroutine(ScaleBy(ringT, -.6f, closeAnimDuration*.8f, end()));

        // Reset the ripple
        Transform rippleT = rippleSphere.GetComponent<Transform>();
        Material rippleM = rippleSphere.GetComponent<MeshRenderer>().material;
        StartCoroutine(ScaleBy(rippleT, -2.0f, closeAnimDuration, end()));
        StartCoroutine(ScaleMaterialComponentBy(rippleM, "_Outline", .1f, closeAnimDuration, end()));

        // Close the canvas
        RectTransform canvasT = textCanvas.GetComponent<RectTransform>();
        StartCoroutine(ScaleByY(canvasT, -0.02f, closeAnimDuration * .4f, ScaleByX(canvasT, -0.02f, closeAnimDuration * .2f, end())));

        prefabOpen = false;
    }
    
    private IEnumerator end()
    {
        yield return 0;
    }

    private IEnumerator ScaleBy(Transform c, float size, float duration, IEnumerator nextCoroutine)
    {
        int steps = (int)(duration * 60);
        for (int i = 0; i < steps; i++)
        {
            Vector3 oldScale = c.localScale;
            float newSize = oldScale.x + size / steps;
            c.localScale = new Vector3(newSize, newSize, newSize);
            yield return new WaitForSeconds(duration / steps);
        }
        StartCoroutine(nextCoroutine);
    }

    private IEnumerator ScaleByX(Transform c, float size, float duration, IEnumerator nextCoroutine)
    {
        int steps = (int)(duration * 60);
        for (int i = 0; i < steps; i++)
        {
            Vector3 oldScale = c.localScale;
            float newSize = oldScale.x + size / steps;
            c.localScale = new Vector3(newSize, oldScale.y, oldScale.z);
            yield return new WaitForSeconds(duration / steps);
        }
        StartCoroutine(nextCoroutine);
    }

    private IEnumerator ScaleByY(Transform c, float size, float duration, IEnumerator nextCoroutine)
    {
        int steps = (int)(duration * 60);
        for (int i = 0; i < steps; i++)
        {
            Vector3 oldScale = c.localScale;
            float newSize = oldScale.y + size / steps;
            c.localScale = new Vector3(oldScale.x, newSize, oldScale.z);
            yield return new WaitForSeconds(duration / steps);
        }
        StartCoroutine(nextCoroutine);
    }

    private IEnumerator ScaleByZ(Transform c, float size, float duration, IEnumerator nextCoroutine)
    {
        int steps = (int)(duration * 60);
        for (int i = 0; i < steps; i++)
        {
            Vector3 oldScale = c.localScale;
            float newSize = oldScale.z + size / steps;
            c.localScale = new Vector3(oldScale.x, oldScale.y, newSize);
            yield return new WaitForSeconds(duration / steps);
        }
        StartCoroutine(nextCoroutine);
    }

    private IEnumerator ScaleMaterialComponentBy(Material m, string field, float value, float duration, IEnumerator nextCoroutine)
    {
        int steps = (int)(duration * 60);
        for (int i = 0; i < steps; i++)
        {
            float oldWidth = m.GetFloat(field);
            float newSize = oldWidth + value / steps;
            m.SetFloat(field, newSize);
            yield return new WaitForSeconds(duration / steps);
        }

        StartCoroutine(nextCoroutine);
    }

    private IEnumerator toggleActive(GameObject o,IEnumerator nextCoroutine)
    {
        o.SetActive(!o.active);
        StartCoroutine(nextCoroutine);
        yield return 0;
    }
}
