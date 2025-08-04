using UnityEngine;

public class WaveDeformer : NeuroBasicAnimator
{
    #region VARIABLES

    #endregion

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
    }

    #endregion

} //END WaveDeformer Class