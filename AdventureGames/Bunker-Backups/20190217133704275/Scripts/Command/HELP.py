# The HELP command allows the player to get some help

def Execute(adventureCommand):
	if adventureCommand.Parameters.Count == 0:
		ConsoleApi.WriteLine(AWApi.GetDefaultHelpText())
		return True

	return SentenceNotRecognised()

