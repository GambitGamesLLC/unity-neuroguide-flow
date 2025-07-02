using gambit.neuroguide;
using UnityEngine;

public class NeuroBasicAnimator : MonoBehaviour, INeuroGuideInteractable
{
    #region VARIABLES

    /// <summary>
    /// Animator for the sun lighting
    /// </summary>
    [SerializeField] private Animator animator = null;

    /// <summary>
    /// String signifying the name of the animation to be played
    /// </summary>
    [SerializeField] private string animationStateName = string.Empty;

    #endregion


    #region MONOBEHAVIOURS

    private void Awake()
    {
        animator.speed = 0f;
    }

    #endregion


    #region METHODS

    #region  PUBLIC - NEUROGUIDE - ON DATA UPDATE

    /// <summary>
    /// Called when the NeuroGuide hardware updates
    /// </summary>
    /// <param name="system">The NeuroGuide system object</param>
    //------------------------------------------------------------------------//
    public virtual void OnDataUpdate(float _value)
    //------------------------------------------------------------------------//
    {
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
            animator.Play(_stateName, 0, _normalizedTime);
        }

    } //END PlayAnimationDirectly

    #endregion

    #endregion
}

