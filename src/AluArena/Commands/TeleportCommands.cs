using AluArena.Common.Arena;
using AluArena.Helpers;
using VampireCommandFramework;

namespace AluArena.Commands
{
    public static class TeleportCommands
    {
        [Command("tp", usage: ".tp <id>", description: "Teleport to waypoint id")]
        public static void PingCommand(ChatCommandContext ctx, int waypointId)
        {
            var position = waypointId switch
            {
                1 => ArenaPositions.Pos_1,
                2 => ArenaPositions.Pos_2,
                3 => ArenaPositions.Pos_3,
                4 => ArenaPositions.Pos_4,
                _ => ArenaPositions.Pos_1
            };

            CharacterHelpers.TeleportToPos(ctx.Event.SenderUserEntity, ctx.Event.SenderCharacterEntity, position);
        }
    }
}
