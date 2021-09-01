using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.CommandHandler;
using Telegram.Bot.Types;

namespace Sample.CommandModules
{
    public class BasicCommands : CommandModule
    {
        // Optional override. Called before a command is executed.
        public override Task BeforeExecutionAsync( ITelegramBotClient client, Update update, CancellationToken ct )
            => Console.Out.WriteLineAsync( "Executing Command..." );

        // Optional override. Called after a command is executed.
        public override Task AfterExecutionAsync( ITelegramBotClient client, Update update, CancellationToken ct )
            => Console.Out.WriteLineAsync( "Executed Command." );

        [Aliases( "pingbot" )]
        [Command( "ping" )]
        public async Task PingCommand(
            ITelegramBotClient client,
            Update             update,
            CancellationToken  ct,
            string             commandData
        )
        {
            await Console.Out.WriteLineAsync( "Ping command request detected." );
            await client.SendTextMessageAsync( update.Message.Chat.Id, "Pong!", cancellationToken: ct );
        }

        [Aliases( "today", "whatistoday" )]
        [Command( "date" )]
        public async Task DateCommand(
            ITelegramBotClient client,
            Update             update,
            CancellationToken  ct,
            string             commandData
        )
        {
            await Console.Out.WriteLineAsync( "Date command request detected." );
            var formattedDate = DateTime.Now.ToString( "dddd, dd MMMM yyyy" );
            await client.SendTextMessageAsync(
                update.Message.Chat.Id,
                $"Today is {formattedDate}",
                cancellationToken: ct
            );
        }

        [Aliases( "info", "information" )]
        [Command( "whoami" )]
        public async Task WhoAmICommand(
            ITelegramBotClient client,
            Update             update,
            CancellationToken  ct,
            string             commandData
        )
        {
            await Console.Out.WriteLineAsync( "Who Am I command detected." );
            await client.SendTextMessageAsync(
                update.Message.Chat.Id,
                $"You are {update.Message.Chat.FirstName} {update.Message.Chat.LastName} of Telegram ID {update.Message.Chat.Id}.",
                cancellationToken: ct
            );
        }
    }
}