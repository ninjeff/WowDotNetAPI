﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WowDotNetAPI.Explorers.Models
{
	public class Character
	{
		public string lastModified { get; set; }
		public string name { get; set; }
		public string realm { get; set; }
		public int @class { get; set; }
		public int race { get; set; }
		public int gender { get; set; }
		public int level { get; set; }
		public int achievementPoints { get; set; }
		public string thumbnail { get; set; }
		public CharacterGuild guild { get; set; }
		public CharacterEquipment items { get; set; }
		public CharacterStats stats { get; set; }
		public ProfessionList professions { get; set; }
		public IEnumerable<Reputation> reputation { get; set; }
		public IEnumerable<CharacterTitle> titles { get; set; }
		public Achievements achievements { get; set; }
		public IEnumerable<CharacterTalentSpecialization> talents { get; set; }
		public CharacterAppearance appearance { get; set; }
		public int[] mounts { get; set; }
		public int[] companions { get; set; }
		public Progression progression { get; set; }
	}
}
