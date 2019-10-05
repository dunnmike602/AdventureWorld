# The TALK command allows the player to interact with NPC's (Non-Player Characters).
from AdventureLandCore.Domain import ConversationStage

# Process and entire conversation
def ProcessConversation(npc):
	
	currentConversation = npc.GetCurrentConversation()
	
	# Run the start conversation script (this happens only once)
	if not LanguageApi.ExecuteConversationScript(currentConversation.ConversationPreprocessScript, npc, currentConversation, ConversationStage.Start):
		return False
		
	inConversation = True
	
	currentConversationText = currentConversation.ConversationText
	
	while (inConversation):
 		if currentConversationText != None:
 			
 			# Execute the conversation text script
 			if not LanguageApi.ExecuteConversationScript(currentConversationText.ConversationPreprocessScript, npc, currentConversationText, ConversationStage.NpcSpeaks):
				return False
 			
 			ConsoleApi.FormattedWrite(currentConversationText.Text)
  			ConsoleApi.WriteLine("")
  			
  			if (currentConversationText.ConversationResponses):
				nextResponse = ProcessConversationResponses(currentConversation.ConversationText.ConversationResponses, npc);
				if nextResponse <> None and nextResponse.ConversationText <> None:
					currentConversationText = nextResponse.ConversationText
				else:
					inConversation = False
			else:
				inConversation = False
		else:
			inConversation = False
			
	return True

def ProcessConversationResponses(responses, npc):

	if not responses:
		return False
		
	sortedResponses = sorted(responses, key=lambda x: x.SortOrder)
	
	for index in range(len(sortedResponses)):
	
		# Execute the conversation text script
 		if not LanguageApi.ExecuteConversationScript(sortedResponses[index].ConversationPreprocessScript, npc, sortedResponses[index], ConversationStage.PlayerResponds):
			return None
 			
		ConsoleApi.FormattedWrite('{}. {}'.format(index+1, sortedResponses[index].Response))
	
	escapeText = currentConversation = npc.GetCurrentConversation().EscapeText
	
	if escapeText:
		ConsoleApi.WriteLine("")
		ConsoleApi.FormattedWrite('({}){}'.format(escapeText[0], escapeText[1:]))
	
	ConsoleApi.WriteLine("")
	
	inputGood = False
	
	selectedResponse = 0
	
	while (not inputGood):
	
		input = ConsoleApi.ReadLine(">")
		
		if escapeText and input[0].lower() == escapeText[0].lower():
			return None
		
		if not input.isdigit() or int(input, 10) < 1 and int(input, 10) > len(sortedResponses):
			ConsoleApi.WriteLine("Not a valid response")
			inputGood = False
			
		inputGood = True
	
	selectedResponse = int(input, 10) - 1
		
	return responses[selectedResponse]

def Execute(adventureCommand):
	global originalCommandWord
	
	originalCommandWord = adventureCommand.ParsedCommand.OriginalWord.upper()

	# No parameter supplied ask the question
	if adventureCommand.Parameters.Count == 0:
		ConsoleApi.FormattedWrite("Who do you want to {0} to?".format(originalCommandWord))
		return False
		
	# If the room is dark you certainly can't see anyone to talk to
	if CurrentLocation.IsDark:
		ConsoleApi.FormattedWrite(IsDarkText)
		return True
		
	# Do we match any npcs
	parametersToCheck = wildcardGameWordList if adventureCommand.Parameters[0].Word.lower() in everythingWords else adventureCommand.GetParametersWithoutStopWords()
	npcs = AWApi.GetNpcsFromNames(parametersToCheck, 70.0, False)
	
	# Check for words we don't understand
	invalidNpcs = list(filter(lambda object: not object.IsValid, npcs))
	validNpcs = list(filter(lambda object: object.IsValid, npcs))
	
	if any(invalidNpcs):
		ConsoleApi.FormattedWrite("I don't know the word \"{0}\".".format(invalidNpcs[0].Name.upper()))
		return False
		
	# You cannot talk to this object		
	if validNpcs.Count == 0 and invalidNpcs.Count == 0:
		ConsoleApi.FormattedWrite("You cannot talk to  \"{0}\".".format(parametersToCheck[0].OriginalWord.upper()))
		return False
		
	# Only one NPC at a time
	if validNpcs.Count > 1:
		ConsoleApi.FormattedWrite("Try talking to one person at a time.")
		return False
			
	targetNpc = validNpcs[0]
	
	# Valid Talk Object phrases
	currentTalkPhrases = []
	
	# Build phrases for Npc to be matched based on the word that matched them
	phrase1 = originalCommandWord + ' {} {} '.format(toWord, targetNpc.WordThatMatchedThis)
	phrase2 = originalCommandWord + ' {} {} {} '.format(toWord, theWord, targetNpc.WordThatMatchedThis)
	
	currentTalkPhrases.extend([ phrase1.strip(), phrase2.strip()])
	
	# Build phrases for objects to be dropped based on their actual names
	phrase1 = originalCommandWord + ' {} {} '.format(toWord, targetNpc.Name)
	phrase2 = originalCommandWord + ' {} {} {} '.format(toWord, theWord, targetNpc.Name)
			
	currentTalkPhrases.extend([ phrase1.strip(), phrase2.strip()])
			
	if LanguageApi.CheckSentenceAgainstList(adventureCommand.JoinOriginalWordAndParameters(), currentTalkPhrases):
	
		# Allow the NPC pre-process script to run
		preProcessResult = Preprocess(targetNpc)
		
		if preProcessResult == False:
			return False
			
		# No conversation set up
		if targetNpc.GetCurrentConversation() == None:
			ConsoleApi.FormattedWrite('The {} has nothing to say!'.format(targetNpc.WordThatMatchedThis))
			return True
		
		return ProcessConversation(targetNpc)
	
	return SentenceNotRecognised()	
