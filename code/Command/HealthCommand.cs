using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox;
using SimpleChat.Permission;
using SimpleChat.Utils;

namespace SimpleChat.Command
{
	internal class HealthCommand : IChatCommand
	{
		public Privilege.Role RequiredRole => Privilege.Role.Moderator;

		public void Execute(IClient sender, params string[] parameters)
		{
			Entity target = sender.Pawn as Entity;
			if (parameters.Length > 2)
			{
				switch (parameters[0])
				{
					case "set":
						target = Util.GetClientByName(parameters[1]).Pawn as Entity;
						target.Health = float.Parse(parameters[2]);
						break;
				}
			}
		}


	}
}
