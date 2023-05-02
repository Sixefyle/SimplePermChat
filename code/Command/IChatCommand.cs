﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox;
using SimpleChat.Permission;

namespace SimpleChat.Command
{
	public interface IChatCommand
	{
		Privilege.Role RequiredRole { get; }
		
		void Execute(IClient sender, params string[] parameters);
	}
}
