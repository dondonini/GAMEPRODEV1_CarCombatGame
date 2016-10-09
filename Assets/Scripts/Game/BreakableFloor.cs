using UnityEngine;
using System.Collections;

public class BreakableFloor : MonoBehaviour {

    [Tooltip("Tags that will activate the breakage of the floor")]
    public string[] m_validTags = new string[] { "Player", "Enemy" };
    [Tooltip("Time until the floor breaks")]
    public float m_timeTillBreakage = 10;
	
    private bool m_isOn = false;
    private float timer = 0;

	// Update is called once per frame
	void Update () {
	
        if (m_isOn)
        {
            // Increment timer
            timer += Time.deltaTime;

            // Break the floor if timer passes limit
            if (timer >= m_timeTillBreakage)
            {
                //TODO: Break floor
            }
        }
        else
        {
            timer = 0;
        }
	}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerEnter(Collider other)
    {
        if (IsValidTag(other.tag))
        {
            m_isOn = true;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerExit(Collider other)
    {
        if (IsValidTag(other.tag))
        {
            m_isOn = false;
        }
    }

    /// <summary>
    /// Checks if tag is valid
    /// </summary>
    /// <param name="tag"></param>
    /// <returns></returns>
    bool IsValidTag(string tag)
    {
        for (int i = 0; i < m_validTags.Length; i++)
        {
            if (m_validTags[i] == tag)
            {
                return true;
            }
        }

        return false;
    }

}
