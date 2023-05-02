using Sandbox;
using SimpleChat.Permission;
using System.Text;

namespace SimpleChat.Command
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
					sb.Append($"/help {command.Value.ToString().Split('.')[0]}");
					sb.AppendLine();
				}

				Chat.AddChatEntry(To.Single(sender), "Help", sb.ToString(), 0, false);
			}
		}
	}
}