namespace MonkeFrames.Compiler.Models;

/// <summary>
/// Transition data such as movement styling and duration.
/// </summary>
public struct Transition
{
    /// <summary>
    /// The type of transition to apply.
    /// </summary>
    public TransitionEffect Effect = TransitionEffect.Linear;

    /// <summary>
    /// The amount of time the transitioning lasts.
    /// </summary>
    public float Duration = 5f;

    /// <summary>
    /// Create a new Transition.
    /// </summary>
    public Transition() { }

    /// <summary>
    /// The default transition.
    /// </summary>
    public static Transition Linear
    {
        get
        {
            field = new Transition();

            field.Effect = TransitionEffect.Linear;
            field.Duration = 5f;

            return field;
        }
    }

}

/// <summary>
/// Transition style to apply.
/// </summary>
public enum TransitionEffect
{
    /// <summary>
    /// Basic direct-line transition.
    /// </summary>
    Linear = 0,

    /// <summary>
    /// Movement is smoothed from start to finish with a sine wave.
    /// </summary>
    Sine,

    /// <summary>
    /// The camera stays at the keyframe's position for the entire duration of the transition.
    /// </summary>
    Cut
}