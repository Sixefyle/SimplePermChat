using Sandbox;
using SimpleChat.Command;
using System.Linq;

namespace SimpleChat
{
	public partial class Chat
	{
		[ConCmd.Client("chat_add", CanBeCalledFromServer = true)]
		public static void AddChatEntry(string name, string message, string playerId = "0", bool isInfo = false)
		{
			Current?.AddEntry(name, message, long.Parse(playerId), isInfo);

			// Only log clientside if we're not the listen server host
			if (!Game.IsListenServer)
			{
				Log.Info($"{name}: {message}");
			}
		}

		[ClientRpc]
		public static void AddChatEntry(string name, string message, long playerId = 0, bool isInfo = false)
		{
			AddChatEntry(name, message, playerId.ToString(), isInfo);
		}

		[ConCmd.Server("say")]
		public static void Say(string message)
		{
			if (!ConsoleSystem.Caller.IsValid()) return;

			// todo - reject more stuff
			if (message.Contains('\n') || message.Contains('\r'))
				return;
			
			if (message.StartsWith('/'))
			{
				DoCommandIfExist(ConsoleSystem.Caller, message.Trim('/'));
			} 
			else
			{
				AddChatEntry(To.Everyone, ConsoleSystem.Caller.Name, message, ConsoleSystem.Caller.SteamId, true);
			}
			Log.Info($"{ConsoleSystem.Caller}: {message}");

		}

		public static void DoCommandIfExist(IClient sender, string command)
		{
			var commandLine = command.Split(' ');
			command = commandLine[0];
			var args = commandLine.Skip(1).ToArray();

			if (!ChatCommandManager.ExecuteCommand(sender, command, args))
			{
				Chat.AddChatEntry(To.Single(sender), "Command Error", "Command not found!", 0, false);
			}
		}
	}
}
