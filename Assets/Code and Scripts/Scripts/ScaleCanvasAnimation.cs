using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleCanvasAnimation : MonoBehaviour {
    public float endSize, endDuration;
    public float endWidth = 0;
    private Transform t;
    private Material m;

    // Basically increase size at certain rate, while reducing 
    // Use this for initialization
    void Start()
    {
        t = gameObject.GetComponent<RectTransform>();
        StartCoroutine(ScaleX(.02f,.25f));
    }

    private IEnumerator ScaleX(float size, float duration)
    {
        int steps = (int)(duration * 60);
        for (int i = 0; i < steps; i++)
        {
            Vector3 oldScale = t.localScale;
            float newSize = oldScale.x + size / steps;
            t.localScale = new Vector3(newSize, oldScale.y, oldScale.z);
            yield return new WaitForSeconds(duration / steps);
        }
        StartCoroutine(ScaleY(.02f, .35f));
    }
    private IEnumerator ScaleY(float size, float duration)
    {
        int steps = (int)(duration * 60);
        for (int i = 0; i < steps; i++)
        {
            Vector3 oldScale = t.localScale;
            float newSize = oldScale.y + size / steps;
            t.localScale = new Vector3(oldScale.x, newSize, oldScale.z);
            yield return new WaitForSeconds(duration / steps);
        }
    }
}
