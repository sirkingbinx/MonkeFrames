namespace MonkeFrames.Editor.Utilities;

public static class ConditionsUtilities
{
    public static int Time
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
}
