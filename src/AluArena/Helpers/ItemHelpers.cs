using AluArena.Common.Structs;
using ProjectM;
using System;
using System.Runtime.InteropServices;
using Unity.Entities;
using Wetstone.API;

namespace AluArena.Helpers
{
    public static class ItemHelpers
    {
        public static Entity AddItemToInventory(Entity characterEntity, PrefabGUID guid, int amount, int slotIdx = 0)
        {
            unsafe
            {
                var gameData = VWorld.Server.GetExistingSystem<GameDataSystem>();
                var bytes = stackalloc byte[Marshal.SizeOf<FakeNull>()];
                var bytePtr = new IntPtr(bytes);

                Marshal.StructureToPtr<FakeNull>(new()
                {
                    value = slotIdx,
                    has_value = true
                }, bytePtr, false);

                var boxedBytePtr = IntPtr.Subtract(bytePtr, 0x10);

                var hack = new Il2CppSystem.Nullable<int>(boxedBytePtr);

                _ = InventoryUtilitiesServer.TryAddItem(
                        Plugin.EntityManager,
                        gameData.ItemHashLookupMap,
                        characterEntity,
                        guid,
                        amount,
                        out _,
                        out Entity e,
                        default,
                        hack);

                return e;
            }
        }
    }
}
