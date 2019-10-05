# The INVENTORY shows what the player is holding

def InventoryHelper():
	if AWApi.GetInventory().Count == 0:
		ConsoleApi.FormattedWrite("You're not holding anything.")
		return True
    	
	ConsoleApi.FormattedWrite("You are holding:")

	for placeableObject in AWApi.GetInventory():
		ConsoleApi.FormattedWrite(placeableObject.InventoryDescription)
     
	return True 
           
def Execute(adventureCommand):
	if adventureCommand.Parameters.Count == 0:
		return InventoryHelper()

	return SentenceNotRecognised()
