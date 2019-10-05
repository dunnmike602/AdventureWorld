using System.Collections.Generic;
using System.Runtime.Serialization;
using AdventureLandCore.Domain;
using AdventureLandCore.Interfaces;
using AdventureLandCore.Services.AdventureCommands;

namespace AdventureLandCore.Games.MagicBear
{
    public enum DogObjectStates
    {
        Default = 0,
        EatingSausage = 1,
    }

    public static class GlobalData
    {
        public static readonly List<string> ItemsRequiredForSwimming = new List<string> {"GOGGLES", "AQUA", "SWIMHAT", "COST"};
        public static readonly List<string> RoomsWhereAqualungIsNeeded = new List<string> { "TUN1", "TUN2" };
        public const int InventorySize = 20;
        public const int MaximumPoints = 100;
        public const int PointsForSwimming = 10;
    }

    public class SausageProcessor : AdventureCommandBase
    {
        public SausageProcessor(IAdventureGameEngine game, IConsole console, IConfiguration configuration, IAdventureApi adventureApi)
            : base(game, console, configuration, adventureApi)
        {
        }

        public override void Execute(ParsedAdventureCommand adventureCommand, AdventureObjectBase targetObject)
        {
            // If sausage is being thrown check to see if dog is in island room
            if (adventureCommand.CommandName == "DROP")
            {
                // Check for dog in room
                var dogObject = AdventureApi.ReturnObjectIfInRoom("DOG", Game.Location.Name);

                if (dogObject != null & Game.Location.Name == "ISLAND")
                {
                    Console.FormattedWrite(
                        "The dog comes up and sniffs the sausage you just dropped. He starts munching it and ignores you!!!");

                    // Destroy the sausage
                    targetObject.IsDestroyed = true;

                    // Set dog to correct state
                    dogObject.State = (int) DogObjectStates.EatingSausage;
                }
            }

            CanContinue = true;
        }
    }

    public class CaveEntranceProcessor :AdventureCommandBase
    {
        public CaveEntranceProcessor(IAdventureGameEngine game, IConsole console, IConfiguration configuration, IAdventureApi adventureApi) : base(game, console, configuration, adventureApi)
        {
        }

        public override void Execute(ParsedAdventureCommand adventureCommand, AdventureObjectBase targetObject)
        {
            // Check for dog in room
            var dogObject = AdventureApi.ReturnObjectIfInRoom("DOG", Game.Location.Name);

            // If dog is hungary then he blocks the exit to the cave
            if(dogObject != null && dogObject.State == (int)DogObjectStates.Default)
            {
                Console.FormattedWrite("The little dog growls and starts walking towards you. He won't let you go that way.");
                CanContinue = false;
                return;
            }

            CanContinue = true;
        }
    }

    public class BearProcessorCommand : AdventureCommandBase
    {
        public BearProcessorCommand(IAdventureGameEngine game, IConsole console, IConfiguration configuration, IAdventureApi adventureApi)
            : base(game, console, configuration, adventureApi)
        {
        }


        public override void Execute(ParsedAdventureCommand adventureCommand, AdventureObjectBase targetObject)
        {
            var gameObject = (GameObject) targetObject;

            if(adventureCommand.Mapping.CommandName== "TAKE" && !gameObject.IsTakeable)
            {
                Console.FormattedWrite("Leave the bear alone, you don't want to wake her up.");
                CanContinue = false;
            }
        }
    }

    [DataContract]
    public class GameLoopPreProcessCommand : GameLoopCommandProcessorBase
    {
        [DataMember]
        private int _aqualungAirLimit;
       
        public dynamic AqualungAirLimit
        {
            get { return _aqualungAirLimit; }
            set { _aqualungAirLimit = value; }
        }

        [DataMember]
        private int _aqualungRunningLow;

        public dynamic AqualungRunningLow
        {
            get { return _aqualungRunningLow; }
            set { _aqualungRunningLow = value; }
        }

        [DataMember]
        private int _aqualungTurn;

        public dynamic AqualungTurn
        {
            get { return _aqualungTurn; }
            set { _aqualungTurn = value; }
        }

        [DataMember]
        private bool _pointsForSwimming;

        public dynamic PointsForSwimming
        {
            get { return _pointsForSwimming; }
            set { _pointsForSwimming = value; }
        }

        public GameLoopPreProcessCommand(IConsole console, IConfiguration 
            configuration, IAdventureApi adventureApi) : base(console, configuration, adventureApi)
        {
        }

        private void Init()
        {
            AqualungAirLimit = 20;
            AqualungRunningLow = 15;
            AqualungTurn = 1;
            PointsForSwimming = false;
        }

        private void CheckAqualung()
        {
            if(!AdventureApi.IsInList(GlobalData.RoomsWhereAqualungIsNeeded, AdventureApi.GetCurrentLocation().Name))
            {
                return;
            }

            AqualungTurn++;

            if (AqualungTurn == AqualungRunningLow)
            {
                Console.FormattedWrite("The air in you aqualung seems to be getting thinner, it might not last much longer.");
                return;
            }

            if (AqualungTurn == AqualungAirLimit)
            {
                Console.FormattedWrite("Cough, cough, splutter, the air runs out and everything goes black......");
                AdventureApi.GameData. Player.IsDestroyed = true;
            }
        }

        public override void Load(dynamic existing)
        {
            AqualungAirLimit = existing.AqualungAirLimit;
            AqualungRunningLow = existing.AqualungRunningLow;
            AqualungTurn = existing.AqualungTurn;
            PointsForSwimming = existing.PointsForSwimming;
        }

        public override void Execute(bool firstTimeThrough)
        {
            if(firstTimeThrough)
            {
                Init();
            }

            CheckAqualung();
        }
    }

    public class PoolRoomProcessor : AdventureCommandBase
    {
        private readonly dynamic _dynamicGame;

        public PoolRoomProcessor(IAdventureGameEngine game, IConsole console, IConfiguration configuration,
            IAdventureApi adventureApi) : base(game, console, configuration, adventureApi)
        {
            _dynamicGame = game;
        }

        private void CheckItemsForSwimming()
        {
            // You must have the following items to swim underwater aqualung, swimming costume, hat and goggles
            if (AdventureApi.AreAllItemsHeld(GlobalData.ItemsRequiredForSwimming))
            {
                Console.FormattedWrite("Splash, you plunge into the pool and start swimming underwater, the aqualung turns on automatically.");
                CanContinue = true;

                if (!_dynamicGame.GamePreProcessor.PointsForSwimming)
                {
                    AdventureApi.IncrementScore(GlobalData.PointsForSwimming);
                    _dynamicGame.GamePreProcessor.PointsForSwimming = true;
                }

                return;
            }

            Console.FormattedWrite("You need the correct equipment to swim underwater.");
            CanContinue = false;
        }

        public override void Execute(ParsedAdventureCommand adventureCommand, AdventureObjectBase targetObject)
        {
            CheckItemsForSwimming();
        }
    }
}
