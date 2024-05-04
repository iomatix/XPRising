﻿using ProjectM;
using ProjectM.Network;
using System;
using System.Collections.Generic;
using BepInEx.Logging;
using OpenRPG.Models;
using Unity.Entities;
using OpenRPG.Utils;
using OpenRPG.Utils.Prefabs;
using LogSystem = OpenRPG.Plugin.LogSystem;

namespace OpenRPG.Systems
{
    using BloodlineMasteryData =  LazyDictionary<BloodlineSystem.BloodType,MasteryData>;
    public class BloodlineSystem
    {
        private static EntityManager _em = Plugin.Server.EntityManager;

        // TODO is this supposed to be config?
        public static bool bloodAmountModification = true;

        public static bool IsDecaySystemEnabled = false;
        public static bool DraculaGetsAll = true;
        public static double GrowthMultiplier = 1;
        public static int DecayInterval = 60;
        public static int OnlineDecayValue = 0;
        public static int OfflineDecayValue = 1;
        public static double VBloodMultiplier = 15;

        public static bool IsBloodlineSystemEnabled = true;
        public static bool MercilessBloodlines = false;
        public static bool EffectivenessSubSystemEnabled = true;
        public static bool GrowthSubsystemEnabled = true;
        public static double GrowthPerEffectiveness = 1.0;
        public static double MaxBloodlineStrength = 100;
        public static double MaxBloodlineEffectiveness = 5;
        public static double MaxBloodlineGrowth = 10;
        public static double MinBloodlineGrowth = 0.1;

        public enum BloodType
        {
            None = Remainders.BloodType_None,
            Brute = Remainders.BloodType_Brute,
            Creature = Remainders.BloodType_Creature,
            Mutant = Remainders.BloodType_Mutant,
            Rogue = Remainders.BloodType_Rogue,
            Scholar = Remainders.BloodType_Scholar,
            VBlood = Remainders.BloodType_VBlood,
            Warrior = Remainders.BloodType_Warrior,
            Worker = Remainders.BloodType_Worker,   
        }
        
        // None and VBlood types don't count for this number.
        private static int bloodTypeCount = Enum.GetValues<BloodType>().Length - 2;

        // This is a "potential" name to blood type map. Multiple keywords map to the same blood type
        public static Dictionary<string, BloodType> KeywordToBloodMap = new()
        {
            { "dracula", BloodType.None },
            { "arwen", BloodType.Creature },
            { "ilvris", BloodType.Warrior },
            { "aya", BloodType.Rogue },
            { "nytheria", BloodType.Brute },
            { "hadubert", BloodType.Scholar },
            { "rei", BloodType.Worker },
            { "semika", BloodType.Mutant },
            { "semi", BloodType.Mutant },
            { "mutant", BloodType.Mutant },
            { "frail", BloodType.None },
            { "creature", BloodType.Creature },
            { "warrior", BloodType.Warrior },
            { "rogue", BloodType.Rogue },
            { "brute", BloodType.Brute },
            { "scholar", BloodType.Scholar },
            { "worker", BloodType.Worker }
        };

        private static readonly Random rand = new Random();

        public static void UpdateBloodline(Entity killer, Entity victim)
        {
            if (killer == victim) return;
            if (_em.HasComponent<Minion>(victim)) return;

            var victimLevel = _em.GetComponentData<UnitLevel>(victim);
            var killerUserEntity = _em.GetComponentData<PlayerCharacter>(killer).UserEntity;
            var steamID = _em.GetComponentData<User>(killerUserEntity).PlatformId;
            
            double growthVal = victimLevel.Level;
            
            BloodType killerBloodType;
            float killerBloodQuality;
            if (_em.TryGetComponentData<Blood>(killer, out var killerBlood)){
                killerBloodQuality = killerBlood.Quality;
                if (!GuidToBloodType(killerBlood.BloodType, true, out killerBloodType)) return;
            }
            else {
                Plugin.Log(LogSystem.Bloodline, LogLevel.Info, $"killer does not have blood: Killer ({killer}), Victim ({victim}");
                return; 
            }

            BloodType victimBloodType;
            float victimBloodQuality;
            if (_em.TryGetComponentData<BloodConsumeSource>(victim, out var victimBlood)) {
                victimBloodQuality = victimBlood.BloodQuality;
                if (!GuidToBloodType(victimBlood.UnitBloodType, false, out victimBloodType)) return;
            }
            else
            {
                Plugin.Log(LogSystem.Bloodline, LogLevel.Info, $"victim does not have blood: Killer ({killer}), Victim ({victim}");
                return;
            }
            
            if (GrowthSubsystemEnabled)
            {
                var bld = Database.playerBloodline[steamID];
                growthVal *= bld[killerBloodType].Growth;
            }

            var isVBlood = victimBloodType == BloodType.VBlood;
            var bloodlineMastery = Database.playerBloodline[steamID][killerBloodType];
            
            if (MercilessBloodlines){
                if (!isVBlood) // VBlood is allowed to boost all blood types
                {
                    if (killerBloodType != victimBloodType)
                    {
                        Plugin.Log(LogSystem.Bloodline, LogLevel.Info,
                            $"merciless bloodlines exit: Blood types are different: Killer ({Enum.GetName(killerBloodType)}), Victim ({Enum.GetName(victimBloodType)}");
                        return;
                    }
                    
                    if (victimBloodQuality <= bloodlineMastery.Mastery)
                    {
                        Plugin.Log(LogSystem.Bloodline, LogLevel.Info,
                            $"merciless bloodlines exit: victim blood quality less than killer mastery: Killer ({bloodlineMastery.Mastery}), Victim ({victimBloodQuality}");
                        return;
                    }

                    if (killerBloodQuality <= bloodlineMastery.Mastery)
                    {
                        Plugin.Log(LogSystem.Bloodline, LogLevel.Info,
                            $"merciless bloodlines exit: killer blood quality less than mastery: blood quality ({killerBloodQuality}), mastery ({bloodlineMastery.Mastery}");
                        return;
                    }
                }

                if (bloodAmountModification) {
                    // Does this refill the blood meter?
                    killerBlood.MaxBlood.SetBaseValue(50000, killerBlood.MaxBlood.GetOrAddModificationsBuffer(_em, killer));
                }

                growthVal *= 1 + ((victimBloodQuality + killerBloodQuality) - (bloodlineMastery.Mastery*2))/100;
            } else if (isVBlood)
            {
                growthVal *= VBloodMultiplier;
            }

            growthVal *= Math.Max(0.1, rand.NextDouble());

            if (_em.HasComponent<PlayerCharacter>(victim))
            {
                var victimGear = _em.GetComponentData<Equipment>(victim);
                var bonusMastery = victimGear.ArmorLevel + victimGear.WeaponLevel + victimGear.SpellLevel;
                growthVal *= (1 + (bonusMastery * 0.01));
            }

            growthVal = growthVal * GrowthMultiplier / 1000;

            var updatedMastery = ModBloodline(steamID, killerBloodType, growthVal);

            if (Database.playerLogBloodline.TryGetValue(steamID, out var isLogging) && isLogging)
            {
                var updatedValue = updatedMastery.Mastery;
                var bloodTypeName = GetBloodTypeName(killerBloodType);
                Output.SendLore(killerUserEntity, $"<color=#ffb700>{bloodTypeName} bloodline has increased by {growthVal:F3}% [{updatedValue:F3}%]</color>");
            }
        }
        
        public static void DecayBloodline(Entity userEntity, DateTime lastDecay)
        {
            var steamID = _em.GetComponentData<User>(userEntity).PlatformId;
            var elapsedTime = DateTime.Now - lastDecay;
            if (elapsedTime.TotalSeconds < DecayInterval) return;

            var decayTicks = (int)Math.Floor(elapsedTime.TotalSeconds / DecayInterval);
            if (decayTicks > 0)
            {
                var decayValue = OfflineDecayValue * decayTicks * -1;

                Output.SendLore(userEntity, $"You've been offline for {elapsedTime.TotalMinutes} minute(s). Your bloodline has decayed by {decayValue * 0.001:F3}%");
                
                var bld = Database.playerBloodline[steamID];

                foreach (var type in Enum.GetValues<BloodType>())
                {
                    bld = ModBloodline(bld, type, decayValue);
                }

                Database.playerBloodline[steamID] = bld;
            }
        }
        
        public static void ResetBloodline(ulong steamID, BloodType type) {
            if (!EffectivenessSubSystemEnabled) {
                if (Helper.FindPlayer(steamID, true, out _, out var targetUserEntity)) {
                    Output.SendLore(targetUserEntity, $"Effectiveness Subsystem disabled, not resetting bloodline.");
                }
                return;
            }

            var bld = Database.playerBloodline[steamID];
            var bloodMastery = bld[type];
            
            // If it is already 0, then this won't have much effect.
            if (bloodMastery.Mastery > 0)
            {
                bld[type] = bloodMastery.ResetMastery(MaxBloodlineStrength, MaxBloodlineEffectiveness,
                    GrowthPerEffectiveness, MaxBloodlineGrowth, MinBloodlineGrowth);
            }

            Database.playerBloodline[steamID] = bld;
        }
        
        public static void BuffReceiver(DynamicBuffer<ModifyUnitStatBuff_DOTS> buffer, Entity owner, ulong steamID) {
            BloodType bloodType;
            if (!_em.TryGetComponentData<Blood>(owner, out var bloodline) ||
                !GuidToBloodType(bloodline.BloodType, false, out bloodType))
            {
                return;
            }
            ApplyBloodlineBuffs(buffer, bloodType, steamID);
        }
        private static void ApplyBloodlineBuffs(DynamicBuffer<ModifyUnitStatBuff_DOTS> buffer, BloodType bloodType, ulong steamID)
        {
            var isDracula = bloodType == BloodType.None && DraculaGetsAll;
            
            var bld = Database.playerBloodline[steamID];

            if (isDracula)
            {
                var draculaMastery = bld.GetValueOrDefault(BloodType.None);
                foreach (var data in bld)
                {
                    var config = Database.bloodlineStatConfig.GetValueOrDefault(data.Key);
                    foreach (var statConfig in config)
                    {
                        var value = CalcDraculaBuffValue(draculaMastery, data.Value, statConfig, bloodTypeCount);
                        buffer.Add(Helper.MakeBuff(statConfig.type, value));
                    }
                }
            }
            else
            {
                var bloodlineMastery = bld[bloodType];
                var config = Database.bloodlineStatConfig.GetValueOrDefault(bloodType);
                foreach (var statConfig in config)
                {
                    var value = CalcBuffValue(bloodlineMastery, statConfig);
                    buffer.Add(Helper.MakeBuff(statConfig.type, value));
                }
            }
        }

        public static double CalcDraculaBuffValue(MasteryData draculaMastery, MasteryData bloodlineMastery, StatConfig config, double typeCount) {
            var effectiveness = bloodlineMastery.Effectiveness;
            var strength = bloodlineMastery.Mastery;
            if (bloodlineMastery.Mastery >= config.strength)
            {
                // If we don't have the required mastery level, return the base value.
                return config.type == UnitStatType.CooldownModifier ? 1.0 : 0;
            }
            strength *= (draculaMastery.Mastery / 100) * draculaMastery.Effectiveness / typeCount;

            return Helper.CalcBuffValue(strength, effectiveness, config.rate, config.type);
        }

        public static double CalcBuffValue(MasteryData bloodlineMastery, StatConfig config) {
            if (bloodlineMastery.Mastery >= config.strength)
            {
                // If we don't have the required mastery level, return the base value.
                return config.type == UnitStatType.CooldownModifier ? 1.0 : 0;
            }

            var effectiveness = EffectivenessSubSystemEnabled ? bloodlineMastery.Effectiveness : 1;
            return Helper.CalcBuffValue(bloodlineMastery.Mastery, effectiveness, config.rate, config.type);
        }

        public static MasteryData ModBloodline(ulong steamID, BloodType type, double changeInMastery)
        {
            var bloodlineMasteryData = Database.playerBloodline[steamID];
            var bloodMastery = bloodlineMasteryData[type];
            
            bloodMastery.Mastery = Math.Clamp(bloodMastery.Mastery + changeInMastery, 0, MaxBloodlineStrength);
            bloodlineMasteryData[type] = bloodMastery;

            Database.playerBloodline[steamID] = bloodlineMasteryData;
            return bloodMastery;
        }
        
        private static BloodlineMasteryData ModBloodline(BloodlineMasteryData bloodlineMasteryData, BloodType type, double changeInMastery)
        {
            var bloodMastery = bloodlineMasteryData[type];
            
            bloodMastery.Mastery = Math.Clamp(bloodMastery.Mastery + changeInMastery, 0, MaxBloodlineStrength);
            bloodlineMasteryData[type] = bloodMastery;

            return bloodlineMasteryData;
        }

        private static bool GuidToBloodType(PrefabGUID guid, bool isKiller, out BloodType bloodType)
        {
            bloodType = BloodType.None;
            if(!Enum.IsDefined(typeof(BloodType), guid.GuidHash)) {
                Plugin.Log(LogSystem.Bloodline, LogLevel.Warning, $"Bloodline not found for guid {guid.GuidHash}. isKiller ({isKiller})", true);
                return false;
            }

            bloodType = (BloodType)guid.GuidHash;
            return true;
        }

        public static string GetBloodTypeName(BloodType type)
        {
            // TODO Consider switching to display flavour names.
            // switch (type)
            // {
            //     case BloodType.VBlood:
            //     case BloodType.None:
            //         return "Dracula, Vampire Progenitor";
            //     case BloodType.Brute:
            //         return "Nytheria the Destroyer";
            //     case BloodType.Creature:
            //         return "Arwen the Godeater";
            //     case BloodType.Mutant:
            //         return "Semika the Ever-shifting";
            //     case BloodType.Rogue:
            //         return "Aya the Shadowlord";
            //     case BloodType.Scholar:
            //         return "Hadubert the Inferno";
            //     case BloodType.Warrior:
            //         return "Ilvris Dragonblood";
            //     case BloodType.Worker:
            //         return "Rei the Binder";
            //     default:
            //         Plugin.Log(LogSystem.Bloodline, LogLevel.Warning, $"Unknown bloodline type {Enum.GetName(type)}.");
            //         return "Unknown bloodline";
            // }
            
            return Enum.GetName(type);
        }

        public static Dictionary<BloodType, List<StatConfig>> DefaultBloodlineConfig()
        {
            return new Dictionary<BloodType, List<StatConfig>>()
            {
                { BloodType.None, new List<StatConfig>() },
                { BloodType.Creature, new List<StatConfig>
                {
                    new(UnitStatType.HolyResistance, 0, 0.25),
                    new(UnitStatType.MovementSpeed, 50, 0.005),
                    new(UnitStatType.DamageVsHumans, 100, 0.0025)
                } },
                { BloodType.Brute, new List<StatConfig>
                {
                    new(UnitStatType.SilverResistance, 0, 0.25),
                    new(UnitStatType.PhysicalCriticalStrikeDamage, 50, 0.01),
                    new(UnitStatType.DamageVsUndeads, 100, 0.0025)
                } },
                { BloodType.Mutant, new List<StatConfig>
                {
                    new(UnitStatType.SpellCriticalStrikeChance, 0, 0.005),
                    new(UnitStatType.MovementSpeed, 50, 0.005),
                    new(UnitStatType.DamageVsHumans, 100, 0.0025)
                } },
                { BloodType.Rogue, new List<StatConfig>
                {
                    new(UnitStatType.SunResistance, 0, 0.25),
                    new(UnitStatType.PhysicalCriticalStrikeChance, 50, 0.001),
                    new(UnitStatType.DamageVsPlayerVampires, 100, 0.0025)
                } },
                { BloodType.Scholar, new List<StatConfig>
                {
                    new(UnitStatType.SpellPower, 0, 0.1),
                    new(UnitStatType.CooldownModifier, 50, 200),
                    new(UnitStatType.DamageVsDemons, 100, 0.0025)
                } },
                { BloodType.VBlood, new List<StatConfig>() },
                { BloodType.Warrior, new List<StatConfig>
                {
                    new(UnitStatType.FireResistance, 0, 0.25),
                    new(UnitStatType.PhysicalPower, 50, 0.1),
                    new(UnitStatType.DamageVsBeasts, 100, 0.0025)
                } },
                { BloodType.Worker, new List<StatConfig>
                {
                    new(UnitStatType.GarlicResistance, 0, 0.25),
                    new(UnitStatType.ResourceYield, 50, 0.01),
                    new(UnitStatType.DamageVsMineral, 100, 0.0025),
                    new(UnitStatType.DamageVsVegetation, 100, 0.0025),
                    new(UnitStatType.DamageVsWood, 100, 0.0025)
                } }
            };
        }
    }
}
