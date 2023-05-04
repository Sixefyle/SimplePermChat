using Sandbox;
using SimplePermChat.Command.Interface;
using SimplePermChat.Permission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SimplePermChat.Command
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

		/// <summary>
		/// Get a command by name
		/// </summary>
		/// <param name="commandName"></param>
		/// <returns>A dictionnary containing all the commands found with the commandName</returns>
		public static Dictionary<string, IChatCommand> GetCommand(string commandName)
		{
			commandName = commandName.ToLower();
			Dictionary<string, IChatCommand> foundCommands = new();

			bool isAbsoluteCommandPath = commandName.Split('.').Length > 1;
			foreach ( var command in _commandHandlers )
			{
				if (isAbsoluteCommandPath ? command.Key == commandName : command.Key.Split('.')[1] == commandName)
				{
					foundCommands[command.Key] = command.Value;
				}
			}

			return foundCommands;
		}

		public static bool ExecuteCommand(IClient sender, string command, params string[] parameters)
		{
			string commandName = command.ToLower();
			bool isAbsoluteCommandPath = commandName.Split('.').Length > 1;

			Dictionary<string, IChatCommand> commandList = GetCommand(command);

			if(commandList.Count == 1)
			{
				IChatCommand commandHandler = commandList.First().Value;
				try
				{
					if(Privilege.GetPlayerData(sender.Client).Role >= commandHandler.RequiredRole)
					{
						commandHandler.Execute(sender, parameters);
					}
					else
					{
						Chat.SendMessage(To.Single(sender), "Command Error", "You don't have permission to use this command!", 0, false);
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
				Chat.SendMessage(To.Single(sender), "Command Error", sb.ToString(), 0, false);
			}

			return false;
		}
	}
}
