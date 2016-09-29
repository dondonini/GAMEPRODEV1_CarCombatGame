using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class GameScript : MonoBehaviour {

    public Transform m_player;

    [Tooltip("The animation type when the segment goes up")]
    public EasingFunction.Ease m_segmentAnimationUp;

    [Tooltip("The animation type when the segment goes down")]
    public EasingFunction.Ease m_segmentAnimationDown;

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
    private List<GameObject[]> m_activeSegments;

    // The Y position for all of the segments
    const float SEGMENT_END_HEIGHT = 0;
    const float SEGMENT_START_HEIGHT = 100;

    private SegmentInfo.Type m_previousSegmentLayerType = SegmentInfo.Type.None;
    private List<string> m_previousSegmentLayerTags = new List<string>();
    private int m_previousCheckpoint = -10;
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

        m_activeSegments = new List<GameObject[]>();

        // Build usable segment arrays
        BuildUsableSegmentArrays();

    }
	
	// Update is called once per frame
	void Update () {

        //Debug.Log(Vector3.Distance(m_player.position, new Vector3(0, 0, 0)) / 10);
        if (m_player.transform.position.z >= (m_previousCheckpoint - ((m_segmentRenderDistance * 10) * 0.5f)))
        {
            m_previousCheckpoint += m_previousTotalZLength;

            //Debug.Log("NEW LAYER");

            StartCoroutine(BuildSegmentLayer());
        }
        Debug.DrawLine(new Vector3(-10, 0, m_previousCheckpoint), new Vector3(10, 0, m_previousCheckpoint));
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

    IEnumerator BuildSegmentLayer()
    {
        // Temp set
        SegmentInfo.Type currentType = SegmentInfo.Type.Land;

        bool isGap = false;

        if (m_previousSegmentLayerType == SegmentInfo.Type.Gap || m_previousSegmentLayerType == SegmentInfo.Type.None)
        {
            currentType = SegmentInfo.Type.Land;
        }
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

        m_previousSegmentLayerType = currentType;

        GameObject[] usableSegments;

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

        float savedZPos = m_previousCheckpoint;

        GameObject[] tempSegs = new GameObject[m_segmentLayerWidth];

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
                    randSeg = m_gapSegments[UnityEngine.Random.Range(0, m_gapSegments.Length - 1)];
                }
                // If there is no bridge yet. It will force-add a bridge at the very end
                else if (!hasBridge && i == m_segmentLayerWidth - 1)
                {
                    randSeg = m_bridgeSegments[UnityEngine.Random.Range(0, m_bridgeSegments.Length - 1)];
                }
                // Default
                else
                {
                    randSeg = usableSegments[UnityEngine.Random.Range(0, usableSegments.Length - 1)];
                }

                // Determinds if there is a bridge
                if (randSeg.GetComponent<SegmentInfo>().type == SegmentInfo.Type.Bridge && !hasBridge)
                {
                    hasBridge = true;
                }

                Debug.Log(i + ": " + hasBridge);

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
        

        m_activeSegments.Add(tempSegs);
        
        yield return null;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="s">Segment</param>
    /// <param name="pos">Position (X,Z)</param>
    /// <returns></returns>
    IEnumerator AddSegment(GameObject s, Vector2 pos)
    {

        // Instantiate new segment
        GameObject newSegment = Instantiate(s) as GameObject;

        // Sets segment STARTER position
        newSegment.transform.localPosition = new Vector3(pos.x, SEGMENT_START_HEIGHT, pos.y);

        // Puts segment into segment pocket
        newSegment.transform.SetParent(m_segementPocket);

        // For loop based on time
        for (float t = 0; t < m_segmentAnimationDuration; t += Time.deltaTime)
        {
            // Calculate Y pos using Penner Easing
            float y = EasingFunction.GetEasingFunction(m_segmentAnimationUp)(SEGMENT_START_HEIGHT, SEGMENT_END_HEIGHT, t / m_segmentAnimationDuration);

            // Sets segment position
            newSegment.transform.localPosition = new Vector3(pos.x, y, pos.y);

            yield return null;
        }

        newSegment.transform.localPosition = new Vector3(pos.x, SEGMENT_END_HEIGHT, pos.y);
        yield return null;

    }

    IEnumerator RemoveSegment(GameObject s)
    {
        // For loop based on time
        for (float t = 0; t < m_segmentAnimationDuration; t += Time.deltaTime)
        {
            // Calculate Y pos using Penner Easing
            float y = EasingFunction.GetEasingFunction(m_segmentAnimationDown)(SEGMENT_END_HEIGHT, SEGMENT_START_HEIGHT, t / m_segmentAnimationDuration);

            // Sets segment position
            s.transform.localPosition = new Vector3(s.transform.localPosition.x, y, s.transform.localPosition.z);

            yield return null;
        }

        // Destroys segment
        Destroy(s);
        yield return null;
    }

    ///////////////////////////////////////
    // Helper Methods
    ///////////////////////////////////////


}
