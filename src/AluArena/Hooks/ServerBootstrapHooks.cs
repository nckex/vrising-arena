using HarmonyLib;
using ProjectM;
using Stunlock.Network;
using ProjectM.Network;
using AluArena.Helpers;
using AluArena.Common.Arena;
using ProjectM.Gameplay.Systems;
using Steamworks;

namespace AluArena.Hooks
{
    [HarmonyPatch(typeof(HandleGameplayEventsSystem), nameof(HandleGameplayEventsSystem.OnUpdate))]
    public class InitializationPatch
    {
        [HarmonyPostfix]
        public static void Initialize_Method()
        {
            Plugin.Initialize();
            Plugin.Harmony.Unpatch(typeof(HandleGameplayEventsSystem).GetMethod("OnUpdate"), typeof(InitializationPatch).GetMethod("Initialize_Method"));
        }
    }

    [HarmonyPatch(typeof(GameBootstrap), nameof(GameBootstrap.Start))]
    public static class GameBootstrap_Patch
    {
        public static void Postfix()
        {
            Plugin.Initialize();
        }
    }

    [HarmonyPatch(typeof(ServerBootstrapSystem), nameof(ServerBootstrapSystem.OnUserConnected))]
    public static class OnUserConnected_Patch
    {
        public static void Postfix(ServerBootstrapSystem __instance, NetConnectionId netConnectionId)
        {
            try
            {
                var em = __instance.EntityManager;
                var userIndex = __instance._NetEndPointToApprovedUserIndex[netConnectionId];
                var serverClient = __instance._ApprovedUsersLookup[userIndex];
                var userEntity = serverClient.UserEntity;
                var userData = __instance.EntityManager.GetComponentData<User>(userEntity);
                bool isNewVampire = userData.CharacterName.IsEmpty;

                if (!isNewVampire)
                {
                    CharacterHelpers.TeleportToPos(serverClient.UserEntity, userData.LocalCharacter._Entity, ArenaPositions.Pos_1);
                }
            }
            catch { }
        }
    }

    [HarmonyPatch(typeof(ServerBootstrapSystem), nameof(ServerBootstrapSystem.OnUserDisconnected))]
    public static class OnUserDisconnected_Patch
    {
        private static void Prefix(ServerBootstrapSystem __instance, NetConnectionId netConnectionId, ConnectionStatusChangeReason connectionStatusReason, string extraData)
        {
            try
            {
                var userIndex = __instance._NetEndPointToApprovedUserIndex[netConnectionId];
                var serverClient = __instance._ApprovedUsersLookup[userIndex];
                var userData = __instance.EntityManager.GetComponentData<User>(serverClient.UserEntity);
                bool isNewVampire = userData.CharacterName.IsEmpty;

                if (!isNewVampire)
                {
                    CharacterHelpers.TeleportToPos(serverClient.UserEntity, userData.LocalCharacter._Entity, ArenaPositions.Pos_AFK);
                }
            }
            catch { };
        }
    }
}
