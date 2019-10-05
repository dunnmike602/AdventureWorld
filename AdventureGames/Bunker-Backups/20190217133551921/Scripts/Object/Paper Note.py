def Execute(adventureObject, adventureCommand):

	if adventureCommand.ParsedCommand.Word.upper() == "READ":
		ConsoleApi.FormattedWrite(adventureObject.Descriptions[1])
		return False
		
	return True
