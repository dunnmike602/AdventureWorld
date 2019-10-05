# The SAVE command allows the player to save the current game.

def Execute(adventureCommand):
	if not ProcessFileOperationParametersForLength(adventureCommand):
		return False

	# One file name parameter supplied the current game can be saved with this game
	if adventureCommand.Parameters.Count == 1:
		FileToSave = adventureCommand.Parameters[0].OriginalWord
		AWApi.Save(FileToSave, AWApi.SerializeToString());
		ConsoleApi.FormattedWrite("Game saved successfully.")
		return True
	
	# More than one file name parameter specified
	ConsoleApi.FormattedWrite("You must specify only one name for the Save command.")

	return False
