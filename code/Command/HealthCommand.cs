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
		public string Helper()
		{
			StringBuilder sb = new StringBuilder();

			sb.Append("Change the target Health (reset on death)");
			sb.AppendLine();
			sb.Append("Exemple usage: /hp set Sixefyle 200");

			return sb.ToString();
		}
	}
}
