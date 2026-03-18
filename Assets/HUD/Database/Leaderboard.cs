using Godot;
using Godot.Collections;
using System;
using System.Linq;

public partial class Leaderboard : Control
{
	[Export] VBoxContainer vBoxContainer;
	[Export] Button SubmitButton;
	[Export] TextEdit textEdit;

	public override void _Ready()
	{
		SubmitButton.Pressed += AddNewEntryToDatabase;
		var httpRequest = new HttpRequest();
		AddChild(httpRequest);
		httpRequest.RequestCompleted += onRequestComplete;
		Error error = httpRequest.Request("https://sea-life-7534c-default-rtdb.europe-west1.firebasedatabase.app/users.json");
		if (error != Error.Ok)
		{
			GD.PushError("An error occurred in the HTTP request.");
		}
	}

	private void onRequestComplete(long result, long responseCode, string[] headers, byte[] body)
	{
		Json json = new Json();
		json.Parse(body.GetStringFromUtf8());
		var response = json.GetData().AsGodotDictionary();
		GD.Print(response);
		foreach (Dictionary users in response.Values)
		{
			var arr = users.Values.ToArray();
			for (int i = 0; i < users.Values.Count / 3; i++)
			{
				CreateEntry(arr[i], arr[i + 1], arr[i + 2]);
			}

		}
	}


	// Creates an entry on the leaderboard
	public void CreateEntry(Variant name, Variant level, Variant money)
	{
		Label label = new Label
		{
			Text = "  Name: " + name.ToString() + " | Level: " + level.ToString() + " | Money: " + money.ToString()
		};
		vBoxContainer.AddChild(label);
	}

	private void AddNewEntryToDatabase()
	{
		Json json = new Json();
		var httpRequest = new HttpRequest();
		AddChild(httpRequest);
		var PlayerName = textEdit.Text;
		Player player = GetTree().GetNodesInGroup("Player")[0] as Player;
		var PlayerLevel = player.playerStats.PlayerLevel;
		var PlayerMoney = player.playerStats.Money;

		var PlayerData = new Dictionary
		{
			{"user", PlayerName},
			{"level", PlayerLevel},
			{"money", PlayerMoney}
		};
		string str = Json.Stringify(PlayerData);
		httpRequest.Request("https://sea-life-7534c-default-rtdb.europe-west1.firebasedatabase.app/users.json", [], HttpClient.Method.Post, str);

	}
}
