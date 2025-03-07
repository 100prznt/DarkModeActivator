# DarkModeActivator
 Enable the __Darkmode__ for __Autodesk Fusion__

## How to use
Download the [current release](https://github.com/100prznt/DarkModeActivator/releases/latest) and copy all files from the zip archive in a directory without access restrictions (e.g. `C:/Program Files/DarkModeActivator`). After the DarkModeActivator.exe has been executed, the dark mode can be selected in the Autodesk Fusion preferences.
Because the settings are reset every time Fusion is updated. You can create a scheduled task that executes the executable each system start. Use the `autoclose` argument, to close the commandline after execution. The DarkModeActivator always checks whether the relevant settings are set; if this is the case, the configuration is not changed.

[![Build status](https://ci.appveyor.com/api/projects/status/796cmu93otgnva0r?svg=true)](https://ci.appveyor.com/project/100prznt/darkmodeactivator)


## Darkmode setup in Fusion
![darkmode.gif](docu/darkmode.gif)

The background of the drawing area can be set individually (to a dark color). The options can be found via the icons at the bottom center of the drawing area.


## Details
This section only describes what DMA (DarkModeActivator) does. It is __not__ necessary to adapt any xml files yourself.

The tool searches for a configuration file of the Neutron UI Toolkit and activates the option to use the dark mode feature. The file is located in the current deploy directory (`%localappdata%\Autodesk\webdeploy\production\<GUID>`) under the path `Neutron\UI\Base\Resources\UIToolkit\theme-featurepacks.xml`.

#### theme-featurepacks.xml
```xml
<?xml version="1.0" encoding="utf-8"?>
<FeaturePacks>
  <FeaturePack Id="ui-themes" Preview="True" FeatureFlag="common-ui-themes" UseDefaultsIfDisabled="True">
    <Title _LCLZId="ui-themes-fp-title" _LCLZText="UI Themes" />
    <Description _LCLZId="ui-themes-fp-desc" _LCLZText="This section controls Fusion UI theme features." />
    <Features>
      <Feature Id="ui-themes-enable" Default="False" RequireRestart="False">
        <Title _LCLZId="ui-themes-enable-fp-title" _LCLZText="Enable UI Themes" />
        <Description _LCLZId="ui-themes-enable-fp-desc" _LCLZText="Allows changing the UI theme in Preferences." />
      </Feature>
    </Features>
  </FeaturePack>
</FeaturePacks>
```

To select the darkmode in the user preferences in Fusion, the `Default` attribute of the `Feature`-node must be set to `True`.
