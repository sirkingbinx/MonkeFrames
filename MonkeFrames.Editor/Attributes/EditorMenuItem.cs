using System;

namespace MonkeFrames.Editor.Attributes;

public class EditorMenuItem : Attribute
{
    public string Name;
    
    public EditorMenuItem(string name) =>
        Name = name;
}