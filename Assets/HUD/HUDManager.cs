using Godot;
using System;

public partial class HUDManager : Control
{
    [Export] private Label MoneyCount;
    [Export] private Label FishCount;
    public int Money = 0;
    public int Fish = 0;
    public int MaxFish = 10;

    public void AddMoney(int Count)
    {
        Money += Count;
        MoneyCount.Text = Money.ToString() + "  "; // Extra padding;
    }
    
    public bool AddFish()
    {
        if (!(Fish >= 10))
        {
            Fish++;
            FishCount.Text = Fish.ToString() + "/" + MaxFish.ToString() + "  "; // Extra padding.
            return true;
        }
        return false;
    }

    public void DeleteFish()
    {
        Fish = 0;
        FishCount.Text = Fish.ToString() + "/" + MaxFish.ToString() + "  "; // Extra padding.
    }
}
