﻿using HarmonyLib;
using Unity.Entities;
using Unity.Collections;
using ProjectM.Network;
using ProjectM;
using RPGMods.Utils;
using RPGMods.Commands;

namespace RPGMods.Hooks;
[HarmonyPatch(typeof(ModifyUnitStatBuffSystem_Spawn), nameof(ModifyUnitStatBuffSystem_Spawn.OnUpdate))]
public class ModifyUnitStatBuffSystem_Spawn_Patch
{
    private static ModifyUnitStatBuff_DOTS Cooldown = new ModifyUnitStatBuff_DOTS()
    {
        StatType = UnitStatType.CooldownModifier,
        Value = 0,
        ModificationType = ModificationType.Set,
        Id = ModificationId.NewId(0)
    };

    private static ModifyUnitStatBuff_DOTS SunCharge = new ModifyUnitStatBuff_DOTS()
    {
        StatType = UnitStatType.SunChargeTime,
        Value = 50000,
        ModificationType = ModificationType.Add,
        Id = ModificationId.NewId(0)
    };

    private static ModifyUnitStatBuff_DOTS Hazard = new ModifyUnitStatBuff_DOTS()
    {
        StatType = UnitStatType.ImmuneToHazards,
        Value = 1,
        ModificationType = ModificationType.Add,
        Id = ModificationId.NewId(0)
    };

    private static ModifyUnitStatBuff_DOTS SunResist = new ModifyUnitStatBuff_DOTS()
    {
        StatType = UnitStatType.SunResistance,
        Value = 10000,
        ModificationType = ModificationType.Add,
        Id = ModificationId.NewId(0)
    };

    private static ModifyUnitStatBuff_DOTS Speed = new ModifyUnitStatBuff_DOTS()
    {
        StatType = UnitStatType.MovementSpeed,
        Value = 10,
        ModificationType = ModificationType.Add,
        Id = ModificationId.NewId(0)
    };

    private static ModifyUnitStatBuff_DOTS PResist = new ModifyUnitStatBuff_DOTS()
    {
        StatType = UnitStatType.PhysicalResistance,
        Value = 10000,
        ModificationType = ModificationType.Add,
        Id = ModificationId.NewId(0)
    };

    private static ModifyUnitStatBuff_DOTS FResist = new ModifyUnitStatBuff_DOTS()
    {
        StatType = UnitStatType.FireResistance,
        Value = 10000,
        ModificationType = ModificationType.Add,
        Id = ModificationId.NewId(0)
    };

    private static ModifyUnitStatBuff_DOTS HResist = new ModifyUnitStatBuff_DOTS()
    {
        StatType = UnitStatType.HolyResistance,
        Value = 10000,
        ModificationType = ModificationType.Add,
        Id = ModificationId.NewId(0)
    };

    private static ModifyUnitStatBuff_DOTS SResist = new ModifyUnitStatBuff_DOTS()
    {
        StatType = UnitStatType.SilverResistance,
        Value = 10000,
        ModificationType = ModificationType.Add,
        Id = ModificationId.NewId(0)
    };

    private static ModifyUnitStatBuff_DOTS GResist = new ModifyUnitStatBuff_DOTS()
    {
        StatType = UnitStatType.GarlicResistance,
        Value = 10000,
        ModificationType = ModificationType.Add,
        Id = ModificationId.NewId(0)
    };

    private static ModifyUnitStatBuff_DOTS SPResist = new ModifyUnitStatBuff_DOTS()
    {
        StatType = UnitStatType.SpellResistance,
        Value = 10000,
        ModificationType = ModificationType.Add,
        Id = ModificationId.NewId(0)
    };

    private static ModifyUnitStatBuff_DOTS PPower = new ModifyUnitStatBuff_DOTS()
    {
        StatType = UnitStatType.PhysicalPower,
        Value = 10000,
        ModificationType = ModificationType.Add,
        Id = ModificationId.NewId(0)
    };

    private static ModifyUnitStatBuff_DOTS SPPower = new ModifyUnitStatBuff_DOTS()
    {
        StatType = UnitStatType.SpellPower,
        Value = 10000,
        ModificationType = ModificationType.Add,
        Id = ModificationId.NewId(0)
    };

    private static ModifyUnitStatBuff_DOTS PHRegen = new ModifyUnitStatBuff_DOTS()
    {
        StatType = UnitStatType.PassiveHealthRegen,
        Value = 10000,
        ModificationType = ModificationType.Add,
        Id = ModificationId.NewId(0)
    };

    private static ModifyUnitStatBuff_DOTS HRecovery = new ModifyUnitStatBuff_DOTS()
    {
        StatType = UnitStatType.HealthRecovery,
        Value = 10000,
        ModificationType = ModificationType.Add,
        Id = ModificationId.NewId(0)
    };

    private static void Prefix(ModifyUnitStatBuffSystem_Spawn __instance)
    {
        EntityManager entityManager = __instance.EntityManager;
        NativeArray<Entity> entities = __instance.__OnUpdate_LambdaJob0_entityQuery.ToEntityArray(Allocator.Temp);

        foreach (var entity in entities)
        {
            PrefabGUID GUID = entityManager.GetComponentData<PrefabGUID>(entity);
            if (GUID.Equals(Database.buff.Buff_VBlood_Perk_Moose))
            {
                Entity Owner = entityManager.GetComponentData<EntityOwner>(entity).Owner;
                if (!entityManager.HasComponent<PlayerCharacter>(Owner)) continue;

                PlayerCharacter playerCharacter = entityManager.GetComponentData<PlayerCharacter>(Owner);
                Entity User = playerCharacter.UserEntity._Entity;
                User Data = entityManager.GetComponentData<User>(User);

                var Buffer = entityManager.GetBuffer<ModifyUnitStatBuff_DOTS>(entity);

                if (Database.nocooldownlist.TryGetValue(Data.PlatformId, out bool b))
                {
                    Buffer.Add(Cooldown);
                }

                if (Database.sunimmunity.TryGetValue(Data.PlatformId, out bool a))
                {
                    Buffer.Add(SunCharge);
                    Buffer.Add(Hazard);
                    Buffer.Add(SunResist);
                }

                if (Database.speeding.TryGetValue(Data.PlatformId, out bool c))
                {
                    Buffer.Add(Speed);
                }

                if (Database.godmode.TryGetValue(Data.PlatformId, out bool d))
                {
                    Buffer.Add(PResist);
                    Buffer.Add(FResist);
                    Buffer.Add(HResist);
                    Buffer.Add(SResist);
                    Buffer.Add(SunResist);
                    Buffer.Add(GResist);
                    Buffer.Add(SPResist);
                    Buffer.Add(PPower);
                    Buffer.Add(SPPower);
                    Buffer.Add(PHRegen);
                    Buffer.Add(HRecovery);
                    Buffer.Add(Hazard);
                    Buffer.Add(SunCharge);
                }
            }
        }
    }
}

[HarmonyPatch(typeof(BuffSystem_Spawn_Server), nameof(BuffSystem_Spawn_Server.OnUpdate))]
public class BuffSystem_Spawn_Server_Patch
{
    private static void Postfix(BuffSystem_Spawn_Server __instance)
    {
        if(HunterHunted.isActive)
        {
            NativeArray<Entity> entities = __instance.__OnUpdate_LambdaJob0_entityQuery.ToEntityArray(Allocator.Temp);
            foreach (var entity in entities)
            {
                if (!__instance.EntityManager.HasComponent<InCombatBuff>(entity)) continue;
                Entity e_Owner = __instance.EntityManager.GetComponentData<EntityOwner>(entity).Owner;
                if (!__instance.EntityManager.HasComponent<PlayerCharacter>(e_Owner)) continue;
                Entity e_User = __instance.EntityManager.GetComponentData<PlayerCharacter>(e_Owner).UserEntity._Entity;
                HunterHunted.HeatManager(e_User, e_Owner, true);
            }
        }
    }
}