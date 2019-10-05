def Execute(adventureObject, adventureCommand):

	if adventureCommand.ParsedCommand.Word.upper() == "TAKE":
		if not AWApi.GetVariable("legDetached"):
			ConsoleApi.FormattedWrite("After a bit of a struggle you manage to wrestle it from the bed. The bed now on three legs drops on your foot ouch!! Something drops out of the mattress and rolls across the floor.")
			adventureObject.HideFromAutoList = False
			adventureObject.IsHeld = True
			adventureObject.Visible = True
			adventureObject.Fixed = False
			AWApi.GetPlaceableObjectFromName(Battery).Visible = True
			AWApi.GetPlaceableObjectFromName(Bed).CurrentDescriptionIndex = 1
			AWApi.SetVariable("legDetached", True)
			return False
			
	return True
	
