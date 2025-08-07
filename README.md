# unity-neuroguide-flow
Unity3D project that utilizes the NeuroGuide hardware to show a visualizer experience.

This application is intended to be launched as a Windows process from the [unity-neuroguide-launcher](https://github.com/GambitGamesLLC/unity-neuroguide-launcher) project, and has variables sent into it via a .json configuration system. You'll find more information about this system in this guide and in the [unity-neuroguide-launcher](https://github.com/GambitGamesLLC/unity-neuroguide-launcher) repository.

In this experience, a ship floats on a calm ocean with a beuatiful orange sky and nearby seagulls fly alongside you. As the user makes `progress` by focusing, the wind picks up and the birds flap their wings. When the `threshold` is reached the seagulls move near the top of the mast of the ship.

If the `progress` falls below the `threshold`, the `progress` value is forcibly set to a value, but in this experience we set this value to the same amount as the threshold, so its as if this feature is ignored. It's been left in place as it isn't hurting the experience.

<img width="412" height="235" alt="image" src="https://github.com/user-attachments/assets/80f6feb3-cb50-4ce5-81fa-3328f569f0e9" />

---

## PLAY INSTRUCTIONS

- Open `Scenes/Main`
- Press Play in the editor
- Use your keyboard up and down keys to fake the UDP data being sent from the NeuroGuide software

---  

## BUILD INSTRUCTIONS

- No special build instructions, simply make a Windows desktop build

---  

## ARTIST NOTES

- The cubes are parented to the `ZeroRotationParent` gameObject that zeroes out the world translation and rotation.
- The `SM_Hypercube` animation plays on awake. It plays at different speeds set in a list according to the experience length set in `NeuroGuide_Main`.
- The `Pieces_rotator` game object spins the `SM_Pieces_Hypercube_Animated` gameObject. 

---

## NEUROGUIDE TERMINOLOGY

- `focus` : The NeuroGuide software is looking for certain responses from the sub-concious of the user. This is referred to as the `focus`.
- `reward` (boolean) : Is the user in the NeuroGuide hardware successfully in a state of `focus`?
- `length` (float) : How long the experience should take to complete if the user in the NeuroGuide hardware was in a `reward` state consistently.
- `score` (float) : How much time the user has spent in the `reward` state. Every time the hardware updates, we update this score by adding or subtracting the DeltaTime between updates to this value.
- `progress` (float) : 0-1 normalized progress value. Calculated by mapping the `score` and `length` to a 0-1 range. EX: 0.5 is 50% through the experience.
- `threshold` (float) : 0-1 normalized value. When the `progress` goes above or below this value, we send a Callback. This is used to enter and leave a reward state in this experience.

---  

## NEUROGUIDE SCORE, PROGRESS & LENGTH IN MORE DETAIL

This NeuroGuide `Energy` app listens for updates from the `NeuroGuideAnimationExperience` script within the `NeuroGuideManager` package and changes the visuals accordingly.

- When the user is focused in the NeuroGuide, this increases the score value
- When the user loses focus, this decreases the score value
- This app has a 'length' value, which the `NeuroGuideAnimationExperience` uses to determine how long the user must be focused to reach the end of the experience.
- Using the `score` and `length` values, our system calculates a normalized 0-1 `progress` value, which is used to update our animations and trigger events
- The `INeuroGuideAnimationExperienceInteractable` interface exposes the callbacks regarding the experience to our various scripts throughout the app.

---  

## NEUROGUIDE ANIMATION EXPERIENCE - THRESHOLD IN DETAIL

- When the current progress goes above or below a normalized 0-1 `threshold` value, an event is called in our `INeuroGuideAnimationExperienceInteractable` interface.
- In the `Energy` app, this threshold is reached after forming the cube and spinning the cube face three times, this is around 90% through the experience.
- This threshold callback system is used to change the state of the experience (in this case, the energy cube enters its final glowing state)  

- In the `Energy` app, we have a script in our Main scene, called `ChangeAnimationWhenScoreFallsBelowThreshold`. This script listens for when our progress falls below the threshold.
- If the progress falls below the threshold, we then forcibly set the progress to a preset normalized 0-1 value, which puts the user at an earlier state of progress within the experience.
- For the `Energy` experience, this forces the user to spin the cube face two more times before it passes the threshold again and enters the glowing energy state.

---

## NEUROGUIDE ANIMATION EXPERIENCE - INeuroGuideAnimationExperienceInteractable

This interface exposes the functionality needed to make this experience.

You'll find this interface added to many of the scripts used to control the experience, as its the primary point of exposure to the NeuroGuide systems for scripting.

```
/// <summary>
/// Called when the user gets their score above the threshold value in the experience.
/// When this happens, this callback will be prevented until the user falls back below the threshold
/// and a set amount of time has passed, configurable in the NeuroGuideAnimationExperience Options object
/// </summary>
void OnAboveThreshold();

/// <summary>
/// Called when the user gets their score above the threshold value in the experience, then
/// the score falls below the threshold
/// </summary>
void OnBelowThreshold();

/// <summary>
/// Called when the user starts or stops recieving a reward from the NeuroGuide hardware
/// </summary>
/// <param name="isRecievingReward">Is the user currently recieving a reward?</param>
void OnRecievingRewardChanged( bool isRecievingReward );

/// <summary>
/// Called 60 times a second by the NeuroGuideAnimationExperience with the latest normalized value of how far the user is from reaching the end goal of the experience
/// </summary>
/// <param name="system">The current normalized value (0-1) of how far we are in the NeuroGuide animation experience</param>
void OnDataUpdate( float normalizedValue );
```

---  

## PROCESS COMMAND LINE VALUE INSTRUCTIONS

NeuroGuide experiences like `Energy` can have their settings variables passed in by the NeuroGuide Launcher process.

- When this project is launched normally, without being started from the NeuroGuide launcher, it will use variables setup within the Unity editor.
- These default variables are located on the `Main` component, present in the only scene used by this app.
- The `ProcessManager` package will pass in variables that will replace these defaults from the NeuroGuide Launcher, which uses the `ConfigManager` json system
- These variables are passed into this project using Windows process command line arguments, but by using the `ProcessManager` we can easily send and recieve these values
- The `NeuroGuide Launcher` application itself uses the `ConfigManager` to allow the variables to be set dynamically

## CONFIGURATION FILE INSTRUCTIONS

You can find the appropriate `configuration json` file within the Resources folder of the [unity-neuroguide-launcher](https://github.com/GambitGamesLLC/unity-neuroguide-launcher). 
This configuration file only exists as part of that repository and is not stored in this one.

**If this app is run via the NeuroGuide launcher, it will use the data passed to it by the Launcher, which comes from a configuration .json file**

- A `configuration json` file is stored in our Resources folder of the NeuroGuide Launcher project, and can be updated to modify the application  
- This `configuration json` file is copied to our `%LOCALAPPDATA%` folder, specifically in the path specified in the `config:path` object  
- If there already exists a `configuration json` at the specified path, we will compare it against the one in the Resources folder. If the local file is out of date or missing, it will be written using the version in Resources.
- It is recommended to have the configuration file that's copied to your `%LOCALAPPDATA%` folder stay at a higher version number than the file inside of the [unity-neuroguide-launcher](https://github.com/GambitGamesLLC/unity-neuroguide-launcher) Unity Project, that way any changes to the configuration will be used when you restart this experience and the launcher to test the new values.
- When you have found values you like, its recommended to make those the new defaults within the configuration files in the `Resources` folder of the [unity-neuroguide-launcher](https://github.com/GambitGamesLLC/unity-neuroguide-launcher) project. That way your next build will include them.

- Locate and open the configuration json file within the resources folder, which has contents similar to this
```json
{
	"config": {
		"version": 2,
		"timestamp": "2025-07-28 12:00:00",
		"path": "%LOCALAPPDATA%\\M3DVR\\Launcher\\Energy.json"
	},
	"app": {
		"name": "Energy",
		"path": "%LOCALAPPDATA%\\M3DVR\\Energy\\Energy.exe",
		"length": 3,
		"debug": true,
		"logs": false,
		"threshold": 0.9
	}
}
```
  
<b>`config` OBJECT  </b>
- `version` - Defines the version number of the configuration file, used to see if this is newer than a config file we're comparing against.  
- `timestamp` - If the version of both config files matches, we check this timestamp to see if one is newer.  
- `path` - The path to the config file on local storage. This path has its environment variables expanded and is deserialized, so it can be used for normal Path operations in Unity.  
  
<b>`app` OBJECT  </b>
- `name` - Used by external software like the M3DVR Neuroguide launcher app to show the app name in a human readable format  
- `path` - The path to the executable for this project. Like other stored Path variables, this will have any environment variables expanded and will be deserialized.  
- `length` - How long should this experience last (in seconds) if the user was in a "reward" state the entire time?
- `debug` - Do we want to enable debug mode for this app? This will fake incoming UDP port traffice as if the NeuroGuide Software was sending us messages
- `logs` - Do we want Unity console logs to be shown in our visual console for debugging?  
- `threshold` - Normalized 0-1 value representing how far into the experience you need to be before triggering the threshold state of the app. EX: For 0.9, that would be 90% into the experience.

---  

## DEPENDENCIES

Relies on several `Unity Asset Store` plugins as well as Open Source `Gambit Games` packages  

Please make sure the proper `scripting define symbols` and packages are imported into your project.  
When opening this project for the first time, the package manager should grab the appropriate versions of these packages for you.

Check the package repos directly for their `scripting define symbols`, `namespaces` and guides.  

- `DoTween` [Gambit Repo](https://github.com/GambitGamesLLC/unity-plugin-dotween) | [Unity Asset Store Link](https://assetstore.unity.com/packages/tools/animation/dotween-hotween-v2-27676)  
- Used to perform tweens

- `In-game Debug Console` [Unity Asset Store Link](https://assetstore.unity.com/packages/tools/gui/in-game-debug-console-68068)  
- Used to display an in-game console for debugging purposes when the 'logs' variable is enabled in the Main component or passed in via the Process data system from the Launcher

- `Skybox Series Free` [Unity Asset Store Link](https://assetstore.unity.com/packages/2d/textures-materials/sky/skybox-series-free-103633)  
- Used one of their assets for the skybox, should be removed outside of just what's needed for this project

- `SpaceSkies Free` [Unity Asset Store Link](https://assetstore.unity.com/packages/2d/textures-materials/sky/spaceskies-free-80503)  
- Used one of their assets for the skybox, should be removed outside of just what's needed for this project

- `Configuration Manager` [Gambit Repo](https://github.com/GambitGamesLLC/unity-config-manager.git?path=Assets/Plugins/Package)  
- Used for manipulation, saving, and loading of `.json` config files  

- `NeuroGuide Manager` [Gambit Repo](https://github.com/GambitGamesLLC/unity-neuroguide-manager.git)  
- Reads data from the NeuroGuide Software via UDP ports  
  
- `Process Manager` [Gambit Repo](https://github.com/GambitGamesLLC/unity-process-manager)  
- Allows us to read process command line variables passed in from the NeuroGuide launcher
  
- `Math Helper` [Gambit Repo](https://github.com/GambitGamesLLC/unity-math-helper)  
- Contains convenience functions for math functionality, such as Map(), which converts one value in a range to another  
  
- `Singleton Manager` [Gambit Repo](https://github.com/GambitGamesLLC/unity-singleton)
- Convenience function to easily create global singletons that retain Unity Lifecycle functionality such as a GameObject Instance
  
- `TotalJSON` [Gambit Repo](https://github.com/GambitGamesLLC/unity-plugin-totaljson) | [Unity Asset Store Link](https://assetstore.unity.com/packages/tools/input-management/total-json-130344)  
- Used for JSON manipulation  
  
