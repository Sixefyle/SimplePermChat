using Sandbox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SimpleChat.Permission.Privilege;

namespace SimpleChat.Permission
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
		}

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

		public static void InitPlayerData(IClient client) 
		{
			if (FileSystem.Data.FileExists($"{client.SteamId}.json"))
			{
				LoadPlayerData(client);
			}
			else
			{
				PlayerData playerData = new PlayerData();
				playerData.Steamid = client.SteamId;
				playerData.LastSeenName = client.Name;
				playerData.Role = Role.Admin;
				playerData.LastSeenDate = DateTime.Now;

				ClientsData[client] = playerData;
				SavePlayerData(client);
			}
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
			InitPlayerData(e.Client);
		}

		[GameEvent.Server.ClientDisconnect]
		public static void OnClientDisconnect(ClientDisconnectEvent e)
		{
			SavePlayerData(e.Client);
		}
	}
}
