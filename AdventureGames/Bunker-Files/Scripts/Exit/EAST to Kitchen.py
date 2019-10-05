def Execute(adventureObject, adventureCommand):
	
	if adventureCommand.CommandMapping.VerbName == "UNLOCK" and adventureObject.IsLocked:
		ConsoleApi.FormattedWrite("You can't open this door yourself.")
		return False
		
	return True
	
