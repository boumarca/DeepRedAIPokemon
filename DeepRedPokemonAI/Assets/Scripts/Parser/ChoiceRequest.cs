using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ChoiceRequest
{
	[System.Serializable]
	public struct Active
	{
		[System.Serializable]
		public struct Moves
		{
			public string move;
			public string id;
			public int pp;
			public int maxpp;
			public string target;
			public bool disabled;
		}

		public Moves[] moves;
	}

	[System.Serializable]
	public struct Side
	{
		[System.Serializable]
		public struct Pokemon
		{
			[System.Serializable]
			public struct Stats
			{
				public int atk;
				public int def;
				public int spa;
				public int spd;
				public int spe;
			}

			public string ident;
			public string details;
			public string condition;
			public bool active;
			public Stats stats;
			public string[] moves;
			public string baseAbility;
			public string item;
			public string pokemon;
			public string ability;
		}

		public string name;
		public string id;
		public Pokemon[] pokemon;
	}
	
	public Active[] active;
	public Side side;
	public int rqid;


	public static ChoiceRequest Parse(string json)
	{
		ChoiceRequest request = JsonUtility.FromJson<ChoiceRequest>(json);
		return request;
	}
}
