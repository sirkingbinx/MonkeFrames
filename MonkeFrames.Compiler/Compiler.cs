using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using MonkeFrames.Compiler.Models;
using MonkeFrames.Compiler.Converters;
using UnityEngine;

using Keyframe = MonkeFrames.Compiler.Models.Keyframe;

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

        status($"Setting up")
        float projectDuration = 0f;

        foreach (Keyframe keyframe in project.Keyframes) {
            projectDuration += Math.Ceiling(keyframe.Transition.Duration);
        }
        
        int projectFrames = Math.Ceiling(projectDuration * project.FPS);
        Keyframe[] compiledKeyframes = new Keyframe[projectFrames];

        int filledKeyframes = 0;

        foreach (Keyframe keyframe in project.Keyframes)
        {
            if (project.Keyframes.Count = (project.Keyframes.IndexOf(keyframe) + 1)) {
                compiledKeyframes[filledKeyframes] = new Keyframe {
                    Position = keyframe.Position,
                    Rotation = keyframe.Rotation,
                    FieldOfView = keyframe.FieldOfView,
                    Compiled = true
                };
            }

            Keyframe nextKeyframe = project.Keyframes[(project.Keyframes.IndexOf(keyframe) + 1)];
            int framesToFill = Math.Floor(keyframe.Duration * project.FPS);

            for (int i = filledKeyframes; i < framesToFill; i++)
            {
                status($"Generating frame {i}")

                Func<float, float, int, int> getNowAction;

                if (keyframe.Transition.Effect == TransitionEffect.Cut)
                    getNowAction = Transitions.Cut;
                else if (keyframe.Transition.Effect == TransitionEffect.Sine)
                    getNowAction = Transitions.Sine;
                else
                    getNowAction = Transitions.Linear;

                float posXStep = getNowAction(keyframe.Position.x, nextKeyframe.Position.x, i - filledKeyframes, framesToFill);
                float posYStep = getNowAction(keyframe.Position.y, nextKeyframe.Position.y, i - filledKeyframes, framesToFill);
                float posZStep = getNowAction(keyframe.Position.z, nextKeyframe.Position.z, i - filledKeyframes, framesToFill);
                
                float rotXStep = getNowAction(keyframe.Rotation.x, nextKeyframe.Rotation.x, i - filledKeyframes, framesToFill);
                float rotYStep = getNowAction(keyframe.Rotation.y, nextKeyframe.Rotation.y, i - filledKeyframes, framesToFill);
                float rotZStep = getNowAction(keyframe.Position.z, nextKeyframe.Rotation.z, i - filledKeyframes, framesToFill);
            
                float fovStep = getNowAction(keyframe.FieldOfView, nextKeyframe.FieldOfView, i - filledKeyframes, framesToFill);
            
                Keyframe newKeyframe = new Keyframe {
                    Position = new Vector3(posXStep, posYStep, posZStep),
                    Rotation = Quaternion.Euler(rotXStep, rotYStep, rotZStep),
                    FieldOfView = fovStep,
                    Compiled = true
                };

                compiledKeyframes[i] = newKeyframe;
            }

            filledKeyframes += framesToFill;
        }

        status("Saving changes")
        project.CompiledKeyframes = new List<Keyframe>(compiledKeyframes);

        timer.Stop();

        status($"Compiled project {project.Name} in {ts.TotalMinutes}m {ts.Seconds:D2}s")
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
        return textInfo.ToTitleCase(input.ToLower()).Replace(" ", "") + ".frames";
    }
}