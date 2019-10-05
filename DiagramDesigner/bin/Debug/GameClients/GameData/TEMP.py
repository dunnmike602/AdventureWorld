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

AWApi.SetVariable("legDetached", False)
AWApi.SetVariable("clockBashed", False)
AWApi.SetVariable("robotOn", False)
AWApi.SetVariable("jailDoorCode", str(Random().Next(1000, 9999)))

possiblePasswords = ["banana", "password","potter","strawberry","donald","mickey"]

AWApi.SetVariable("robotPassword", possiblePasswords[Random().Next(0,5)].upper())






