﻿using MLDComputing.ObjectBrowser.Attributes;

namespace AdventureLandCore.Domain
{
    [IgnoreInObjectBrowser]
    public enum MessageIds
    {
        CommandNotFound,
        ParameterMissing,
        Yes,
        AreYouSure,
        AssemblyName,
        ItemTaken,
        AlreadyHolding,
        UntakeableObject,
        ObjectNotHere,
        ItemDropped,
        CantDrop,
        HoldingMessage,
        NoItemsHoldingMessage,
        CantDoMoveType,
        CantDoThat,
        Score,
        CantDoAction,
        Ok,
        WaitAction,
        InitialisationCode,
        CommonCode,
        GameLoopPreProcessCode,
        GameLoopPostProcessCode,
        Title,
        Introduction,
        NothingToTake,
        TitleWithScore,
        HandsFull,
        NoFile,
        LoadSuccessfully,
        DeleteSuccessfully,
        SaveSuccess,
        InvalidVersion,
        Id,
        NotHolding,
        DefaultLockMessage,
        SentenceNotRecognised,
        WordNotRecognised,
        MissingNoun,
        MatchesMoreThanOneItem,
        MultipleExamine,
        NoExamine,
        NoUse,
        ItemThrown,
        UseSentence,
    }
}