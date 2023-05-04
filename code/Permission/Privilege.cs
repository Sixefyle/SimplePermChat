using Sandbox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SimplePermChat.Permission.Privilege;

namespace SimplePermChat.Permission
{
	public class Privilege
	{
		public enum Role
		{
			Player, 
			Vip, 
			Moderator, 
			Admin,
			Owner
		}

		public class PlayerData{
			public long Steamid { get; set; }
			public string LastSeenName { get; set; }
			public DateTime LastSeenDate { get; set; }
			public Role Role { get; set; }
			public DateTime RoleEndTime { get; set; }
			public DateTime BanEndTime { get; set; }
		}

		//Maybe use SetValue() from client
		public static Dictionary<IClient, PlayerData> ClientsData = new();

		public static PlayerData GetPlayerData(IClient client)
		{
			if (ClientsData.ContainsKey(client))
			{
				return ClientsData[client];
			}

			return null;
		}

		public static void SetPlayerRole(IClient client, Role role)
		{
			ClientsData[client].Role = role;
			SavePlayerData(client);
		}

		public static void SetPlayerRole(IClient client, Role role, int days)
		{
			ClientsData[client].RoleEndTime = DateTime.Now.AddDays(days);
			SetPlayerRole(client, role);
		}

		public static PlayerData InitPlayerData(IClient client) 
		{
			if (FileSystem.Data.FileExists($"{client.SteamId}.json"))
			{
				LoadPlayerData(client);
				ClientsData[client].LastSeenDate = DateTime.Now;
				ClientsData[client].LastSeenName = client.Name;
			}
			else
			{
				PlayerData playerData = new PlayerData();
				playerData.Steamid = client.SteamId;
				playerData.LastSeenName = client.Name;
				playerData.Role = Role.Player;
				playerData.LastSeenDate = DateTime.Now;

				ClientsData[client] = playerData;
			}
			SavePlayerData(client);
			return ClientsData[client];
		}

		public static void LoadPlayerData(IClient client)
		{
			ClientsData[client] = FileSystem.Data.ReadJson<PlayerData>($"{client.SteamId}.json");
		}

		public static void SavePlayerData(IClient client)
		{
			if (!ClientsData.ContainsKey(client)) return;

			Log.Info($"Player '{client}' data saved!");
			FileSystem.Data.WriteJson($"{client.SteamId}.json", ClientsData[client]);
		}

		[GameEvent.Server.ClientJoined]
		public static void OnClientJoin(ClientJoinedEvent e)
		{
			var client = e.Client;
			var playerData = InitPlayerData(client);
			var now = DateTime.Now;

			if(playerData.BanEndTime > now)
			{
				client.Kick();
			}

			if(playerData.RoleEndTime > DateTime.MinValue && playerData.RoleEndTime < now)
			{
				playerData.Role = Role.Player;
				playerData.RoleEndTime = DateTime.MinValue;
				SavePlayerData(client);
			}
		}

		[GameEvent.Server.ClientDisconnect]
		public static void OnClientDisconnect(ClientDisconnectEvent e)
		{
			SavePlayerData(e.Client);
		}
	}
}
