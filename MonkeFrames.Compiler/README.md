# MonkeFrames.Compiler

![Diagram explaining how the Compiler works](.github/Compiler.svg)

MonkeFrames.Compiler is the backbone to MonkeFrames, responsible for the keyframe class, calculating keyframe transitions, and obviously, compiling the keyframes into a big long list for playback.

MonkeFrames.Compiler can be embedded into any program for free, which makes it a pretty good backbone for other camera mods looking to add keyframe-based camera movement without a large amount of hassle.

## Installation
You can pick up a copy of the compiler library (`MonkeFrames.Compile.dll`) from the [releases](https://github.com/sirkingbinx/MonkeFrames/releases/latest) page.

To start using it with your project, go to the Solution Explorer, right click on `References`, click Add Reference, then move to the `Browse` tab, click `Add`, select `MonkeFrames.Compiler.dll`, then press Ok.

## Usage
Create a new project for adding keyframes:
```cs
using MonkeFrames.Compiler.Models;

// The "Exporter" field tells other programs where the project was originally exported from.
Exporter myModDetails = new Exporter("myname.mymodname", "Mod display name");

// Projects hold keyframes and metadata. They can be saved and loaded by any mod that uses
// MonkeFrames.Compiler
Project project = new Project("my project", myModDetails);
```

Add keyframes:
```cs
// Assuming you have your project set up already:

Keyframe keyframe = new Keyframe();

// Add your values
keyframe.Position = new Vector3(px, py, pz);
keyframe.Rotation = new Vector3(rx, ry, rz);
keyframe.FieldOfView = 70;

// Change transitions
keyframe.Transition.Effect = TransitionEffect.Linear; // Transition effects
keyframe.Transition.Duration = 6f; // Time it takes (in seconds) to go through the transition

project.Keyframes.Add(keyframe); // Add keyframe to the project
```

Compile your keyframes:
```cs
using MonkeFrames.Compiler;

List<Keyframe> keyframes = project.Build();
```

Load/save projects:
```cs
string projectJson = project.ToJson();

// do whatever yaba yaba

Project savedProject = Project.FromJson(projectJson);
```

For more information about using MonkeFrames.Compiler, the [Discord](https://discord.gg/rdqRNQ9nXT)'s a good place to start.
