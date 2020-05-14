using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemMenu : MonoBehaviour
{

    public int numItems = 5;

    public int selected = 0;

    public GameObject[] icons = null;
    public GameObject defaultIcon;
    GameObject lastSelected = null;

    string[] descriptions = { "Call to attention", "Exit", "Annotate", "Voice Chat", "Test", "Clear annotations", "Restart Video" };
    public UnityEngine.UI.Text selectedDescription; 
    void Start()
    {
        string[] textIcons = {""+'\uf25a',
                              ""+'\uf135',
                              ""+'\uf040',
                              ""+'\uf130',
                              ""+'\uf0d0',
                              ""+'\uf12d',
                              ""+'\uf049'};
        Color[] colors = { Color.black, Color.red, Color.blue, Color.green, Color.grey, Color.magenta, Color.yellow };
        icons = new GameObject[numItems];
        for (int i = 0; i < numItems; i++)
        {
            float radius = 38.75f;
            float theta = 28.0f * (numItems / 2) + -28.0f * i + 90.0f;
            float radTheta = Mathf.PI * theta / 180.0f;
            float xPos = radius * Mathf.Cos(radTheta) * 1.005f;
            float yPos = radius * Mathf.Sin(radTheta) * 1.06f;

            GameObject icon = Instantiate(defaultIcon, this.transform);
            icon.transform.localPosition = new Vector3(xPos, yPos, 0);
            icon.SetActive(true);
            UnityEngine.UI.RawImage image = (UnityEngine.UI.RawImage)icon.GetComponent(typeof(UnityEngine.UI.RawImage));
            UnityEngine.UI.Text iconText = (UnityEngine.UI.Text) icon.GetComponentInChildren(typeof(UnityEngine.UI.Text));
            iconText.text = textIcons[i];
            image.color = colors[i];

            icons[i] = icon;

            // Set font awesome icon text, Keep track of which is which.
        }

        InvokeRepeating("incrementSelected", 0.0f, 1.0f);

    }

    void incrementSelected()
    {
        selected = (selected + 1) % numItems;
    }

    // Update is called once per frame
    void Update()
    {
        updateSelected(selected);
    }
    void updateSelected(int selected)
    {
        for (int i = 0; i < numItems; i++)
        {
            if (i == selected)
            {
                icons[i].transform.localScale = new Vector3(0.16f, 0.16f, 1.0f);
                selectedDescription.text = descriptions[i];
            }
            else
            {
                icons[i].transform.localScale = new Vector3(0.14f, 0.14f, 1.0f);
            }
        }
    }
}