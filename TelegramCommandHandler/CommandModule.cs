﻿using CommandHandler.Types;
using System.Threading.Tasks;

namespace CommandHandler
{
    /// <summary>
    /// Base class for command modules.
    /// </summary>
    public abstract class CommandModule
    {

        /// <summary>
        /// Called before a command is executed.
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public virtual Task BeforeExecutionAsync(CommandContext ctx) => Task.Delay(0);

        /// <summary>
        /// Called after a command is executed.
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public virtual Task AfterExecutionAsync(CommandContext ctx) => Task.Delay(0);

    }
}
