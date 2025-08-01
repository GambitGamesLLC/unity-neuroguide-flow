using System.Collections;
using UnityEngine;

public class SeagullAnimStates : MonoBehaviour
{
    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator.SetTrigger("LightFlap");

        StartCoroutine(ExecuteAfterDelay());
    }

    IEnumerator ExecuteAfterDelay()
    {
        while (true)
        {
            //Debug.Log("Starting the delay...");

            // Wait for 3 seconds
            yield return new WaitForSeconds(3f);

            animator.SetTrigger("StrongFlap");

            //Debug.Log("3 seconds have passed!");

            // You can add more actions here after the delay
            // For example, another wait or a different action
            yield return new WaitForSeconds(1f);
            //Debug.Log("Another second has passed!");
            animator.SetTrigger("LightFlap");
        }
    }
}