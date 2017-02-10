<Query Kind="Program">
  <NuGetReference>MoreLinq</NuGetReference>
  <NuGetReference>NAudio</NuGetReference>
  <NuGetReference>Newtonsoft.Json</NuGetReference>
  <Namespace>MoreLinq</Namespace>
  <Namespace>Newtonsoft.Json.Linq</Namespace>
  <Namespace>System.Media</Namespace>
</Query>

void Main()
{
    var soundRoot = Util.CurrentQuery.GetLinqPadMetaFolder("soundRoot");
    var path = Path.Combine(soundRoot, @"ACID Loops\Drums & Percussion\One Shots\Rim Shot 2.wav");
    var path2 = Path.Combine(soundRoot, @"ACID Loops\Drums & Percussion\One Shots\Rim Shot 4.wav");

    var players = new[]
    {
        new SoundPlayer(path),
        new SoundPlayer(path2),
    };

    players.ForEach(i =>
    {
        i.Dump();
        try
        {
            i.PlaySync();
        }
        catch(Exception ex)
        {
            ex.Dump();
        }
        finally
        {
            i.Dispose(); 
        }
    });
}