using System;

public enum GameStates
{
	[StateScene("")]
	NullState = 0,

	[StateScene("Title")]
	Title,

	[StateScene("Main")]
	Main,

	[StateScene("GameOver")]
	GameOver,

	[StateScene("HighScore")]
	HighScore,

	[StateScene("Credits")]
	Credits
}
