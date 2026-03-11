using Godot;
using System;


[GlobalClass]
public partial class FishBase : Resource
{
    [Export] public string Name;
    [Export(PropertyHint.MultilineText)] public string Description;
    [Export] public Texture2D Icon;
    [Export] public ERarity Rarity;
}

public enum ERarity
{
    Common,
    Rare,
    Legendary
}
