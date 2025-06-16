using System;
using CommandSystem;
using LabApi.Features.Wrappers;

namespace ScpProximityChat;

[CommandHandler(typeof(ClientCommandHandler))]
public sealed class ToggleProximityCommand : ICommand
{

    public string Command => "toggleProximityChat";
    public string[] Aliases { get; } = ["pxc"];
    public string Description => "Toggles proximity chat, if applicable.";

    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        if (!Player.TryGet(sender, out var player))
        {
            response = "Only players can use this command.";
            return false;
        }

        if (!player.CanUseProximityChat())
        {
            response = "Your current role does not have permission to use proximity chat.";
            return false;
        }

        response = player.ToggleProximityChat()
            ? "Enabled proximity chat."
            : "Disabled proximity chat.";
        return true;
    }

}
