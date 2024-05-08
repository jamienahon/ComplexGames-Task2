using System.Collections.Generic;
using UnityEngine;

public class CalculateVertexPositions : MonoBehaviour
{
    public List<Vector3> pZFaceVertPos, nZFaceVertPos,
        pYFaceVertPos, nYFaceVertPos,
        pXFaceVertPos, nXFaceVertPos;

    public List<Vector3> vertices;

    void Start()
    {
        foreach (Vector3 vertex in GetComponent<MeshFilter>().mesh.vertices)
        {
            vertices.Add(vertex);
        }

        foreach (Vector3 vertex in vertices)
        {
            if (vertex.z == 0.5f && !pZFaceVertPos.Contains(vertex))
            {
                pZFaceVertPos.Add(vertex);
            }
        }
        RemoveVertices(pZFaceVertPos);

        foreach (Vector3 vertex in vertices)
        {
            if (vertex.z == -0.5f && !nZFaceVertPos.Contains(vertex))
            {
                nZFaceVertPos.Add(vertex);
            }
        }
        RemoveVertices(nZFaceVertPos);

        foreach (Vector3 vertex in vertices)
        {
            if (vertex.y == 0.5f && !pYFaceVertPos.Contains(vertex))
            {
                pYFaceVertPos.Add(vertex);
            }
        }
        RemoveVertices(pYFaceVertPos);

        foreach (Vector3 vertex in vertices)
        {
            if (vertex.y == -0.5f && !nYFaceVertPos.Contains(vertex))
            {
                nYFaceVertPos.Add(vertex);
            }
        }
        RemoveVertices(nYFaceVertPos);

        foreach (Vector3 vertex in vertices)
        {
            if (vertex.x == 0.5f && !pXFaceVertPos.Contains(vertex))
            {
                pXFaceVertPos.Add(vertex);
            }
        }
        RemoveVertices(pXFaceVertPos);

        foreach (Vector3 vertex in vertices)
        {
            if (vertex.x == -0.5f && !nXFaceVertPos.Contains(vertex))
            {
                nXFaceVertPos.Add(vertex);
            }
        }
        RemoveVertices(nXFaceVertPos);
    }

    void RemoveVertices(List<Vector3> list)
    {
        foreach (Vector3 vertex in list)
        {
            if (vertices.Contains(vertex))
                vertices.Remove(vertex);
        }
    }
}
