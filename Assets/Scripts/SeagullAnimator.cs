#region IMPORTS

#if GAMBIT_NEUROGUIDE
using gambit.neuroguide;
#endif

using UnityEngine;

#endregion

public class SeagullAnimator : MonoBehaviour, INeuroGuideAnimationExperienceInteractable
{
    #region PRIVATE - VARIABLES

    /// <summary>
    /// Animator for the sun lighting
    /// </summary>
    [SerializeField] protected Animator animator = null;

    /// <summary>
    /// String signifying the name of the animation to be played
    /// </summary>
    [SerializeField] protected string animationStateName = string.Empty;

    public float animSpeed;

    public string animIn;

    /// <summary>
    /// Flag set when going above and below the threshold
    /// </summary>
    [SerializeField] protected bool isAboveThreshold = false;

    #endregion

    #region PUBLIC - AWAKE



    private void Awake()
    {
        animator.speed = animSpeed;
    }

    #endregion

    #region PUBLIC - NEUROGUIDE - ON RECIEVING REWARD CHANGED

    /// <summary>
    /// Called when the NeuroGuide software starts or stops sending the user a reward
    /// </summary>
    /// <param name="isRecievingReward">Is the user currently recieiving a reward?</param>
    //--------------------------------------------------------------------//
    public void OnRecievingRewardChanged(bool isRecievingReward)
    //--------------------------------------------------------------------//
    {

    } //END OnRecievingRewardChanged

    #endregion

    #region  PUBLIC - NEUROGUIDE - ON DATA UPDATE

    /// <summary>
    /// Called when the NeuroGuide hardware updates
    /// </summary>
    /// <param name="system">The NeuroGuide system object</param>
    //------------------------------------------------------------------------//
    public virtual void OnDataUpdate(float _value)
    //------------------------------------------------------------------------//
    {
        if (isAboveThreshold == false)
        {
            animator.SetBool(animIn, false);

        }
        if (isAboveThreshold == true)
        {
            animator.SetBool(animIn, true);
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
       // animationStateName = aboveAnimState;

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
        //animationStateName = belowAnimState;

    } //END OnBelowThreshold

    #endregion

    #region PUBLIC - PLAY ANIMATION DIRECTLY

    /// <summary>
    /// Call this method to directly play an animation state
    /// </summary>
    /// <param name="_stateName"></param>
    //-----------------------------------------------------------------//
    public virtual void PlayAnimationDirectly(string _stateName, int _layer = 0, float _normalizedTime = 0f)
    //-----------------------------------------------------------------//
    {
        if (animator != null && animator.gameObject.activeSelf)
        {
            animator.Play(_stateName, 0, _normalizedTime);
        }

    } //END PlayAnimationDirectly

    #endregion

} //END NeuroBasicAnimator Class