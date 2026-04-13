using MonkeFrames.Compiler.Serializable;

namespace MonkeFrames.Compiler.Models;

// Frame is a representation of a point in the timeline post-compilation
// If you're looking to add more features to MonkeFrames, you are looking
// for MonkeFrames.Models.Keyframe.
public struct Frame
{
    public Vec3 Position;
    public Vec3 Rotation;
    public float FieldOfView;
}