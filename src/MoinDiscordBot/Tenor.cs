namespace MoinDiscordBot;

public class Tenor
{
    private readonly static TenorClient tenorClient = new TenorClient();
    private IList<string> gifUrls;
    private int? previousIndex;

    public Tenor()
    {
        gifUrls = tenorClient.GifUrls;
    }

    public string GetGif()
    {
        int index = Random.Shared.Next(0, 51);

        if (previousIndex.HasValue)
        {
            while (index == previousIndex.Value)
            {
                index = Random.Shared.Next(0, 51);
            }
        }
        else
        {
            previousIndex = index;
        }

        return gifUrls[index];
    }

}