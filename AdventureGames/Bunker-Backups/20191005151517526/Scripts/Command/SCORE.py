# The SCORE command allows the player to check there current score

def Execute(adventureCommand):
	if adventureCommand.Parameters.Count > 0:
		return SentenceNotRecognised()
	
	ShowScoreHelper()
	return True	
	