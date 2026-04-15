# MonkeFrames.Compiler
MonkeFrames.Compiler is the backbone to MonkeFrames, responsible for the keyframe class, calculating keyframe transitions, and obviously, compiling the keyframes into a big long list for playback.

MonkeFrames.Compiler can be embedded into any program for free, which makes it a pretty good backbone for other camera mods looking to add keyframe-based camera movement without a large amount of hassle.

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
keyframe.Transition.Effect = TransitionEffect.Sine; // Transition effects
keyframe.Transition.Duration = 6f; // Time it takes (in seconds) to go through the transition

project.Keyframes.Add(keyframe); // Add keyframe to the project
```

Compile and play your keyframes:
```cs
using MonkeFrames.Compiler;

Camera camera = GorillaTagger.instance.thirdPersonCamera.GetComponent<Camera>();
List<Keyframe> keyframes = project.Build(); // Puts all of the keyframes into a big list for playback
```

Load/save projects:
```cs
string projectJson = project.ToJson(); // Compiler.ConvertToJSON(project);

// do whatever yaba yaba

Project savedProject = Project.FromJson(projectJson); // Compiler.ConvertFromJSON(projectJson);
```