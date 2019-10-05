namespace DiagramDesigner.AdventureWorld.Domain
{
    public enum ValidationTypes
    {
        All = 0,
        TitleMissing,
        IntroductionMissing,
        WonGameMissing,
        PlayerLostMissing,
        AnotherGameMissing,
        MaximumScoreNotSet,
        StartRoomNotSet,
        InventorySizeNotSet,
        GameNameNotSet,
        GameExecutableNotSet,
        NameNotSet,
        DescriptionNotSet,
        ObjectNotInRoom,
        UserNameNotSet,
        GameDescriptionNotSet,
        Duplicate,
        EmptyCommand,
        DuplicateCommand,
        MissingCommandPromptText,
        NoExitDirection,
        AnotherGameYesResponseMissing,
        NoParent,
        ResponseMissing,
        NpcSpeaksMissing,
        InvalidConversation,
        MissingNotItemsInRoomText,
    }
}