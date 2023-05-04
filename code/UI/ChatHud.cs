using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox;
using Sandbox.UI;

namespace SimplePermChat.UI
{
	public class ChatHud : HudEntity<RootPanel>
	{
		public ChatHud() 
		{
			if (!Game.IsClient) return;

			RootPanel.StyleSheet.Load("/UI/Styles/chat.scss");

			RootPanel.AddChild<Chat>();
		}
	}
}
