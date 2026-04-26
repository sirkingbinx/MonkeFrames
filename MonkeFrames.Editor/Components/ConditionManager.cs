using UnityEngine;

namespace MonkeFrames.Editor.Components;

public class ConditionManager : MonoBehaviour
{
    public static int Time;

    public static int _Time
    {
        get => BetterDayNightManager.instance.currentTimeIndex;
        set {
            if (value >= BetterDayNightManager.instance.dayNightLightmapNames.Length || value < 0)
                throw new System.Exception("Time invalid");

            BetterDayNightManager.instance.SetTimeOfDay(value);
        }
    }

    public static BetterDayNightManager.WeatherType Conditions
    {
        get => BetterDayNightManager.instance.CurrentWeather();
        set => BetterDayNightManager.instance.SetFixedWeather(value);
    }

    private void Start()
    {
        Time = _Time;
    }

    private void Update()
    {
        if (Time != _Time)
            _Time = Time;
    }
}
