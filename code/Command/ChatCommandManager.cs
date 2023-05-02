using Sandbox;
using SimpleChat.Permission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SimpleChat.Command
{
	public static class ChatCommandManager
	{
		internal static Dictionary<string, IChatCommand> _commandHandlers = new();

		public static void RegisterCommand(string commandName, IChatCommand commandHandler)
		{
			if (Game.IsClient) return;

			var finalCommandName = (commandHandler.ToString().Split('.')[0] + "." + commandName).ToLower();
			if (_commandHandlers.ContainsKey(finalCommandName.ToLower()))
			{
				Log.Info($"Attention : The command '{finalCommandName}' is already registered.");
			}
			else
			{
				Log.Info($"Registering command '{finalCommandName}'");
				_commandHandlers[finalCommandName] = commandHandler;
			}
		}

		public static bool ExecuteCommand(IClient sender, string command, params string[] parameters)
		{
			string commandName = command.ToLower();
			bool isAbsoluteCommandPath = commandName.Split('.').Length > 1;


			List<KeyValuePair<string, IChatCommand>> commandList = new();
			foreach(var currentCommand in _commandHandlers)
			{
				if (isAbsoluteCommandPath ? currentCommand.Key == commandName : currentCommand.Key.Split('.')[1] == commandName) 
				{
					commandList.Add(currentCommand);
				}
			}

			if(commandList.Count == 1)
			{
				IChatCommand commandHandler = commandList[0].Value;
				try
				{
					if(Privilege.GetPlayerData(sender.Client).Role >= commandHandler.RequiredRole)
					{
						commandHandler.Execute(sender, parameters);
					}
					else
					{
						Chat.AddChatEntry(To.Single(sender), "Command Error", "You don't have permission to use this command!", 0, false);
					}
				}
				catch (Exception ex)
				{
					Log.Error(ex.Source + " " + ex.Message + ex.StackTrace);
					return false;
				}
				return true;
			} 
			else if(commandList.Count > 0) 
			{
				StringBuilder sb = new StringBuilder();
				sb.Append("There is multiple command with this name:");
				foreach(var currentCommand in commandList)
				{
					sb.AppendLine();
					sb.Append(currentCommand.Key);
				}
				Chat.AddChatEntry(To.Single(sender), "Command Error", sb.ToString(), 0, false);
			}

			return false;
		}
	}
}
