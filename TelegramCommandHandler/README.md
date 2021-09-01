# Telegram Command Handler

A better way to detect and handle commands sent to a telegram client.

## Requirements

* [Telegram.Bot library](https://github.com/TelegramBots/Telegram.Bot)
* .NET 5.0 or higher

## Usage 

### Program.cs

Register your command classs(es) using `commandHandler.RegisterCommands<Class>` where `Class` is a subclass of `CommandModule`, then initialize the `commandHandler` with your `botClient`.

```csharp
var botClient = new TelegramBotClient( "<token>" );

var commandHandler = new TelegramCommandHandler();
commandHandler.RegisterCommands<Commands>();

// Type objects may be used instead of generics.
// commandHandler.RegisterCommands( typeof( Commands ) );

// start receiving commands using the new polling extensions.
var updateHandler = new CommandUpdateHandler( commandHandler );
await botClient.ReceiveAsync( updateHandler );
```
### Commands.cs

```csharp
public class Commands : CommandModule
{
    // All command methods MUST have the following signature:
    // Task <command>( ITelegramBotClient client, Update update, CancellationToken ct, string commandData );
    // where `commandData` is a string containing everything after the command (i.e. /ping), if present.
    [Command( "ping" )]
    public Task Ping( ITelegramBotClient client, Update update, CancellationToken ct, string commandData )
    {
        return client.SendTextMessageAsync( update.Message.Chat.Id, "Pong!", cancellationToken: ct );
    }
}
```
## Extras

### Prefix

You can change the `/` prefix to anything you want by passing a paremeter to the `TelegramCommandHandler` initialization, like so:

```csharp
TelegramCommandHandler commandHandler = new TelegramCommandHandler("!");
```

This will cause `!` to invoke your commands, instead of `/`.

### Pre/Post-command execution

```csharp
public override async Task BeforeExecutionAsync( ITelegramBotClient client, Update update, CancellationToken ct )
{
    // Will be called before execution of a method
}

public override async Task AfterExecutionAsync( ITelegramBotClient client, Update update, CancellationToken ct )
{
    // Will be called after executionn of a method
}
```
