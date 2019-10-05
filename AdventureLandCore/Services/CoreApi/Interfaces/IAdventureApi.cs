using System.Collections.Generic;
using AdventureLandCore.Domain;
using AdventureLandCore.Interfaces;

namespace AdventureLandCore.Services.CoreApi.Interfaces
{
    /// <summary>
    /// This interface defines the Api (Applications Programming Interface) that can be used by scripts to access the game engine and perform operations on
    /// players, rooms and placeable objects. All access to the api for the currently running game is through the variable AWApi available to Python and C# scripts.
    /// </summary>
    public interface IAdventureApi
    {
        /// <summary>
        /// Get all containers in the game.
        /// </summary>
        /// <returns>A list of containers.</returns>
        List<Container> GetAllContainers();

        /// <summary>
        /// Get all Npcs in the game.
        /// </summary>
        /// <returns>A list of Npcs.</returns>
        List<Npc> GetAllNpcs();

        /// <summary>
        /// Get all Npcs in the game that are set to AutoFollow.
        /// </summary>
        /// <returns>A list of Auto Follow Npcs.</returns>
        List<Npc> GetAllAutoFollowNpcs();

        /// <summary>
        /// Get all Placeable objects in the game.
        /// </summary>
        /// <returns>A list of Placeable objects.</returns>
        List<PlaceableObject> GetAllPlaceableObjects();
      
        /// <summary>
        /// Retrieves a flag that determines if debug mode has been enabled in the game. This flag is used in scripts to determine
        /// whether debug commands should be enabled.
        /// </summary>
        /// <returns>IsDebugEnabled flag is returned.</returns>
        bool IsDebugEnabled();


        /// <summary>
        /// Retrieves all the possible directions that can be used in a move, full text and abbreviations.
        /// </summary>
        /// <returns>List of all directions that can be moved to.</returns>
        List<string> GetMoveLimits();

        /// <summary>
        /// Retrieves a the direction for the specified abbreviation.
        /// </summary>
        /// <returns>The full text name of the abbreviation, or null if it does not exist..</returns>
        string GetDirectionFromAbbreviation(string abbreviation);

        /// <summary>
        /// Original configuration information that was used to load the current game. Similar to GameData but has some other useful static information
        /// not saved in GameData as it is not required for save games.
        /// </summary>
        IGameConfiguration Configuration { get; set; }

        /// <summary>
        /// Retrieves a list of all adventure objects in the game. Rooms, objects (and containers) and Exits.
        /// </summary>
        /// <returns>The list of all objects.</returns>
        List<AdventureObjectBase> GetAllAdventureObjects();

        /// <summary>
        /// Retrieves the container PlaceableObject for the specified one (i.e. its immediate parent)
        /// </summary>
        /// <param name="placeableObject">PlaceableObject to check.</param>
        /// <returns>The container object, or null if the object is not in a container.</returns>
        PlaceableObject GetContainerForPlaceableObject(PlaceableObject placeableObject);

        /// <summary>
        /// If the specified object contains things, this command will retrieve them all.
        /// </summary>
        List<PlaceableObject> GetChildObjects(PlaceableObject placeableObject);

        /// <summary>
        /// If the specified object contains things, this command will list their current descriptions. If the object has no children have any, nothing is shown.
        /// </summary>
        /// <param name="placeableObject">The object to list the descriptions of.</param>
        void ListChildObjectDescriptions(PlaceableObject placeableObject);

        /// <summary>
        /// Cause the the current game to be ended.
        /// </summary>
        void QuitGame();

        /// <summary>
        /// Replaces the current in-game data structures with those supplied in the parameter. This allows a new game loaded from disk to replace the current one.
        /// </summary>
        /// <param name="newGameData">NewGameData object to load</param>
        void InitGameData(GameData newGameData);

        /// <summary>
        /// Serialises the current game to a string, that can be saved to disk.
        /// </summary>
        /// <returns>String containing the current game state.</returns>
        string SerializeToString();

        /// <summary>
        /// Creates a GameData object from the supplied data. This allows the current game to be loaded from an Xml file on disk.
        /// </summary>
        /// <param name="xmlData">Xml data created using the SerializeToString method.</param>
        /// <returns>A GameData object created from XmlData.</returns>
        GameData DeserializeFromString(string xmlData);

        /// <summary>
        /// Retrieves the default help text associated with each command that the game recognised.
        /// </summary>
        /// <returns>Formatted list of the default help text.</returns>
        string GetDefaultHelpText();

        /// <summary>
        /// Returns the current location of the player.
        /// </summary>
        /// <returns>A Room object that represents the players current Location.</returns>
        Room GetCurrentLocation();

        /// <summary>
        /// Sets the current location of the player. Not that this Api call automatically sets the Visited flag of the last room.
        /// to true.
        /// </summary>
        /// <param name="newRoom">A Room object that is the new location of the player</param>
        void SetCurrentLocation(Room newRoom);

        /// <summary>
        /// Helper function that shows information about the current location. If the room is dark, its dark description will be shown. If the
        /// ShowItemsInRoom feature toggle is false, then the items will not be listed (also not shown if the room is dark).
        /// </summary>
        void ShowRoomInformation();

        /// <summary>
        /// Determines if the specified item is available to examine. That is it is held by the player or in the current room and is currently visible.
        /// </summary>
        /// <param name="objectToCheck">The game object to check.</param>
        /// <returns>True is it can be examined, false otherwise.</returns>
        bool IsItemAvailableToExamine(PlaceableObject objectToCheck);

        /// <summary>
        /// Property used to control the execution of user scripts. Can be set by any user script to determine if processing should continue after that script.
        /// </summary>
        bool CanContinue { get; set; }

        /// <summary>
        /// Top level object that contains all data (flags etc.) relating the the game.
        /// </summary>
        GameData GameData { get; set; }

        /// <summary>
        /// Gets/sets the last sentence input by the player.
        /// </summary>
        string LastPlayerInput { get; set; }

        /// <summary>
        /// Stops the game engine performing its own language processing on the last input from the user. Call this method in your own processing script
        /// if you want to handle all processing yourself. You will have to write all your own commands and perform all parsing as well.
        /// </summary>
        void SuppressAutomaticInputProcessing();

        /// <summary>
        /// Gets the command that has been parsed by the internal game engine from the text in LastPlayerInput
        /// </summary>
        ParsedAdventureCommand LastParsedCommand { get; set; }

        /// <summary>
        /// Gets the last command to be executed, useful in pre-process scripts to determine which command caused the script to be executed
        /// </summary>
        ParsedAdventureCommand LastExecutedCommand { get; set; }

        /// <summary>
        /// Determine if the named object is in the players inventory.
        /// </summary>
        /// <param name="name">String containing the name of the object.</param>
        /// <returns>True if the invemtory contains the object, false otherwise.</returns>
        bool DoIHaveTheObject(string name);

        /// <summary>
        /// Retrieve the current players inventory.
        /// </summary>
        /// <returns>A list of PlaceableObjects that the player is holding.</returns>
        List<PlaceableObject> GetInventory();

        /// <summary>
        /// Retrieves the count of items in the player's inventory.
        /// </summary>
        int CurrentInventoryCount { get;  }

        /// <summary>
        /// Cause the specified object to be dropped.
        /// </summary>
        /// <param name="objectToDrop">The PlaceableObject to drop.</param>
        void Drop(PlaceableObject objectToDrop);

        /// <summary>
        /// Causes the specified object to be thrown through an exit.
        /// </summary>
        /// <param name="objectToThrow">The PlaceableObject to throw.</param>
        /// <param name="roomName">The name of the room the exit thrown through leads to</param>
        void ThrowToRoom(PlaceableObject objectToThrow, string roomName);

        /// <summary>
        /// Cause the specified object to be taken.
        /// </summary>
        /// <param name="objectToTake">The PlaceableObject to taken.</param>
        void Take(PlaceableObject objectToTake);

        /// <summary>
        /// Finds all Game Objects that match the specified word list. Each word is checked against all possible words and a list returned.
        /// Any unmatched words are returned as invalid Game Objects. This also accepts All and Everything to return all items
        /// in the room.
        /// </summary>
        /// <param name="sentenceWords">List of GameWords that constitutes the sentence to check.</param>
        /// <param name="percentOfWordToMatch">Allows partial matches if set to anything other than 100. Default is 70.</param>
        /// <param name="allRefersToHeldItems">If True, then All/Everthing refers to held items, otherwise it refers to things that can be seen in the room.</param>
        /// <returns>A list of Npcs (IsValid = true) and invalid words (IsValid = false and Name set to original word).</returns>
        List<PlaceableObject> GetObjectsFromNames(List<GameWord> sentenceWords, double percentOfWordToMatch = 70.0, bool allRefersToHeldItems = false);

        /// <summary>
        /// Finds all Game Objects that match the specified word list and are NPC's only. Each word is checked against all possible words and a list returned.
        /// Any unmatched words are returned as invalid Game Objects. This also accepts All and Everything to return all items
        /// in the room.
        /// </summary>
        /// <param name="sentenceWords">List of GameWords that constitutes the sentence to check.</param>
        /// <param name="percentOfWordToMatch">Allows partial matches if set to anything other than 100. Default is 70.</param>
        /// <param name="allRefersToHeldItems">If True, then All/Everthing refers to held items, otherwise it refers to things that can be seen in the room.</param>
        /// <returns>A list of PlaceableObjects (IsValid = true) and invalid words (IsValid = false and Name set to original word).</returns>
        List<Npc> GetNpcsFromNames(List<GameWord> sentenceWords, double percentOfWordToMatch = 70.0,
            bool allRefersToHeldItems = false);

        /// <summary>
        /// Finds all Rooms that match the specified word list. Each word is checked against all possible words and a list returned.
        /// Any unmatched words are returned as invalid Room objects.
        /// </summary>
        /// <param name="sentenceWords">List of GameWords  that constitutes the sentence to check.</param>
        /// <param name="percentOfWordToMatch">Allows partial matches if set to anything other than 100. Default is 70.</param>
        /// <returns>A list of valid rooms (IsValid = true) and invalid words (IsValid = false and Name set to original word).</returns>
        List<Room> GetRoomsFromNames(List<GameWord> sentenceWords, double percentOfWordToMatch = 70.0);

        /// <summary>
        /// Returns the room that the game started in.
        /// </summary>
        /// <returns>A Room object that the game started in.</returns>
        Room GetStartRoom();

        /// <summary>
        /// Retrieves the Exit from the specified room at the specified direction. Returns null if the exit does not exist or is invisible.
        /// </summary>
        /// <param name="roomName">Name of the room</param>
        /// <param name="direction">Direction to travel</param>
        /// <returns>An Exit object representing the Exit, this may be null.</returns>
        Exit GetExitFromName(string roomName, string direction);

        /// <summary>
        /// Retrieves the Exit from the specified room at the specified direction. Returns null if the exit does not exist or is invisible.
        /// </summary>
        /// <param name="room">The room object</param>
        /// <param name="direction">Direction to travel</param>
        /// <returns>An Exit object representing the Exit, this may be null.</returns>
        Exit GetExit(Room room, string direction);

        /// <summary>
        /// Given the entire name of a Room return the programmatic object that represents it.
        /// </summary>
        /// <param name="roomName">Name of the Room.</param>
        /// <returns>Room object for the specified name, or null if it does not exist.</returns>
        Room GetRoomFromName(string roomName);

        /// <summary>
        /// Given the entire name of a PlaceableObject return the programmatic object that represents it.
        /// </summary>
        /// <param name="objectName">Name of the PlaceableObject.</param>
        /// <returns>GamObject object for the specified name, or null if it does not exist.</returns>
        PlaceableObject GetPlaceableObjectFromName(string objectName);

        /// <summary>
        /// Given the entire name of an Npc return the programmatic object that represents it.
        /// </summary>
        /// <param name="npcName">Name of the PlaceableObject.</param>
        /// <returns>GamObject object for the specified name, or null if it does not exist.</returns>
        Npc GetNpcFromName(string npcName);

        /// <summary>
        /// Given the entire name of an Container return the programmatic object that represents it.
        /// </summary>
        /// <param name="containerName">Name of the Container.</param>
        /// <returns>GamObject object for the specified name, or null if it does not exist.</returns>
        Container GetContainerFromName(string containerName);

        /// <summary>
        /// Get all game objects currently in the room (regardless of whether they are Visible or listable). Ignores objects held by player.
        /// </summary>
        /// <param name="roomName">Name of the room to check, if empty current room is used.</param>
        /// <returns>List of PlaceableObjects.</returns>
        List<PlaceableObject> GetPlaceableObjectsInRoom(string roomName);

        /// <summary>
        /// Get all game objects currently in the room. Invisible ones and objects held by player are ignored.
        /// </summary>
        /// <param name="roomName">Name of the room to check, if empty current room is used.</param>
        /// <returns>List of PlaceableObjects.</returns>
        List<PlaceableObject> GetVisiblePlaceableObjectsInRoom(string roomName);

        /// <summary>
        /// Get all game objects currently in the room. Invisible ones, non-listable ones and objects held by player are ignored.
        /// </summary>
        /// <param name="roomName">Name of the room to check, if empty current room is used.</param>
        /// <returns>List of PlaceableObjects.</returns>
        List<PlaceableObject> GetVisibleAndListablePaceableObjectsInRoom(string roomName = "");

        /// <summary>
        /// Return the held object, identified by its name.
        /// </summary>
        /// <param name="name">String containing the name of the object.</param>
        /// <returns>The requested object</returns>
        PlaceableObject GetHeldObjectFromName(string name);

        /// <summary>
        /// Save the current game to disk.
        /// </summary>
        /// <param name="fileStem">Name of the save game.</param>
        /// <param name="data">Data to be saved</param>
        void Save(string fileStem, string data);

        /// <summary>
        /// Load a previously saved game.
        /// </summary>
        /// <param name="fileStem">Name of the game to load.</param>
        /// <returns>String containing the saved game data.</returns>
        string LoadSaveGame(string fileStem);
        
        /// <summary>
        /// Deletes a Save Game.
        /// </summary>
        /// <param name="fileStem">Name of the game to delete.</param>
        void DeleteSaveGame(string fileStem);


        /// <summary>
        /// Returns a list of the names of all save games.
        /// </summary>
        /// <returns>List of strings containing the names of saved games.</returns>
        List<string> ListSaveGames();

        /// <summary>
        /// Kills the current player, sets Player.IsDestroyed to false.
        /// </summary>
        void KillPlayer();

        /// <summary>
        /// Resurects the current player, sets Player.IsDestroyed to true.
        /// </summary>
        void ResurrectPlayer();

        /// <summary>
        /// Call this to clear all currently set user-defined game variables.
        /// </summary>
        void ClearVariables();

        /// <summary>
        /// Call this to add a new user-defined variable. These are game variables that persist between script calls and are visible to all scripts.
        /// </summary>
        /// <param name="name">Name of the variable to be set. Must be a valid non-null string.</param>
        /// <param name="value">Value of the variable. Variable will hold any object type.</param>
        void SetVariable(string name, object value);
        
        /// <summary>
        /// Gets the value of the specified variable (may be any type).
        /// </summary>
        /// <param name="name">Name of the variable whose value is required.</param>
        /// <returns>Value of the variable.</returns>
        object GetVariable(string name);

        /// <summary>
        /// Clears the specified game variable without effecting any others.
        /// </summary>
        /// <param name="name">Name of the variable to clear.</param>
        void ClearVariable(string name);

        /// <summary>
        /// Checks if the specified game variable exists.
        /// </summary>
        /// <param name="name">String containing the name of the game variable to check.</param>
        /// <returns>Returns a boolean value, true if the specified variable exists already, false otherwise.</returns>
        bool HasVariable(string name);
    }
}