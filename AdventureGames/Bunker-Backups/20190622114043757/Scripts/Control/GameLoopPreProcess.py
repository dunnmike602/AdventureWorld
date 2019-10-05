import math
from System import Char

def ShowTitle(remainingTimeSpan):
	
	milliSecondsLeft = remainingTimeSpan.TotalMilliseconds
	hoursLeft = "{:02d}".format(remainingTimeSpan.Hours)
	minutesLeft = "{:02d}".format(remainingTimeSpan.Minutes)
	secondsLeft = "{:02d}".format(remainingTimeSpan.Seconds)
	
	if milliSecondsLeft > 0:
		time = "{0}:{1}:{2}".format(hoursLeft, minutesLeft, secondsLeft)
		ConsoleApi.Title = "{0} : Remaining Time {1}".format(AWApi.GameData.Title, time)
		AWApi.SetVariable("timeLeft", time)
	else:
		AWApi.KillPlayer()
		ConsoleApi.SimulateKeyPress(ESCAPE)
		ConsoleApi.SimulateKeyPress(RETURN)
		
	AWApi.SetVariable("timeAllowed", milliSecondsLeft)
	
	return

startGame = AWApi.GetVariable("startGame")
timeAllowed = AWApi.GetVariable("timeAllowed")

if startGame and timeAllowed == None:
	timeAllowed = 60 * 60 * 1000
	
if startGame:
	AWApi.SetVariable("startGame", False)
	ConsoleApi.CountDownTimer(timeAllowed, 1000, None, None, ShowTitle)

		

