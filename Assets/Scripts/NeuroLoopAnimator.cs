#region IMPORTS

#if GAMBIT_NEUROGUIDE
using gambit.neuroguide;
#endif

using UnityEngine;

#endregion

public class NeuroLoopAnimator : MonoBehaviour, INeuroGuideInteractable
{

    #region PRIVATE - VARIABLES

    /// <summary>
    /// Animator for the sun lighting
    /// </summary>
    [SerializeField] private Animator animator = null;

    /// <summary>
    /// String signifying the name of the animation to be played
    /// </summary>
    [SerializeField] private string animationStateName = string.Empty;

    #endregion

    #region PRIVATE - AWAKE

    private void Awake()
    {
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

    #region  PUBLIC - NEUROGUIDE - ON DATA UPDATE

    /// <summary>
    /// Called when the NeuroGuide hardware updates
    /// </summary>
    /// <param name="system">The NeuroGuide system object</param>
    //------------------------------------------------------------------------//
    public virtual void OnDataUpdate(float _value)
    //------------------------------------------------------------------------//
    {
        if (_value >= 0.99f)
        {
            return;
        }

        PlayAnimationDirectly(animationStateName, 0, _value);
    }

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
            animator.Play(_stateName, 0, 1f);
        }

    } //END PlayAnimationDirectly

    #endregion

} //END NeuroLoopAnimator Class