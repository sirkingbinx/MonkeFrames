using System;

namespace MonkeFrames.Editor.Attributes;

public class EditorMenuItem : Attribute
{
    public string Name;
    public Action Action;
    
    public EditorMenuItem(string name) =>
        Name = name;
}