namespace MonkeFrames.Components;

public static class KeyframeManager
{
    public List<Keyframe> Keyframes = [];
    public Dictionary<int, float> KeyframeDurations = new();

    public void UpdateData()
    {
        for (int i = 0; i < Keyframes.Length; i++)
        {
            var keyframe = Keyframes[i];

            keyframe.KeyframeID = $"K{i + 1}";
            keyframe.KeyframeIndex = i;
        }
    }
}