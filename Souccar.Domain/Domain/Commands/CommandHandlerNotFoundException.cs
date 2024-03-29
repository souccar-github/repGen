﻿using System;

namespace Souccar.Domain.Commands
{
    public class CommandHandlerNotFoundException : Exception
    {
        public CommandHandlerNotFoundException(Type type)
            : base(string.Format("Command handler not found for command type: {0}", type))
        {
        }

        public CommandHandlerNotFoundException(Type commandType, Type commandResult)
            : base(
                string.Format("Command handler not found for command type: {0}, and command result type: {1}",
                              commandType, commandResult))
        {
        }
    }
}