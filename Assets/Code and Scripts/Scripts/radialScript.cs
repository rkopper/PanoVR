using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class radialScript : MonoBehaviour {

    public float myAngle = 0;
	private App app;

    // Use this for initialization
    void Start () {
		app=GameObject.FindObjectOfType<App>(); 
		if (app == null)
			Debug.LogError ("Can't find App script");

        float avatarSize = gameObject.GetComponent<RectTransform>().sizeDelta.x;
        Texture2D newTex = CalculateTexture((int)avatarSize, (int)avatarSize, avatarSize / 2, avatarSize / 2, avatarSize / 2);
        gameObject.GetComponent<RawImage>().texture = newTex;
    }

    private Texture2D CalculateTexture(int h, int w, float r, float cx, float cy)
    {
        var cam = app.view.cameras.mainCamera;
        var radAngle = cam.fieldOfView * Mathf.Deg2Rad;
        var hFOV = 2 * Mathf.Atan(Mathf.Tan(radAngle / 2) * cam.aspect);

        var xSpan = hFOV / 2.0f;

        Texture2D b = new Texture2D(h, w);
        for (int i = (int)(cx - r); i < cx + r; i++)
        {
            for (int j = (int)(cy - r); j < cy + r; j++)
            {
                float dx = i - cx;
                float dy = j - cy;
                float d = Mathf.Sqrt(dx * dx + dy * dy);
                float angle = Mathf.Acos(dx / d);
                if (d <= r && angle > Mathf.PI/2-xSpan && angle < Mathf.PI/2+xSpan && dy > 0)
                    b.SetPixel(i - (int)(cx - r), j - (int)(cy - r), Color.white);
                else
                    b.SetPixel(i - (int)(cx - r), j - (int)(cy - r), Color.clear);
            }
        }
        b.Apply();
        return b;
    }
}
