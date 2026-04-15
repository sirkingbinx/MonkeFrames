<h1 id="readme"><img src="/MonkeFrames.Editor/Resources/MFtitleWhite.png" height=200></h1>

MonkeFrames is a keyframe-based camera animator loosely based on the Orion Drift spectator view that allows you to plan out camera movements with transitions for each property.

Create a keyframe by pressing V. It's properties will show up on the MonkeFrames panel in the top right. You can tweak its transitions, position, and rotation, or replace the currently selected keyframe by pressing X.

## Installation
> [!NOTE]
> Upon release, MonkeFrames will be avaliable on [MonkeModManager](https://github.com/sirkingbinx/MonkeModManager/releases/latest)

   1. Download `MonkeFrames.zip` from the [releases](https://github.com/sirkingbinx/MonkeFrames/releases/latest) page
   2. Extract the zip file into `BepInEx/plugins/` (BepInEx) or `Mods` (MelonLoader)

## Usage
Press `V` to create a new keyframe. You can press `T` to create a new keyframe looking at the monke, `X` to replace the current keyframe with a new one, or click `Keyframe` > `Delete Keyframe` on the topbar.

Once a keyframe is created, you can see it's values with the Keyframe Editor. Press `View` > `Keyframe Editor` to view and select every keyframe in your project.

Once you are done with editing, you can compile your project (turn those keyframes into movement) with `Project` > `Compile`, then press `Project > Play`. Press Space to exit the player and return to MonkeFrames.

You can save your project with `Project > Save`, then reopen it by selecting `Project > Open` and choosing your project. All projects are saved in a special folder you can access by pressing `Win` + `R`, and then entering `%USERFOLDER%/AppData/LocalLow/Another Axiom/Gorilla Tag/MonkeFrames/projects`.

## For Developers
You can embed the keyframe functionality of MonkeFrames into your own projects. See [MonkeFrames.Compiler](/MonkeFrames.Compiler).

## Credits
- [sirkingbinx (bingus)](https://github.com/sirkingbinx): Developer, documentation, concept art
- [uhJames](https://www.youtube.com/@uhJamesvr): Design, logos, art, commissioner

## Extras
![Concept Art](/MonkeFrames.Editor/Resources/MonkeFrames_Concept.png)
