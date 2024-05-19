using System.Collections.Generic;
using UnityEngine;

public class Module
{
    public Module()
    {
        pXFaceVertPos = new List<Vector3>(); nXFaceVertPos = new List<Vector3>();
        pYFaceVertPos = new List<Vector3>(); nYFaceVertPos = new List<Vector3>();
        pZFaceVertPos = new List<Vector3>(); nZFaceVertPos = new List<Vector3>();

        pXFaceNorms = new List<Vector3>(); nXFaceNorms = new List<Vector3>();
        pYFaceNorms = new List<Vector3>(); nYFaceNorms = new List<Vector3>();
        pZFaceNorms = new List<Vector3>(); nZFaceNorms = new List<Vector3>();

        pXFaceModules = new List<Module>(); nXFaceModules = new List<Module>();
        pYFaceModules = new List<Module>(); nYFaceModules = new List<Module>();
        pZFaceModules = new List<Module>(); nZFaceModules = new List<Module>();
    }

    public Mesh mesh;
    public GameObject moduleObject;
    public int rotation;

    public List<Vector3> pXFaceVertPos, nXFaceVertPos,
    pYFaceVertPos, nYFaceVertPos,
    pZFaceVertPos, nZFaceVertPos;

    public List<Vector3> pXFaceNorms, nXFaceNorms,
    pYFaceNorms, nYFaceNorms,
    pZFaceNorms, nZFaceNorms;

    public List<Module> pXFaceModules, nXFaceModules,
    pZFaceModules, nZFaceModules,
    pYFaceModules, nYFaceModules;

}

public class CalculatePossibleAdjacentModules : MonoBehaviour
{
    public List<GameObject> moduleObjects;
    List<Module> modules = new List<Module>();
    public GameObject modulePrefab;

    void Start()
    {
        foreach (GameObject modObj in moduleObjects)
        {
            Mesh modObjMesh = modObj.GetComponent<MeshFilter>().sharedMesh;

            Module newModule = new Module();
            newModule.mesh = modObjMesh;
            newModule.moduleObject = modObj;
            newModule.rotation = 0;
            if (modObj != null)
                DetermineVertexPositionsOfEachFace(newModule, modObjMesh);
            DetermineNormalsOfEachFace(newModule, modObjMesh);
            modules.Add(newModule);

            if (!DoListsContainSameData(ConvertArrToList(modObjMesh.vertices),
            ConvertArrToList(RotateArrayOfVector3(modObjMesh.vertices, 90))))
            {
                for (int i = 90; i < 360; i += 90)
                {
                    Mesh newMesh = new Mesh();
                    newMesh.vertices = RotateArrayOfVector3(modObjMesh.vertices, i);
                    newMesh.triangles = modObjMesh.triangles;
                    newMesh.normals = RotateArrayOfVector3(modObjMesh.normals, i);
                    newMesh.tangents = modObjMesh.tangents;
                    newMesh.bounds = modObjMesh.bounds;
                    newMesh.uv = modObjMesh.uv;

                    Module rotatedModule = new Module();
                    rotatedModule.mesh = newMesh;
                    rotatedModule.moduleObject = modObj;
                    rotatedModule.rotation = i;
                    if (newMesh != null)
                        DetermineVertexPositionsOfEachFace(rotatedModule, newMesh);
                    DetermineNormalsOfEachFace(rotatedModule, newMesh);
                    modules.Add(rotatedModule);
                }
            }
        }
        Module blankModule = new Module();
        blankModule.moduleObject = modulePrefab;
        modules.Add(blankModule);

        DeterminePossibleAdjacentModules();

        WaveFunctionCollapseAlgorithm algorithm = GetComponent<WaveFunctionCollapseAlgorithm>();
        foreach(Module mod in modules)
        {
            algorithm.modules.Add(mod);
        }

        //PrintVertexPositions(modules[1]);
        //PrintAllAdjacentTiles(modules[25]);
        //Debug.Log(modules[2].pXFaceModules[0].moduleGameObject.name);
        //PrintModuleList();
        //PrintNormals(modules[0]);
        //foreach (Vector3 norm in modules[3].mesh.normals)
        //{
        //    Debug.Log(norm);
        //}

        //GameObject module = Instantiate(modules[0].moduleObject, new Vector3(0,1,-1), Quaternion.identity);
        //module.GetComponent<MeshFilter>().mesh = modules[0].mesh;

        //GameObject module2 = Instantiate(modules[1].moduleObject, new Vector3(0, 1, 0), Quaternion.identity);
        //module2.GetComponent<MeshFilter>().mesh = modules[1].mesh;
    }


    void DetermineVertexPositionsOfEachFace(Module module, Mesh moduleMesh)
    {
        List<Vector3> vertices = new List<Vector3>();
        foreach (Vector3 vertex in moduleMesh.vertices)
        {
            vertices.Add(vertex);
        }

        foreach (Vector3 vertex in vertices)
        {
            if (vertex.x == 0.5f && !module.pXFaceVertPos.Contains(vertex))
                module.pXFaceVertPos.Add(vertex);

            if (vertex.x == -0.5f && !module.nXFaceVertPos.Contains(vertex))
                module.nXFaceVertPos.Add(vertex);

            if (vertex.y == 0.5f && !module.pYFaceVertPos.Contains(vertex))
                module.pYFaceVertPos.Add(vertex);

            if (vertex.y == -0.5f && !module.nYFaceVertPos.Contains(vertex))
                module.nYFaceVertPos.Add(vertex);

            if (vertex.z == 0.5f && !module.pZFaceVertPos.Contains(vertex))
                module.pZFaceVertPos.Add(vertex);

            if (vertex.z == -0.5f && !module.nZFaceVertPos.Contains(vertex))
                module.nZFaceVertPos.Add(vertex);
        }
    }

    void DetermineNormalsOfEachFace(Module module, Mesh moduleMesh)
    {
        List<Vector3> normals = new List<Vector3>();
        foreach (Vector3 norm in moduleMesh.normals)
        {
            normals.Add(norm);
        }

        foreach (Vector3 norm in normals)
        {
            if (norm.x == 1f && !module.pXFaceNorms.Contains(norm))
                module.pXFaceNorms.Add(norm);

            if (norm.x == -1f && !module.nXFaceNorms.Contains(norm))
                module.nXFaceNorms.Add(norm);

            if (norm.y == 1f && !module.pYFaceNorms.Contains(norm))
                module.pYFaceNorms.Add(norm);

            if (norm.y == -1f && !module.nYFaceNorms.Contains(norm))
                module.nYFaceNorms.Add(norm);

            if (norm.z == 1f && !module.pZFaceNorms.Contains(norm))
                module.pZFaceNorms.Add(norm);

            if (norm.z == -1f && !module.nZFaceNorms.Contains(norm))
                module.nZFaceNorms.Add(norm);
        }
    }

    void DeterminePossibleAdjacentModules()
    {
        foreach (Module mod in modules)
        {
            foreach (Module adjMod in modules)
            {
                if (DoListsContainSameData(mod.pXFaceVertPos, MultiplyVectorList(adjMod.nXFaceVertPos, new Vector3(-1, 1, 1))))
                {
                    if (mod.pXFaceVertPos.Count == 0 && !mod.pXFaceModules.Contains(modules[modules.Count - 1]))
                        mod.pXFaceModules.Add(modules[modules.Count - 1]);
                    if (mod.pYFaceNorms.Count > 0 || DoListsContainSameData(mod.nZFaceNorms, adjMod.nZFaceNorms))
                    {
                        if (mod.pXFaceVertPos.Count != 0 && !mod.pXFaceModules.Contains(adjMod))
                            mod.pXFaceModules.Add(adjMod);
                    }
                    if(mod.mesh == null && adjMod.nXFaceVertPos.Count == 0 && !mod.pXFaceModules.Contains(adjMod))
                        mod.pXFaceModules.Add(adjMod);
                }

                if (DoListsContainSameData(mod.nXFaceVertPos, MultiplyVectorList(adjMod.pXFaceVertPos, new Vector3(-1, 1, 1))))
                {
                    if (mod.nXFaceVertPos.Count == 0 && !mod.nXFaceModules.Contains(modules[modules.Count - 1]))
                        mod.nXFaceModules.Add(modules[modules.Count - 1]);
                    if (mod.pYFaceNorms.Count > 0 || DoListsContainSameData(mod.nZFaceNorms, adjMod.nZFaceNorms))
                    {
                        if (mod.nXFaceVertPos.Count != 0 && !mod.nXFaceModules.Contains(adjMod))
                            mod.nXFaceModules.Add(adjMod);
                    }
                    if (mod.mesh == null && adjMod.pXFaceVertPos.Count == 0 && !mod.nXFaceModules.Contains(adjMod))
                        mod.nXFaceModules.Add(adjMod);
                }

                if (DoListsContainSameData(mod.pYFaceVertPos, MultiplyVectorList(adjMod.nYFaceVertPos, new Vector3(1, -1, 1))))
                {
                    if (mod.pYFaceVertPos.Count == 0 && !mod.pYFaceModules.Contains(modules[modules.Count - 1]))
                        mod.pYFaceModules.Add(modules[modules.Count - 1]);
                    if (DoListsContainSameData(mod.nXFaceNorms, adjMod.nXFaceNorms) && DoListsContainSameData(mod.nZFaceNorms, adjMod.nZFaceNorms))
                    {
                        if (mod.pYFaceVertPos.Count != 0 && !mod.pYFaceModules.Contains(adjMod))
                            mod.pYFaceModules.Add(adjMod);
                    }
                    if (mod.mesh == null && adjMod.nYFaceVertPos.Count == 0 && !mod.pYFaceModules.Contains(adjMod))
                        mod.pYFaceModules.Add(adjMod);
                }

                if (DoListsContainSameData(mod.nYFaceVertPos, MultiplyVectorList(adjMod.pYFaceVertPos, new Vector3(1, -1, 1))))
                {
                    if (mod.nYFaceVertPos.Count == 0 && !mod.nYFaceModules.Contains(modules[modules.Count - 1]))
                        mod.nYFaceModules.Add(modules[modules.Count - 1]);
                    if (DoListsContainSameData(mod.nXFaceNorms, adjMod.nXFaceNorms) && DoListsContainSameData(mod.nZFaceNorms, adjMod.nZFaceNorms))
                    {
                        if (mod.nYFaceVertPos.Count != 0 && !mod.nYFaceModules.Contains(adjMod))
                            mod.nYFaceModules.Add(adjMod);
                    }
                    if (mod.mesh == null && adjMod.pYFaceVertPos.Count == 0 && !mod.nYFaceModules.Contains(adjMod))
                        mod.nYFaceModules.Add(adjMod);
                }

                if (DoListsContainSameData(mod.pZFaceVertPos, MultiplyVectorList(adjMod.nZFaceVertPos, new Vector3(1, 1, -1))))
                {
                    if (mod.pZFaceVertPos.Count == 0 && !mod.pZFaceModules.Contains(modules[modules.Count - 1]))
                        mod.pZFaceModules.Add(modules[modules.Count - 1]);
                    if (mod.pYFaceNorms.Count > 0 || DoListsContainSameData(mod.nXFaceNorms, adjMod.nXFaceNorms))
                    {
                        if (mod.pZFaceVertPos.Count != 0 && !mod.pZFaceModules.Contains(adjMod))
                            mod.pZFaceModules.Add(adjMod);
                    }
                    if (mod.mesh == null && adjMod.nZFaceVertPos.Count == 0 && !mod.pZFaceModules.Contains(adjMod))
                        mod.pZFaceModules.Add(adjMod);
                }

                if (DoListsContainSameData(mod.nZFaceVertPos, MultiplyVectorList(adjMod.pZFaceVertPos, new Vector3(1, 1, -1))))
                {
                    if (mod.nZFaceVertPos.Count == 0 && !mod.nZFaceModules.Contains(modules[modules.Count - 1]))
                        mod.nZFaceModules.Add(modules[modules.Count - 1]);
                    if (mod.pYFaceNorms.Count > 0 || DoListsContainSameData(mod.nXFaceNorms, adjMod.nXFaceNorms))
                    {
                        if (mod.nZFaceVertPos.Count != 0 && !mod.nZFaceModules.Contains(adjMod))
                            mod.nZFaceModules.Add(adjMod);
                    }
                    if (mod.mesh == null && adjMod.pZFaceVertPos.Count == 0 && !mod.nZFaceModules.Contains(adjMod))
                        mod.nZFaceModules.Add(adjMod);
                }
            }
        }
    }

    Vector3[] RotateArrayOfVector3(Vector3[] vertices, float rotationAmount)
    {
        Vector3[] newVerts = new Vector3[vertices.Length];
        Quaternion angle = Quaternion.AngleAxis(rotationAmount, Vector3.up);
        for (int i = 0; i < vertices.Length; i++)
        {
            newVerts[i] = angle * vertices[i];
        }
        return RoundVector(newVerts);
    }

    Vector3[] RoundVector(Vector3[] vectors)
    {
        Vector3[] newVec = new Vector3[vectors.Length];
        for (int i = 0; i < vectors.Length; i++)
        {
            newVec[i] = new Vector3(Mathf.Round(vectors[i].x * 1000) / 1000, Mathf.Round(vectors[i].y * 1000) / 1000, Mathf.Round(vectors[i].z * 1000) / 1000);
        }
        return newVec;
    }

    List<Vector3> MultiplyVectorList(List<Vector3> list, Vector3 vector)
    {
        List<Vector3> newVectorList = new List<Vector3>();
        foreach (Vector3 vec in list)
        {
            newVectorList.Add(new Vector3(vec.x * vector.x, vec.y * vector.y, vec.z * vector.z));
        }
        return newVectorList;
    }

    bool DoListsContainSameData(List<Vector3> list1, List<Vector3> list2)
    {
        if (list1.Count != list2.Count)
            return false;

        List<Vector3> newList = new List<Vector3>();
        for (int i = 0; i < list1.Count; i++)
        {
            newList.Add(new Vector3(list1[i].x, list1[i].y, list1[i].z));
        }

        for (int i = 0; i < list2.Count; i++)
        {
            if (IsItemInList(newList, list2[i]))
            {
                newList.Remove(list2[i]);
            }
            else
                return false;
        }
        return true;
    }

    List<Vector3> ConvertArrToList(Vector3[] verts)
    {
        List<Vector3> newList = new List<Vector3>();
        foreach (Vector3 vert in verts)
        {
            newList.Add(vert);
        }
        return newList;
    }

    bool IsItemInList(List<Vector3> list, Vector3 item)
    {
        foreach (Vector3 vec in list)
        {
            if (vec == item)
            {
                return true;
            }
        }
        return false;
    }

    void PrintVertexPositions(Module module)
    {
        Debug.Log(module.moduleObject.name + " " + module.rotation);
        Debug.Log("--------------- pXFaceVertPos ---------------");
        foreach (Vector3 pos in module.pXFaceVertPos)
        {
            Debug.Log(pos);
        }

        Debug.Log("--------------- nXFaceVertPos ---------------");
        foreach (Vector3 pos in module.nXFaceVertPos)
        {
            Debug.Log(pos);
        }

        Debug.Log("--------------- pYFaceVertPos ---------------");
        foreach (Vector3 pos in module.pYFaceVertPos)
        {
            Debug.Log(pos);
        }

        Debug.Log("--------------- nYFaceVertPos ---------------");
        foreach (Vector3 pos in module.nYFaceVertPos)
        {
            Debug.Log(pos);
        }

        Debug.Log("--------------- pZFaceVertPos ---------------");
        foreach (Vector3 pos in module.pZFaceVertPos)
        {
            Debug.Log(pos);
        }

        Debug.Log("--------------- nZFaceVertPos ---------------");
        foreach (Vector3 pos in module.nZFaceVertPos)
        {
            Debug.Log(pos);
        }
    }

    void PrintNormals(Module module)
    {
        Debug.Log(module.moduleObject.name + " " + module.rotation);
        Debug.Log("--------------- pXFaceNorms ---------------");
        foreach (Vector3 norm in module.pXFaceNorms)
        {
            Debug.Log(norm);
        }

        Debug.Log("--------------- nXFaceNorms ---------------");
        foreach (Vector3 norm in module.nXFaceNorms)
        {
            Debug.Log(norm);
        }

        Debug.Log("--------------- pYFaceNorms ---------------");
        foreach (Vector3 norms in module.pYFaceNorms)
        {
            Debug.Log(norms);
        }

        Debug.Log("--------------- nYFaceNorms ---------------");
        foreach (Vector3 norms in module.nYFaceNorms)
        {
            Debug.Log(norms);
        }

        Debug.Log("--------------- pZFaceNorms ---------------");
        foreach (Vector3 norms in module.pZFaceNorms)
        {
            Debug.Log(norms);
        }

        Debug.Log("--------------- nZFaceNorms ---------------");
        foreach (Vector3 norms in module.nZFaceNorms)
        {
            Debug.Log(norms);
        }
    }

    void PrintAllAdjacentTiles(Module module)
    {
        Debug.Log(module.moduleObject.name + " " + module.rotation);
        Debug.Log("--------------- pX Face ---------------");
        foreach (Module mod in module.pXFaceModules)
        {
            Debug.Log(mod.moduleObject.name + " " + mod.rotation);
        }

        Debug.Log("--------------- nX Face ---------------");
        foreach (Module mod in module.nXFaceModules)
        {
            Debug.Log(mod.moduleObject.name + " " + mod.rotation);
        }

        Debug.Log("--------------- pY Face ---------------");
        foreach (Module mod in module.pYFaceModules)
        {
            Debug.Log(mod.moduleObject.name + " " + mod.rotation);
        }

        Debug.Log("--------------- nY Face ---------------");
        foreach (Module mod in module.nYFaceModules)
        {
            Debug.Log(mod.moduleObject.name + " " + mod.rotation);
        }

        Debug.Log("--------------- pZ Face ---------------");
        foreach (Module mod in module.pZFaceModules)
        {
            Debug.Log(mod.moduleObject.name + " " + mod.rotation);
        }

        Debug.Log("--------------- nZ Face ---------------");
        foreach (Module mod in module.nZFaceModules)
        {
            Debug.Log(mod.moduleObject.name + " " + mod.rotation);
        }
    }

    void PrintModuleList()
    {
        for (int i = 0; i < modules.Count; i++)
        {
            Debug.Log(i + ". " + modules[i].moduleObject.name + " " + modules[i].rotation);
        }
    }
}