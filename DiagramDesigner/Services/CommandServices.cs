using System;
using System.Collections.Generic;
using System.Reflection;
using AdventureLandCore.Domain;
using AdventureLandCore.Extensions;
using DiagramDesigner.AdventureWorld.Domain;

namespace DiagramDesigner.Services
{
    public static class CommandServices
    {
        public static List<AdventureCommandMapping> GetAdventureCommandMappings()
        {
            var mappings = new List<AdventureCommandMapping>
            {
                new AdventureCommandMapping
                {
                    ControlId = new Guid("AA791207-CCCC-4720-BE27-3F8BAC40C821"),
                    VerbName = "TALK",
                    AliasList =  string.Join(Environment.NewLine, "SPEAK", "CHAT"),
                    HelpText = "Talk to a NPC (non-player character).",
                    IsBuiltInCommand = true,
                    ScriptCommand =  new Script{Source = Assembly.GetExecutingAssembly().GetCommandTemplateByName("Talk")},
                },
                new AdventureCommandMapping
                {
                    ControlId = new Guid("AA791207-FDEC-4720-BE27-3F8BAC40C821"),
                    VerbName = "INVENTORY",
                    AliasList =  string.Join(Environment.NewLine, "INV", "HOLDING"),
                    HelpText = "Show items you are holding.",
                    IsBuiltInCommand = true,
                    ScriptCommand =  new Script{Source = Assembly.GetExecutingAssembly().GetCommandTemplateByName("Inventory")},
                },
                new AdventureCommandMapping
                {
                    ControlId = new Guid("3C9335BF-E648-46B8-924D-58D90A02BABF"),
                    VerbName = "QUIT",
                    AliasList =  string.Join(Environment.NewLine,  "Q"),
                    HelpText = "Quit the game.",
                    IsBuiltInCommand = true,
                    ScriptCommand =  new Script{Source = Assembly.GetExecutingAssembly().GetCommandTemplateByName("Quit")},
                },
                new AdventureCommandMapping
                {
                    ControlId = new Guid("0AE85C8E-3850-47CA-8D1C-57978A8DBF63"),
                    VerbName = "LOOK",
                    AliasList = string.Join(Environment.NewLine, "EXAMINE", "INSPECT"),
                    HelpText = "Describe an object or look at whats around.",
                    IsBuiltInCommand = true,
                    ScriptCommand =  new Script{Source = Assembly.GetExecutingAssembly().GetCommandTemplateByName("Look")},
                },
                new AdventureCommandMapping
                {
                    ControlId = new Guid("C0826AAC-33FA-4A33-A8BE-435187C708CA"),
                    VerbName = "SCORE",
                    HelpText = "Show the current score.",
                    IsBuiltInCommand = true,
                    ScriptCommand =  new Script{Source = Assembly.GetExecutingAssembly().GetCommandTemplateByName("Score")},
                },
                new AdventureCommandMapping
                {
                    ControlId = new Guid("BC7D61FA-6DCC-4C9E-BEF7-A08D18CD97DD"),
                    VerbName = "WALK",
                    AliasList = string.Join(Environment.NewLine, "GO", "MOVE", "STEP"),
                    OneWordSubstitutionList =  string.Join( Environment.NewLine, "N", "NORTH", "S", "SOUTH", "E", "EAST", "W", "WEST", "SE", 
                        "SOUTHEAST", "SW", "SOUTHWEST", "NE", "NORTHEAST", "NW", "NORTHWEST", "U", "UP", "D", "DOWN"),
                    HelpText = "Walk in the specified direction.",
                    IsBuiltInCommand = true,
                    ScriptCommand =  new Script{Source = Assembly.GetExecutingAssembly().GetCommandTemplateByName("Walk")},
                },
                new AdventureCommandMapping
                {
                    ControlId = new Guid("BC7D61FA-6DCC-9999-BEF7-A08D18CD97DD"),
                    VerbName = "UNLOCK",
                    HelpText = "Unlock an object or an exit.",
                    IsBuiltInCommand = true,
                    ScriptCommand =  new Script{Source = Assembly.GetExecutingAssembly().GetCommandTemplateByName("Unlock")},
                },
                new AdventureCommandMapping
                {
                    ControlId = new Guid("BC7D6DDF-6DCC-9999-BEF7-A08D18CD97DD"),
                    VerbName = "OPEN",
                    HelpText = "Open a container object.",
                    IsBuiltInCommand = true,
                    ScriptCommand =  new Script{Source = Assembly.GetExecutingAssembly().GetCommandTemplateByName("Open")},
                },
                new AdventureCommandMapping
                {
                    ControlId = new Guid("AC7D81FA-6DCC-4C9E-BEF7-A08D18CD37DD"),
                    VerbName = "RUN",
                    AliasList = string.Join(Environment.NewLine, "SPRINT", "JOG"),
                    HelpText = "Run in the specified direction.",
                    IsBuiltInCommand = true,
                    ScriptCommand =  new Script{Source = Assembly.GetExecutingAssembly().GetCommandTemplateByName("Run")},
                },
                new AdventureCommandMapping
                {
                    ControlId = new Guid("B1663BD0-C540-4F30-929C-4E7B6FDCB0D0"),
                    VerbName = "SWIM",
                    AliasList = string.Join(Environment.NewLine, "PADDLE"),
                    HelpText = "Swim in the specified direction.",
                    IsBuiltInCommand = true,
                    ScriptCommand =  new Script{Source = Assembly.GetExecutingAssembly().GetCommandTemplateByName("Swim")},
                },
                new AdventureCommandMapping
                {
                    ControlId = new Guid("2F1B4CE8-41FF-4B91-BCC6-A1B4D1499317"),
                    VerbName = "CRAWL",
                    AliasList = string.Join(Environment.NewLine, "WORM", "SLITHER"),
                    HelpText = "Crawl in the specified direction.",
                    IsBuiltInCommand = true,
                    ScriptCommand =  new Script{Source = Assembly.GetExecutingAssembly().GetCommandTemplateByName("Crawl")},
                },
                new AdventureCommandMapping
                {
                    ControlId = new Guid("7810BF31-F7A0-475C-A15A-BDD482BD44F7"),
                    VerbName = "LIST",
                    AliasList = string.Join(Environment.NewLine, "LS"),
                    HelpText = "List saved games.",
                    IsBuiltInCommand = true,
                    ScriptCommand =  new Script{Source = Assembly.GetExecutingAssembly().GetCommandTemplateByName("List")},
                },
                new AdventureCommandMapping
                {
                    ControlId = new Guid("D4CDC8A9-61BE-41F6-8B85-C9900D72CCD3"),
                    VerbName = "SWITCH",
                    AliasList = string.Join(Environment.NewLine, "TURN"),
                    HelpText = "Switch a lightable object on or off.",
                    IsBuiltInCommand = true,
                    ScriptCommand =  new Script{Source = Assembly.GetExecutingAssembly().GetCommandTemplateByName("Switch")}
                },
                new AdventureCommandMapping
                {
                    ControlId = new Guid("353F2448-65C6-46A7-9FEB-BC751344D8CC"),
                    VerbName = "USE",
                    AliasList = string.Join(Environment.NewLine, "HANDLE"),
                    HelpText = "Interact with an object.",
                    IsBuiltInCommand = true,
                    ScriptCommand =  new Script{Source = Assembly.GetExecutingAssembly().GetCommandTemplateByName("Use")},
                },
                new AdventureCommandMapping
                {
                    ControlId = new Guid("154B6BD8-8605-4E48-AEA9-4BE4C2F73855"),
                    VerbName = "TAKE",
                    AliasList = string.Join(Environment.NewLine, "GET", "SNATCH", "SEIZE", "COLLECT", "SNAG", "OBTAIN", "CLASP", "GRASP", "GATHER", "PICK"),
                    HelpText = "Take object(s).",
                    IsBuiltInCommand = true,
                    ScriptCommand =  new Script{Source = Assembly.GetExecutingAssembly().GetCommandTemplateByName("Take")},
                },
                new AdventureCommandMapping
                {
                    ControlId = new Guid("70632081-DC82-44F1-A1D4-C03B42262E1F"),
                    VerbName = "DROP",
                    AliasList = string.Join(Environment.NewLine, "LEAVE"),
                    HelpText = "Drop object(s).",
                    IsBuiltInCommand = true,
                    ScriptCommand =  new Script{Source = Assembly.GetExecutingAssembly().GetCommandTemplateByName("Drop")},
                },
                new AdventureCommandMapping
                {
                    ControlId = new Guid("79367091-E460-42BF-9B24-A5FFED3B3480"),
                    VerbName = "SAVE",
                    AliasList = string.Join(Environment.NewLine, "SAV"),
                    HelpText = "Saves the current game with the specified name.",
                    IsBuiltInCommand = true,
                    ScriptCommand =  new Script{Source = Assembly.GetExecutingAssembly().GetCommandTemplateByName("Save")},
                },
                new AdventureCommandMapping
                {
                    ControlId = new Guid("D7BE3E3A-2060-4438-8A21-49C30EF6D48C"),
                    VerbName = "LOAD",
                    AliasList = string.Join(Environment.NewLine, "LD"),
                    HelpText = "Loads the saved game with the specified name.",
                    ScriptCommand =  new Script{Source = Assembly.GetExecutingAssembly().GetCommandTemplateByName("Load")},
                },
                new AdventureCommandMapping
                {
                    ControlId = new Guid("0830CEE2-3FBB-4F36-ABBE-9A7160FF7788"),
                    VerbName = "DELETE",
                    AliasList = string.Join(Environment.NewLine, "DEL", "ERASE"),
                    HelpText = "Deletes the saved game with the specified name.",
                    ScriptCommand =  new Script{Source = Assembly.GetExecutingAssembly().GetCommandTemplateByName("Delete")},
                },
                new AdventureCommandMapping
                {
                    ControlId = new Guid("BC0920C7-EC1D-42E2-8759-C1F655BFE246"),
                    VerbName = "THROW",
                    AliasList = string.Join(Environment.NewLine, "LOB", "HURL", "HEAVE", "FLING"),
                    HelpText = "Throw an object, optionally in a particular direction.",
                    IsBuiltInCommand = true,
                    ScriptCommand =  new Script{Source = Assembly.GetExecutingAssembly().GetCommandTemplateByName("Throw")},
                },
                new AdventureCommandMapping
                {
                    ControlId = new Guid("22C3E0B9-AE8B-4134-9779-C5EE15DE2EF4"),
                    VerbName = "HELP",
                    HelpText = "Show help.",
                    IsBuiltInCommand = true,
                    ScriptCommand =  new Script{Source = Assembly.GetExecutingAssembly().GetCommandTemplateByName("Help")},
                },
                new AdventureCommandMapping
                {
                    ControlId = new Guid("3D932E34-A69D-4D64-BA02-812E0FE7FCF3"),
                    VerbName = "WAIT",
                    AliasList = string.Join(Environment.NewLine, "HANG", "CHILL"),
                    HelpText = "Wait around for a bit.",
                    IsBuiltInCommand = true,
                    ScriptCommand =  new Script{Source = Assembly.GetExecutingAssembly().GetCommandTemplateByName("Wait")},
                },
            };

            return mappings;
        }
    }
}