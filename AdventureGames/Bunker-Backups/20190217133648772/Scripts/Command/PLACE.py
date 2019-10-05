def Execute(adventureCommand):

	originalCommandWord = adventureCommand.ParsedCommand.OriginalWord.upper()
	
	# No parameter supplied ask the question
	if adventureCommand.Parameters.Count == 0:
		ConsoleApi.FormattedWrite("What do you want to {0}?".format(originalCommandWord))
		return False
	
	# If the room is dark you certainly can't place anything
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
	objectToPlace = validObjects[0] if validObjects.Count >= 1 else None
	objectToBePlaced = validObjects[1] if validObjects.Count >= 2 else None
	
	# We can't place anything
	if objectToPlace == None:
		ConsoleApi.FormattedWrite("{} what?".format(originalCommandWord))
		return False
		
	# We need a something to be places
	if objectToBePlaced == None:
		ConsoleApi.FormattedWrite("Where do you want to {} it?".format(originalCommandWord))
		return False
		
	# We need the right thing
	if objectToPlace.Name <> Battery:
		ConsoleApi.FormattedWrite("You can't use the {} like that".format(objectToBePlaced.Name))
		return False

	# Valid hit Object phrases
	currentHitObjectsPhrases = []
	
	currentHitObjectsPhrases.extend(['{} {} {} {}'.format(originalCommandWord, theWord, objectToPlace.WordThatMatchedThis, inWord, theWord, objectToBePlaced.WordThatMatchedThis)])
	currentHitObjectsPhrases.extend(['{} {} {} {}'.format(originalCommandWord, objectToPlace.WordThatMatchedThis, inWord, theWord, objectToBePlaced.WordThatMatchedThis)])
	currentHitObjectsPhrases.extend(['{} {} {} {}'.format(originalCommandWord, objectToPlace.WordThatMatchedThis, inWord, objectToBePlaced.WordThatMatchedThis)])
	currentHitObjectsPhrases.extend(['{} {} {} {}'.format(originalCommandWord, theWord, objectToPlace.WordThatMatchedThis, insideWord, theWord, objectToBePlaced.WordThatMatchedThis)])
	currentHitObjectsPhrases.extend(['{} {} {} {}'.format(originalCommandWord, objectToPlace.WordThatMatchedThis, insideWord, theWord, objectToBePlaced.WordThatMatchedThis)])
	currentHitObjectsPhrases.extend(['{} {} {} {}'.format(originalCommandWord, objectToPlace.WordThatMatchedThis, insideWord, objectToBePlaced.WordThatMatchedThis)])
	currentHitObjectsPhrases.extend(['{} {} {} {}'.format(originalCommandWord, theWord, objectToPlace.WordThatMatchedThis, intoWord, theWord, objectToBePlaced.WordThatMatchedThis)])
	currentHitObjectsPhrases.extend(['{} {} {} {}'.format(originalCommandWord, objectToPlace.WordThatMatchedThis, intoWord, theWord, objectToBePlaced.WordThatMatchedThis)])
	currentHitObjectsPhrases.extend(['{} {} {} {}'.format(originalCommandWord, objectToPlace.WordThatMatchedThis, intoWord, objectToBePlaced.WordThatMatchedThis)])

	currentHitObjectsPhrases.extend(['{} {} {} {}'.format(originalCommandWord, theWord, objectToPlace.Name, inWord, theWord, objectToBePlaced.Name)])
	currentHitObjectsPhrases.extend(['{} {} {} {}'.format(originalCommandWord, objectToPlace.Name, inWord, theWord, objectToBePlaced.Name)])
	currentHitObjectsPhrases.extend(['{} {} {} {}'.format(originalCommandWord, objectToPlace.Name, inWord, objectToBePlaced.Name)])
	currentHitObjectsPhrases.extend(['{} {} {} {}'.format(originalCommandWord, theWord, objectToPlace.Name, insideWord, theWord, objectToBePlaced.Name)])
	currentHitObjectsPhrases.extend(['{} {} {} {}'.format(originalCommandWord, objectToPlace.Name, insideWord, theWord, objectToBePlaced.Name)])
	currentHitObjectsPhrases.extend(['{} {} {} {}'.format(originalCommandWord, objectToPlace.Name, insideWord, objectToBePlaced.Name)])
	currentHitObjectsPhrases.extend(['{} {} {} {}'.format(originalCommandWord, theWord, objectToPlace.Name, intoWord, theWord, objectToBePlaced.Name)])
	currentHitObjectsPhrases.extend(['{} {} {} {}'.format(originalCommandWord, objectToPlace.Name, intoWord, theWord, objectToBePlaced.Name)])
	currentHitObjectsPhrases.extend(['{} {} {} {}'.format(originalCommandWord, objectToPlace.Name, intoWord, objectToBePlaced.Name)])

	if LanguageApi.CheckSentenceAgainstList(adventureCommand.JoinOriginalWordAndParameters(), currentHitObjectsPhrases):
	
		if not AWApi.DoIHaveTheObject(objectToPlace.Name):
			ConsoleApi.FormattedWrite("You are not holding that at the moment.")
			return False
		
		if not AWApi.IsItemAvailableToExamine(objectToBePlaced):
			ConsoleApi.FormattedWrite("You can't see that at the moment.")
			return False
			
		if not Preprocess(objectToPlace):
			return False
		
		if not Preprocess(objectToBePlaced):
			return False
			
		return True
			
	return SentenceNotRecognised()
