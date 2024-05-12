using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Module
{
    public Module()
    {
        pXFaceVertPos = new List<Vector3>(); nXFaceVertPos = new List<Vector3>();
        pYFaceVertPos = new List<Vector3>(); nYFaceVertPos = new List<Vector3>();
        pZFaceVertPos = new List<Vector3>(); nZFaceVertPos = new List<Vector3>();

        pXFaceModules = new List<Module>(); nXFaceModules = new List<Module>();
        pYFaceModules = new List<Module>(); nYFaceModules = new List<Module>();
        pZFaceModules = new List<Module>(); nZFaceModules = new List<Module>();
    }

    public GameObject moduleGameObject;

    public List<Vector3> pXFaceVertPos, nXFaceVertPos,
    pYFaceVertPos, nYFaceVertPos,
    pZFaceVertPos, nZFaceVertPos;


    public List<Module> pXFaceModules, nXFaceModules,
    pZFaceModules, nZFaceModules,
    pYFaceModules, nYFaceModules;

}

public class CalculatePossibleAdjacentModules : MonoBehaviour
{
    public List<GameObject> moduleObjects;
    List<Module> modules = new List<Module>();

    void Start()
    {
        foreach (GameObject modObj in moduleObjects)
        {
            Module newModule = new Module();
            newModule.moduleGameObject = modObj;
            DetermineVertexPositionsOfEachFace(newModule, modObj.GetComponent<MeshFilter>().sharedMesh);
            modules.Add(newModule);
        }

        foreach (Module mod in modules)
        {
            DeterminePossibleAdjacentModules();
        }
        //PrintVertexPositions(modules[6]);
        PrintAllAdjacentTiles(modules[6]);
        //Debug.Log(modules[2].pXFaceModules[0].moduleGameObject.name);
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

    void DeterminePossibleAdjacentModules()
    {
        foreach (Module mod in modules)
        {
            foreach (Module adjMod in modules)
            {
                if (AreListsEqual(mod.pXFaceVertPos, MultiplyVectorList(adjMod.nXFaceVertPos, new Vector3(-1, 1, 1))))
                {
                    if (!mod.pXFaceModules.Contains(adjMod))
                        mod.pXFaceModules.Add(adjMod);
                }

                if (AreListsEqual(mod.nXFaceVertPos, MultiplyVectorList(adjMod.pXFaceVertPos, new Vector3(-1, 1, 1))))
                {
                    if (!mod.nXFaceModules.Contains(adjMod))
                        mod.nXFaceModules.Add(adjMod);
                }

                if (AreListsEqual(mod.pYFaceVertPos, MultiplyVectorList(adjMod.nYFaceVertPos, new Vector3(1, -1, 1))))
                {
                    if (!mod.pYFaceModules.Contains(adjMod))
                        mod.pYFaceModules.Add(adjMod);
                }

                if (AreListsEqual(mod.nYFaceVertPos, MultiplyVectorList(adjMod.pYFaceVertPos, new Vector3(1, -1, 1))))
                {
                    if (!mod.nYFaceModules.Contains(adjMod))
                        mod.nYFaceModules.Add(adjMod);
                }

                if (AreListsEqual(mod.pZFaceVertPos, MultiplyVectorList(adjMod.nZFaceVertPos, new Vector3(1, 1, -1))))
                {
                    if (!mod.pZFaceModules.Contains(adjMod))
                        mod.pZFaceModules.Add(adjMod);
                }

                if (AreListsEqual(mod.nZFaceVertPos, MultiplyVectorList(adjMod.pZFaceVertPos, new Vector3(1, 1, -1))))
                {
                    if (!mod.nZFaceModules.Contains(adjMod))
                        mod.nZFaceModules.Add(adjMod);
                }
            }
        }
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

    bool AreListsEqual(List<Vector3> list1, List<Vector3> list2)
    {
        if (list1.Count != list2.Count || list1.Count == 0)
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

    void PrintAllAdjacentTiles(Module module)
    {
        Debug.Log("--------------- pX Face ---------------");
        foreach (Module mod in module.pXFaceModules)
        {
            Debug.Log(mod.moduleGameObject.name);
        }

        Debug.Log("--------------- nX Face ---------------");
        foreach (Module mod in module.nXFaceModules)
        {
            Debug.Log(mod.moduleGameObject.name);
        }

        Debug.Log("--------------- pY Face ---------------");
        foreach (Module mod in module.pYFaceModules)
        {
            Debug.Log(mod.moduleGameObject.name);
        }

        Debug.Log("--------------- nY Face ---------------");
        foreach (Module mod in module.nYFaceModules)
        {
            Debug.Log(mod.moduleGameObject.name);
        }

        Debug.Log("--------------- pZ Face ---------------");
        foreach (Module mod in module.pZFaceModules)
        {
            Debug.Log(mod.moduleGameObject.name);
        }

        Debug.Log("--------------- nZ Face ---------------");
        foreach (Module mod in module.nZFaceModules)
        {
            Debug.Log(mod.moduleGameObject.name);
        }
    }
}