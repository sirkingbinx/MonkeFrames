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

        status($"Setting up");
        double projectDuration = 0f;

        foreach (Keyframe keyframe in project.Keyframes) {
            projectDuration += Math.Ceiling(keyframe.Transition.Duration);
        }
        
        int projectFrames = (int)Math.Ceiling(projectDuration * project.FPS);
        List<Keyframe> compiledKeyframes = [];

        int filledKeyframes = 0;

        for (int i = 0; i < project.Keyframes.Count; i++)
        {
            Keyframe keyframe = project.Keyframes.ElementAt(i);
            int framesToFill = (int)Math.Floor(keyframe.Transition.Duration * project.FPS);

            if (i == project.Keyframes.Count - 1)
            {
                foreach (int j in Enumerable.Range(filledKeyframes, filledKeyframes + framesToFill))
                {
                    Keyframe newKeyframe = new Keyframe
                    {
                        Position = keyframe.Position,
                        Rotation = keyframe.Rotation,
                        FieldOfView = keyframe.FieldOfView,
                        Compiled = true
                    };

                    compiledKeyframes[j] = newKeyframe;
                }
            }

            Keyframe nextKeyframe = project.Keyframes.ElementAt(i + 1);

            for (int j = filledKeyframes; j < framesToFill; j++)
            {
                status($"Generating frame {j}");

                Func<float, float, int, int, float> getNowAction;

                if (keyframe.Transition.Effect == TransitionEffect.Cut)
                    getNowAction = Transitions.Cut;
                else if (keyframe.Transition.Effect == TransitionEffect.Sine)
                    getNowAction = Transitions.Sine;
                else
                    getNowAction = Transitions.Linear;

                float posXStep = getNowAction(keyframe.Position.x, nextKeyframe.Position.x, j - filledKeyframes, framesToFill);
                float posYStep = getNowAction(keyframe.Position.y, nextKeyframe.Position.y, j - filledKeyframes, framesToFill);
                float posZStep = getNowAction(keyframe.Position.z, nextKeyframe.Position.z, j - filledKeyframes, framesToFill);

                float rotXStep = getNowAction(keyframe.Rotation.x, nextKeyframe.Rotation.x, j - filledKeyframes, framesToFill);
                float rotYStep = getNowAction(keyframe.Rotation.y, nextKeyframe.Rotation.y, j - filledKeyframes, framesToFill);
                float rotZStep = getNowAction(keyframe.Position.z, nextKeyframe.Rotation.z, j - filledKeyframes, framesToFill);

                float fovStep = getNowAction(keyframe.FieldOfView, nextKeyframe.FieldOfView, j - filledKeyframes, framesToFill);

                Keyframe newKeyframe = new Keyframe
                {
                    Position = new Vector3(posXStep, posYStep, posZStep),
                    Rotation = new Vector3(rotXStep, rotYStep, rotZStep),
                    FieldOfView = fovStep,
                    Compiled = true
                };

                compiledKeyframes[j] = newKeyframe;
            }

            filledKeyframes += framesToFill;
        }

        status("Saving changes");
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