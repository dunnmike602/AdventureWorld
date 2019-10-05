# The OPEN command allows the player to Open container objects.

originalCommandWord = ""

def ProcessPlaceableObjectHelper(objectToProcess):
	# Try the pre-process script for the current room and return its result
	preProcessResult = Preprocess(objectToProcess)
			
	if not preProcessResult:
		return False		

	return True
	
def OpenObjectHelper(adventureCommand):

	# Do we match any objects
	allObjects = AWApi.GetObjectsFromNames(adventureCommand.GetParametersWithoutStopWords())
	
	# Check for words we don't understand
	invalidObjects = list(filter(lambda object: not object.IsValid, allObjects))
	
	if not UnknownWordsHelper(invalidObjects):
		return False
	
	# At this point we must be trying to open a real object
	objectToOpen = allObjects[0]
	
	objectOpenPhrases = []
	
	for objectName in [objectToOpen.WordThatMatchedThis, objectToOpen.Name]:
		# Process allowable phrases
		objectOpenPhrases.extend([ '{} {}'.format(originalCommandWord, objectName), 
		'{} {} {}'.format(originalCommandWord, theWord, objectName)])
	
	if not LanguageApi.CheckSentenceAgainstList(adventureCommand.JoinOriginalWordAndParameters(), objectOpenPhrases):
		return SentenceNotRecognised()
			
	if not AWApi.IsItemAvailableToExamine(objectToOpen):
		ConsoleApi.FormattedWrite("I don't see that here")
		return False
		
	if not type(objectToOpen) is Container:
		ConsoleApi.FormattedWrite("Try as you might it doesn't seem to open.")
		return False
	
	if objectToOpen.IsLocked:
		ConsoleApi.FormattedWrite("Try unlocking it first.")
		return False
		
	if objectToOpen.IsOpen:
		ShowAlreadyOpenMessage()
		return False
			
	objectToOpen.IsOpen = True
				
	# Give the chance for customisations to run and bail out of default actions.
	preProcessResult = Preprocess(objectToOpen)
			
	if not preProcessResult:
		return False

	ShowOpenMessage(objectToOpen)
		
	return True
		
def ShowAlreadyOpenMessage():
	ConsoleApi.FormattedWrite("There is no point, it's already open.")
	return
	
def ShowOpenMessage(objectToOpen):
	ConsoleApi.FormattedWrite("The {0} is now open.".format(objectToOpen.Name))
	return

	
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
	if adventureCommand.GetParametersWithoutStopWords().Count == 0:
		ConsoleApi.FormattedWrite("What do you want to {0}?".format(originalCommandWord))
		return False
	
	# We can only open a single object 
	if adventureCommand.GetParametersWithoutStopWords().Count == 1:
		return OpenObjectHelper(adventureCommand)
		
	return SentenceNotRecognised()
