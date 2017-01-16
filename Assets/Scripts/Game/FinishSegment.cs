using UnityEngine;
using System.Collections;

public class FinishSegment : MonoBehaviour {

    public Transform m_endPosition;
    public float animationDuration = 5.0f;
    public EasingFunction.Ease animation1Type;
    public EasingFunction.Ease animation2Type;

    private bool touched = false;
    private bool active = false;

    private float currentTime = 0.0f;

    private float animation1Duration, animation2Duration;

	// Use this for initialization
	void Start () {

        animation1Duration = Mathf.Lerp(0.0f, animationDuration, 0.75f);
        animation2Duration = Mathf.Lerp(0.0f, animationDuration, 0.25f);
	}

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            touched = true;
        }
    }
	
	// Update is called once per frame
	void Update () {
	
        if (touched)
        {
            

            currentTime += Time.deltaTime;
        }
	}
}
