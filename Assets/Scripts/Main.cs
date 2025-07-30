#region IMPORTS

using UnityEngine;
using System;
using System.Collections.Generic;

#if EXT_DOTWEEN
#endif

#if GAMBIT_NEUROGUIDE
using gambit.neuroguide;
#endif

#if GAMBIT_CONFIG
using gambit.config;
#endif

#if UNITY_INPUT
#endif

#if EXT_TOTALJSON
#endif

#if GAMBIT_PROCESS
using gambit.process;
#endif

#if EXT_INGAMEDEBUGCONSOLE
using IngameDebugConsole;
#endif

#endregion

/// <summary>
/// Primary entry point of the project
/// </summary>
public class Main : MonoBehaviour
{

    #region PUBLIC - VARIABLES

    /// <summary>
    /// Should we enable the debug logs?
    /// </summary>
    public bool logs = true;

    /// <summary>
    /// Should we enable the debug system for the NeuroGear hardware? This will enable keyboard events to control simulated NeuroGear hardware data spawned during the Create() method of NeuroGuideManager.cs
    /// </summary>
    public bool debug = true;

    /// <summary>
    /// How long should this experience last if the user was in a reward state continuously?
    /// </summary>
    public float length = 1f;

    /// <summary>
    /// UDP port address to listen to for NeuroGuide communication
    /// </summary>
    public string address = "127.0.0.1";

    /// <summary>
    /// UDP port to listen to for NeuroGuide communication
    /// </summary>
    public int port = 50000;

    #endregion

    #region PRIVATE - VARIABLES

    /// <summary>
    /// The config manager system instantiated at Start()
    /// </summary>
    private ConfigManager.ConfigManagerSystem configSystem;

    #endregion

    #region PUBLIC - START

    /// <summary>
    /// Unity lifecycle method
    /// </summary>
    //----------------------------------//
    public void Start()
    //----------------------------------//
    {
#if !EXT_DOTWEEN
        Debug.LogError( "Main.cs Start() Missing 'EXT_DOTWEEN' scripting define symbol and/or package" );
#endif
#if !GAMBIT_NEUROGUIDE
        Debug.LogError( "Main.cs Start() Missing 'GAMBIT_NEUROGUIDE' scripting define symbol and/or package" );
#endif
#if !GAMBIT_CONFIG
        Debug.LogError( "Main.cs Start() Missing 'GAMBIT_CONFIG' scripting define symbol and/or package" );
#endif
#if !EXT_TOTALJSON
        Debug.LogError( "Main.cs Start() Missing 'EXT_TOTALJSON' scripting define symbol and/or package" );
#endif

        LoadDataFromProcess();

    } //END Start Method

    #endregion

    #region PRIVATE - LOAD DATA FROM PROCESS IF AVAILABLE

    /// <summary>
    /// Loads data that was passed into the process
    /// </summary>
    //-------------------------------------//
    private void LoadDataFromProcess()
    //-------------------------------------//
    {

#if GAMBIT_PROCESS


        List<string> keys = ProcessManager.ReadArgumentKeys();
        List<string> values = ProcessManager.ReadArgumentValues();

        Debug.Log( "Main.cs LoadDataFromProcess() keys.count = " + keys.Count + ", values.count = " + values.Count );

        //If we have no command line key/values to read, skip this step
        if(keys == null || (keys != null && keys.Count == 0) ||
           values == null || (values != null && values.Count == 0))
        {
            Debug.LogWarning( "Main.cs LoadDataFromProcess() either keys or values are null, usind defaults set in editor instead of data from process" );
            CreateVisualLog();
            return;
        }

        //If our key/value pairs are not in sync, something went wrong, skip this step
        if(keys.Count != values.Count)
        {
            //In Unity Editor, we expect our count to be above zero but it can not match, no need to log a warning
#if !UNITY_EDITOR
            Debug.LogWarning( "Main.cs LoadDataFromProcess() keys & values don't have a matching Count, usind defaults set in editor instead of data from process" );
#endif
            CreateVisualLog();
            return;
        }

        for(int i = 0; i < keys.Count; ++i)
        {

            string key = keys[ i ];
            string value = values[ i ];

            Debug.Log( key + " : " + value );

            if(key == "logs")
            {
                logs = bool.Parse( value );
            }
            else if(key == "debug")
            {
                debug = bool.Parse( value );
            }
            else if(key == "length")
            {
                length = int.Parse( value );
            }
            else if(key == "address")
            {
                address = value;
            }
            else if(key == "port")
            {
                port = int.Parse( value );
            }

        }

        CreateVisualLog();

#endif

    } //END LoadDataFromProcess Method

    #endregion

    #region PRIVATE - CREATE VISUAL LOGGER

    /// <summary>
    /// Creates a visual debug log debug logs have been enabled and we are not in the Unity Editor
    /// </summary>
    //----------------------------------//
    private void CreateVisualLog()
    //----------------------------------//
    {
#if EXT_INGAMEDEBUGCONSOLE
#if UNITY_EDITOR
        if(DebugLogManager.Instance != null ) DebugLogManager.Instance.gameObject.SetActive( false );
#else
        if(DebugLogManager.Instance != null ) DebugLogManager.Instance.gameObject.SetActive( logs );
#endif
#endif

        CreateNeuroGuideManager();

    } //END CreateVisualLog Method

    #endregion

    #region PRIVATE - CREATE NEUROGUIDE MANAGER

    /// <summary>
    /// Creates the NeuroGuideManager
    /// </summary>
    //---------------------------------------------//
    private void CreateNeuroGuideManager()
    //---------------------------------------------//
    {

#if GAMBIT_NEUROGUIDE

        NeuroGuideManager.Create
        (
            //Options
            new NeuroGuideManager.Options()
            {
                showDebugLogs = logs,
                enableDebugData = debug,
                udpAddress = address,
                udpPort = port
            },

            //OnSuccess
            (NeuroGuideManager.NeuroGuideSystem system) =>
            {
                if (logs) Debug.Log("Main.cs CreateNeuroGuideManager() Successfully created NeuroGuideManager");
                CreateNeuroGuideExperience();
            },

            //OnError
            LogError,

            //OnDataUpdate
            (NeuroGuideData data) =>
            {
                //if( logs ) Debug.Log( "NeuroGuideDemo CreateNeuroGuideManager() Data Updated" );
            },

            //OnStateUpdate
            (NeuroGuideManager.State state) =>
            {
                if (logs) Debug.Log("Main.cs CreateNeuroGuideManager() State changed to " + state.ToString());
            });

#endif

    } //END CreateNeuroGuideManager Method

    #endregion

    #region PRIVATE - CREATE NEUROGUIDE EXPERIENCE

    /// <summary>
    /// Initializes a NeuroGuideExperience once the hardware is ready
    /// </summary>
    //---------------------------------------------//
    private void CreateNeuroGuideExperience()
    //---------------------------------------------//
    {

#if GAMBIT_NEUROGUIDE

        NeuroGuideExperience.Create
        (
            //Options
            new NeuroGuideExperience.Options()
            {
                showDebugLogs = logs,
                totalDurationInSeconds = length
            },

            //OnSuccess
            (NeuroGuideExperience.NeuroGuideExperienceSystem system) =>
            {
                if (logs) Debug.Log("Main.cs CreateNeuroGuideExperience() Successfully created NeuroGuideExperience");
            },

            //OnFailed
            LogError,

            //OnDataUpdate
            (float value) =>
            {
                if (logs) Debug.Log("Main.cs CreateNeuroGuideExperience() Data Updated = " + value);
            }

        );

#endif

    } //END CreateNeuroGuideExperience Method

    #endregion

    #region PRIVATE - LOG WARNING

    /// <summary>
    /// Logs warning if the writing to the console log has been enabled
    /// </summary>
    /// <param name="warning"></param>
    //------------------------------------------//
    private void LogWarning(string warning)
    //------------------------------------------//
    {
        if (logs)
            Debug.LogWarning(warning);

    } //END LogWarning Method

    #endregion

    #region PRIVATE - LOG ERROR

    /// <summary>
    /// Logs errors if the writing to the console log has been enabled
    /// </summary>
    /// <param name="error"></param>
    //------------------------------------------//
    private void LogError(string error)
    //------------------------------------------//
    {
        if (logs)
            Debug.LogError(error);

    } //END LogError Method

    #endregion

} //END Main Class