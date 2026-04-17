using System;

namespace MonkeFrames.Compiler.Models;

public struct CompilerDiagnostics
{
    public TimeSpan BuildTime;

    public int BuiltFrames;
    public int FramesPerSecond;
}