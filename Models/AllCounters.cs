using System.Collections.ObjectModel;

namespace Counter.Models;

internal class AllCounters
{
    public ObservableCollection<CounterItem> Counters { get; set; } = new ObservableCollection<CounterItem>();

    public AllCounters() =>
        LoadCounters();

    public void LoadCounters()
    {
        Counters.Clear();
        string appDataPath = FileSystem.AppDataDirectory;
        IEnumerable<CounterItem> counters = Directory
            .EnumerateFiles(appDataPath, "*.counter.txt")
            .Select(filename =>
            {
                string[] lines = File.ReadAllLines(filename);

                int value = 0;
                if (lines.Length > 1)
                    int.TryParse(lines[1], out value);

                return new CounterItem()
                {
                    Filename = filename,
                    Name = lines.Length > 0 ? lines[0] : "Counter",
                    Value = value
                };
            }).OrderBy(counter => counter.Name);

        foreach (CounterItem counter in counters)
            Counters.Add(counter);
    }
}
