using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox;

namespace SimpleChat.Utils
{
	public class Util
	{
		public static IClient GetClientByName(string name)
		{
			foreach (var client in Game.Clients)
			{
				if (client.Name.ToLower() == name.ToLower())
				{
					return client;
				}
			}
			return null;
		}
	}
}
