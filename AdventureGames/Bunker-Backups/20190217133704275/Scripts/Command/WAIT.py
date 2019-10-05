# The WAIT command allows the player to wait around for a bit

def Execute(adventureCommand):
	if adventureCommand.Parameters.Count == 0:
		# Try the pre-process script for the current room and return its result
		preProcessResult = Preprocess(AWApi.GetCurrentLocation())
			
		if not preProcessResult:
			return False
			
		ConsoleApi.FormattedWrite("Time passes...Yawn...")
		return True

	return SentenceNotRecognised()
