# The LIST command shows all available games that have been saved
# Note this command is only every run on its own and so always returns false to stop command processing
def Execute(adventureCommand):
	if adventureCommand.Parameters.Count == 0:
		# Get a list of save games
		saveGames = AWApi.ListSaveGames()
		
		if saveGames.Count == 0:
			ConsoleApi.FormattedWrite("There are no saved games.")	
			return False
			
		# Loop through them and display each one
		for file in saveGames:
			ConsoleApi.WriteLine(file)
			
		return True

	return SentenceNotRecognised()
