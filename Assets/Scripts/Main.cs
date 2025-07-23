#region IMPORTS

using UnityEngine;
using System;

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

#endregion

/// <summary>
/// Primary entry point of the project
/// </summary>
public class Main : MonoBehaviour
{

    #region PUBLIC - VARIABLES

    [Tooltip("Set this to the config file path and name in the resources folder, do not include the file extension")]
    public string pathAndFilenameToConfigInResources = "config";

    /// <summary>
    /// Should we enable the debug logs?
    /// </summary>
    public bool logs = true;

    /// <summary>
    /// Should we enable the debug system for the NeuroGear hardware? This will enable keyboard events to control simulated NeuroGear hardware data spawned during the Create() method of NeuroGuideManager.cs
    /// </summary>
    [NonSerialized]
    public bool debug = true;

    /// <summary>
    /// How long should this experience last if the user was in a reward state continuously?
    /// </summary>
    [NonSerialized]
    public float experienceLengthInSeconds = 5f;

    /// <summary>
    /// Path to store the configuration file for this neuroguide experience. Can contain environment variables, and can contain escaped character sequences like \\ or \n
    /// </summary>
    [NonSerialized]
    public string configPath = "%LOCALAPPDATA%\\M3DVR\\BuildingBlocks\\config.json";

    /// <summary>
    /// UDP port address to listen to for NeuroGuide communication
    /// </summary>
    [NonSerialized]
    public string address = "127.0.0.1";

    /// <summary>
    /// UDP port to listen to for NeuroGuide communication
    /// </summary>
    [NonSerialized]
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

        LoadDataFromConfig();

    } //END Start Method

    #endregion

    #region PRIVATE - LOAD DATA FROM CONFIG - UPDATE CONFIG IF NEEDED

    /// <summary>
    /// Loads data from the local config, but first we check if our local is out of date and update it from resources
    /// </summary>
    //-------------------------------------//
    private void LoadDataFromConfig()
    //-------------------------------------//
    {

#if GAMBIT_CONFIG && EXT_TOTALJSON

        ConfigManager.UpdateLocalDataAndReturn
        (
            pathAndFilenameToConfigInResources,
            logs,
            (ConfigManager.ConfigManagerSystem system) =>
            {
                configSystem = system;
                SetVariablesFromConfig();
            },
            LogError
        );

#endif

    } //END LoadDataFromConfig Method

    #endregion

    #region PRIVATE - SET VARIABLES FROM CONFIG

#if GAMBIT_CONFIG && EXT_TOTALJSON

    /// <summary>
    /// Pull variables from the config file to use as our experience variables
    /// </summary>
    /// <param name="json"></param>
    //---------------------------------------------------//
    private void SetVariablesFromConfig()
    //---------------------------------------------------//
    {
        //How many variables do we need to wait for to load?
        int isReady = 0;
        int waitForCount = 5;


        //Set the 'address' variable, should be within the 'communication' object and the 'address' key
        ConfigManager.GetNestedString
        (
            configSystem,
            new string[] { "communication", "address" },
            (string value) =>
            {
                address = value;
                isReady++;
                if (isReady == waitForCount)
                {
                    CreateNeuroGuideManager();
                }
            },
            LogError
        );

        //Set the 'port' variable, should be within the 'communication' object and the 'port' key
        ConfigManager.GetNestedInteger
        (
            configSystem,
            new string[] { "communication", "port" },
            (int value) =>
            {
                port = value;
                isReady++;
                if (isReady == waitForCount)
                {
                    CreateNeuroGuideManager();
                }
            },
            LogError
        );

        //Set the 'logs' variable, should be within the 'experience' object and 'logs' key
        ConfigManager.GetNestedBool
        (
            configSystem,
            new string[] { "experience", "logs" },
            (bool value) =>
            {
                logs = value;
                isReady++;
                if (isReady == waitForCount)
                {
                    CreateNeuroGuideManager();
                }
            },
            (string error) =>
            {
                LogWarning(error);
                isReady++;
                if (isReady == waitForCount)
                {
                    CreateNeuroGuideManager();
                }
            }
        );

        //Set the 'debug' variable, should be within the 'experience' object and 'debug' key
        ConfigManager.GetNestedBool
        (
            configSystem,
            new string[] { "experience", "debug" },
            (bool value) =>
            {
                debug = value;
                isReady++;
                if (isReady == waitForCount)
                {
                    CreateNeuroGuideManager();
                }
            },
            (string error) =>
            {
                LogWarning(error);
                isReady++;
                if (isReady == waitForCount)
                {
                    CreateNeuroGuideManager();
                }
            }
        );

        //Set the 'length' variable, should be within the 'experience' object and the 'length' key
        ConfigManager.GetNestedFloat
        (
            configSystem,
            new string[] { "experience", "length" },
            (float value) =>
            {
                experienceLengthInSeconds = value;
                isReady++;
                if (isReady == waitForCount)
                {
                    CreateNeuroGuideManager();
                }
            },
            (string error) =>
            {
                LogWarning(error);
                isReady++;
                if (isReady == waitForCount)
                {
                    CreateNeuroGuideManager();
                }
            }
        );

    } //END SetVariablesFromConfig Method

#endif

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
                totalDurationInSeconds = experienceLengthInSeconds
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