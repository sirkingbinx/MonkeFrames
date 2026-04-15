using MonkeFrames.Compiler;
using MonkeFrames.Compiler.Models;
using MonkeFrames.Editor.Components;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace MonkeFrames.Editor.Utilities;

public static class SaveUtilities
{
    public static Dictionary<string, Project> LoadableProjects
    {
        get {
            if (field == null)
                field = GetProjects();

            return field;
        }
        
        set => field = value;
    }

    private static bool IsValidJson(string json) {
        try
        {
            JToken.Parse(json);
            return true;
        }
        catch (JsonReaderException)
        {
            return false;
        }
    }

    public static void Save()
    {
        Project project = KeyframeManager.Instance.Project;
        string projectJson = project.ToJson();

        string projectDir = Path.Combine(Application.persistentDataPath, "MonkeFrames", "projects");
        string projectPath = Path.Combine(projectDir, Compiler.Compiler.ProjectNameToFilename(project.Name));

        Directory.CreateDirectory(projectDir);

        File.WriteAllText(projectPath, projectJson);
    }

    public static Dictionary<string, Project> GetProjects()
    {
        string projectDir = Path.Combine(Application.persistentDataPath, "MonkeFrames", "projects");
        string[] projectFiles = Directory.GetFiles(projectDir, "*.frames");
        Dictionary<string, Project> projects = new();

        foreach (string filename in projectFiles) {
            string projectJson = File.ReadAllText(filename);
            
            if (!IsValidJson(projectJson))
                continue;
            
            try {
                Project p = Project.FromJson(projectJson);
                projects.Add(p.Name, p);
            } catch { }
        }

        return projects;
    }
}