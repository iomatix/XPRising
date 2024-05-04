﻿using BepInEx.Configuration;
using BepInEx.Logging;
using OpenRPG.Systems;
using OpenRPG.Utils;

namespace OpenRPG.Configuration;

public static class WantedConfig
{
    private static ConfigFile _configFile;
    
    public static void Initialize()
    {
        Plugin.Log(Plugin.LogSystem.Plugin, LogLevel.Info, "Loading Wanted config");
        var configPath = AutoSaveSystem.ConfirmFile(AutoSaveSystem.ConfigPath, "WantedConfig.cfg");
        _configFile = new ConfigFile(configPath, true);
        
        // Currently, we are never updating and saving the config file in game, so just load the values.
        HunterHuntedSystem.isActive = _configFile.Bind("HunterHunted", "Enable", true, "Enable/disable the HunterHunted system.").Value;
        HunterHuntedSystem.heat_cooldown = _configFile.Bind("HunterHunted", "Heat Cooldown", 10, "Set the reduction value for player heat per minute.").Value;
        HunterHuntedSystem.ambush_interval = _configFile.Bind("HunterHunted", "Ambush Interval", 60, "Set how many seconds player can be ambushed again since last ambush.").Value;
        HunterHuntedSystem.ambush_chance = _configFile.Bind("HunterHunted", "Ambush Chance", 50, "Set the percentage that an ambush may occur for every cooldown interval.").Value;
        var ambushTimer = _configFile.Bind("HunterHunted", "Ambush Despawn Timer", 300f, "Despawn the ambush squad after this many second if they are still alive.\n" +
            "Must be higher than 1.").Value;
        HunterHuntedSystem.vBloodMultiplier = _configFile.Bind("HunterHunted", "VBlood Heat Multiplier", 20, "Multiply the heat generated by VBlood kills.").Value;

        if (ambushTimer < 1) ambushTimer = 300f;
        HunterHuntedSystem.ambush_despawn_timer = ambushTimer;
    }
}