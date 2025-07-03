using gambit.mathhelper;
using gambit.neuroguide;
using UnityEngine;

public class WaterDroplets : NeuroBasicAnimator, INeuroGuideInteractable
{
    #region VARIABLES

    /// <summary>
    /// Material for the water droplets that appear on the screen
    /// </summary>
    [SerializeField] private Material dropletMat = null;
    /// <summary>
    /// GameObject of the water droplets that appear on the screen
    /// </summary>
    [SerializeField] private GameObject dropletObject = null;
    [Space]
    /// <summary>
    /// Minimum value this material should be set to 
    /// </summary>
    [SerializeField] private float dropletMin = 0f;

    /// <summary>
    /// Maximum value this material should be set to; Default to 1
    /// </summary>
    [SerializeField] private float dropletMax = 1f;
    [Space]
    /// <summary>
    /// How far into the NeuroGuideExperience should we be before we cross the threshold? Uses a 0-1 normalized percentage value
    /// </summary>
    [SerializeField] private float threshold = 0f;

    #endregion


    #region MONOBEHAVIOURS

    private void Awake()
    {
        if (dropletMat != null)
        {
            dropletMat.SetFloat("_RainAmount", dropletMin);
        }
    }

    private void OnApplicationQuit()
    {
        if (dropletMat != null)
        {
            dropletMat.SetFloat("_RainAmount", dropletMin);
        }
    }

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

        //Animate our cube grunge texture
#if GAMBIT_MATHHELPER

        if (dropletMat != null)
            dropletMat.SetFloat("_RainAmount", _value);
#endif

    }

    #endregion

    #endregion
}

