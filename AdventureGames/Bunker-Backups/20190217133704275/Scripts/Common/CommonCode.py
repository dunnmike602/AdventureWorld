# This script contains common code routines for all scripts in your game. It ships with code that is used by the built in commands.
# You can add your own routines or change behaviour of existing commands. Please note if you change the existing commands incorrectly they
# will likely not function correctly.
from AdventureLandCore.Domain import GameWord
from System.Collections.Generic import List
from AdventureLandCore.Domain import Container
from System import Char
from System import String
from System import Object
from System import Linq 
from AdventureLandCore.Domain import ParsedAdventureCommand
from System import Environment
from AdventureLandCore.Domain import WordType
from System.IO import Path
from AdventureLandCore.Domain import GlobalConstants

import clr
clr.AddReference("System.Core")
import System
clr.ImportExtensions(System.Linq)

originalCommandWord = ""
	
# some word constants
lookWord = "look"
roomWord = "room"
aroundWord = "around"
theWord = "the"
aWord = "a"
atWord = "at"
toWord = "to"
examineWord = "examine"
inspectWord = "inspect"
andWord = "and"
onWord = "on"
offWord = "off"
withWord = "with"
inWord = "in"
insideWord = "inside"
intoWord = "into"

exitWords = ["exit", "door"]
everythingWords = ["everything", "all"]

wildcardGameWordList = List[GameWord]()
wildcardGameWord = GameWord()
wildcardGameWord.Word = "*"
wildcardGameWordList.Add(wildcardGameWord)

CurrentLocation = AWApi.GetCurrentLocation()
FileNameMinLength = 3
FileNameMustBeCorrectLength = 'The filename must be at least {} characters.'
ThereIsNoFile = "Sorry I can't find that game."
InvalidGameVersion = "I cannot load this it does not match the current game version."
DontKnowWord = "I don't know the word \"{0}\"."
IsDarkText = "You can't see anything."

# Defines the limits of directions for move commands
MoveValueLimits = AWApi.GetMoveLimits()

def GetExitFromDirection(room, direction):
	# Apply abbreviations
	mappedDirection = AWApi.GetDirectionFromAbbreviation(direction)
	
	if not mappedDirection == None:
    	 direction = mappedDirection
	
	return AWApi.GetExitFromName(CurrentLocation.Name, direction)

def IsNullOrWhiteSpace(source):
	if not source:
		return True
	elif not source.strip():
		return True
	else:
		return False

def NameIsCorrectLength(source):
	if len(source) < FileNameMinLength:
		return False
	else:
		return True
		
def SentenceNotRecognised():
	ConsoleApi.FormattedWrite("That sentence isn't one I recognise.")
	
	return False
	
def Preprocess(targetObject):
 	return LanguageApi.ExecuteObjectScript(targetObject.PreProcessScript, targetObject, AWApi.LastExecutedCommand) 
 	
def ProcessFileOperationParametersForLength(adventureCommand):
	# One parameter not of the correct length specified
	if adventureCommand.Parameters.Count == 0 or (adventureCommand.Parameters.Count == 1 and not NameIsCorrectLength(adventureCommand.Parameters[0].OriginalWord)):
		ConsoleApi.FormattedWrite(FileNameMustBeCorrectLength.format(FileNameMinLength))
		return False
		
	return True
	
def NotNone(s,d):
    if s is None:
        return d
    else:
        return s
 
# This function is used to show the current score, both by the built-in score command and automatically when the game finishes.
# Change it here if you want to customise it
def ShowScoreHelper():
	if AWApi.GameData.EnableScore:
		ConsoleApi.FormattedWrite("Current Score is {0} out of {1}.".format(AWApi.GameData.CurrentScore, AWApi.GameData.MaximumScore))
	
	return
	
# Executes the specified move checking against the various allowable phrases
def ProcessTheMoveHelper(adventureCommand):
	
	nextMoveDirection = adventureCommand.Parameters[0].OriginalWord
	
	exit = GetExitFromDirection(CurrentLocation, nextMoveDirection)

	# We can't move this way
	if exit == None:
		ConsoleApi.FormattedWrite("I can't {0} {1}".format(originalCommandWord.lower(), nextMoveDirection))
		return False
		
	# We next check if the exit is locked (and not visible of course)
	if not CheckExitLocked(exit):
		return False
	
	originalCommand = adventureCommand.ParsedCommand.Word.upper()

	# We next check if the move type is supported through this exit
	canWalk = (exit.CanWalk and originalCommand== "WALK")
	canRun = (exit.CanRun and originalCommand == "RUN")
	canCrawl = (exit.CanCrawl and originalCommand == "CRAWL")
	canSwim = (exit.CanSwim and originalCommand == "SWIM")

	if not canWalk and not canRun and not canCrawl and not canSwim:
		ConsoleApi.FormattedWrite("Sorry you cannot {0} this way.".format(originalCommandWord))
		return False
		
	# Finally we can do the move
	AWApi.SetCurrentLocation(AWApi.GetRoomFromName(exit.RoomName))
			   	
	# Give the chance for customisations to run and bail out of default actions.
	preProcessResult = Preprocess(exit)
		
	if not preProcessResult:
		return False
		
	# Describe the exit if this feature toggle is enabled
	if AWApi.GameData.EnableExitDescriptions:
		ConsoleApi.FormattedWrite(exit.CurrentDescription)

	return True
	
# Peforms a generic Move operation, used by SWIM, RUN, WALK and CRAWL. Can be modified by an Exit pre-processing script
# to change the action depending on Move type. For example player can be made to SWIM a river.
def ExecuteGenericMove(adventureCommand):
	global originalCommandWord
	
	originalCommandWord = adventureCommand.ParsedCommand.OriginalWord.upper()

	# If the room is dark you certainly can't see the exits
	if CurrentLocation.IsDark:
		ConsoleApi.FormattedWrite("You can't see any exits!!")
		return False
		
	# Must specify a direction to move
	if adventureCommand.Parameters.Count == 0:
		ConsoleApi.FormattedWrite("Where do you want to {0} to?".format(originalCommandWord))
		return False
		
	# Must specify a known direction
	unknownItems = set(adventureCommand.GetOriginalWordsWithoutStopWords()).difference(set(MoveValueLimits))
	
	if unknownItems.Count > 0:
		ConsoleApi.FormattedWrite(DontKnowWord.format(unknownItems.pop()))
		return False

	# If we are in the current room we can process the sentence as the current room
	movePhrases = [ '{} {}'.format(originalCommandWord, adventureCommand.Parameters[0].OriginalWord)]
	
	if LanguageApi.CheckSentenceAgainstList(adventureCommand.JoinOriginalWordAndParameters(), movePhrases):
			return ProcessTheMoveHelper(adventureCommand)
	
	return SentenceNotRecognised()

def CheckExitLocked(exit):
	if exit.IsLocked and exit.Visible:
		ConsoleApi.FormattedWrite("Sorry the exit is locked.")
		return False

	return True

# Removed items from inventory one by one (can be THROW or DROP for example)
def ProcessObjectListForDrop(objectsToLeave, verb):
	for nextObject in objectsToLeave:
		itemIsAvailable = AWApi.IsItemAvailableToExamine(nextObject)
		
		if not itemIsAvailable or not nextObject.IsHeld:
			ConsoleApi.FormattedWrite("{0}: You are not holding that.".format(nextObject.Name))
		
		else:
			AWApi.Drop(nextObject)
			
			# Give the chance for customisations to run and bail out of default actions if required.
			preProcessResult = Preprocess(nextObject)
			
			if not preProcessResult:
				return False
				
			ConsoleApi.FormattedWrite("{0}: {1}.".format(nextObject.Name, verb))
		
	return True

# THE FOLLOWING SCRIPTS ARE USED BY THE DEFAULT LANGUAGE PROCESSOR - IF YOU REMOVE IT WILL STOP WORKING
# YOU CAN WRITE YOUR OWN SIMPLY BY REPLACING THE CODE IN PROCESS INPUT.
# IT IS PASSED A STRING CONTAINING THE LAST TEXT INPUT BY THE USER. IT MUST PROCESS IT AND RETURN THE 
# CORRECT ParsedAdventureCommand

def MoveAutoFollowNpcs():

	for npc in AWApi.GetAllAutoFollowNpcs():
		npc.Parent = AWApi.GetCurrentLocation()
		
	return
	
def ProcessInput(playerInputText):

	cleanedPlayerInputText = Clean(playerInputText)
	
	tokenList = Tokenise(cleanedPlayerInputText);
	    
	return CreateCommandFromSentence(tokenList, playerInputText)

def CreateCommandFromSentence(input, playerInputText):
	newParsedAdventureCommand = ParsedAdventureCommand();
	
	# Handle some simple commands directly
	if input.Count > 0:
		if ShowDebugInformation(input[0]) or ClearScreen(input[0]) or ControlLogging(playerInputText):
			return newParsedAdventureCommand
    
	adventureCommand = ConvertInputToCommand(input)
 
 	if (adventureCommand.IsValid):
 		newParsedAdventureCommand = adventureCommand
	else:
		ConsoleApi.FormattedWrite(adventureCommand.ErrorMessage)
              
	return newParsedAdventureCommand

def ConvertInputToCommand(inputTokens):
	# Firstly convert the sentence, represented by the list of words in the input. 
	# We can use an Api call to do this
	outputTokens = LanguageApi.ConvertSentence(inputTokens)
	
	# We can now convert this list of word tokens into a command and check it for validity
	return ConvertTokensToCommand(outputTokens)
    
def ConvertTokensToCommand(tokens):
	
	commandTypes = [WordType.Command, WordType.ReplacedCommand]

	parsedAdventureCommand = ParsedAdventureCommand()
	parsedAdventureCommand.IsValid = False
	
	commandCount = sum(1 for token in tokens if token.Type in commandTypes)

	# Check for validity
	if tokens.Count == 0:
		parsedAdventureCommand.ErrorMessage = "Pardon?"
	elif commandCount == 0:
		parsedAdventureCommand.ErrorMessage = "That sentence has no action I recognise.";
	elif (commandCount > 1 or (not tokens.First().Type in commandTypes) and not tokens.Last().Type in commandTypes):
		firstToken = tokens.First(lambda token: token.Type in commandTypes).OriginalWord.upper()
		parsedAdventureCommand.ErrorMessage = "You used the word '{0}' in a way that I don't understand.".format(firstToken)
	else:
		parsedAdventureCommand.IsValid = True
	
	for token in tokens:
		if token.Type in commandTypes:
			parsedAdventureCommand.ParsedCommand = token
			parsedAdventureCommand.CommandMapping = AWApi.Configuration.CommandMappings.First(lambda command: command.VerbName.upper() == token.Word.upper())
 		else:
 			parsedAdventureCommand.Parameters.Add(token)
                    
	return parsedAdventureCommand;
	

def ClearScreen(input):
	if input.upper() == "CLS":
		ConsoleApi.ClearScreen()
		return True

	return False

def ShowDebugInformation(input):
	if input.upper() == "DEBUG" and AWApi.IsDebugEnabled():
		ConsoleApi.WriteLine("ENVIRONMENT INFO:" + Environment.NewLine)
		ConsoleApi.WriteLine("Current Directory: " + Environment.CurrentDirectory)
		ConsoleApi.WriteLine("Save Game Directory: " + Path.Combine(Environment.CurrentDirectory, GlobalConstants.SaveGameDirectory))
		ConsoleApi.WriteLine("Path to game file: " + AWApi.Configuration.FullFilePath)
		ConsoleApi.WriteLine("Game Name: " + AWApi.Configuration.GameName)
		ConsoleApi.WriteLine("Start Room: " + AWApi.Configuration.StartRoom)
		ConsoleApi.WriteLine("Current Room: " + AWApi.GetCurrentLocation().Name)
		return True

	return False

def ControlLogging(playerInputText):
	input = playerInputText.split(' ', 1)

	if input[0].upper() == "LOGOFF" and AWApi.IsDebugEnabled():
		ConsoleApi.TurnLoggingOff();
		return True

	if input[0].upper() == "LOGON" and AWApi.IsDebugEnabled():
		if len(input) == 1:
			ConsoleApi.WriteLine("You must specify a file to start the logger")
			return True

		ConsoleApi.TurnLoggingOn(input[1]);
		return True

	return False

def Tokenise(sentence):
	return sentence.split() if sentence else List[String]()
        
def Clean(playerInputText):
	return StripPunctuation(playerInputText, True) if playerInputText else ""

def StripPunctuation(playerInputText, preserveQuotes):
	source = ""
	
	for chr in playerInputText:
		 if not Char.IsPunctuation(chr) or (preserveQuotes and chr == '"'):
		 	source += chr
    	
	return source
	
# Keyboard virtual keycodes
LBUTTON = 1
RBUTTON = 2
CANCEL = 3
MBUTTON = 4
XBUTTON1 = 5
XBUTTON2 = 6
BACK = 8
TAB = 9
CLEAR = 12
RETURN = 13 
SHIFT = 16
CONTROL = 17
MENU = 18
PAUSE = 19
CAPITAL = 20
HANGEUL = 21
HANGUL = 21 
KANA = 21
JUNJA = 23
FINAL = 24
HANJA = 25
KANJI = 25
ESCAPE = 27
CONVERT = 28
NONCONVERT = 29
ACCEPT = 30
MODECHANGE = 31
SPACE = 32
PRIOR = 33
NEXT = 34
END = 35
HOME = 36
LEFT = 37
UP = 38
RIGHT = 39
DOWN = 40
SELECT = 41
PRINT = 42
EXECUTE = 43
SNAPSHOT = 44
INSERT = 45
DELETE = 46
HELP = 47
VK_0 = 48
VK_1 = 49
VK_2 = 50
VK_3 = 51
VK_4 = 52
VK_5 = 53
VK_6 = 54
VK_7 = 55
VK_8 = 56
VK_9 = 57
VK_A = 65
VK_B = 66
VK_C = 67
VK_D = 68
VK_E = 69
VK_F = 70
VK_G = 71
VK_H = 72
VK_I = 73
VK_J = 74
VK_K = 75
VK_L = 76
VK_M = 77
VK_N = 78
VK_O = 79
VK_P = 80
VK_Q = 81
VK_R = 82
VK_S = 83
VK_T = 84
VK_U = 85
VK_V = 86
VK_W = 87
VK_X = 88
VK_Y = 89
VK_Z = 90
LWIN = 91
RWIN = 92
APPS = 93
SLEEP = 95
NUMPAD0 = 96
NUMPAD1 = 97
NUMPAD2 = 98
NUMPAD3 = 99
NUMPAD4 = 100
NUMPAD5 = 101
NUMPAD6 = 102
NUMPAD7 = 103
NUMPAD8 = 104
NUMPAD9 = 105
MULTIPLY = 106
ADD = 107
SEPARATOR = 108
SUBTRACT = 109
DECIMAL = 110
DIVIDE = 111
F1 = 112
F2 = 113
F3 = 114
F4 = 115
F5 = 116
F6 = 117
F7 = 118
F8 = 119
F9 = 120
F10 = 121
F11 = 122
F12 = 123
F13 = 124
F14 = 125
F15 = 126
F16 = 127
F17 = 128
F18 = 129
F19 = 130
F20 = 131
F21 = 132
F22 = 133
F23 = 134
F24 = 135
NUMLOCK = 144
SCROLL = 145
LSHIFT = 160
RSHIFT = 161
LCONTROL = 162
RCONTROL = 163
LMENU = 164
RMENU = 165
BROWSER_BACK = 166
BROWSER_FORWARD = 167
BROWSER_REFRESH = 168
BROWSER_STOP = 169
BROWSER_SEARCH = 170
BROWSER_FAVORITES = 171
BROWSER_HOME = 172
VOLUME_MUTE = 173
VOLUME_DOWN = 174
VOLUME_UP = 175
MEDIA_NEXT_TRACK = 176
MEDIA_PREV_TRACK = 177
MEDIA_STOP = 178
MEDIA_PLAY_PAUSE = 179
LAUNCH_MAIL = 180
LAUNCH_MEDIA_SELECT = 181
LAUNCH_APP1 = 182
LAUNCH_APP2 = 183
OEM_1 = 186
OEM_PLUS = 187
OEM_COMMA = 188
OEM_MINUS = 189
OEM_PERIOD = 190
OEM_2 = 191
OEM_3 = 192
OEM_4 = 219
OEM_5 = 220
OEM_6 = 221
OEM_7 = 222
OEM_8 = 223
OEM_102 = 226
PROCESSKEY = 229
PACKET = 231
ATTN = 246
CRSEL = 247
EXSEL = 248
EREOF = 249
PLAY = 250
ZOOM = 251
NONAME = 252
PA1 = 253
M_CLEAR = 254

