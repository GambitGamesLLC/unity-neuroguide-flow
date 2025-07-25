#if !GAMBIT_NEUROGUIDE
    //Class is unused if gambit.neuroguide package is missing
#else

/// <summary>
/// Zoom the camera in and out based on input
/// </summary>

#region IMPORTS

#if GAMBIT_NEUROGUIDE
using gambit.neuroguide;
using static UnityEngine.Rendering.DebugUI;

#endif

#if GAMBIT_MATHHELPER
using gambit.mathhelper;
#endif

using UnityEngine;

#endregion


public class CameraZoom : MonoBehaviour, INeuroGuideInteractable
{

    public Animator animator;

    public float threshold = 0.99f;

    public void Start()
    {
        PlayAnimationDirectly("CameraInitial");
        animator.speed = 0f;
    }

    public void OnDataUpdate(float value)
    {
        PlayAnimationDirectly("CameraAnim", 0, value);

        if (value >= threshold)
        {
            PlayAnimationDirectly("CameraZoomed", 0, value);
        }
        else
        {
            PlayAnimationDirectly("CameraAnim", 0, value);
        }
    }

    public void PlayAnimationDirectly(string stateName, int layer = 0, float normalizedTime = 0f)
    //-----------------------------------------------------------------//
    {
        if (animator != null && animator.gameObject.activeSelf)
        {
            animator.Play(stateName, 0, normalizedTime);
        }

    } //END PlayAnimationDirectly

    public void PlayAnimationTrigger(string triggerName)
    //----------------------------------------------------------//
    {
        if (animator != null)
        {
            animator.SetTrigger(triggerName);
        }

    } //END PlayAnimationTrigger
}

#endif