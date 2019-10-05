AWApi.SetVariable("startGame", True)

ConsoleApi.WriteLine("")
ConsoleApi.WriteBanner(AWApi.GameData.Title)

ConsoleApi.DrawLine(ConsoleColor.Red)
ConsoleApi.FormattedWrite(AWApi.GameData.Introduction, ConsoleColor.Yellow)
ConsoleApi.DrawLine(ConsoleColor.Red)

RustyLeg = "Rusty Leg"
Bed = "Bed"
Alcove = "Wiring Alcove"
Battery = "Cylinder"
Hammer = "Hammer"
JailCell = "Jail Cell"
Robot = "Robot"
EastCorridor = "East Corridor"

AWApi.SetVariable("jacketTaken", False)
AWApi.SetVariable("legDetached", False)
AWApi.SetVariable("clockBashed", False)
AWApi.SetVariable("robotOn", False)
AWApi.SetVariable("passWordEntered", False)
AWApi.SetVariable("passWordTries", 1)
AWApi.SetVariable("jailDoorCode", str(Random().Next(1000, 9999)))

possiblePasswords = ["banana", "password","potter","strawberry","donald","mickey"]

AWApi.SetVariable("robotPassword", possiblePasswords[Random().Next(0,5)].upper())






