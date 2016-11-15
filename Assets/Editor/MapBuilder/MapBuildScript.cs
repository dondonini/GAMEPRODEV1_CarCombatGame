using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapBuildScript : MonoBehaviour {

    public Transform m_mapPocket;

    private bool m_active = false;

    public void BuildMap()
    {
        if (m_active) { return; }

        // Prevents build if pocket is empty
        if (m_mapPocket.childCount <= 0)
        {
            Debug.LogError("Map pocket is empty!");
            m_active = false;
            return;
        }

        // Initialize
        Debug.Log("Starting up...");
        List<List<GameObject>> temp2DArray = new List<List<GameObject>>();

        Debug.Log("Getting map distance...");

        float farthestDistance = 0;

        // Finding the farthest distance and also finding width of whole map

        for (int i = 0; i < m_mapPocket.childCount; i++)
        {
            if (m_mapPocket.GetChild(i).transform.position.z > farthestDistance)
            {
                farthestDistance = m_mapPocket.GetChild(i).transform.position.z;
            }
        }
        Debug.Log("Farthest distance: " + farthestDistance);

        Debug.Log("Building layers array...");

        // Building layers array
        int numOfLayers = ExtensionMethods.RoundToClosest10((int)farthestDistance) / 10 + 1;

        for (int l = 0; l < numOfLayers; l++)
        {
            temp2DArray.Add(new List<GameObject>());

            for (int c = 0; c < m_mapPocket.childCount; c++)
            {
                if (m_mapPocket.GetChild(c).transform.position.z == l * 10)
                {
                    temp2DArray[l].Add(m_mapPocket.GetChild(c).gameObject);
                    
                }
            }
            Debug.LogFormat(string.Format("Layer #{0}: {1} segments", l, temp2DArray[l].Count));
        }

        Debug.Log("Total layers built: " + temp2DArray.Count);


        // Building game object of map

        GameObject newMap = Instantiate(new GameObject()) as GameObject;
        newMap.name = "Map";
        newMap.transform.position = new Vector3(0, 0, 0);
        newMap.transform.SetParent(transform);

        for (int l = 0; l < temp2DArray.Count; l++)
        {
            // New Layer
            GameObject newLayer = Instantiate(new GameObject()) as GameObject;
            newLayer.name = "Layer_" + l;
            newLayer.transform.position = new Vector3(0, 0, l * 10);
            newLayer.transform.SetParent(newMap.transform);

            for (int s = 0; s < temp2DArray[l].Count; s++)
            {
                // Moving segment into layer
                temp2DArray[l][s].transform.SetParent(newLayer.transform);
            }
        }

        m_active = false;
        Debug.Log("Build done!");
    }
}
