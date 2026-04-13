using System;
using System.Globalization;
using System.IO;
using Newtonsoft.Json;
using MonkeFrames.Compiler.Models;
using UnityEngine;
using System.Collections.Generic;

namespace MonkeFrames.Compiler.IO;

public static class ProjectManager
{
    public const string ProjectExtension = "mframes";
    
    public static void AssureProjectPathExists() {
        string path = ProjectNameToPath();

        if (!Directory.Exists(path)) {
            Directory.CreateDirectory(path);
        }
    }

    public static string ProjectNameToPath(string projectName = "LEAVEBLANKFORPROJECTFOLDER")
    {
        string projectFolder = Path.Combine(Application.persistentDataPath, "MonkeFrames", "projects");
        
        if (projectName == "LEAVEBLANKFORPROJECTFOLDER")
            return projectFolder;
        else
            return Path.Combine(projectFolder, $"{projectName}.{ProjectExtension}");
    }

    public static string FixProjectName(string projectName) {
        TextInfo textInfo = CultureInfo.InvariantCulture.TextInfo;
        string titleCase = textInfo.ToTitleCase(projectName.ToLower().Trim());
        return titleCase.Replace(" ", "");
    }

    public static void SaveProject(Project project) {
        string projectName = FixProjectName(project.ProjectName);
        string projectData = JsonConvert.SerializeObject(project, Formatting.Indented);
        string projectPath = ProjectNameToPath(projectName);
        File.WriteAllText(projectPath, projectData);
    }

    public static (Project, List<string>) LoadProject(string projectName) {
        string projectPath = ProjectNameToPath(projectName);
        string projectData = File.ReadAllText(projectPath);

        Project project = (Project)JsonConvert.DeserializeObject(projectData);
        List<string> warnings = [];

        if (project.ProjectVersion != Constants.Version) {
            warnings.Add($"The project was created in a different version of MonkeFrames ({project.ProjectVersion}), which may cause compatability issues if project formats are different.");
        }

        return (project, warnings);
    }
}