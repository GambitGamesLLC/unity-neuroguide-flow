using UnityEngine;

public class WaveDeformer : NeuroBasicAnimator
{
    #region VARIABLES

    /// <summary>
    /// The wave deformer game object
    /// </summary>
    [SerializeField] private GameObject waveDeformer;

    /// <summary>
    /// How far into the NeuroGuideExperience should we be before we cross the threshold? Uses a 0-1 normalized percentage value
    /// </summary>
    [SerializeField] private float threshold = 0f;

    #endregion


    #region METHODS

    #region  PUBLIC - NEUROGUIDE - ON DATA UPDATE

    /// <summary>
    /// Called when the NeuroGuide hardware updates
    /// </summary>
    /// <param name="system">The NeuroGuide system object</param>
    //------------------------------------------------------------------------//
    public override void OnDataUpdate(float _value)
    //------------------------------------------------------------------------//
    {
        base.OnDataUpdate(_value);

        // Hide the deformer at a certain point
        //waveDeformer.SetActive(_value < threshold);
    }

    #endregion


    #endregion
}
