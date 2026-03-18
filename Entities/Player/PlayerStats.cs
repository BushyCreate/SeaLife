using Godot;
using System;

public partial class PlayerStats : Node
{
	public int Money = 0;
	public int Fish = 0;
	public int MaxFish = 10;
	public int PlayerLevel = 1;
	[Export] HUDManager hudManager;


	public void AddMoney(int Count)
	{
		Money += Count;
		hudManager.MoneyCount.Text = Money.ToString() + "  "; // Extra padding;
	}
	public void RemoveMoney(int Count)
	{
		Money -= Count;
		hudManager.MoneyCount.Text = Money.ToString() + "  "; // Extra padding;		
	}
	public bool AddFish()
	{
		if (!(Fish >= 10 * PlayerLevel))
		{
			Fish += PlayerLevel;
			hudManager.FishCount.Text = Fish.ToString() + "/" + (MaxFish * PlayerLevel).ToString() + "  "; // Extra padding.
			return true;
		}
		return false;
	}

	public void DeleteFish()
	{
		Fish = 0;
		hudManager.FishCount.Text = Fish.ToString() + "/" + (MaxFish * PlayerLevel).ToString() + "  "; // Extra padding.
	}

	public void IncreaseLevel()
	{
		PlayerLevel++;
		hudManager.FishCount.Text = Fish.ToString() + "/" + (MaxFish * PlayerLevel).ToString() + "  "; // Extra padding.
	}
}
