# The SWITCH command allows the player to switch things on
from System.Collections.Generic import List
from System import String

originalCommandWord = ""
	
def SetObjectToLightState(objectToLight, state):
	# Can't set to the same state
	if (objectToLight.IsLit and state == "on") or (not objectToLight.IsLit and state == "off"):
		ConsoleApi.FormattedWrite("The {0} is already {1}.".format(objectToLight.Name, state))
	else:
		ConsoleApi.FormattedWrite("Success!!")
		objectToLight.IsLit = True if state == "on" else False
	
		# Give the chance for customisations to run and bail out of default actions if required.
		preProcessResult = Preprocess(objectToLight)
			
		if not preProcessResult:
			return False
			
	return True
	
def Execute(adventureCommand):
	
	global originalCommandWord
	
	originalCommandWord = adventureCommand.ParsedCommand.OriginalWord.upper()
	
	# If the room is dark you certainly can't pick anything up
	if CurrentLocation.IsDark:
		ConsoleApi.FormattedWrite("You can't see anything!!")
		return True
		
	# No parameter supplied ask the question
	if adventureCommand.Parameters.Count == 0:
		ConsoleApi.FormattedWrite("What do you want to {0}?".format(originalCommandWord))
		return False
		
	# Do we match any objects
	objects = AWApi.GetObjectsFromNames(adventureCommand.GetParametersWithoutStopWords(), 70.0, True)
	
	# Check for words we don't understand
	invalidObjects = list(filter(lambda object: not object.IsValid, objects))
	validObjects = list(filter(lambda object: object.IsValid, objects))
	
	# Words we don't know
	if any(invalidObjects):
		ConsoleApi.FormattedWrite("I don't know the word \"{0}\".".format(invalidObjects[0].Name.upper()))
		return False

	# Allowable phrases
	objectSwitchPhrases = []
	objectToSwitch = validObjects[0]
	
	for objectName in [objectToSwitch.WordThatMatchedThis, objectToSwitch.Name]:
		# Process allowable phrases
		objectSwitchPhrases.extend([ '{} {} {}'.format(originalCommandWord, objectName, onWord), 
		'{} {} {} {}'.format(originalCommandWord, theWord, objectName, onWord),
		'{} {} {}'.format(originalCommandWord, objectName, offWord), 
		'{} {} {} {}'.format(originalCommandWord, theWord, objectName, offWord)])

	if not LanguageApi.CheckSentenceAgainstList(adventureCommand.JoinOriginalWordAndParameters(), objectSwitchPhrases):
		return SentenceNotRecognised()
	
	# Nothing to light or the room is dark or the object is not visible
	# NOTE this behavior can be changed in the pre-process scripts to customise light behavior or
	# directly in the command if required.
	if validObjects.Count == 0 or not validObjects[0].Visible:
		ConsoleApi.FormattedWrite("You can't see that here.")
		return False
	
	if (validObjects.Count > 0 and not validObjects[0].IsLightSource):
		ConsoleApi.FormattedWrite("You can't do that the {}.".format(validObjects[0]))
		return False
		
	onOrOff = onWord if adventureCommand.GetWords().IndexOf(onWord.upper()) > 0 else offWord

	# We can attempt the light operation
	return SetObjectToLightState(validObjects[0], onOrOff)
	
	
