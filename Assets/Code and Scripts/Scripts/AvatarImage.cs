using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AvatarImage : MonoBehaviour
{
    public enum AvatarType
    {
        Monster,
        Human,
        Animal
    }

    public int avatarSize = 128;
    public AvatarType avatarType;

    public Texture2D tex;
    public string url = "https://api.adorable.io/avatars/";
    private string uID = "Testing";

    IEnumerator Start()
    {
        generateURL();
        tex = new Texture2D(4, 4, TextureFormat.DXT1, false);
        WWW www = new WWW(url);
        yield return www;
        //www.LoadImageIntoTexture(tex);
        Texture2D circleTexture = CalculateTexture(avatarSize, avatarSize, avatarSize / 2, avatarSize / 2, avatarSize / 2, tex);
        gameObject.GetComponent<RawImage>().texture = circleTexture;
    }

    private Texture2D CalculateTexture(int h, int w, float r, float cx, float cy, Texture2D sourceTex)
    {
        Texture2D b = new Texture2D(h, w);
        for (int i = (int)(cx - r); i < cx + r; i++)
        {
            for (int j = (int)(cy - r); j < cy + r; j++)
            {
                float dx = i - cx;
                float dy = j - cy;
                float d = Mathf.Sqrt(dx * dx + dy * dy);
                if (d <= r)
                    b.SetPixel(i - (int)(cx - r), j - (int)(cy - r), sourceTex.GetPixel(i, j));
                else
                    b.SetPixel(i - (int)(cx - r), j - (int)(cy - r), Color.clear);
            }
        }
        b.Apply();
        return b;
    }

    private void generateURL()
    {
        print(avatarType);
        switch (avatarType)
        {
            case AvatarType.Monster:
                uID = System.Guid.NewGuid().ToString().Substring(0, 8);
                url = "https://api.adorable.io/avatars/";
                url = string.Format("{0}{1}/{2}.png", url, avatarSize, uID);
                break;
            case AvatarType.Human:
                url = "https://randomuser.me/api/portraits/";
                url = string.Format("{0}/{1}/{2}.jpg", url, (Random.value > 0.5) ? "women" : "men", (int)(Random.value * 99.0f));
                print(url);
                break;
            case AvatarType.Animal:
                url = "";
                break;
            default:
                url = "shit";
                break;
        }
    }
}