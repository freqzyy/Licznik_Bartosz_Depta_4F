namespace Counter.Views;

[QueryProperty(nameof(ItemId), nameof(ItemId))]
public partial class CounterPage : ContentPage
{
    public string ItemId
    {
        set { LoadCounter(value); }
    }

    public CounterPage()
    {
        InitializeComponent();

        string appDataPath = FileSystem.AppDataDirectory;
        string randomFileName = $"{Path.GetRandomFileName()}.counter.txt";

        LoadCounter(Path.Combine(appDataPath, randomFileName));
    }

    private void LoadCounter(string fileName)
    {
        Models.CounterItem counterModel = new Models.CounterItem();
        counterModel.Filename = fileName;
        counterModel.Name = string.Empty;
        counterModel.Value = 0;

        if (File.Exists(fileName))
        {
            string[] lines = File.ReadAllLines(fileName);

            if (lines.Length > 0)
                counterModel.Name = lines[0];

            if (lines.Length > 1 && int.TryParse(lines[1], out int value))
                counterModel.Value = value;
        }

        BindingContext = counterModel;
        ValueLabel.Text = counterModel.Value.ToString();
    }

    private void PlusButton_Clicked(object sender, EventArgs e)
    {
        if (BindingContext is Models.CounterItem counter)
        {
            counter.Value++;
            ValueLabel.Text = counter.Value.ToString();
        }
    }

    private void MinusButton_Clicked(object sender, EventArgs e)
    {
        if (BindingContext is Models.CounterItem counter)
        {
            counter.Value--;
            ValueLabel.Text = counter.Value.ToString();
        }
    }

    private async void SaveButton_Clicked(object sender, EventArgs e)
    {
        if (BindingContext is Models.CounterItem counter)
        {
            string name = NameEditor.Text;
            if (string.IsNullOrWhiteSpace(name))
                name = "Counter";
            counter.Name = name.Replace("\r", " ").Replace("\n", " ").Trim();
            File.WriteAllText(counter.Filename, counter.Name + Environment.NewLine + counter.Value);
        }

        await Shell.Current.GoToAsync("..");
    }

    private async void DeleteButton_Clicked(object sender, EventArgs e)
    {
        if (BindingContext is Models.CounterItem counter)
        {
            if (File.Exists(counter.Filename))
                File.Delete(counter.Filename);
        }

        await Shell.Current.GoToAsync("..");
    }
}
