namespace Solution;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        
        var view = new Views.MainView()
        {
            DataContext = new ViewModels.MainViewModel()
        };

        view.ShowDialog();
    }
}
