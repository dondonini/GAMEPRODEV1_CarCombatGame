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

    public Transform m_segementPocket;
    private List<GameObject[]> m_activeSegments;

    // The Y position for all of the segments
    const float SEGMENT_END_HEIGHT = 0;
    const float SEGMENT_START_HEIGHT = -100;

    private SegmentInfo.Type m_previousSegmentLayerType = SegmentInfo.Type.None;
    private List<string> m_previousSegmentLayerTags = new List<string>();
    private int m_previousCheckpoint = -10;
    private int m_previousTotalZLength = 10;

    private bool m_updateLandscape = false;

    public Transform test;

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

    }
	
	// Update is called once per frame
	void Update () {

        //Debug.Log(Vector3.Distance(m_player.position, new Vector3(0, 0, 0)) / 10);
        if (m_player.transform.position.z >= (m_previousCheckpoint - ((m_segmentRenderDistance * 10) * 0.5f)))
        {
            m_previousCheckpoint += m_previousTotalZLength;

            test.position = new Vector3(0, 0, m_previousCheckpoint);
            Debug.Log("NEW LAYER");

            StartCoroutine(BuildSegmentLayer());
        }
        Debug.DrawLine(new Vector3(-10, 0, m_previousCheckpoint), new Vector3(10, 0, m_previousCheckpoint));
    }


    IEnumerator BuildSegmentLayer()
    {
        // Temp set
        SegmentInfo.Type currentType = SegmentInfo.Type.Land;

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
                    break;
                case 1:
                    currentType = SegmentInfo.Type.Gap;
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

        GameObject[] usableSegments;

        switch (currentType)
        {
            case SegmentInfo.Type.Land:
                usableSegments = new GameObject[m_landSegments.Length];

                for (int i = 0; i < m_landSegments.Length; i++)
                {
                    usableSegments[i] = m_landSegments[i];
                }
                break;
            case SegmentInfo.Type.Gap:
                usableSegments = new GameObject[m_landSegments.Length];

                for (int i = 0; i < m_gapSegments.Length; i++)
                {
                    usableSegments[i] = m_landSegments[i];
                }
                break;
            default:
                usableSegments = new GameObject[m_landSegments.Length];

                for (int i = 0; i < m_landSegments.Length; i++)
                {
                    usableSegments[i] = m_landSegments[i];
                }
                break;
        }

        float savedZPos = m_previousCheckpoint;

        for (int p = -1 * (int)(m_segmentLayerWidth * 0.5 - 0.5); p <= (int)(m_segmentLayerWidth * 0.5 - 0.5); p++)
        {
            Debug.Log(p);

            GameObject randSeg = usableSegments[UnityEngine.Random.Range(0, usableSegments.Length - 1)];

            Vector2 newPos = new Vector2(p * 10, savedZPos);

            StartCoroutine(AddSegment(randSeg, newPos));

            yield return new WaitForSeconds(m_segmentAnimationDelay);
        }
        yield return null;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="s"></param>
    /// <param name="pos"></param>
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
