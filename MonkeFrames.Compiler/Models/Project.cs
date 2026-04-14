using System;
using System.Collections.Generic;

namespace MonkeFrames.Compiler.Models;

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
    /// FPS of the project. Can be either 30 or 60.
    /// </summary>
    public int FPS {
        get => field;
        set {
            if (value is 30 or 60)
                field = value;
            else
                throw new ArgumentException("FPS must be either 30 or 60.", nameof(value));
        }
    }

    /// <summary>
    /// Create a new project.
    /// </summary>
    public Project(string projectName, Exporter projectExporter = Exporter.Default)
    {
        Name = projectName;
        Version = Constants.Version;
        Exporter = projectExporter;
    }
}