#region IMPORTS

using UnityEngine;
using gambit.mathhelper;


#if GAMBIT_NEUROGUIDE
using gambit.neuroguide;
#endif

#endregion



public class ChangeAnimationWhenScoreFallsBelowThreshold : MonoBehaviour, INeuroGuideAnimationExperienceInteractable
{
    #region PUBLIC - VARIABLES

    /// <summary>
    /// The time in the animation to switch to, normalized 0-1
    /// </summary>
    public float changeToProgress = 0.9f;

    #endregion

    #region PUBLIC - START

    /// <summary>
    /// Unity lifecycle function
    /// </summary>
    //------------------------------//
    public void Start()
    //------------------------------//
    {
        PreventChangeToProgressValueBeingAboveThreshold();

    } //END Start Method

    #endregion

    #region PRIVATE - PREVENT CHANGE TO PROGRESS VALUE BEING ABOVE THRESHOLD

    /// <summary>
    /// Prevents the change to progress value from accidentally being set to a value higher then the threshold.
    /// If this was allowed to occur, you could enter a state that impossible to leave
    /// </summary>
    //-------------------------------------------------------------------//
    private void PreventChangeToProgressValueBeingAboveThreshold()
    //-------------------------------------------------------------------//
    {

        if(NeuroGuideAnimationExperience.system == null)
        {
            return;
        }

        if(changeToProgress > NeuroGuideAnimationExperience.system.options.threshold)
        {
            changeToProgress = NeuroGuideAnimationExperience.system.options.threshold;
            Debug.LogWarning( "ChangeAnimationWhenScoreFallsBelowThreshold.cs PreventChangeToProgressValueBeingAboveThreshold() The changeToProgress value cannot be above the threshold. Forcibly setting the changeToProgress value to the same value as the threshold." );
        }

    } //END PreventChangeToProgressValueBeingAboveThreshold Method

    #endregion

    #region PUBLIC - NEUROGUIDE - ON ABOVE THRESHOLD

    public void OnAboveThreshold()
    {
        
    }

    #endregion

    #region PUBLIC - NEUROGUIDE - ON BELOW THRESHOLD

    /// <summary>
    /// Called by the NeuroGuideAnimationExperience when the score goes below the threshold.
    /// We set the score to an arbitrary value, resetting the animation to a far earlier state
    /// </summary>
    //--------------------------------------//
    public void OnBelowThreshold()
    //--------------------------------------//
    {
        if(NeuroGuideAnimationExperience.system != null)
        {
            float currentProgressInSeconds = MathHelper.Map( changeToProgress, 0f, 1f, 0f, NeuroGuideAnimationExperience.system.options.totalDurationInSeconds );
            NeuroGuideAnimationExperience.system.currentProgressInSeconds = currentProgressInSeconds;
        }
        
    } //END OnBelowThreshold Method

    #endregion

    #region PUBLIC - NEUROGUIDE - ON DATA UPDATE

    public void OnDataUpdate( float normalizedValue )
    {
        //Debug.Log( NeuroGuideAnimationExperience.system.currentProgressInSeconds );
    }

    #endregion

    #region PUBLIC - NEUROGUIDE - ON RECIEVING REWARD CHANGED

    public void OnRecievingRewardChanged( bool isRecievingReward )
    {
        
    }

    #endregion

} //END ChangeAnimationWhenScoreFallsBelowThreshold Class