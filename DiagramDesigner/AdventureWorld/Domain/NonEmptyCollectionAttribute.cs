﻿using System;

namespace DiagramDesigner.AdventureWorld.Domain
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class NonEmptyCollectionAttribute : Attribute
    {

    }
}