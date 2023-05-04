using SimplePermChat;
using SimplePermChat.Permission;
using SimplePermChat.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox;
using SimplePermChat.Command.Interface;

namespace SimplePermChat.Command
{
    public class KickCommand : IChatCommand
	{
		public Privilege.Role RequiredRole => Privilege.Role.Moderator;

		public void Execute(IClient sender, params string[] parameters)
		{
			if(parameters.Length > 0) 
			{
				var target = Util.GetClientByName(parameters[0]);
				if(target != null && target.IsValid) 
				{
					target.Kick();
				}
			}
			else
			{
				Chat.SendMessage(To.Single(sender), "Command Error", "Command usage: /kick <PlayerName>", 0, false);
			}
		}

		public string Helper()
		{
			StringBuilder sb = new StringBuilder();

			sb.Append("Kick someone from the server");
			sb.AppendLine();
			sb.Append("Exemple usage: /kick Sixefyle");

			return sb.ToString();
		}
	}
}
