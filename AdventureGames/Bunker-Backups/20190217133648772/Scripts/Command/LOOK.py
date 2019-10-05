# The LOOK command allows the player to examine rooms and objects
originalCommandWord = ""

def ShowRoomInformationHelper():
	# Give the chance for customisations to run and bail out of default actions if required.
	preProcessResult = Preprocess(CurrentLocation)
			
	if not preProcessResult:
		return False
		
	AWApi.ShowRoomInformation()
	
	return True
		
def ProcessPlaceableObjectHelper(objectToProcess, adventureCommand):
	matchedObjectName = objectToProcess.WordThatMatchedThis
	
	# Valid object phrases
	currentObjectPhrases = []
	
	for objectName in [objectToProcess.WordThatMatchedThis, objectToProcess.Name]:
		currentObjectPhrases.extend([ '{} {} {}'.format(lookWord, atWord, objectName), 
		'{} {} {} {}'.format(lookWord, atWord, theWord, objectName),
		'{} {} {}'.format(examineWord, theWord, objectName),
		'{} {}'.format(examineWord, objectName),
		'{} {} {}'.format(inspectWord, theWord, objectName),
		'{} {}'.format(inspectWord, objectName)])

	if not LanguageApi.CheckSentenceAgainstList(adventureCommand.JoinOriginalWordAndParameters(), currentObjectPhrases):
		return SentenceNotRecognised()
	
	if not AWApi.IsItemAvailableToExamine(objectToProcess):
		ConsoleApi.FormattedWrite("I don't see that here.")
		return False
		
	# Give the chance for customisations to run and bail out of default actions if required.
	preProcessResult = Preprocess(objectToProcess)
			
	if not preProcessResult:
		return False		
	
	# Describe the object
	ConsoleApi.FormattedWrite(objectToProcess.CurrentDescription)
	
	# If it is a container and open describe the object
	if type(objectToProcess) is Container and objectToProcess.IsOpen and AWApi.GetChildObjects(objectToProcess).Count > 0:
		ConsoleApi.FormattedWrite("Inside the {0}, you can see:".format(objectToProcess.Name))
		AWApi.ListChildObjectDescriptions(objectToProcess);
	
	return True
	
def Execute(adventureCommand):
	
	global originalCommandWord
	
	originalCommandWord = adventureCommand.ParsedCommand.OriginalWord.upper()
	
	# If the room is dark you certainly can't pick anything up
	if CurrentLocation.IsDark:
		ConsoleApi.FormattedWrite(IsDarkText)
		return True

	# Build a list of phrases that mean look at the current room
	lookAtRoomPhrases = [ lookWord , '{} {}'.format(lookWord, aroundWord), 
	'{} {} {}'.format(lookWord, aroundWord, roomWord), '{} {} {} {}'.format(lookWord, aroundWord, theWord, roomWord),
	'{} {} {}'.format(lookWord, atWord, roomWord), '{} {} {} {}'.format(lookWord, atWord, theWord, roomWord)]
	
	# If the command is look and there are no parameters or the parameter is a specific word we assume we are looking at the current room
	if LanguageApi.CheckSentenceAgainstList(adventureCommand.JoinOriginalWordAndParameters(), lookAtRoomPhrases):
		return ShowRoomInformationHelper()
	
	# If the command is not look and there are no parameters we need more information
	if originalCommandWord <> "LOOK" and adventureCommand.Parameters.Count == 0:
		ConsoleApi.FormattedWrite("What do you want to {0}?".format(originalCommandWord))
		return False

	# Do we match any rooms or object (we must ignore the stop words at the moment)
	rooms = AWApi.GetRoomsFromNames(adventureCommand.GetParametersWithoutStopWords())
	objects = AWApi.GetObjectsFromNames(adventureCommand.GetParametersWithoutStopWords())
	
	# Check for words we don't understand
	invalidRooms = list(filter(lambda room: not room.IsValid, rooms))
	invalidObjects = list(filter(lambda object: not object.IsValid, objects))
	
	# Check for words we don't understand
	if any(invalidObjects) and any(invalidRooms):
		badObject = NotNone(invalidObjects[0], invalidRooms[0])
		ConsoleApi.FormattedWrite("I don't know the word \"{0}\".".format(badObject.Name.upper()))
		return False
	
	# We have only stopwords and therefore a meaningless sentence
	if rooms.Count == 0 + objects.Count == 0:
		return SentenceNotRecognised()
	
	# This command cannot work on multiple objects/rooms
	validRooms = list(filter(lambda room: room.IsValid, rooms))
	validObjects = list(filter(lambda object: object.IsValid, objects))
	
	if validObjects.Count > 1:
		ConsoleApi.FormattedWrite("One thing at a time please!")
		return False
	
	if rooms.Count == 1 and AWApi.IsNameCurrentRoom(rooms[0].Name, 100.0):
		matchedRoomName = rooms[0].WordThatMatchedThis
		
		# If we are in the current room we can process the sentence as the current room
		currentRoomPhrases = [ '{} {} {}'.format(originalCommandWord, atWord, matchedRoomName), 
		'{} {} {} {}'.format(originalCommandWord, atWord, theWord, matchedRoomName),
		'{} {} {} {}'.format(originalCommandWord, aroundWord, theWord, matchedRoomName),
		'{} {} {}'.format(originalCommandWord, aroundWord, matchedRoomName)]
		
		if LanguageApi.CheckSentenceAgainstList(adventureCommand.JoinOriginalWordAndParameters(), currentRoomPhrases):
			return ShowRoomInformationHelper()
			
		return SentenceNotRecognised()

	# Process the examine command on the object selected (running the pre-process script of course)
	return ProcessPlaceableObjectHelper(validObjects[0], adventureCommand)

