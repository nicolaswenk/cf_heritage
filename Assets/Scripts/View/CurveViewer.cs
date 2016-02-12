using UnityEngine;
using System.Collections;

public class CurveViewer : MonoBehaviour
{
    public BezierSpline spline;

    public int quadNumber = 10;
    public float scrollSpeed = 0;


    public Material materials;
    public float zOffset    = 0.0f;
    public float meshWidth  = 10.0f;

    public float uvTile = 1.0f;

    private Renderer rend;

    public static bool IsOdd(int value)
    {
        return value % 2 != 0;
    }

    void Update()
    {
        float offset = Time.time * scrollSpeed / 10.0f;
        rend.material.SetTextureOffset("_MainTex", new Vector2(offset, 0));

        int verticeCount = quadNumber * 2 + 2;
        int verticePerTriangle = quadNumber * 2 * 3;

        Vector3[] newVertices = new Vector3[verticeCount];

        Vector2[] newUV = new Vector2[verticeCount];
        int[] newTriangles = new int[verticePerTriangle];



        int vCount = 0;

        for (int i = 0; i <= quadNumber; i++)
        {
            float uvDivide = (1.0f / quadNumber * 1.0f);

            Vector3 position = spline.GetPoint(i * uvDivide) - this.transform.position;

            Vector3 bezierVector = spline.GetDirection(i * uvDivide);

            bezierVector.z = zOffset;
            
            var newVector = Vector3.Cross(bezierVector, Vector3.forward);

            newVector.Normalize();

            newVertices[vCount] = -meshWidth * newVector + position;
            vCount++;
            newVertices[vCount] = meshWidth * newVector + position;
            vCount++;

        }

        int tCount = 0;
        for (int i = 0; i < newVertices.Length - 2; i++)
        {
            if (IsOdd(i))
            {
                newTriangles[tCount] = i;
                tCount++;
                newTriangles[tCount] = i + 1;
                tCount++;
                newTriangles[tCount] = i + 2;
                tCount++;
            }
            else
            {
                newTriangles[tCount] = i + 2;
                tCount++;
                newTriangles[tCount] = i + 1;
                tCount++;
                newTriangles[tCount] = i;
                tCount++;
            }
        }

        int uvCounter = 0;
        for (int i = 0; i < newVertices.Length; i++)
        {
            float uvDivide = (1.0f / quadNumber * uvTile);
            if (!IsOdd(i))
            {
                newUV[i] = new Vector2((uvDivide * uvCounter * offset)%1.0f, 0);
            }
            else
            {
                newUV[i] = new Vector2((uvDivide * uvCounter * offset)%1.0f, 1);
                uvCounter++;
            }
        }

        Mesh mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        mesh.vertices = newVertices;
        mesh.uv = newUV;
        mesh.triangles = newTriangles;

        rend.sharedMaterial = materials;
    }

    void Start()
    {

        rend = GetComponent<Renderer>();
        rend.enabled = true;
    }
}
