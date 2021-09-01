using System.Threading;
using System.Threading.Tasks;
using Interactivity.Extensions;
using Telegram.Bot;
using Telegram.Bot.Interactivity.Commands;
using Telegram.Bot.Types;

namespace Sample.CommandModules
{
    public class InteractivityCommands : CommandModule
    {
        [Command( "conversation" )]
        public async Task StartConversation(
            ITelegramBotClient client,
            Update             update,
            CancellationToken  ct,
            string             commandData
        )
        {
            if( client is not TelegramBotClient botClient ) return;

            // Ask the user what's their name.
            await client.SendTextMessageAsync(
                update.Message.Chat.Id,
                "Hello! What's your name?",
                cancellationToken: ct
            );

            // Wait for a result.
            var result = await botClient.GetInteractivity()
                                        .WaitForMessageAsync( update.Message.Chat, update.Message.From );
            if( result.Value == null )
            {
                //Timed out
                await client.SendTextMessageAsync(
                    update.Message.Chat.Id,
                    "Timed out. Please try again.",
                    cancellationToken: ct
                );
            }
            else
            {
                //Didn't time out.
                //Get the message
                var message = result.Value;
                //Get the bot's user.
                var me = await client.GetMeAsync();
                //Respond to the command.
                await client.SendTextMessageAsync(
                    update.Message.Chat.Id,
                    $"Hello, {message.Text}! I am {me.FirstName}.",
                    cancellationToken: ct
                );
            }
        }
    }
}