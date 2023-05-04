using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox;
using SimplePermChat.Command;
using SimplePermChat.UI;

namespace SimplePermChat
{
	public partial class SimplePermChat
	{
		public static ChatHud ChatHud { get; protected set; }

		[GameEvent.Tick.Client]
		public static void InitChatHud()
		{
			if (ChatHud == null)
			{
				CreateChatHud();
			}
		}

		[GameEvent.Entity.PostSpawn]
		public static void RegisterCommands()
		{
			ChatCommandManager.RegisterCommand("help", new HelpCommand());
			ChatCommandManager.RegisterCommand("hp", new HealthCommand());
			ChatCommandManager.RegisterCommand("kick", new KickCommand());
			ChatCommandManager.RegisterCommand("role", new RoleCommand());
			ChatCommandManager.RegisterCommand("ban", new BanCommand());
		}

		public static void CreateChatHud()
		{
			ChatHud = new ChatHud();
		}
	}
}
