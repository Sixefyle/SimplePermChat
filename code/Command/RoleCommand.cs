using SimplePermChat.Command.Interface;
using SimplePermChat.Permission;
using SimplePermChat.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox;

namespace SimplePermChat.Command
{
	internal class RoleCommand : IChatCommand
	{
		public Privilege.Role RequiredRole => Privilege.Role.Admin;

		public void Execute(IClient sender, params string[] parameters)
		{
			if (parameters.Length >= 2) 
			{
				var target = Util.GetClientByName(parameters[0]);
				Privilege.Role role;
				
				if (target != null && Enum.TryParse(parameters[1], out role))
				{
					if (parameters[2] != null)
					{
						var time = int.Parse(parameters[2]);
						Privilege.SetPlayerRole(target.Client, role, time);
						Chat.SendMessage(To.Single(target), "Role", $"You are now {role} for {time} days", 0, false);
						Chat.SendMessage(To.Single(sender), "Role", $"{target.Name} is now {role} for {time} days", 0, false);
					}
					else
					{
						Privilege.SetPlayerRole(target.Client, role);
						Chat.SendMessage(To.Single(target), "Role", $"You are now {role}", 0, false);
						Chat.SendMessage(To.Single(sender), "Role", $"{target.Name} is now {role}", 0, false);
					}
				}
				else
				{
					Chat.SendMessage(To.Single(sender), "Role Error", $"The role '{parameters[1]}' or the player '{parameters[0]}' does not exist!", 0, false);
				}
			}
		}
		public string Helper()
		{
			StringBuilder sb = new StringBuilder();

			sb.Append("Change role for targeted player");
			sb.AppendLine();
			sb.Append("Exemple usage: /role Sixefyle Admin");

			return sb.ToString();
		}
	}
}
