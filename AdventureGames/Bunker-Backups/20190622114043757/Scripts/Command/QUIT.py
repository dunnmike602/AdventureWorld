# The QUIT command allows the player to exit the game.

from System.Globalization import CultureInfo

def Execute(adventureCommand):
	if adventureCommand.Parameters.Count == 0:
		ConsoleApi.FormattedWrite("Are you sure?")
		
		if ConsoleApi.ReadLine(AWApi.GameData.CommandPromptText).StartsWith("Y", True, CultureInfo.InvariantCulture):
			AWApi.QuitGame()
		else:
			ConsoleApi.FormattedWrite("Ok please carry on?")

		return False

	return SentenceNotRecognised()
