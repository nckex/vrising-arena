using ProjectM.Network;
using VampireCommandFramework;

namespace AluArena.Commands
{
    public static class PingCommands
    {
        [Command("ping", usage: ".ping", description: "Shows your latency")]
        public static void PingCommand(ChatCommandContext ctx)
        {
            var ping = (int)(Plugin.EntityManager.GetComponentData<Latency>(ctx.Event.SenderCharacterEntity).Value * 1000);

            var color = ping switch
            {
                < 100 => "#00FF00",
                < 200 => "#ffff00",
                _ => "#FF0000"
            };

            ctx.Reply($"Ping: <color={color}>{ping}</color> ms");
        }
    }
}
