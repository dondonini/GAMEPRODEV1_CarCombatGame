﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class GameScript : MonoBehaviour {

    public Transform m_player;

    [Tooltip("The animation type when the segment goes up")]
    public EasingFunction.Ease m_segmentAnimationIn;

    [Tooltip("The animation type when the segment goes down")]
    public EasingFunction.Ease m_segmentAnimationOut;

    public float m_segmentAnimationDuration = 1;
    public float m_segmentAnimationDelay = 0.4f;

    [Tooltip("Distance of segments to see")]
    public uint m_segmentRenderDistance = 5;

    [Tooltip("Amount of segments making the width (Must be odd!)")]
    [SerializeField]
    private uint m_segmentLayerWidth = 3;

    public GameObject[] m_bridgeSegments;
    public GameObject[] m_gapSegments;
    public GameObject[] m_landSegments;

    private GameObject[] m_usableSegmentsForGaps;
    private GameObject[] m_usableSegmentsForLand;

    public Transform m_segementPocket;
    private List<List<GameObject>> m_activeSegments = new List<List<GameObject>>();

    // The Y position for all of the segments
    const float SEGMENT_END_HEIGHT = 0;
    const float SEGMENT_START_HEIGHT = 100;

    private SegmentInfo.Type m_previousSegmentLayerType = SegmentInfo.Type.None;
    private List<string> m_previousSegmentLayerTags = new List<string>();
    private int m_previousCheckpoint = 0;
    private int m_previousTotalZLength = 10;

    private bool m_updateLandscape = false;

    void OnValidate()
    {
        // Fixes segmentLayerWidth variable before running
        if (m_segmentLayerWidth <= 2)
        {
            Debug.LogWarning("Segment Layer Width must be higher or equal to 3! Will fix automatically...");
            m_segmentLayerWidth = 3;
        }
        else if (m_segmentLayerWidth % 2 == 0)
        {
            Debug.LogWarning("Segment Layer Width must be odd! Will fix automatically...");
            m_segmentLayerWidth += 1;
        }
    }

	// Use this for initialization
	void Start () {

        // Build usable segment arrays
        BuildUsableSegmentArrays();

    }
	
	// Update is called once per frame
	void Update () {

        //Debug.Log(Vector3.Distance(m_player.position, new Vector3(0, 0, 0)) / 10);
        Debug.DrawLine(new Vector3(-20, 0, m_previousCheckpoint), new Vector3(20, 0, m_previousCheckpoint), Color.white);
        Debug.DrawLine(new Vector3(-20, 0, m_previousCheckpoint - (m_segmentRenderDistance * 10)), new Vector3(20, 0, m_previousCheckpoint - (m_segmentRenderDistance * 10)), Color.blue);
        Debug.DrawLine(new Vector3(-20, 0, m_previousCheckpoint + (m_segmentRenderDistance * 10)), new Vector3(20, 0, m_previousCheckpoint + (m_segmentRenderDistance * 10)), Color.green);

        if (m_player.transform.position.z >= (m_previousCheckpoint - (m_segmentRenderDistance * 10)))
        {
            //Debug.Log("NEW LAYER");

            StartCoroutine(BuildSegmentLayer());
            if (m_previousCheckpoint - ((m_segmentRenderDistance * 10) * 2) >= m_player.transform.position.z - (m_segmentRenderDistance * 10) && m_player.transform.position.z - (m_segmentRenderDistance * 10) > 50)
            {
                //Debug.Log("DELETING OLD LAYER!");
                StartCoroutine(RemoveSegmentLayer());
            }
            m_previousCheckpoint += m_previousTotalZLength;

            
        }

        Debug.DrawLine(new Vector3(-20, 0, m_player.transform.position.z - (m_segmentRenderDistance * 10)), new Vector3(20, 0, m_player.transform.position.z - (m_segmentRenderDistance * 10)), Color.red);
        Debug.DrawLine(new Vector3(-20, 0, m_previousCheckpoint - (m_segmentRenderDistance * 10) * 2), new Vector3(20, 0, m_previousCheckpoint - (m_segmentRenderDistance * 10) * 2), Color.red);
        
    }

    private void BuildUsableSegmentArrays()
    {
        // Building usable segments for land array
        m_usableSegmentsForLand = new GameObject[m_landSegments.Length];

        for (int i = 0; i < m_landSegments.Length; i++)
        {
            m_usableSegmentsForLand[i] = m_landSegments[i];
        }

        // Building usable segments for gaps array
        m_usableSegmentsForGaps = new GameObject[m_gapSegments.Length + m_bridgeSegments.Length];

        int count = 0;
        
        for (int i = 0; i < m_gapSegments.Length; i++)
        {
            m_usableSegmentsForGaps[count] = m_gapSegments[i];

            count++;
        }

        for (int i = 0; i < m_bridgeSegments.Length; i++)
        {
            m_usableSegmentsForGaps[count] = m_bridgeSegments[i];
        }

        Debug.Log("Usable land segments: " + m_usableSegmentsForLand.Length);
        Debug.Log("Usable gap segments: " + m_usableSegmentsForGaps.Length);
    }

    private IEnumerator BuildSegmentLayer()
    {
        // Temp set
        SegmentInfo.Type currentType = SegmentInfo.Type.Land;

        // Locally save new Z position
        float savedZPos = m_previousCheckpoint;

        // Holds the usable segments for layer
        GameObject[] usableSegments;

        // Layer order
        GameObject[] tempSegs = new GameObject[m_segmentLayerWidth];

        // Layer type is gap
        bool isGap = false;

        // Prevents having two gap layers in a row
        if (m_previousSegmentLayerType == SegmentInfo.Type.Gap || m_previousSegmentLayerType == SegmentInfo.Type.None)
        {
            currentType = SegmentInfo.Type.Land;
        }
        // Selects a random layer type
        else
        {
            int randType = UnityEngine.Random.Range(0,Enum.GetNames(typeof(SegmentInfo.Type)).Length - 1);

            switch (randType)
            {
                case 0:
                    currentType = SegmentInfo.Type.Bridge;
                    isGap = true;
                    break;
                case 1:
                    currentType = SegmentInfo.Type.Gap;
                    isGap = true;
                    break;
                case 2:
                    currentType = SegmentInfo.Type.Land;
                    break;
                default:
                    currentType = SegmentInfo.Type.Land;
                    break;
            }
        }

        // Prevent having a whole layer of bridges. Aha...
        if (currentType == SegmentInfo.Type.Bridge)
        {
            currentType = SegmentInfo.Type.Gap;
        }

        // Sets current type selected to be previous for next run through
        m_previousSegmentLayerType = currentType;

        // Build usable segments array
        switch (currentType)
        {
            case SegmentInfo.Type.Land:
                usableSegments = m_usableSegmentsForLand;
                break;
            case SegmentInfo.Type.Gap:
                usableSegments = m_usableSegmentsForGaps;
                break;
            default:
                usableSegments = m_usableSegmentsForLand;
                break;
        }

        // Generates layer order
        if (isGap)
        {
            bool hasBridge = false;

            tempSegs = new GameObject[m_segmentLayerWidth];

            for (int i = 0; i < m_segmentLayerWidth; i++)
            {
                GameObject randSeg;

                // Avoids adding more bridges
                if (hasBridge)
                {
                    randSeg = m_gapSegments[UnityEngine.Random.Range(0, m_gapSegments.Length)];
                }
                // If there is no bridge yet. It will force-add a bridge at the very end
                else if (!hasBridge && i == m_segmentLayerWidth - 1)
                {
                    randSeg = m_bridgeSegments[UnityEngine.Random.Range(0, m_bridgeSegments.Length)];
                }
                // Default
                else
                {
                    randSeg = usableSegments[UnityEngine.Random.Range(0, usableSegments.Length)];
                }

                // Determinds if there is a bridge
                if (randSeg.GetComponent<SegmentInfo>().type == SegmentInfo.Type.Bridge && !hasBridge)
                {
                    hasBridge = true;
                }

                //Debug.Log(i + ": " + hasBridge);

                // Adds segment to spawn
                tempSegs[i] = randSeg;
            }
        }
        else
        {
            tempSegs = new GameObject[m_segmentLayerWidth];

            for (int i = 0; i < m_segmentLayerWidth; i++)
            {
                tempSegs[i] = usableSegments[UnityEngine.Random.Range(0, usableSegments.Length - 1)];
            }
        }

        

        // Reverses land spawning pattern
        #region Reverse pattern toggle
        bool reversePattern;

        if (m_activeSegments.Count % 2 == 0)
        {
            reversePattern = true;
        }
        else
        {
            reversePattern = false;
        }

        #endregion

        // Spawns segments into world
        if (!reversePattern)
        {
            // Index
            int i = 0;

            for (int p = -1 * (int)(m_segmentLayerWidth * 0.5f - 0.5f); p <= (int)(m_segmentLayerWidth * 0.5f - 0.5f); p++)
            {
                // Create position vector
                Vector2 newPos = new Vector2(p * 10, savedZPos);

                // Adds segment
                StartCoroutine(AddSegment(tempSegs[i], newPos));

                // Incement index
                i++;

                yield return new WaitForSeconds(m_segmentAnimationDelay);
            }
        }
        else
        {
            // Index
            int i = 0;

            for (int p = (int)(m_segmentLayerWidth * 0.5f - 0.5f); p >= -(int)(m_segmentLayerWidth * 0.5f - 0.5f); p--)
            {
                // Create position vector
                Vector2 newPos = new Vector2(p * 10, savedZPos);

                // Adds segment
                StartCoroutine(AddSegment(tempSegs[i], newPos));

                // Incement index
                i++;

                yield return new WaitForSeconds(m_segmentAnimationDelay);
            }
        }

        yield return null;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private IEnumerator RemoveSegmentLayer()
    {
        if (m_activeSegments.Count == 0)
            yield break;
        //StopCoroutine(RemoveSegmentLayer());

        Debug.Log(m_activeSegments[0].Count);

        GameObject[] tempArray = m_activeSegments[0].ToArray();//new GameObject[m_activeSegments[0].Count];

        /*for (int i = 0; i < m_activeSegments[0].Count; i++)
        {
            tempArray[i] = m_activeSegments[0][i];
        }*/

        // Remove oldest layer from being active
        m_activeSegments.RemoveAt(0);

        Debug.Log("Remove array size: " + tempArray.Length);

        for (int i = 0; i < tempArray.Length; i++)
        {
            StartCoroutine(RemoveSegment(tempArray[i]));

            yield return new WaitForSeconds(m_segmentAnimationDelay);
        }

        yield return null;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="s">Segment</param>
    /// <param name="pos">Position (X,Z)</param>
    /// <returns></returns>
    private IEnumerator AddSegment(GameObject s, Vector2 pos)
    {

        // Instantiate new segment
        GameObject newSegment = Instantiate(s) as GameObject;

        // Sets segment STARTER position
        newSegment.transform.localPosition = new Vector3(pos.x, SEGMENT_START_HEIGHT, pos.y);

        // Puts segment into segment pocket
        newSegment.transform.SetParent(m_segementPocket);

        m_activeSegments.Add(new List<GameObject>());

        // For loop based on time
        for (float t = 0; t < m_segmentAnimationDuration; t += Time.deltaTime)
        {
            // Calculate Y pos using Penner Easing
            float y = EasingFunction.GetEasingFunction(m_segmentAnimationIn)(SEGMENT_START_HEIGHT, SEGMENT_END_HEIGHT, t / m_segmentAnimationDuration);

            // Sets segment position
            newSegment.transform.localPosition = new Vector3(pos.x, y, pos.y);

            yield return null;
        }

        newSegment.transform.localPosition = new Vector3(pos.x, SEGMENT_END_HEIGHT, pos.y);

        m_activeSegments[(int)(pos.y * 0.1)].Add(newSegment);
        Debug.Log("Layer #: " + (int)(pos.y * 0.1) + ", Segment position: " + (int)(pos.x * 0.1));
        Debug.Log("Layer Array Size: " + m_activeSegments[(int)(pos.y * 0.1)].Count);

        yield return null;

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="s">Segment</param>
    /// <returns></returns>
    private IEnumerator RemoveSegment(GameObject s)
    {
        GameObject currentSeg = s;

        Debug.Log("Segment getting removed: " + currentSeg);

        // For loop based on time
        for (float t = 0; t < m_segmentAnimationDuration; t += Time.deltaTime)
        {
            // Calculate Y pos using Penner Easing
            float y = EasingFunction.GetEasingFunction(m_segmentAnimationOut)(SEGMENT_END_HEIGHT, SEGMENT_START_HEIGHT, t / m_segmentAnimationDuration);

            // Sets segment position
            currentSeg.transform.localPosition = new Vector3(currentSeg.transform.localPosition.x, y, currentSeg.transform.localPosition.z);

            yield return null;
        }

        // Destroys segment
        Destroy(currentSeg);

        yield return null;
    }

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