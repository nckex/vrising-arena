using HarmonyLib;
using ProjectM;
using Unity.Collections;
using ProjectM.Network;
using AluArena.Helpers;
using AluArena.Common.Arena;
using AluArena.Commands;
using AluArena.Common.Prefabs;
using Unity.Entities;
using Unity.Scenes;

namespace AluArena.Hooks
{
    [HarmonyPatch(typeof(Destroy_TravelBuffSystem), nameof(Destroy_TravelBuffSystem.OnUpdate))]
    public class Destroy_TravelBuffSystem_Patch
    {
        private static void Postfix(Destroy_TravelBuffSystem __instance)
        {
            if (__instance.__OnUpdate_LambdaJob0_entityQuery != null)
            {
                var entities = __instance.__OnUpdate_LambdaJob0_entityQuery.ToEntityArray(Allocator.Temp);
                foreach (var entity in entities)
                {
                    PrefabGUID GUID = __instance.EntityManager.GetComponentData<PrefabGUID>(entity);
                    if (GUID.Equals(BuffPrefabs.AB_Interact_TombCoffinSpawn_Travel))
                    {
                        var Owner = __instance.EntityManager.GetComponentData<EntityOwner>(entity).Owner;
                        if (!__instance.EntityManager.HasComponent<PlayerCharacter>(Owner)) return;

                        var userEntity = __instance.EntityManager.GetComponentData<PlayerCharacter>(Owner).UserEntity._Entity;
                        var user = __instance.EntityManager.GetComponentData<User>(userEntity);

                        KitCommands.AddSanguineKit(user.LocalCharacter._Entity);
                        CharacterHelpers.TeleportToPos(userEntity, user.LocalCharacter._Entity, ArenaPositions.Pos_1);
                    }
                }
            }
        }
    }

    [HarmonyPatch(typeof(ModifyUnitStatBuffSystem_Spawn), nameof(ModifyUnitStatBuffSystem_Spawn.OnUpdate))]
    public class ModifyUnitStatBuffSystem_Spawn_Patch
    {
        private static void Prefix(ModifyUnitStatBuffSystem_Spawn __instance)
        {
            if (__instance.__OnUpdate_LambdaJob0_entityQuery == null) return;

            NativeArray<Entity> entities = __instance.__OnUpdate_LambdaJob0_entityQuery.ToEntityArray(Allocator.Temp);
            foreach (var entity in entities)
            {
                PrefabGUID GUID = __instance.EntityManager.GetComponentData<PrefabGUID>(entity);
                
                Entity Owner = Plugin.EntityManager.GetComponentData<EntityOwner>(entity).Owner;
                if (!Plugin.EntityManager.HasComponent<PlayerCharacter>(Owner)) 
                    continue;

                PlayerCharacter playerCharacter = Plugin.EntityManager.GetComponentData<PlayerCharacter>(Owner);
                Entity User = playerCharacter.UserEntity._Entity;
                User Data = Plugin.EntityManager.GetComponentData<User>(User);

                if (GUID == BuffPrefabs.WolfNormal || GUID == BuffPrefabs.WolfStygian)
                {
                    CharacterHelpers.ChangeSpeed(Data.LocalCharacter._Entity, 15.00f);
                }
            }
        }
    }

    [HarmonyPatch(typeof(ModifyUnitStatBuffSystem_Destroy), nameof(ModifyUnitStatBuffSystem_Destroy.OnUpdate))]
    public class ModifyUnitStatBuffSystem_Destroy_Patch
    {
        private static void Prefix(ModifyUnitStatBuffSystem_Destroy __instance)
        {
            if (__instance.__OnUpdate_LambdaJob0_entityQuery == null) return;

            NativeArray<Entity> entities = __instance.__OnUpdate_LambdaJob0_entityQuery.ToEntityArray(Allocator.Temp);
            foreach (var entity in entities)
            {
                PrefabGUID GUID = __instance.EntityManager.GetComponentData<PrefabGUID>(entity);

                Entity Owner = Plugin.EntityManager.GetComponentData<EntityOwner>(entity).Owner;
                if (!Plugin.EntityManager.HasComponent<PlayerCharacter>(Owner))
                    continue;

                PlayerCharacter playerCharacter = Plugin.EntityManager.GetComponentData<PlayerCharacter>(Owner);
                Entity User = playerCharacter.UserEntity._Entity;
                User Data = Plugin.EntityManager.GetComponentData<User>(User);

                if (GUID == BuffPrefabs.WolfNormal || GUID == BuffPrefabs.WolfStygian)
                {
                    CharacterHelpers.ChangeSpeed(Data.LocalCharacter._Entity, 4.4f);
                }
            }
        }
    }
}
