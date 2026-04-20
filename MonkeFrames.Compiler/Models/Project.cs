using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MonkeFrames.Compiler.Models;

/// <summary>
/// Projects hold metadata for an animation.
/// </summary>
public class Project
{
    /// <summary>
    /// Display name of the project.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// The version of the MonkeFrames compiler that the project was exported with.
    /// </summary>
    public string Version { get; internal set; }

    /// <summary>
    /// The exporter that managed the generation of the project.
    /// </summary>
    public Exporter Exporter { get; internal set; }

    /// <summary>
    /// Keyframes associated with the project.
    /// </summary>
    public List<Keyframe> Keyframes { get; set; }

    /// <summary>
    /// Properties used by a certain exporter for the project.
    /// </summary>
    public Dictionary<string, object> CustomProperties { get; set; };

    /// <summary>
    /// FPS of the project. Can be either 30 or 60.
    /// </summary>
    public int FPS {
        get => field;
        set {
            if (value is 30 or 60 or 120)
                field = value;
            else
                throw new ArgumentException("FPS must be either 30, 60, or 120.", nameof(value));
        }
    }

    /// <summary>
    /// A list of built keyframes for the project for use with cameras.
    /// </summary>
    [JsonIgnore]
    public List<Keyframe> CompiledKeyframes;

    /// <summary>
    /// Represents if the project has been compiled.
    /// </summary>
    [JsonIgnore]
    public bool IsCompiled;

    /// <summary>
    /// Build the project. Shorthand for Compiler.Build(p)
    /// </summary>
    public async Task Build(Action<string> onStatusUpdate = null) => await Compiler.Build(this);

    /// <summary>
    /// Convert the project into savable JSON data. Shorthand for Compiler.ConvertToJSON(p)
    /// </summary>
    public string ToJson() => Compiler.ConvertToJSON(this);

    /// <summary>
    /// Loads a project from JSON data. Shorthand for Compiler.ConvertFromJSON(p)
    /// </summary>
    public static Project FromJson(string json) => Compiler.ConvertFromJSON(json);

    /// <summary>
    /// Create a new project.
    /// </summary>
    public Project(string projectName, Exporter projectExporter)
    {
        Name = projectName;
        Version = Constants.Version;
        Exporter = projectExporter;
        CustomProperties = new();
        FPS = 30;
        Keyframes = new List<Keyframe>();
    }
}