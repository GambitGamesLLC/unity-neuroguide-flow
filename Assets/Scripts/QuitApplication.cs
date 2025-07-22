using UnityEngine;
using UnityEngine.InputSystem;

public class QuitApplication: MonoBehaviour
{

    #region PUBLIC - UPDATE

    /// <summary>
    /// Unity Lifecycle Method
    /// </summary>
    //------------------------------------------------------//
    public void Update()
    //------------------------------------------------------//
    {
        CheckIfApplicationShouldClose();

    } //END Update

    #endregion

    #region PRIVATE - CHECK IF APPLICATION SHOULD CLOSE

    /// <summary>
    /// Polls the Unity keyboard input to see if the Escape key is pressed, then closes the app
    /// </summary>
    //------------------------------------------------------//
    private void CheckIfApplicationShouldClose()
    //------------------------------------------------------//
    {
        bool isPressed = false;

#if UNITY_INPUT
        if(Keyboard.current.escapeKey.isPressed)
        {
            isPressed = true;
        }
#else
        if ( Input.GetKeyUp( KeyCode.Escape ) )
        {
            isPressed = true;
        }
#endif

        // Check if the Escape key is pressed
        if( isPressed )
        {
            // Quit the application
            Application.Quit();

            // If running in the Unity Editor, stop play mode
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }

    } //END CheckIfApplicationShouldClose Method

    #endregion

} //END QuitApplication Class