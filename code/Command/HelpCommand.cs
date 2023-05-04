using Sandbox;
using SimplePermChat.Command.Interface;
using SimplePermChat.Permission;
using System;
using System.Linq;
using System.Text;

namespace SimplePermChat.Command
{
    internal class HelpCommand : IChatCommand
	{
		public Privilege.Role RequiredRole => Privilege.Role.Player;

		public void Execute(IClient sender, params string[] parameters)
		{
			if(parameters.Length == 0) 
			{
				StringBuilder sb = new StringBuilder();
				
				foreach (var command in ChatCommandManager._commandHandlers)
				{
					sb.Append($"/help {command.Key.Split('.')[1]}");
					sb.AppendLine();
				}

				Chat.SendMessage(To.Single(sender), "Help", sb.ToString(), 0, false);
			}
			else if(parameters.Length >= 1)
			{
				var commandName = parameters[0];
				var commands = ChatCommandManager.GetCommand(commandName);

				if(commands.Count == 1)
				{
					Chat.SendMessage(To.Single(sender), $"Help {commandName}", commands.First().Value.Helper(), 0, false);
				}
			}
		}

		public string Helper()
		{
			StringBuilder sb = new StringBuilder();

			sb.Append("This command is used to know how other command work, and how they should be used");
			sb.AppendLine();
			sb.Append("Exemple usage: /help hp");

			return sb.ToString();
		}
	}
}