using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Telegram.Bot.Interactivity.Commands
{
    public class CommandUpdateHandler : IUpdateHandler
    {
        private readonly TelegramCommandHandler                                        _handler;
        private readonly ImmutableDictionary<CommandModule, ImmutableList<MethodInfo>> _modules;

        public CommandUpdateHandler( TelegramCommandHandler handler )
        {
            this._handler = handler ?? throw new ArgumentNullException( nameof( handler ) );

            var modules = new Dictionary<CommandModule, ImmutableList<MethodInfo>>();
            foreach( var module in this._handler.RegisteredCommandModules )
            {
                var methods = from method in module.GetType().GetMethods()
                              let parameters = method.GetParameters()
                              where parameters.Length == 4
                              where typeof( ITelegramBotClient ).IsAssignableFrom( parameters[0].ParameterType )
                              where typeof( Update ).IsAssignableFrom( parameters[1].ParameterType )
                              where typeof( CancellationToken ).IsAssignableFrom( parameters[2].ParameterType )
                              where typeof( string ).IsAssignableFrom( parameters[3].ParameterType )
                              where typeof( Task ).IsAssignableFrom( method.ReturnType )
                              where method.GetCustomAttribute<CommandAttribute>( false ) is not null
                              select method;

                modules.Add( module, methods.ToImmutableList() );
            }

            this._modules = modules.ToImmutableDictionary();
        }

        public UpdateType[] AllowedUpdates { get; } = {UpdateType.Message};

        public async Task HandleUpdate( ITelegramBotClient client, Update update, CancellationToken ct )
        {
            if( update?.Message?.Type != MessageType.Text ) return;
            if( String.IsNullOrWhiteSpace( update?.Message?.Text ) ) return;

            var text = update.Message.Text.TrimStart();
            if( text[0] == this._handler.Prefix )
            {
                //Get the command without the prefix
                var command = text[1..];
                foreach( var (module, methods) in this._modules )
                {
                    foreach( var method in methods )
                    {
                        var commandAttribute = method.GetCustomAttribute<CommandAttribute>( false );
                        var aliasesAttribute = method.GetCustomAttribute<AliasesAttribute>( false );

                        var names = ( aliasesAttribute?.Aliases ?? ImmutableArray<string>.Empty ).Prepend(
                            commandAttribute!.Name
                        );

                        var cmp = this._handler.IsCaseSensitive
                                      ? StringComparison.Ordinal
                                      : StringComparison.OrdinalIgnoreCase;

                        var match = names.FirstOrDefault( x => command.StartsWith( x, cmp ) );
                        if( match is null ) continue;

                        // Call on the Before Execution Async method
                        await module.BeforeExecutionAsync( client, update, ct ).ConfigureAwait( false );

                        // everything after the prefix and command name/alias
                        var commandData = text[match.Length..].TrimStart();

                        //Call invoke the actual command.
                        var task = (Task)method.Invoke( module, new object[] {client, update, ct, commandData} );
                        if( task is not null ) await task.ConfigureAwait( false );

                        //Call on the After Execution Async method
                        await module.AfterExecutionAsync( client, update, ct ).ConfigureAwait( false );

                        return;
                    }
                }
            }
        }

        public virtual Task HandleError( ITelegramBotClient client, Exception ex, CancellationToken ct )
            => ex is not null ? Task.FromException( ex ) : Task.CompletedTask;
    }
}