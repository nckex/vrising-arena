using AluArena.Helpers;
using VampireCommandFramework;
using Wetstone.API;

namespace AluArena.Commands.Admin
{
    public static class SpeedCommands
    {
        [Command("speed", "Update speed", adminOnly: true)]
        public static void PositionCommand(ChatCommandContext ctx, float speed)
        {
            CharacterHelpers.ChangeSpeed(ctx.Event.SenderCharacterEntity, speed);
            ctx.Event.User.SendSystemMessage($"Speed set to <color=#ffff00ff>{speed}</color>");
        }
    }
}
