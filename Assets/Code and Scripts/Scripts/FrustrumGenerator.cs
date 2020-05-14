using UnityEngine;
using System.Linq;
using System.Collections;

public class FrustrumGenerator : MonoBehaviour {
    public int xResolution = 2;
    public int yResolution = 3;
    public float edgeThickness = 1;
    public float distance = 1.0f;

    private Mesh mesh;
	private App app;

	void Start () {
		app=GameObject.FindObjectOfType<App>(); 
		if (app == null)
			Debug.LogError ("Can't find App script");
		
        GenerateMesh();
    }

    void Update()
    {
        
    }

    private void GenerateMesh()
    {
        GetComponent<MeshFilter>().mesh = mesh = new Mesh();
        mesh.name = "Frustum Grid";
        Camera cam = app.view.cameras.mainCamera;

        var radAngle = cam.fieldOfView * Mathf.Deg2Rad;
        var vFOV = 2 * Mathf.Atan(Mathf.Tan(radAngle / 2)); // Vertical Radian Height
        var hFOV = 2 * Mathf.Atan(Mathf.Tan(radAngle / 2) * cam.aspect);

        var xSpan = hFOV / 2.0f;
        var ySpan = vFOV / 2.0f;

        var edgeThicknessX = edgeThickness * Mathf.Deg2Rad;
        var edgeThicknessY = edgeThicknessX / 1.5f;

        var m_1 = meshFromAngles(-xSpan, -xSpan + edgeThicknessX, -ySpan, ySpan, 1, yResolution);
        var m_2 = meshFromAngles(xSpan - edgeThicknessX, xSpan, -ySpan, ySpan, 1, yResolution);
        var m_3 = meshFromAngles(-xSpan, xSpan, -ySpan, -ySpan + edgeThicknessY, xResolution, 1);
        var m_4 = meshFromAngles(-xSpan, xSpan, ySpan - edgeThicknessY, ySpan, xResolution, 1);

        CombineInstance[] combine = new CombineInstance[4];

        combine[0] = new CombineInstance(); combine[0].mesh = m_1; combine[0].subMeshIndex = 0;
        combine[1] = new CombineInstance(); combine[1].mesh = m_2; combine[1].subMeshIndex = 0;
        combine[2] = new CombineInstance(); combine[2].mesh = m_3; combine[2].subMeshIndex = 0;
        combine[3] = new CombineInstance(); combine[3].mesh = m_4; combine[3].subMeshIndex = 0;

        mesh.subMeshCount = 4;
        mesh.CombineMeshes(combine,true,false);

        gameObject.GetComponent<MeshFilter>().mesh = mesh;
        gameObject.GetComponent<MeshFilter>().sharedMesh = mesh;

    }

    private Mesh meshFromAngles(float hStart, float hEnd, float vStart, float vEnd, int xRes, int yRes)
    {
        var output = new Mesh();

        Vector3[] verts = new Vector3[(xRes + 1) * (yRes + 1)];

        int i = 0;
        for (float y = 0; y <= yRes; y += 1)
        {
            for (float x = 0; x <= xRes; x += 1, i++)
            {
                var yAngle = vStart + y * (vEnd-vStart) / yRes;
                var xAngle = hStart + x * (hEnd-hStart) / xRes;

                var r = distance;
                var phi = Mathf.PI / 2 + yAngle;
                var theta = xAngle;

                var xCoord = r * Mathf.Sin(phi) * Mathf.Sin(theta);
                var zCoord = r * Mathf.Sin(phi) * Mathf.Cos(theta);
                var yCoord = r * Mathf.Cos(phi);

                verts[i] = new Vector3(xCoord, yCoord, zCoord);
            }
        }

        output.vertices = verts;

        int[] triangles = new int[(xRes) * (yRes) * 6];
        for (int ti = 0, vi = 0, y = 0; y < yRes; y++, vi++)
        {
            for (int x = 0; x < xRes; x++, ti += 6, vi++)
            {

                triangles[ti] = vi;
                triangles[ti + 3] = triangles[ti + 2] = vi + 1;
                triangles[ti + 4] = triangles[ti + 1] = vi + xRes + 1;
                triangles[ti + 5] = vi + xRes + 2;
            }
        }
        output.triangles = triangles.Reverse().ToArray();

        return output;
    }
}

