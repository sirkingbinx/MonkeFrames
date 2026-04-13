namespace MonkeFrames.Compiler;

public static class Compiler
{
    public static CompilationState State;
}

public enum CompilationState
{
    Ready = 0,
    DeterminingLength,
    ApplyingTransitions,
    SavingData
}