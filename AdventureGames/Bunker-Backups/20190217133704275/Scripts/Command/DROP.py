# The DROP command allows the player to leave objects lying around

def Execute(adventureCommand):
	
	global originalCommandWord
	
	originalCommandWord = adventureCommand.ParsedCommand.OriginalWord.upper()

	if adventureCommand.Parameters.Count == 0:
		ConsoleApi.FormattedWrite("What do you want to {0}?".format(originalCommandWord))
		return False
		
	# Do we match any objects, first check to see if we are asking for anything
	parametersToCheck = wildcardGameWordList if adventureCommand.Parameters[0].Word.lower() in everythingWords else adventureCommand.GetParametersWithoutStopWords()
	objects = AWApi.GetObjectsFromNames(parametersToCheck, 70.0, True)

	# Check for words we don't understand
	invalidObjects = list(filter(lambda object: not object.IsValid, objects))
	validObjects = list(filter(lambda object: object.IsValid, objects))
	
	if any(invalidObjects):
		ConsoleApi.FormattedWrite(DontKnowWord.format(invalidObjects[0].Name.upper()))
		return False
		
	# Valid Drop Object phrases
	currentDropObjectsPhrases = []
	
	# Build phrases for objects to be dropped based on the word that matched them
	phrase1 = originalCommandWord + " "
	phrase2 = originalCommandWord + " "
	
	for i in range(len(validObjects)):
		actualAndWord = andWord if i < len(validObjects) - 1 else ""
		phrase1 = phrase1 + '{} {} '.format(validObjects[i].WordThatMatchedThis, actualAndWord)
		phrase2 = phrase2 + '{} {} {} '.format(theWord, validObjects[i].WordThatMatchedThis, actualAndWord)
		
	currentDropObjectsPhrases.extend([ phrase1.strip(), phrase2.strip()])
	
	# Build phrases for objects to be dropped based on their actual names
	phrase1 = originalCommandWord + " "
	phrase2 = originalCommandWord + " "
		
	for i in range(len(validObjects)):
		actualAndWord = andWord if i < len(validObjects) - 1 else ""
		phrase1 = phrase1 + '{} {} '.format(validObjects[i].Name, actualAndWord)
		phrase2 = phrase2 + '{} {} {} '.format(theWord, validObjects[i].Name, actualAndWord)
			
	currentDropObjectsPhrases.extend([ phrase1.strip(), phrase2.strip()])
	
	# Add "all" words to support DROP ALL
	for word in everythingWords:
		currentDropObjectsPhrases.extend(['{} {}'.format(originalCommandWord, word)])
	
	if LanguageApi.CheckSentenceAgainstList(adventureCommand.JoinOriginalWordAndParameters(), currentDropObjectsPhrases):
		# We can't drop anything
		if validObjects.Count == 0:
			ConsoleApi.FormattedWrite("There isn't anything to drop!")
			return True
	
		return ProcessObjectListForDrop(validObjects, "dropped")
	
	return SentenceNotRecognised()
