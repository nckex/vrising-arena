using AluArena.Helpers;
using HarmonyLib;
using ProjectM;
using ProjectM.Network;
using Unity.Collections;
using Unity.Entities;

namespace AluArena.Hooks;
[HarmonyPatch]
public class DeathEventListenerSystem_Patch
{
    [HarmonyPatch(typeof(DeathEventListenerSystem), "OnUpdate")]
    [HarmonyPostfix]
    public static void Postfix(DeathEventListenerSystem __instance)
    {
        if (__instance._DeathEventQuery != null)
        {
            NativeArray<DeathEvent> deathEvents = __instance._DeathEventQuery.ToComponentDataArray<DeathEvent>(Allocator.Temp);
            foreach (DeathEvent ev in deathEvents)
            {
                // Player Creature Kill Tracking
                //if (__instance.EntityManager.HasComponent<PlayerCharacter>(ev.Killer) && __instance.EntityManager.HasComponent<Movement>(ev.Died))
                //{

                //}

                // Auto Respawn
                if (__instance.EntityManager.HasComponent<PlayerCharacter>(ev.Died))
                {
                    PlayerCharacter player = __instance.EntityManager.GetComponentData<PlayerCharacter>(ev.Died);
                    Entity userEntity = player.UserEntity._Entity;
                    User user = __instance.EntityManager.GetComponentData<User>(userEntity);
                    //ulong SteamID = user.PlatformId;

                    // Check for AutoRespawn
                    if (user.IsConnected)
                    {
                        CharacterHelpers.RespawnCharacter(ev.Died, player, userEntity);
                        CharacterHelpers.ResetSkillsCooldown(user.LocalCharacter._Entity);
                    }
                }
            }
        }
    }
}