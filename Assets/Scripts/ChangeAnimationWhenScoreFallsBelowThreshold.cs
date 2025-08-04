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
        Debug.Log( NeuroGuideAnimationExperience.system.currentProgressInSeconds );
    }

    #endregion

    #region PUBLIC - NEUROGUIDE - ON RECIEVING REWARD CHANGED

    public void OnRecievingRewardChanged( bool isRecievingReward )
    {
        
    }

    #endregion

} //END ChangeScoreWhenBelowThreshold Class