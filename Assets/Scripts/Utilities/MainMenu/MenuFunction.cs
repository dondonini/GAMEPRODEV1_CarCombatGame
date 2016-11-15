using UnityEngine;
using System.Collections;

public class MenuFunction : MonoBehaviour {

    public GameObject overlay;
    public GameObject mb_ExitConfirm;

	public void ExitConfirm()
    {
        overlay.SetActive(true);
        mb_ExitConfirm.SetActive(true);

        // Fade background
        overlay.GetComponent<Animator>().SetTrigger("FadeIn");

        // Show messagebox
        mb_ExitConfirm.GetComponent<Animator>().SetTrigger("In");
    }

    public void ExitCancelled()
    {
        StartCoroutine(ExitCancelledAnimate());
    }
    
    IEnumerator ExitCancelledAnimate()
    {
        // Fade background
        overlay.GetComponent<Animator>().SetTrigger("FadeOut");

        // Show messagebox
        mb_ExitConfirm.GetComponent<Animator>().SetTrigger("Out");

        yield return new WaitForSeconds(1);

        overlay.SetActive(false);
        mb_ExitConfirm.SetActive(false);
    }
}
