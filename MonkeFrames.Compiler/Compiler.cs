using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using Newtonsoft.Json;
using MonkeFrames.Compiler.Models;
using UnityEngine;

using Keyframe = MonkeFrames.Compiler.Models.Keyframe;
using System.Threading.Tasks;

namespace MonkeFrames.Compiler;

/// <summary>
/// Manages all MonkeFrames.Compiler-related actions.
/// </summary>
public static class Compiler
{
    /// <summary>
    /// Asyncronously build a project.
    /// </summary>
    /// <param name="project">The project to build.</param>
    /// <param name="onStatusUpdate">Called every time the project reaches a new target. The current step is sent as a string.</param>
    /// <returns></returns>
    public static async Task Build(Project project, Action<string> onStatusUpdate = null)
    {
        Stopwatch timer = new Stopwatch(); // Project build time
        timer.Start();

        Action<string> status = (text) =>
        {
            if (onStatusUpdate == null)
                return;

            try {
                onStatusUpdate($"Compiling project {project.Name}: {text}");
            } catch { }
        };
        
        List<Keyframe> compiledKeyframes = [];

        foreach (var (keyframe, i) in project.Keyframes.Select((value, i) => (value, i)))
        {
            int frames = (int)Math.Ceiling(keyframe.Transition.Duration * project.FPS);
            
            for (int j = 0; j < frames; j++) {
                status($"K{i} - Generating frame {compiledKeyframes.Count} ({project.FPS} FPS)");

                if (i == project.Keyframes.Count - 1) {
                    Keyframe strippedKeyframe = new Keyframe {
                        Position = keyframe.Position,
                        Rotation = keyframe.Rotation,
                        FieldOfView = keyframe.FieldOfView,
                        Compiled = true
                    };

                    compiledKeyframes.Add(strippedKeyframe);
                    continue;
                }

                Vector3 newPosition;
                Quaternion newRotation;
                float newFOV;

                Keyframe next = project.Keyframes[i + 1];

                switch (keyframe.Transition.Effect)
                {
                    case TransitionEffect.Sine:
                        newPosition = Transitions.Sine(keyframe.Position, next.Position, j, frames);
                        newRotation = Transitions.Sine(keyframe.QuatRotation, next.QuatRotation, j, frames);
                        newFOV = Transitions.Sine(keyframe.FieldOfView, next.FieldOfView, j, frames);
                        break;
                    case TransitionEffect.Cut:
                        newPosition = Transitions.Cut(keyframe.Position, next.Position, j, frames);
                        newRotation = Transitions.Cut(keyframe.QuatRotation, next.QuatRotation, j, frames);
                        newFOV = Transitions.Cut(keyframe.FieldOfView, next.FieldOfView, j, frames);
                        break;
                    default:
                    case TransitionEffect.Linear:
                        newPosition = Transitions.Linear(keyframe.Position, next.Position, j, frames);
                        newRotation = Transitions.Linear(keyframe.QuatRotation, next.QuatRotation, j, frames);
                        newFOV = Transitions.Linear(keyframe.FieldOfView, next.FieldOfView, j, frames);
                        break;
                }

                Keyframe compiledKeyframe = new Keyframe {
                    Position = newPosition,
                    Rotation = newRotation.eulerAngles,
                    FieldOfView = newFOV,
                    Compiled = true
                };

                compiledKeyframes.Add(compiledKeyframe);
            }
        }

        project.CompiledKeyframes = compiledKeyframes;
        project.IsCompiled = true;

        timer.Stop();

        status($"Compiled {project.Name} in {Math.Floor(timer.Elapsed.TotalMinutes)}m {timer.Elapsed.Seconds:D2}s");
    }

    /// <summary>
    /// Convert the project into savable JSON data.
    /// </summary>
    public static string ConvertToJSON(Project project)
    {
        var settings = new JsonSerializerSettings {
            Converters = new[] { new Vector3Converter() }
        };

        string json = JsonConvert.SerializeObject(project, settings);

        return json;
    }

    /// <summary>
    /// Loads a project from JSON data.
    /// </summary>
    /// <param name="json">The JSON to turn into a project.</param>
    /// <returns>The project embedded in the JSON.</returns>
    public static Project ConvertFromJSON(string json)
    {
        var settings = new JsonSerializerSettings {
            Converters = new[] { new Vector3Converter() }
        };

        Project project = JsonConvert.DeserializeObject<Project>(json, settings);

        return project;
    }

    /// <summary>
    /// Convert a project name into a properly formatted file name.
    /// </summary>
    /// <param name="projectName">The project name to convert.</param>
    /// <returns>A string formatted in UpperCamelCase and ends with the .frames extension.</returns>
    public static string ProjectNameToFilename(string projectName)
    {
        TextInfo textInfo = CultureInfo.InvariantCulture.TextInfo;
        return textInfo.ToTitleCase(projectName.ToLower()).Replace(" ", "") + ".frames";
    }
}