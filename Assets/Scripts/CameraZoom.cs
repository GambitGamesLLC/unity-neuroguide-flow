#region IMPORTS

#if GAMBIT_NEUROGUIDE
using gambit.neuroguide;
#endif

#if GAMBIT_MATHHELPER
using gambit.mathhelper;
#endif

using UnityEngine;

#endregion

public class CameraZoom : MonoBehaviour, INeuroGuideAnimationExperienceInteractable
{
    #region PUBLIC - VARIABLES
    
    public Animator animator;

    private bool isAboveThreshold;

    #endregion

    #region PUBLIC - START

    public void Start()
    {
        PlayAnimationDirectly("CameraInitial");
        animator.speed = 0f;
    }

    #endregion

    #region PUBLIC - NEUROGUIDE - ON RECIEVING REWARD CHANGED

    /// <summary>
    /// Called when the NeuroGuide software starts or stops sending the user a reward
    /// </summary>
    /// <param name="isRecievingReward">Is the user currently recieiving a reward?</param>
    //--------------------------------------------------------------------//
    public void OnRecievingRewardChanged( bool isRecievingReward )
    //--------------------------------------------------------------------//
    {

    } //END OnRecievingRewardChanged

    #endregion

    #region PUBLIC - NEUROGUIDE - ON DATA UPDATE

    public void OnDataUpdate(float value)
    {
        PlayAnimationDirectly("CameraAnim", 0, value);

        if (isAboveThreshold)
        {
            PlayAnimationDirectly("CameraZoomed", 0, value);
        }
        else
        {
            PlayAnimationDirectly("CameraAnim", 0, value);
        }
    }

    #endregion

    #region PUBLIC - NEUROGUIDE - ON ABOVE THRESHOLD

    /// <summary>
    /// Called when the NeuroGuideAnimationExperience has a score thats above the threshold value
    /// </summary>
    //------------------------------------//
    public void OnAboveThreshold()
    //------------------------------------//
    {
        isAboveThreshold = true;

    } //END OnAboveThreshold

    #endregion

    #region PUBLIC - NEUROGUIDE - ON BELOW THRESHOLD

    /// <summary>
    /// Called when the NeuroGuideAnimationExperience has a score thats below the threshold value
    /// </summary>
    //-------------------------------------//
    public void OnBelowThreshold()
    //-------------------------------------//
    {
        isAboveThreshold = false;

    } //END OnBelowThreshold

    #endregion

    #region PUBLIC - PLAY ANIMATION DIRECTLY

    public void PlayAnimationDirectly(string stateName, int layer = 0, float normalizedTime = 0f)
    //-----------------------------------------------------------------//
    {
        if (animator != null && animator.gameObject.activeSelf)
        {
            animator.Play(stateName, 0, normalizedTime);
        }

    } //END PlayAnimationDirectly

    #endregion

    #region PUBLIC - PLAY ANIMATION TRIGGER

    public void PlayAnimationTrigger(string triggerName)
    //----------------------------------------------------------//
    {
        if (animator != null)
        {
            animator.SetTrigger(triggerName);
        }

    } //END PlayAnimationTrigger

    #endregion

} //END CameraZoom Method