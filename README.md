<h1 id="readme"><img src="/MonkeFrames.Core/Resources/MFtitleWhite.png" height=200></h1>

MonkeFrames is a keyframe-based camera animator loosely based on the Orion Drift spectator view that allows you to plan out camera movements with transitions for each property.

Create a keyframe by pressing V. It's properties will show up on the MonkeFrames panel in the top right. You can tweak its transitions, position, and rotation, or replace the currently selected keyframe by pressing X.

## Installation
> [!NOTE]
> Upon release, MonkeFrames will be avaliable on [MonkeModManager](https://github.com/sirkingbinx/MonkeModManager/releases/latest)

   1. Download `MonkeFrames.zip` from the [releases](https://github.com/sirkingbinx/MonkeFrames/releases/latest) page
   2. Extract the zip file into `BepInEx/plugins/` (BepInEx) or `Mods` (MelonLoader)

## Usage
### Keyframe
| Keybind       | Needs Selection | Action                                                                                      |
| ------------- | --------------- | ------------------------------------------------------------------------------------------- |
| `V`           | ❌             | Creates a keyframe at the current location.                                                 |
| `X`           | ✔️             | Replaces the selected keyframe with a new keyframe at the current location.                 |
| `F`           | ✔️             | Teleports to the selected keyframe.                                                         |
| `T`           | ❌              | Create keyframe looking at the monke                                                 |

## For Developers
You can embed the keyframe functionality of MonkeFrames into your own projects. See [MonkeFrames.Compiler](/MonkeFrames.Compiler).

## Credits
- [sirkingbinx (bingus)](https://github.com/sirkingbinx): Developer, documentation, concept art
- [uhJames](https://www.youtube.com/@uhJamesvr): Design, logos, art, commissioner

## Extras
![Concept Art](/MonkeFrames.Core/Resources/MonkeFrames_Concept.png)
