def Execute(adventureCommand):

	originalCommandWord = adventureCommand.ParsedCommand.OriginalWord.upper()
	
	# No parameter supplied ask the question
	if adventureCommand.Parameters.Count == 0:
		ConsoleApi.FormattedWrite("What do you want to {0}?".format(originalCommandWord))
		return False
	
	# If the room is dark you certainly can't hit anything
	if CurrentLocation.IsDark:
		ConsoleApi.FormattedWrite(IsDarkText)
		return True
		
	# Do we match any objects
	objects = AWApi.GetObjectsFromNames(adventureCommand.GetParametersWithoutStopWords())
	
	# Check for words we don't understand
	invalidObjects = list(filter(lambda object: not object.IsValid, objects))
	validObjects = list(filter(lambda object: object.IsValid, objects))
	
	if any(invalidObjects):
		ConsoleApi.FormattedWrite("I don't know the word \"{0}\".".format(invalidObjects[0].Name.upper()))
		return False
	
	# There must be two objects
	objectToHit = validObjects[0] if validObjects.Count >= 1 else None
	tool = validObjects[1] if validObjects.Count >= 2 else None
	
	# We can't hit anything
	if objectToHit == None:
		ConsoleApi.FormattedWrite("There isn't anything to {}!".format(originalCommandWord))
		return False
		
	# We need a tool
	if tool == None:
		ConsoleApi.FormattedWrite("You need something to use as a tool.")
		return False
		
	# We need a tool
	if tool.Name <> Hammer:
		ConsoleApi.FormattedWrite("You can't use the {} like that".format(tool.Name))
		return False

	# Valid hit Object phrases
	currentHitObjectsPhrases = []
	
	currentHitObjectsPhrases.extend(['{} {} {} {}'.format(originalCommandWord, objectToHit.WordThatMatchedThis, withWord, tool.WordThatMatchedThis)])
	currentHitObjectsPhrases.extend(['{} {} {} {} {} {}'.format(originalCommandWord, theWord, objectToHit.WordThatMatchedThis, withWord, theWord, tool.WordThatMatchedThis)])
	currentHitObjectsPhrases.extend(['{} {} {} {} {}'.format(originalCommandWord, objectToHit.WordThatMatchedThis, withWord, theWord, tool.WordThatMatchedThis)])
	currentHitObjectsPhrases.extend(['{} {} {} {} {}'.format(originalCommandWord, theWord, objectToHit.WordThatMatchedThis, withWord, tool.WordThatMatchedThis)])

	currentHitObjectsPhrases.extend(['{} {} {} {}'.format(originalCommandWord, objectToHit.Name, withWord, tool.Name)])
	currentHitObjectsPhrases.extend(['{} {} {} {} {} {}'.format(originalCommandWord, theWord, objectToHit.Name, withWord, theWord, tool.Name)])
	currentHitObjectsPhrases.extend(['{} {} {} {} {}'.format(originalCommandWord, objectToHit.Name, withWord, theWord, tool.Name)])
	currentHitObjectsPhrases.extend(['{} {} {} {} {}'.format(originalCommandWord, theWord, objectToHit.Name, withWord, tool.Name)])

	if LanguageApi.CheckSentenceAgainstList(adventureCommand.JoinOriginalWordAndParameters(), currentHitObjectsPhrases):
	
		if not AWApi.DoIHaveTheObject(tool.Name):
			ConsoleApi.FormattedWrite("You are not holding that at the moment.")
			return False
		
		if not AWApi.IsItemAvailableToExamine(objectToHit):
			ConsoleApi.FormattedWrite("You can't see that at the moment.")
			return False
	
		if not Preprocess(objectToHit):
			return False
		
		if not Preprocess(tool):
			return False
			
		return True
			
	return SentenceNotRecognised()
