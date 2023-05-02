using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox;
using SimpleChat.Command;
using SimpleChat.UI;

namespace SimpleChat
{
	public partial class SimpleChat
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
			ChatCommandManager.RegisterCommand("health", new HealthCommand());
			ChatCommandManager.RegisterCommand("kick", new KickCommand());
		}

		public static void CreateChatHud()
		{
			ChatHud = new ChatHud();
		}
	}
}
