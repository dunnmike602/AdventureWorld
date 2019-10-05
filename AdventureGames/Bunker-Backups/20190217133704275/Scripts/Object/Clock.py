def Execute(adventureObject, adventureCommand):

	if adventureCommand.ParsedCommand.Word.upper() == "LOOK":
		ConsoleApi.FormattedWrite(adventureObject.Descriptions[1])
		return False
		
	if adventureCommand.ParsedCommand.Word.upper() == "TAKE":
		ConsoleApi.FormattedWrite("With a huge effort you move it a bit, if only you had something to hit it with.")
		return False
	
	if adventureCommand.ParsedCommand.Word.upper() == "HIT":
		
		if not AWApi.GetVariable("clockBashed"):
			ConsoleApi.FormattedWrite("With a mighty blow you smash the clock with the hammer. It doesn't come off completely but behind it you can see a alcove in the wall.")
			adventureObject.CurrentDescriptionIndex = 2
			AWApi.GetPlaceableObjectFromName(Alcove).Visible = True
			AWApi.SetVariable("clockBashed", True)
		else:
			ConsoleApi.FormattedWrite("You've done that once, no need for more violence!!")
			
		return False

	return True
