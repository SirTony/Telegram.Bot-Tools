using System;

namespace Telegram.Bot.CommandHandler
{
    /// <summary>
    ///     Marks a Task as a Command
    /// </summary>
    [AttributeUsage( AttributeTargets.Method )]
    public class CommandAttribute : Attribute
    {
        /// <summary>
        ///     Determines what invokes the command to be called
        /// </summary>
        public string Name { get; }

        /// <summary>
        ///     New Command attribute
        /// </summary>
        /// <param name="name">Determines what invokes the command to be called, without prefix</param>
        public CommandAttribute( string name )
        {
            if( String.IsNullOrWhiteSpace( name ) ) throw new ArgumentNullException( nameof( name ) );
            this.Name = name;
        }
    }
}