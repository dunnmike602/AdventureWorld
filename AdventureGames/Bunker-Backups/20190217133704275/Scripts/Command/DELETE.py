# The DELETE command allows you to delete a previous save game

def Execute(adventureCommand):
	if not ProcessFileOperationParametersForLength(adventureCommand):
		return False

	# Parameter is of the correct the file can be deleted
	if adventureCommand.Parameters.Count == 1:
		FileToDelete = adventureCommand.Parameters[0].OriginalWord
		
		gameText = AWApi.LoadSaveGame(FileToDelete);
		
		# This file does not exist
		if gameText == None:
			ConsoleApi.FormattedWrite(ThereIsNoFile);
			return False
			
		AWApi.DeleteSaveGame(FileToDelete);
		ConsoleApi.FormattedWrite("Save game successfully deleted.")
		return True
	
	return SentenceNotRecognised()
