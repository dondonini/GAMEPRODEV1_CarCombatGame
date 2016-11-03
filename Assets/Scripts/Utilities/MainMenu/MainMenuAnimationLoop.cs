using UnityEngine;
using System.Collections;

public class MainMenuAnimationLoop : MonoBehaviour {

    public GameObject[] m_miniMaps;
    public EasingFunction.Ease m_segmentAnimationIn;
    public EasingFunction.Ease m_segmentAnimationOut;
    public float m_segmentAnimationDuration = 1;
    public float m_segmentAnimationDelay = 0.1f;
    public float m_waitTillNextMap = 10.0f;
    public float SEGMENT_START_HEIGHT = 100;
    public float END_HEIGHT = 0;

    private int m_previousMap = -1;

    // Use this for initialization
    void Start () {
        StartCoroutine(AnimationLoop());
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    private IEnumerator AnimationLoop()
    {
        while (true)
        {
            int randMap = Random.Range(0, m_miniMaps.Length - 1);

            if (m_previousMap != randMap)
            {
                m_previousMap = randMap;
                //Debug.Log((m_segmentAnimationDuration * m_miniMaps[randMap].transform.childCount));

                // Segment exit final position

                for (int s = 0; s < m_miniMaps[randMap].transform.childCount; s++)
                {
                    m_miniMaps[randMap].transform.GetChild(s).position = new Vector3(m_miniMaps[randMap].transform.GetChild(s).position.x, SEGMENT_START_HEIGHT, m_miniMaps[randMap].transform.GetChild(s).position.z);
                }

                // Load new segments

                m_miniMaps[randMap].SetActive(true);

                // Segment enter

                for (int s = 0; s < m_miniMaps[randMap].transform.childCount; s++)
                {

                    StartCoroutine(AddSegment(m_miniMaps[randMap].transform.GetChild(s).gameObject));

                    yield return new WaitForSeconds(m_segmentAnimationDelay);
                }

                yield return new WaitForSeconds(m_segmentAnimationDelay * m_miniMaps[randMap].transform.childCount);

                yield return new WaitForSeconds(m_waitTillNextMap);

                // Segment exit

                for (int s = 0; s < m_miniMaps[randMap].transform.childCount; s++)
                {

                    StartCoroutine(RemoveSegment(m_miniMaps[randMap].transform.GetChild(s).gameObject));

                    yield return new WaitForSeconds(m_segmentAnimationDelay);
                }

                yield return new WaitForSeconds(m_segmentAnimationDelay * m_miniMaps[randMap].transform.childCount + m_segmentAnimationDuration);

                m_miniMaps[randMap].SetActive(false);

                for (int s = 0; s < m_miniMaps[randMap].transform.childCount; s++)
                {
                    m_miniMaps[randMap].transform.GetChild(s).position = new Vector3(m_miniMaps[randMap].transform.GetChild(s).position.x, END_HEIGHT, m_miniMaps[randMap].transform.GetChild(s).position.z);
                }
            }
        }
    }

    private IEnumerator AddSegment(GameObject s)
    {
        GameObject currentSegment = s;

        // Sets segment STARTER position
        currentSegment.transform.position = new Vector3(
            currentSegment.transform.position.x,   // X
            -SEGMENT_START_HEIGHT,                       // Y
            currentSegment.transform.position.z);  // Z

        // For loop based on time
        for (float t = 0; t < m_segmentAnimationDuration; t += Time.deltaTime)
        {
            // Calculate Y pos using Penner Easing
            float y = EasingFunction.GetEasingFunction(m_segmentAnimationIn)
                (-SEGMENT_START_HEIGHT, END_HEIGHT, t / m_segmentAnimationDuration
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

    private IEnumerator RemoveSegment(GameObject s)
    {
        GameObject currentSegment = s;

        // Sets segment STARTER position
        currentSegment.transform.position = new Vector3(
            currentSegment.transform.position.x,   // X
            END_HEIGHT,                       // Y
            currentSegment.transform.position.z);  // Z

        // For loop based on time
        for (float t = 0; t < m_segmentAnimationDuration; t += Time.deltaTime)
        {
            // Calculate Y pos using Penner Easing
            float y = EasingFunction.GetEasingFunction(m_segmentAnimationOut)
                (END_HEIGHT, -SEGMENT_START_HEIGHT, t / m_segmentAnimationDuration
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
            -SEGMENT_START_HEIGHT,                                 // Y
            currentSegment.transform.position.z    // Z
        );

        yield return null;

    }
}
