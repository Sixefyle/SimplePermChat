using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox;
using SimplePermChat.Command.Interface;
using SimplePermChat.Permission;
using SimplePermChat.Utils;

namespace SimplePermChat.Command
{
	public class BanCommand : IChatCommand
	{
		public Privilege.Role RequiredRole => Privilege.Role.Moderator;

		public void Execute(IClient sender, params string[] parameters)
		{ 
			try
			{
				var target = Util.GetClientByName(parameters[0]);

				if(target == sender)
				{
					Chat.SendMessage("Command Error", "You can't ban yourself!", 0, false);
					return;
				}

				if(target != null) 
				{
					var endDateTime = DateTime.Now.AddHours(float.Parse(parameters[1]));

					Privilege.GetPlayerData(target).BanEndTime = endDateTime;
					Privilege.SavePlayerData(target);
					target.Kick();
				}
			}
			catch (Exception ex)
			{
				Chat.SendMessage(To.Single(sender), "Command Error", "Command usage: /ban <Name> <Time>", 0, false);
			}
		}

		public string Helper()
		{
			StringBuilder sb = new StringBuilder();

			sb.Append("Ban someone from the server for a time in hour");
			sb.AppendLine();
			sb.Append("Exemple usage: /ban Sixefyle 3");

			return sb.ToString();
		}
	}
}
