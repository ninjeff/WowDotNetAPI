﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WowDotNetAPI.Explorers.Models
{
	public class Guild
	{
		public long lastModified { get; set; }
		public string name { get; set; }
		public string realm { get; set; }
		public int level { get; set; }
		public int side { get; set; }
		public int achievementPoints { get; set; }
		public Achievements achievements { get; set; }
		public IEnumerable<GuildMember> members { get; set; }
	}
}
