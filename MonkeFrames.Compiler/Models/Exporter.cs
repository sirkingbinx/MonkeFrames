namespace MonkeFrames.Compiler.Models;

/// <summary>
/// A mod that uses MonkeFrames.Compiler should have an Exporter footprint attached to any project it generates.
/// </summary>
public struct Exporter
{
    /// <summary>
    /// GUID (Global Unique Identifier) of the exporter.
    /// </summary>
    public string GUID;

    /// <summary>
    /// Display name of the exporter.
    /// </summary>
    public string DisplayName;

    /// <summary>
    /// Create a new exporter.
    /// </summary>
    public Exporter(string guid, string displayName)
    {
        GUID = guid;
        DisplayName = displayName;
    }

    /// <summary>
    /// Default exporter which is automatically selected by the MonkeFrames compiler.
    /// </summary>
    public static Exporter Default => new Exporter("monkeframes.compiler", "compiler auto-generated exporter");
}