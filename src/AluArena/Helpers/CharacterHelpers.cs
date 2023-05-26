using AluArena.Common.Structs;
using ProjectM;
using ProjectM.Network;
using System;
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Wetstone.API;

namespace AluArena.Helpers
{
    public static class CharacterHelpers
    {
        public static void TeleportToPos(Entity userEntity, Entity characterEntity, float2 pos)
        {
            var entity = Plugin.EntityManager.CreateEntity(
                    ComponentType.ReadWrite<FromCharacter>(),
                    ComponentType.ReadWrite<PlayerTeleportDebugEvent>());

            Plugin.EntityManager.SetComponentData<FromCharacter>(entity, new()
            {
                User = userEntity,
                Character = characterEntity
            });

            Plugin.EntityManager.SetComponentData<PlayerTeleportDebugEvent>(entity, new()
            {
                Position = new float2(pos.x, pos.y),
                Target = PlayerTeleportDebugEvent.TeleportTarget.Self
            });
        }

        public static void RespawnCharacter(Entity VictimEntity, PlayerCharacter player, Entity userEntity)
        {
            var bufferSystem = VWorld.Server.GetOrCreateSystem<EntityCommandBufferSystem>();
            var commandBufferSafe = new EntityCommandBufferSafe(Allocator.Temp)
            {
                Unsafe = bufferSystem.CreateCommandBuffer()
            };

            unsafe
            {
                var playerLocation = player.LastValidPosition;

                var bytes = stackalloc byte[Marshal.SizeOf<NullableFloat>()];
                var bytePtr = new IntPtr(bytes);
                Marshal.StructureToPtr<NullableFloat>(new()
                {
                    value = new float3(playerLocation.x, 0, playerLocation.y),
                    has_value = true
                }, bytePtr, false);
                var boxedBytePtr = IntPtr.Subtract(bytePtr, 0x10);

                var spawnLocation = new Il2CppSystem.Nullable<float3>(boxedBytePtr);
                var server = VWorld.Server.GetOrCreateSystem<ServerBootstrapSystem>();

                server.RespawnCharacter(commandBufferSafe, userEntity, customSpawnLocation: spawnLocation, previousCharacter: VictimEntity, fadeOutEntity: userEntity);
            }
        }

        public static void ClearInventory(Entity characterEntity)
        {
            if (!InventoryUtilities.TryGetInventoryEntity(Plugin.EntityManager, characterEntity, out Entity playerInventory) || playerInventory == Entity.Null)
                return;

            var inventoryBuffer = Plugin.EntityManager.GetBuffer<InventoryBuffer>(playerInventory);

            foreach (var inventoryItem in inventoryBuffer)
            {
                InventoryUtilitiesServer.TryRemoveItem(Plugin.EntityManager, playerInventory, inventoryItem.ItemType, inventoryItem.Stacks);
            }
        }

        public static void ResetSkillsCooldown(Entity characterEntity)
        {
            var AbilityBuffer = Plugin.EntityManager.GetBuffer<AbilityGroupSlotBuffer>(characterEntity);
            foreach (var ability in AbilityBuffer)
            {
                var AbilitySlot = ability.GroupSlotEntity._Entity;
                var ActiveAbility = Plugin.EntityManager.GetComponentData<AbilityGroupSlot>(AbilitySlot);
                var ActiveAbility_Entity = ActiveAbility.StateEntity._Entity;

                var b = PrefabHelpers.GetPrefabGUID(ActiveAbility_Entity);
                if (b.GuidHash == 0) continue;

                var AbilityStateBuffer = Plugin.EntityManager.GetBuffer<AbilityStateBuffer>(ActiveAbility_Entity);
                foreach (var state in AbilityStateBuffer)
                {
                    var abilityState = state.StateEntity._Entity;
                    var abilityCooldownState = Plugin.EntityManager.GetComponentData<AbilityCooldownState>(abilityState);
                    abilityCooldownState.CooldownEndTime = 0;
                    Plugin.EntityManager.SetComponentData(abilityState, abilityCooldownState);
                }
            }
        }

        public static void ChangeSpeed(Entity characterEntity, float speed)
        {
            var component = Plugin.EntityManager.GetComponentData<Movement>(characterEntity);
            component.Speed = ModifiableFloat.Create(characterEntity, Plugin.EntityManager, speed);
            Plugin.EntityManager.SetComponentData(characterEntity, component);
        }
    }
}
