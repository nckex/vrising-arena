using ProjectM.Network;
using ProjectM;
using VampireCommandFramework;
using Wetstone.API;
using AluArena.Helpers;
using Unity.Entities;
using AluArena.Common.Prefabs;

namespace AluArena.Commands
{
    public static class HealthCommands
    {
        [Command("r", usage: ".r", description: "Restore hp and reset skills cooldown")]
        public static void HealthCommand(ChatCommandContext ctx)
        {
            var UserIndex = ctx.Event.User.Index;
            var component = Plugin.EntityManager.GetComponentData<Health>(ctx.Event.SenderCharacterEntity);

            float restore_hp = ((component.MaxHealth / 100) * 100) - component.Value;

            var healthEvent = new ChangeHealthDebugEvent()
            {
                Amount = (int)restore_hp
            };

            VWorld.Server.GetExistingSystem<DebugEventsSystem>().ChangeHealthEvent(UserIndex, ref healthEvent);

            CharacterHelpers.ResetSkillsCooldown(ctx.Event.SenderCharacterEntity);

            if (BuffUtility.TryGetBuff(Plugin.EntityManager, ctx.Event.SenderCharacterEntity, BuffPrefabs.InCombatBuff, out var inCombatBuff))
            {
                Plugin.EntityManager.AddComponent<DestroyTag>(inCombatBuff);
            }

            ctx.Reply("Health and skills cooldown restored.");
        }
    }
}
