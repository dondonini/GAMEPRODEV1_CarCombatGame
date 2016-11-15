using UnityEngine;
using System.Collections;

public class OnTriggerSends_T1 : MonoBehaviour {

    public Turret1_SM script;
	
	public void OnTriggerEnter(Collider other)
    {
        script.OnTriggerEnter(other);
    }

    public void OnTriggerExit(Collider other)
    {
        script.OnTriggerExit(other);
    }
}
