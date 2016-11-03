using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelLoader : MonoBehaviour {

	public void LoadLevel(string level)
    {
        SceneManager.LoadSceneAsync(level);
    }
}
