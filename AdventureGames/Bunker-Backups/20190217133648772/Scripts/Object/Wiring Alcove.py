def Execute(adventureObject, adventureCommand):

	if adventureCommand.ParsedCommand.Word.upper() == "LOOK":
		ConsoleApi.FormattedWrite("Its too high to look at, you need something to stand on.")
		return False
		
	return True
	
