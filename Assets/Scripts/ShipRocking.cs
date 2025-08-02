#region IMPORTS

#if GAMBIT_NEUROGUIDE
using gambit.neuroguide;
#endif

#if GAMBIT_MATHHELPER
using gambit.mathhelper;
#endif

using UnityEngine;

#endregion

public class ShipRocking : MonoBehaviour, INeuroGuideInteractable
{

    #region PUBLIC - VARIABLES

    public Animator animator;

    public float threshold = 0.99f;

    #endregion

    #region PUBLIC - START

    public void Start()
    {
        PlayAnimationDirectly("ShipInitial");
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
        PlayAnimationDirectly("ShipRockAnim", 0, value);

        if (value >= threshold)
        {
            PlayAnimationDirectly("ShipRockAnim", 0, value);
        }
        else
        {
            PlayAnimationDirectly("ShipRockAnim", 0, value);
        }
    }

    #endregion

    #region PUBLIC - PLAY ANIMATION DIRECTLY

    //-----------------------------------------------------------------//
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

    //----------------------------------------------------------//
    public void PlayAnimationTrigger(string triggerName)
    //----------------------------------------------------------//
    {
        if (animator != null)
        {
            animator.SetTrigger(triggerName);
        }

    } //END PlayAnimationTrigger

    #endregion

} //END ShipRocking Class