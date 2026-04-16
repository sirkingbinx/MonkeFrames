using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using MonkeFrames.Compiler.Models;
using MonkeFrames.Compiler.Converters;
using UnityEngine;

using Keyframe = MonkeFrames.Compiler.Models.Keyframe;
using System.Diagnostics;
using System.Globalization;
using System.Linq;

namespace MonkeFrames.Compiler;

public static class Compiler
{
    public static List<Keyframe> Build(Project project, Action<string> onStatusUpdate = null)
    {
        Stopwatch timer = Stopwatch.StartNew(); // Project build time

        Action<string> status = (text) =>
        {
            if (onStatusUpdate == null)
                return;

            try {
                onStatusUpdate($"Compiling project {project.Name}: {text}");
            } catch { }
        };
        
        List<Keyframe> compiledKeyframes = [];

        foreach (var (i, keyframe) in project.Keyframes.Index())
        {
            status($"K{i}");

            int frames = (int)Math.Ceiling(keyframe.Transition.Duration * FPS);
            
            for (int j = 0; j < frames; j++) {
                status($"K{i} - Generating frame {compiledKeyframes.Count} ({FPS} FPS)");

                if (i == project.Keyframes.Count - 1) {
                    Keyframe compiledKeyframe = new Keyframe {
                        Position = keyframe.Position,
                        Rotation = keyframe.Rotation,
                        FieldOfView = keyframe.FieldOfView,
                        Compiled = true
                    };

                    compiledKeyframes.Add(compiledKeyframe);
                    continue;
                }

                var transition = compiledKeyframe.Transition.Effect switch {
                    TransitionEffect.Linear => Transitions.Linear,
                    TransitionEffect.Sine => Transitions.Sine,
                    TransitionEffect.Cut => Transitions.Cut,
                    _  => Transitions.Linear
                };

                Keyframe next = project.Keyframes[i + 1];

                float posX = transition(keyframe.Position.x, next.Position.x, j, frames);
                float posY = transition(keyframe.Position.y, next.Position.y, j, frames);
                float posZ = transition(keyframe.Position.z, next.Position.z, j, frames);

                float rotX = transition(keyframe.Rotation.x, next.Rotation.x, j, frames);
                float rotX = transition(keyframe.Rotation.y, next.Rotation.y, j, frames);
                float rotX = transition(keyframe.Rotation.z, next.Rotation.z, j, frames);

                float fov = transition(keyframe.FieldOfView, next.FieldOfView, j, frames);

                Keyframe compiledKeyframe = new Keyframe {
                    Position = new Vector3(posX, posY, posZ),
                    Rotation = new Vector3(rotX, rotY, rotZ),
                    FieldOfView = fov,
                    Compiled = true
                };

                compiledKeyframes.Add(compiledKeyframe);
            }
        }

        project.CompiledKeyframes = compiledKeyframes;

        timer.Stop();

        status($"Compiled project {project.Name} in {timer.Elapsed.TotalMinutes}m {timer.Elapsed.Seconds:D2}s");
        return project.CompiledKeyframes;
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
    public static Project ConvertFromJSON(string json)
    {
        var settings = new JsonSerializerSettings {
            Converters = new[] { new Vector3Converter() }
        };

        Project project = JsonConvert.DeserializeObject<Project>(json, settings);

        return project;
    }

    public static string ProjectNameToFilename(string projectName)
    {
        TextInfo textInfo = CultureInfo.InvariantCulture.TextInfo;
        return textInfo.ToTitleCase(projectName.ToLower()).Replace(" ", "") + ".frames";
    }
}