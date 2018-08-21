using System;

public enum GameStates
{
	[StateScene("")]
	NullState = 0,

	[StateScene("Title")]
	Title,

	[StateScene("Main")]
	Main,


	//CustomizeAvatar,

	//[StateScene("CreateParty")]
	//CreateParty,

	//JoinParty,

	//[StateScene("VideoSearch")]
	//VideoSearch,

	[StateScene("Credits")]
	Credits
}
