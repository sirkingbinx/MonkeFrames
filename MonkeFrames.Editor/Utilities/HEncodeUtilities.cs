using UnityEngine;

namespace MonkeFrames.Editor.Utilities;

public static class HEncodeUtilities
{
    public static string GetGoodEncoder()
    {
        string gpuModel = SystemInfo.graphicsDeviceName.ToLower();

        if (gpuModel.Contains("rtx"))
            return "h264_nvenc";
        if (gpuModel.Contains("radeon"))
            return "h264_amf";

        return "libx264";
    }
}
