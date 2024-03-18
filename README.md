# Stellar Wolf - Settings

Stellar Wolf Settings is a Unity package designed to simplify the creation and management of settings within Unity projects.

## Installation

To install the Stellar Wolf Settings package in your unity project, follow these steps:

1. Open your Unity project.
2. In the Unity Editor, go to `Window` > `Package Manager`.
3. Click on the `+` button in the top-left corner of the Package Manager window.
4. Select `Install package from git URL`.
5. Paste `https://github.com/StellarWolfEntertainment/StellarWolf-Settings.git` into the text field.
6. Click `Install`

## Usage

Stellar Wolf Settings provides a convenient way to create and manage settings for your Unity project. The following example showcases how the system is meant to work:

```csharp
using StellarWolf;
using UnityEngine;

[Settings(SettingsUsage.Runtime, "AudioSettings")]
public class AudioSettings : Settings<AudioSettings>
{
    [SerializeField] private bool m_EnableSound;
    [SerializeField] private float m_MasterVolume;
    
    public static bool enableSound => instance.m_EnableSound;
    public static float masterVolume => instance.m_MasterVolume;
}
```

## Disclaimer

This package is heavily based on the package by [Hextant](https://github.com/hextantstudios/com.hextantstudios.utilities) that serves a similar purpose. We express our gratitude to the creators of the original package for their contributions to the Unity community.

## License

This package is licensed under the Creative Commons Attribution 4.0 International License. See the [LICENSE](https://creativecommons.org/licenses/by/4.0/) for details.

## Author

Created by Raistlin Wolfe.
Contact: stellarwolfentertainment@gmail.com