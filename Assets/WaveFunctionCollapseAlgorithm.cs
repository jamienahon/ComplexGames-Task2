using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Position
{
    public Position(List<Module> mods, Vector3 pos)
    {
        possibleModules = mods;
        position = pos;
    }

    public List<Module> possibleModules = new List<Module>();
    public Vector3 position;
}

public class WaveFunctionCollapseAlgorithm : MonoBehaviour
{
    public List<Module> modules = new List<Module>();
    public int terrainLength, terrainWidth, terrainHeight;
    public List<Position> positions = new List<Position>();

    void Start()
    {
        for (int l = 0; l < terrainLength; l++)
        {
            for (int w = 0; w < terrainWidth; w++)
            {
                for (int h = 0; h < terrainHeight; h++)
                {
                    List<Module> mods = new List<Module>();
                    foreach(Module mod in modules)
                    {
                        Module newMod = new Module();
                        newMod = mod;
                        mods.Add(newMod);
                    }
                    positions.Add(new Position(mods, new Vector3(l, h, w)));
                }
            }
        }
        //PrintPossibleModules(-1);
    }

    void PrintPossibleModules(int index)
    {
        if (index == -1)
        {
            for(int i = 0; i < positions.Count; i++)
            {
                Debug.Log("--------------- " + positions[i].position + " ---------------");
                foreach (Module mod in positions[i].possibleModules)
                {
                    Debug.Log(mod.moduleObject.name + " " + mod.rotation);
                }
            }
        }
        else
        {
            Debug.Log("--------------- " + positions[index].position + " ---------------");
            foreach (Module mod in positions[index].possibleModules)
            {
                Debug.Log(mod.moduleObject.name + " " + mod.rotation);
            }
        }
    }
}
