using System;
using System.Threading.Tasks;
using Interactivity.Extensions;
using Interactivity.Types;
using Sample.CommandModules;
using Telegram.Bot;
using Telegram.Bot.CommandHandler;

namespace Sample
{
    internal class Program
    {
        private static async Task Main( string[] args )
        {
            // Create a bot client.
            var botClient = new TelegramBotClient( Environment.GetEnvironmentVariable( "TelegramKey" ) );
            botClient.UseInteractivity(
                new InteractivityConfiguration
                {
                    DefaultTimeOutTime = TimeSpan.FromSeconds( 5 ),
                }
            );

            // Get the bot's User.
            var me = await botClient.GetMeAsync();

            Console.WriteLine( $"Hello, World! I am user {me.Id} and my name is {me.FirstName}." );

            var commandHandler = new TelegramCommandHandler();
            commandHandler.RegisterCommands<BasicCommands>();
            commandHandler.RegisterCommands<InteractivityCommands>();

            var updateHandler = new CommandUpdateHandler( commandHandler );
            await botClient.ReceiveAsync( updateHandler );
        }
    }
}