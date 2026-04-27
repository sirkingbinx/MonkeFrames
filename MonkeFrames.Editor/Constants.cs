using System.Collections.Generic;
using System.IO;
using MonkeFrames.Compiler.Models;
using UnityEngine;

namespace MonkeFrames.Editor;

public static class Constants
{
    public const string Name = "MonkeFrames";
    public const string Guid = "bingus.monkeframes";
    public const string Version = "1.1";
    public static readonly string VersionID = $"{Version} Stable 1";
    public const string Author = "bingus";

    public static string DataFolder => Path.Combine(Application.persistentDataPath, "MonkeFrames");
    public static readonly Exporter Exporter = new Exporter(Guid, "MonkeFrames");

    public static readonly Dictionary<string, string> Contributors = new()
    {
        {"SirKingBinx", "Developer" },
        {"uhJames", "Developer" },
        {"", "" },
        {"MrNubbaWubington", "Tester" },
        {"nebwella", "Tester" },
        {"ritesies", "Tester" },
        {"tfsdemon", "Tester" },
        {"Vistro", "Tester" },
        {"arctrie", "Tester" },
        {"lucid", "Tester" },
        {"masondoesxd", "Tester" },
        {"Sl4bs", "Tester" },
        {"TheGreatAqua", "Tester" },
        {"WaterMan", "Tester" },
        {"xtreme", "Tester" },
        {"Aspire", "Tester" },
        {"itzPX", "Tester" },
        {"AGoofyGoose", "Tester" },
        {"AllMightyMonk", "Tester" },
        {"DumDum", "Tester" },
        {"embee", "Tester" },
        {"EmperorPop", "Tester" },
        {"eyeoftheseer", "Tester" },
        {"I drift like Gojo", "Tester" },
        {"JulyDog", "Tester" },
        {"Medievalz", "Tester" },
        {"Micro", "Tester" },
        {"Penny2819", "Tester" },
        {"Pz", "Tester" },
        {"Royradio", "Tester" },
        {"RTXFjp", "Tester" },
        {"swmb", "Tester" },
        {"tehbaconvr", "Tester" },
        {"Violet", "Tester" },
        {"YourBoiAlex", "Tester" },
        {"Gobo", "Tester" },
        {"Atlantic", "Tester" },
        {"Cap", "Tester" },
        {"Circuits", "Tester" },
        {"ItzTapu", "Tester" },
        {"munklurvr", "Tester" },
        {"quantum", "Tester" },
        {"rusty", "Tester" },
        {"Hexann", "Tester" },
    };

    public static string Loader = "";
}