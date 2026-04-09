namespace MonkeFrames.Components;

public static class KeyframeManager
{
    public List<Keyframe> Keyframes = [];
    public float TotalDuration;

    public void UpdateData()
    {
        TotalDuration = 0;

        for (int i = 0; i < Keyframes.Length; i++)
        {
            var keyframe = Keyframes[i];
            keyframe.KeyframeID = $"K{i + 1}";
            keyframe.KeyframeIndex = i;

            keyframe.Playback_RelationalStart = TotalDuration;
            keyframe.Playback_RelationalEnd = TotalDuration + keyframe.Transition.Duration;

            TotalDuration += keyframe.Transition.Duration;
        }
    }

    public bool TryGetKeyframeAtTimestamp(float timestamp, out Keyframe keyframe)
    {
        if (timestamp > TotalDuration || timestamp < 0)
            return false; // Looking for timestamp outside of playback range

        foreach (Keyframe currentKeyframe in Keyframes) {
            float start = currentKeyframe.Playback_RelationalStart;
            float end = currentKeyframe.Playback_RelationalEnd;

            if (timestamp >= start && timestamp <= end)
            {
                keyframe = currentKeyframe;
                return true;
            }
        }

        return false; // Unknown error
    }
}