using AluArena.Helpers;
using BepInEx;
using BepInEx.IL2CPP;
using HarmonyLib;
using ProjectM;
using System.Reflection;
using Unity.Entities;
using VampireCommandFramework;
using Wetstone.API;

namespace AluArena;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
[BepInDependency("gg.deca.VampireCommandFramework")]
[BepInDependency("xyz.molenzwiebel.wetstone")]
[Reloadable]
public class Plugin : BasePlugin, IRunOnInitialized
{
    public static Harmony Harmony { get; private set; }
    public static EntityManager EntityManager => VWorld.Server.EntityManager;

    public override void Load()
    {
        Harmony = new Harmony(MyPluginInfo.PLUGIN_GUID);
        Harmony.PatchAll(Assembly.GetExecutingAssembly());

        CommandRegistry.RegisterAll();

        Log.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} version {MyPluginInfo.PLUGIN_VERSION} is loaded!");
    }

    public override bool Unload()
    {
        CommandRegistry.UnregisterAssembly(Assembly.GetExecutingAssembly());
        return true;
    }

    public void OnGameInitialized()
    {
        Initialize();
    }

    public static void Initialize()
    {
        DebugSystemHelper.SetDebugSetting(DebugSettingType.DropsDisabled, true);
        DebugSystemHelper.SetDebugSetting(DebugSettingType.DayNightCycleDisabled, true);
    }
}