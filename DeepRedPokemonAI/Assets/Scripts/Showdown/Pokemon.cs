using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pokemon
{
	public class Move
	{
		public enum TargetType { Normal }

		public string Name;
		public string Id;
		public int Pp;
		public int MaxPp;
		public TargetType Target;
		public bool Disabled;
	}

	public enum Genders { Genderless, Male, Female }
	public enum StatusCondition { Normal }

	public string Position;
	public string Name;
	public string Species;
	public bool Shiny;
	public Genders Gender;
	public int Level;
	public int CurrentHp;
	public int MaxHp;
	public StatusCondition Condition;
	public int Attack;
	public int Defense;
	public int SpecialAttack;
	public int SpecialDefense;
	public int Speed;
	public Move[] Moves;
	public string BaseAbility;
	public string Item;
	public string Pokeball;
	public string Ability;
}
