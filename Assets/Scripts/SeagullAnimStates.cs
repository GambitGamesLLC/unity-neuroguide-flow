#region IMPORTS

#if GAMBIT_NEUROGUIDE
using gambit.neuroguide;
#endif

using UnityEngine;
using System.Collections;

#endregion

public class SeagullAnimStates : MonoBehaviour, INeuroGuideAnimationExperienceInteractable
{
    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {

       // StartCoroutine(ExecuteAfterDelay());
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

    public void OnAboveThreshold()
    {
       
    }

    public void OnBelowThreshold()
    {
        
    }

    public void OnRecievingRewardChanged(bool isRecievingReward)
    {
        if(isRecievingReward == true)
        {
            //Debug.Log("Reward");
            animator.SetBool("LightFlap", true);
            animator.SetBool("Glide", false);
            // StartCoroutine(ExecuteAfterDelay());
        }

        else
        {
            //Debug.Log("noReward");
            animator.SetBool("LightFlap", false);
            animator.SetBool("Glide", true);
        }
    }

    public void OnDataUpdate(float normalizedValue)
    {

    }
}