using System;
using System.Collections.Immutable;

namespace Telegram.Bot.Interactivity.Commands
{
    public class TelegramCommandHandler
    {
        private readonly ImmutableHashSet<CommandModule>.Builder _commandModules;

        /// <summary>
        ///     All the registered command modules.
        /// </summary>
        public ImmutableHashSet<CommandModule> RegisteredCommandModules => this._commandModules.ToImmutable();

        /// <summary>
        ///     Prefix for the commands. Defaults to a slash (/)
        /// </summary>
        public char Prefix { get; init; } = '/';

        /// <summary>
        ///     Determines whether or not commands are case-sensitive.
        /// </summary>
        public bool IsCaseSensitive { get; init; } = false;

        public TelegramCommandHandler() => this._commandModules = ImmutableHashSet.CreateBuilder<CommandModule>();

        /// <summary>
        ///     Register a CommandModule subclass as a command class. You can register multiple classes.
        /// </summary>
        /// <typeparam name="T">CommandModule subclass</typeparam>
        /// <param name="constructorArgs">A list of arguments to pass to the <see cref="CommandModule" /> constructor.</param>
        /// <returns>True if the commands have been registered, false otherwise.</returns>
        public bool RegisterCommands<T>( params object[] constructorArgs ) where T : CommandModule
            => this.RegisterCommands( typeof( T ), constructorArgs );

        /// <summary>
        ///     Register a CommandModule subclass as a command class. You can register multiple classes.
        /// </summary>
        /// <param name="t">CommandModule subclass</param>
        /// <param name="constructorArgs">A list of arguments to pass to the <see cref="CommandModule" /> constructor.</param>
        /// <returns>True if the commands have been registered, false otherwise.</returns>
        public bool RegisterCommands( Type t, params object[] constructorArgs )
        {
            if( t is null ) throw new ArgumentNullException( nameof( t ) );
            if( !typeof( CommandModule ).IsAssignableFrom( t ) )
                throw new ArgumentException(
                    $"Type must be a subclass of {typeof( CommandModule ).FullName}",
                    nameof( t )
                );

            var instance = Activator.CreateInstance( t, constructorArgs );
            if( instance is null ) throw new InvalidOperationException( "failed to create CommandModule instance" );

            return this._commandModules.Add( (CommandModule)instance );
        }
    }
}