using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SegmentAutoAlign : MonoBehaviour {

    public bool m_update = false;

	void OnValidate()
    {
        m_update = false;

        Vector3 newEuler = new Vector3(0, 0, 0);

        transform.rotation = Quaternion.Euler(newEuler);

        // Building aligned position
        int x = ExtensionMethods.RoundToClosest10((int)transform.position.x);
        int y = 0;
        int z = ExtensionMethods.RoundToClosest10((int)transform.position.z);

        // Applying position to segment
        transform.position = new Vector3(x, y, z);

        
    }

    void Update()
    {
        // Deletes self if it's not in the right scene
        if (SceneManager.GetActiveScene() != SceneManager.GetSceneByName("MapBuilder"))
        {
            Destroy(this);
        }
    }

    
}
