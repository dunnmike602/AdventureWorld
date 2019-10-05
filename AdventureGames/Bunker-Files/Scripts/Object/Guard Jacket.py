def Execute(adventureObject, adventureCommand):

	if adventureCommand.ParsedCommand.Word.upper() == "LOOK" and AWApi.GetVariable("jacketTaken") and adventureObject.IsHeld:
		ConsoleApi.FormattedWrite("Now you have the jacket in your hand you can take a closer look. You notice it has a breast pocket closed by a flap.")
		return False
	
	if adventureCommand.ParsedCommand.Word.upper() == "LOOK" and not adventureObject.IsHeld:
		ConsoleApi.FormattedWrite("You will have to be holding it to take a closer look.")
		return False
		
	# Every time we take the jacket ensure we can see what's in it
	if adventureCommand.ParsedCommand.Word.upper() == "TAKE":
			adventureObject.IsOpen = True
	
	# Can't see whats in the jacket if we havn't got it
	if adventureCommand.ParsedCommand.Word.upper() == "DROP":
			adventureObject.IsOpen = False

	if adventureCommand.ParsedCommand.Word.upper() == "TAKE" and not AWApi.GetVariable("jacketTaken"):
		leg = AWApi.GetPlaceableObjectFromName(RustyLeg)
		
		if leg.IsHeld:
			ConsoleApi.FormattedWrite("With the bed leg in hand you manage to reach through the bars and snag the jacket - well done!")
			adventureObject.Visible = True
			adventureObject.IsHeld = True
			adventureObject.HideFromAutoList = False
			adventureObject.IsOpen = True
			AWApi.GetRoomFromName(JailCell).CurrentDescriptionIndex = 1
			AWApi.SetVariable("jacketTaken", True)
		elif not leg.Visible:
			ConsoleApi.FormattedWrite("It's just out of reach, if only you had something to grab it with.")
					
		return False
		
	return True
