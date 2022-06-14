using System.Text.RegularExpressions;
using dotenv.net;
using DSharpPlus;
using MoinDiscordBot;

DotEnv.Load();

CancellationTokenSource cts = new();

Console.CancelKeyPress += (sender, args) =>
{
    args.Cancel = true;
    cts.Cancel();
};

var client = new DiscordClient(new DiscordConfiguration()
{
    Token = Environment.GetEnvironmentVariable("DISCORD_TOKEN"),
    TokenType = TokenType.Bot,
    Intents = DiscordIntents.AllUnprivileged
});

var tenor = new Tenor();

client.MessageCreated += async (client, args) =>
{
    if (args.Author.IsCurrent)
    {
        return;
    }

    if (Regex.IsMatch(args.Message.Content, ".*moin.*", RegexOptions.IgnoreCase))
    {
        await args.Message.RespondAsync(tenor.GetGif());
    }
};

await client.ConnectAsync();

while (!cts.IsCancellationRequested)
    await Task.Delay(100);

cts.Dispose();

client.Dispose();