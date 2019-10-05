def Execute(adventureObject, adventureCommand):
	
	# Robot not activated
	if adventureCommand.ParsedCommand.Word.upper() == "TALK" and not AWApi.GetVariable("robotOn"):
		ConsoleApi.FormattedWrite("Silence..the robot seems to be deactivated.")
		return False
		
	# Password not entered
	if adventureCommand.ParsedCommand.Word.upper() == "TALK" and not AWApi.GetVariable("passWordEntered"):
	
		if (AWApi.GetVariable("passWordTries") > 3):
			ConsoleApi.FormattedWrite("Sorry your access is suspended.")
			return False
			
		ConsoleApi.FormattedWrite("Greetings I am the Cyber systems Concierge Robot Model 5656. Please input your operator password.")
		pwdLen = len(AWApi.GetVariable("robotPassword"))
		code = ConsoleApi.ReadAlphaNumericString(pwdLen, "?" )
		Console.WriteLine()
	
		if code.lower() == AWApi.GetVariable("robotPassword").lower():
			AWApi.SetVariable("passWordEntered", True)
			ConsoleApi.FormattedWrite("Password is correct. Thank you.")
			adventureObject.SetCurrentConversation("Conversation1")
			return True
		else:
			AWApi.SetVariable("passWordTries", AWApi.GetVariable("passWordTries") + 1)
			triesLeft = 3 -  AWApi.GetVariable("passWordTries")
			ConsoleApi.FormattedWrite("Incorrect pasword, you have {} more tries.".format(triesLeft))
			return False
	
	if adventureCommand.ParsedCommand.Word.upper() == "LOOK" and not AWApi.GetVariable("robotOn"):
		ConsoleApi.FormattedWrite("The robot is silent and looks to be deactivated. As you look closer you see there is an empty cylinder shaped alcove in the robot's back.")
		return False
	
	if adventureCommand.ParsedCommand.Word.upper() == "LOOK" and AWApi.GetVariable("robotOn"):
		ConsoleApi.FormattedWrite("The robot's face lights blink on and off.")
		return False
		
	if adventureCommand.ParsedCommand.Word.upper() == "PLACE":
	 	AWApi.SetVariable("robotOn", True)
	 	adventureObject.CurrentDescriptionIndex = 1
	 	AWApi.GetPlaceableObjectFromName(Battery).Visible = False
		ConsoleApi.FormattedWrite("As you place the battery in the Robot, the lights on the front start to flicker.")
		ConsoleApi.FormattedWrite("An thin electronic voice says: 'CYBER SYSTEMS Concierge Robot Model 5656 starting up.'")
		return False
		
	return True
	
