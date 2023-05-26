using Unity.Transforms;
using VampireCommandFramework;

namespace AluArena.Commands.Admin
{
    public static class PositionCommands
    {
        [Command("pos", "Get current position", adminOnly: true)]
        public static void PositionCommand(ChatCommandContext ctx)
        {
            var location = Plugin.EntityManager.GetComponentData<LocalToWorld>(ctx.Event.SenderCharacterEntity).Position;
            ctx.Reply($"Current Position X: {location.x} Y: {location.y} Z: {location.z}");
        }
    }
}
