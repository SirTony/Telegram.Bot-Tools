using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace Telegram.Bot.Interactivity.Commands
{
    /// <summary>
    ///     Base class for command modules.
    /// </summary>
    public abstract class CommandModule
    {
        /// <summary>
        ///     Invoked before to command execution.
        /// </summary>
        /// <param name="client">The current Telegram bot client.</param>
        /// <param name="update">The <see cref="Update" /> that triggered the command execution.</param>
        /// <param name="ct">Async cancellation support.</param>
        public virtual Task BeforeExecutionAsync( ITelegramBotClient client, Update update, CancellationToken ct )
            => Task.Delay( 0 );

        /// <summary>
        ///     Invoked after command execution.
        /// </summary>
        /// <param name="client">The current Telegram bot client.</param>
        /// <param name="update">The <see cref="Update" /> that triggered the command execution.</param>
        /// <param name="ct">Async cancellation support.</param>
        public virtual Task AfterExecutionAsync( ITelegramBotClient client, Update update, CancellationToken ct )
            => Task.Delay( 0 );
    }
}