def Execute(adventureObject, adventureCommand):
	
	if adventureCommand.CommandMapping.VerbName == "UNLOCK" and adventureObject.IsLocked:
		ConsoleApi.FormattedWrite("If you have the code you can type it on the keypad.")
		code = ConsoleApi.ReadAlphaNumericString(4, "?" )
		Console.WriteLine()
	
		if code == AWApi.GetVariable("jailDoorCode"):
			ConsoleApi.FormattedWrite("The keypad flashes green and with a whoosh of hydraulic levers the door slides smoothly open.")
			adventureObject.IsLocked = False
			AWApi.GetRoomFromName(JailCell).CurrentDescriptionIndex = 2
		else:
			ConsoleApi.FormattedWrite("The keypad flashes red, that is clearly the wrong code.")
			
		return False
		
	return True 
