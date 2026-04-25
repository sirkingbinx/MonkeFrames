using System.IO;
using MonkeFrames.Compiler.Models;
using UnityEngine;

namespace MonkeFrames.Editor;

public static class Constants
{
    public const string Name = "MonkeFrames";
    public const string Guid = "bingus.monkeframes";
    public const string Version = "1.1";
    public static readonly string VersionID = $"{Version} Beta 1";
    public const string Author = "bingus";

    public static string DataFolder => Path.Combine(Application.persistentDataPath, "MonkeFrames");
    public static readonly Exporter Exporter = new Exporter(Guid, "MonkeFrames");

    public static string Loader = "";
}