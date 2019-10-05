def Execute(adventureObject, adventureCommand):

	if adventureCommand.ParsedCommand.Word.upper() == "LOOK":
		leg = AWApi.GetPlaceableObjectFromName(RustyLeg)
		leg.Visible = True
		leg.Fixed = False
		ConsoleApi.FormattedWrite("As you search the bed you notice that one of the legs is a bit loose. If you really wanted to you could probably take it off.")
		return False
		
	return True
	
