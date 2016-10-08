using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class LevelManager : MonoBehaviour {

    // The player
    public Transform m_player;

    [Tooltip("The animation type when the segment spawns in")]
    public EasingFunction.Ease m_segmentAnimationIn;

    [Tooltip("The animation type when the segment is removed")]
    public EasingFunction.Ease m_segmentAnimationOut;

    [Tooltip("The animation type when the enemy spawns in")]
    public EasingFunction.Ease m_enemyAnimationIn;

    [Tooltip("The animation type when the enemy is removed")]
    public EasingFunction.Ease m_enemyAnimationOut;

    public float m_segmentAnimationDuration = 1;
    public float m_segmentAnimationDelay = 0.4f;

    public float m_enemyAnimationDuration = 1;
    public float m_enemyAnimationDelay = 0.4f;

    [Tooltip("Distance of segments to see")]
    public uint m_segmentRenderDistance = 5;

    public GameObject m_map;

    // The Y position for all of the segments
    [SerializeField]
    private float END_HEIGHT = 0;
    [SerializeField]
    private float SEGMENT_START_HEIGHT = -100;
    [SerializeField]
    private float ENEMY_START_HEIGHT = 100;

    private List<List<GameObject>> m_mapArray = new List<List<GameObject>>();

    private int m_previousCheckpoint = 0;
    private const int m_TOTAL_Z_LENGTH = 10;

    void OnValidate()
    {

    }

	// Use this for initialization
	void Start () {

        // Build usable segment arrays
        SetMapToStartPositions();

    }
	
	// Update is called once per frame
	void Update () {

        //Debug.Log(Vector3.Distance(m_player.position, new Vector3(0, 0, 0)) / 10);
        // Debug lines
        Debug.DrawLine(new Vector3(-20, 0, m_previousCheckpoint), new Vector3(20, 0, m_previousCheckpoint), Color.white);
        Debug.DrawLine(new Vector3(-20, 0, m_previousCheckpoint - (m_segmentRenderDistance * 10)), new Vector3(20, 0, m_previousCheckpoint - (m_segmentRenderDistance * 10)), Color.blue);
        Debug.DrawLine(new Vector3(-20, 0, m_previousCheckpoint + (m_segmentRenderDistance * 10)), new Vector3(20, 0, m_previousCheckpoint + (m_segmentRenderDistance * 10)), Color.green);

        // Checks if previous checkpoint has been passed
        if (m_player.transform.position.z >= (m_previousCheckpoint - (m_segmentRenderDistance * 10)))
        {
            //Debug.Log("NEW LAYER");
            // Build new layer
            StartCoroutine(BuildSegmentLayer());

            if (m_player.transform.position.z - (m_segmentRenderDistance * 10) > 50)
            {
                //Debug.Log("DELETING OLD LAYER!");
                // Remove old layer
                //StartCoroutine(RemoveSegmentLayer());
            }

            // Update previous checkpoint to now
            m_previousCheckpoint += m_TOTAL_Z_LENGTH;

            
        }


        Debug.DrawLine(new Vector3(-20, 0, m_player.transform.position.z - (m_segmentRenderDistance * 10)), new Vector3(20, 0, m_player.transform.position.z - (m_segmentRenderDistance * 10)), Color.red);
        Debug.DrawLine(new Vector3(-20, 0, m_previousCheckpoint - (m_segmentRenderDistance * 10) * 2), new Vector3(20, 0, m_previousCheckpoint - (m_segmentRenderDistance * 10) * 2), Color.red);
        
    }

    private void SetMapToStartPositions()
    {
        // Basically converts the whole map into an array for ease of use while
        // setting all positions of each segment to starter position
        for (int l = 0; l < m_map.transform.childCount; l++)
        {
            m_mapArray.Add(new List<GameObject>());

            Transform currentLayer = m_map.transform.FindChild("Layer_" + l);

            for (int s = 0; s < currentLayer.childCount; s++)
            {
                // Add segment into 2D array
                m_mapArray[l].Add(currentLayer.GetChild(s).gameObject);

                // Set segment to start position
                currentLayer.GetChild(s).position = new Vector3(
                    currentLayer.GetChild(s).position.x,   // X
                    SEGMENT_START_HEIGHT,                  // Y
                    currentLayer.GetChild(s).position.z);  // Z

                // Set to be deactivated
                currentLayer.GetChild(s).gameObject.SetActive(false);
            }
        }

        Debug.LogFormat(string.Format("Total layers: {0}", m_map.transform.childCount));
    }

    private IEnumerator BuildSegmentLayer()
    {

        #region Local Variables

        // Locally saves Z position
        int savedPosZ = m_previousCheckpoint;

        int currentLayer = Mathf.Clamp(Mathf.FloorToInt(savedPosZ / 10), 1, m_mapArray.Count);

        #endregion

        if (savedPosZ > m_map.transform.childCount * 10) { yield break; }

        // Spawns segments into world
        #region Segment Spawning

        if (currentLayer % 2 == 0)
        {
            for (int s = 0; s < m_mapArray[currentLayer - 1].Count; s++)
            {
                StartCoroutine(AddSegment(m_mapArray[currentLayer - 1][s]));

                yield return new WaitForSeconds(m_segmentAnimationDelay);
            }
        }
        else
        {
            for (int s = m_mapArray[currentLayer - 1].Count - 1; s >= 0; s--)
            {

                StartCoroutine(AddSegment(m_mapArray[currentLayer - 1][s]));

                yield return new WaitForSeconds(m_segmentAnimationDelay);
            }
        }

        #endregion

        yield return null;
    }

    //private void SetupAndSpawnEnemy(Vector3 p)
    //{
    //    // Generate random number between 0 and 100
    //    int randomNum = UnityEngine.Random.Range(0, 100);

    //    // Checks if random number is lower than m_changeForEnemy number
    //    if (randomNum <= m_chanceForEnemy)
    //    {


    //        int randomCorner = UnityEngine.Random.Range(0, 3);

    //        Vector3 enemyOffset;

    //        switch (randomCorner)
    //        {
    //            // Farthest Left
    //            case (0):
    //                enemyOffset = new Vector3(-4, 0, 4);
    //                break;
    //            // Farthest Right
    //            case (1):
    //                enemyOffset = new Vector3(4, 0, 4);
    //                break;
    //            // Closest Left
    //            case (2):
    //                enemyOffset = new Vector3(-4, 0, -4);
    //                break;
    //            // Closest Right
    //            case (3):
    //                enemyOffset = new Vector3(4, 0, -4);
    //                break;
    //            // Default
    //            default:
    //                enemyOffset = new Vector3(0, 0, 0);
    //                break;
    //        }

    //        Vector3 finalEnemyPosition = p + enemyOffset;

    //        StartCoroutine(AddEnemy(m_enemies[0], finalEnemyPosition));
    //    }
    //}

    /// <summary>
    /// Removes previous layer
    /// </summary>
    /// <returns></returns>
    //private IEnumerator RemoveSegmentLayer()
    //{
    //    if (m_activeSegments.Count == 0)
    //        yield break;
    //    //StopCoroutine(RemoveSegmentLayer());

    //    Debug.Log(m_activeSegments[0].Count);

    //    GameObject[] tempArray = m_activeSegments[0].ToArray();//new GameObject[m_activeSegments[0].Count];

    //    /*for (int i = 0; i < m_activeSegments[0].Count; i++)
    //    {
    //        tempArray[i] = m_activeSegments[0][i];
    //    }*/

    //    // Remove oldest layer from being active
    //    m_activeSegments.RemoveAt(0);

    //    Debug.Log("Remove array size: " + tempArray.Length);

    //    for (int i = 0; i < tempArray.Length; i++)
    //    {
    //        StartCoroutine(RemoveSegment(tempArray[i]));

    //        yield return new WaitForSeconds(m_segmentAnimationDelay);
    //    }

    //    yield return null;
    //}

    /// <summary>
    /// Spawns segment into world
    /// </summary>
    /// <param name="s">Segment</param>
    /// <param name="pos">Position (X,Z)</param>
    /// <returns></returns>
    private IEnumerator AddSegment(GameObject s)
    {
        GameObject currentSegment = s;

        // Sets segment STARTER position
        currentSegment.transform.position = new Vector3(
            currentSegment.transform.position.x,   // X
            SEGMENT_START_HEIGHT,                       // Y
            currentSegment.transform.position.z);  // Z

        currentSegment.SetActive(true);

        // For loop based on time
        for (float t = 0; t < m_segmentAnimationDuration; t += Time.deltaTime)
        {
            // Calculate Y pos using Penner Easing
            float y = EasingFunction.GetEasingFunction(m_segmentAnimationIn)
                (SEGMENT_START_HEIGHT, END_HEIGHT, t / m_segmentAnimationDuration
            );

            // Sets segment position
            currentSegment.transform.position = new Vector3(
                currentSegment.transform.position.x, // X
                y,                                        // Y
                currentSegment.transform.position.z  // Z
            );

            yield return null;
        }

        currentSegment.transform.position = new Vector3(
            currentSegment.transform.position.x,   // X
            END_HEIGHT,                                 // Y
            currentSegment.transform.position.z    // Z
        );

        yield return null;

    }

    /// <summary>
    /// Removes given segment
    /// </summary>
    /// <param name="s">Segment</param>
    /// <returns></returns>
    //private IEnumerator RemoveSegment(GameObject s)
    //{
    //    GameObject currentSeg = s;

    //    Debug.Log("Segment getting removed: " + currentSeg);

    //    // For loop based on time
    //    for (float t = 0; t < m_segmentAnimationDuration; t += Time.deltaTime)
    //    {
    //        // Calculate Y pos using Penner Easing
    //        float y = EasingFunction.GetEasingFunction(m_segmentAnimationOut)(END_HEIGHT, SEGMENT_START_HEIGHT, t / m_segmentAnimationDuration);

    //        // Sets segment position
    //        currentSeg.transform.localPosition = new Vector3(currentSeg.transform.localPosition.x, y, currentSeg.transform.localPosition.z);

    //        yield return null;
    //    }

    //    // Destroys segment
    //    Destroy(currentSeg);

    //    yield return null;
    //}

    /// <summary>
    /// Spawns enemy to world
    /// </summary>
    /// <param name="e">Enemy</param>
    /// <param name="pos">Position (X,Z)</param>
    /// <returns></returns>
    //private IEnumerator AddEnemy(GameObject e, Vector2 pos)
    //{
    //    // Cloning enemy
    //    GameObject newEnemy = Instantiate(e) as GameObject;

    //    // Sets enemy STARTER position
    //    newEnemy.transform.localPosition = new Vector3(pos.x, ENEMY_START_HEIGHT, pos.y);

    //    // Puts segment into segment pocket
    //    newEnemy.transform.SetParent(m_segementPocket);

    //    // For loop based on time
    //    for (float t = 0; t < m_segmentAnimationDuration; t += Time.deltaTime)
    //    {
    //        // Calculate Y pos using Penner Easing
    //        float y = EasingFunction.GetEasingFunction(m_enemyAnimationIn)(ENEMY_START_HEIGHT, END_HEIGHT, t / m_segmentAnimationDuration);

    //        // Sets segment position
    //        newEnemy.transform.localPosition = new Vector3(pos.x, y, pos.y);

    //        yield return null;
    //    }

    //    // Sets enemy position to final position
    //    newEnemy.transform.localPosition = new Vector3(pos.x, END_HEIGHT, pos.y);

    //    // Adding enemy to active list
    //    m_activeEnemies.Add(newEnemy);

    //    yield return null;
    //}

    ///////////////////////////////////////
    // Helper Methods
    ///////////////////////////////////////

    /// <summary>
    /// 
    /// </summary>
    /// <param name="value">Input value</param>
    /// <param name="roundTo">Number to round up to</param>
    /// <returns></returns>
    private int RoundUpTo(float value, int roundTo)
    {
        return (roundTo - (int)value % roundTo) + (int)value;
    }

}
