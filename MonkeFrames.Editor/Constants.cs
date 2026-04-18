using MonkeFrames.Compiler.Models;

namespace MonkeFrames.Editor;

public static class Constants
{
    public const string Name = "MonkeFrames";
    public const string Guid = "bingus.monkeframes";
    public const string Version = "1.0";
    public static readonly string VersionID = $"{Version} Beta 5.1";
    public const string Author = "bingus";

    public static readonly Exporter Exporter = new Exporter(Guid, "MonkeFrames");

    public static string Loader = "";
}