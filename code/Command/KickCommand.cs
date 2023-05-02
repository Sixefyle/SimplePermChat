using SimpleChat;
using SimpleChat.Command;
using SimpleChat.Permission;
using SimpleChat.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox;

namespace SimpleChat.Command
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
				Chat.AddChatEntry(To.Single(sender), "Command Error", "Command usage: /kick <PlayerName>", 0, false);
			}
		}
	}
}
