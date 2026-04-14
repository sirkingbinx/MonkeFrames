using MonkeFrames.Compiler.Models;

namespace MonkeFrames.Core;

public static class Constants
{
    public const string Name = "MonkeFrames";
    public const string Guid = "bingus.monkeframes";
    public const string Version = "1.0";
    public const string Author = "bingus";

    public static readonly Exporter Exporter = new Exporter(Guid, "MonkeFrames Keyframe Editor");

    public static string Loader = "";
}