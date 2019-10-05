# The UNLOCK command allows the player to unlock rooms and exit

def UnlockObjectHelper(objectToUnlock):
	
	if not AWApi.IsItemAvailableToExamine(objectToUnlock):
		ConsoleApi.FormattedWrite("I don't see that here.")
		return False
	
	if not type(objectToUnlock) is Container:
		ConsoleApi.FormattedWrite("Try as you might it can't be done")
		return False
		
	if not objectToUnlock.IsLocked:
		ShowAlreadyUnlockedMessage()
		return False
	
	# Give the chance for customisations to run and bail out of default actions.
	preProcessResult = Preprocess(objectToUnlock)
				
	if not preProcessResult:
		return False
	
	objectToUnlock.IsLocked = False

	ShowUnlockedMessage()
		
	return True

def UnlockExitHelper(directionToUnlock):

	# See if the exit exists
	exit = GetExitFromDirection(CurrentLocation, directionToUnlock)

	# We can't move this way
	if exit == None:
		ConsoleApi.FormattedWrite("I can't see anything over there.")
		return False
		
	# Already unlocked
	if not exit.IsLocked:
		ShowAlreadyUnlockedMessage()
		return False
				
	# Give the chance for customisations to run and bail out of default actions.
	preProcessResult = Preprocess(exit)
	
	if not preProcessResult:
		return False
		
	exit.IsLocked = False

	ShowUnlockedMessage()
	
	return True
		
def ShowAlreadyUnlockedMessage():
	ConsoleApi.FormattedWrite("There is no point, it's already unlocked.")
	return
	
def ShowUnlockedMessage():
	ConsoleApi.FormattedWrite("Its unlocked, well done!!")
	return
	
def UnknownDirectionsHelper(invalidDirections):
	if any(invalidDirections):
		badDirection = invalidDirections.pop()
		ConsoleApi.FormattedWrite(DontKnowWord.format(badDirection.upper()))
		return False
		
	return True
	
def UnknownWordsHelper(invalidObjects):
	if any(invalidObjects):
		badObject = invalidObjects[0]
		ConsoleApi.FormattedWrite(DontKnowWord.format(badObject.Name.upper()))
		return False
		
	return True
	
def Execute(adventureCommand):
	global originalCommandWord
	
	originalCommandWord = adventureCommand.ParsedCommand.OriginalWord.upper()

	# No parameter supplied ask the question
	if adventureCommand.Parameters.Count == 0:
		ConsoleApi.FormattedWrite("What do you want to {0}?".format(originalCommandWord))
		return False
	
	unlockDirection = None
	objectToUnlock = None
	
	# Special case trying to open generic door or exit
	if ((adventureCommand.Parameters.Count == 1 and adventureCommand.Parameters[0].Word.lower() in exitWords)
	or (adventureCommand.Parameters.Count == 2 and adventureCommand.Parameters[0].Word.lower() == theWord
	and adventureCommand.Parameters[1].Word.lower() in exitWords)):
		unlockableExits = AWApi.GetCurrentLocation().GetUnlockableExits()
		
		if unlockableExits.Count == 1:
			return UnlockExitHelper(unlockableExits[0].Direction)
		elif unlockableExits.Count > 1:
			ConsoleApi.FormattedWrite("Which one?")
			return False
		else:
			ConsoleApi.FormattedWrite("There is nothing to unlock?")
			return False	
			
	if adventureCommand.GetParametersWithoutStopWords().Count > 1:
		directionList = List[String]()
		unlockDirection = adventureCommand.GetParametersWithoutStopWords()[1].Word
		directionList.Add(unlockDirection)
	
		unknownDirections = set(directionList).difference(set(MoveValueLimits))
	
		# Must specify a known direction
		if unknownDirections.Count > 0:
			ConsoleApi.FormattedWrite(DontKnowWord.format(unknownDirections.pop()))
			return False
	
	elif adventureCommand.GetParametersWithoutStopWords().Count > 0:
		objectList = List[GameWord]()
		objectList.Add(adventureCommand.GetParametersWithoutStopWords()[0])
		
		objects = AWApi.GetObjectsFromNames(objectList, 70.0, True)
	
		# Check for words we don't understand
		invalidObjects = list(filter(lambda object: not object.IsValid, objects))
		validObjects = list(filter(lambda object: object.IsValid, objects))
	
		if any(invalidObjects):
			ConsoleApi.FormattedWrite(DontKnowWord.format(invalidObjects[0].Name.upper()))
			return False

		objectToUnlock = validObjects[0]
		
	# Valid Unlock phrases
	currentUnlockObjectsPhrases = []
	
	if objectToUnlock <> None:
		currentUnlockObjectsPhrases.extend(['{} {}'.format(originalCommandWord, objectToUnlock.WordThatMatchedThis)])
		currentUnlockObjectsPhrases.extend(['{} {} {}'.format(originalCommandWord, theWord, objectToUnlock.WordThatMatchedThis)])

		currentUnlockObjectsPhrases.extend(['{} {}'.format(originalCommandWord, objectToUnlock.Name)])
		currentUnlockObjectsPhrases.extend(['{} {} {}'.format(originalCommandWord, theWord, objectToUnlock.Name)])
	
	elif unlockDirection <> None:
		for exitWord in exitWords:
			currentUnlockObjectsPhrases.extend(['{} {} {} {}'.format(originalCommandWord, theWord, exitWord, unlockDirection)])
			currentUnlockObjectsPhrases.extend(['{} {} {} {} {}'.format(originalCommandWord, exitWord, toWord, theWord, unlockDirection)])
			currentUnlockObjectsPhrases.extend(['{} {} {} {} {} {}'.format(originalCommandWord, theWord, exitWord, toWord, theWord, unlockDirection)])
			currentUnlockObjectsPhrases.extend(['{} {} {} {}'.format(originalCommandWord, exitWord, toWord, unlockDirection)])
			currentUnlockObjectsPhrases.extend(['{} {} {} {} {}'.format(originalCommandWord, theWord, exitWord, toWord, unlockDirection)])
			currentUnlockObjectsPhrases.extend(['{} {} {}'.format(originalCommandWord, exitWord, unlockDirection)])

	if LanguageApi.CheckSentenceAgainstList(adventureCommand.JoinOriginalWordAndParameters(), currentUnlockObjectsPhrases):
		# Are we unlocking an object or an exit
		if objectToUnlock <> None:
			# We can't unlock anything
			if validObjects.Count == 0:
				ConsoleApi.FormattedWrite("There isn't anything to unlock!")
				return True
			
			return UnlockObjectHelper(objectToUnlock)
		elif unlockDirection <> None:
			return UnlockExitHelper(unlockDirection)
		
	return SentenceNotRecognised()		
