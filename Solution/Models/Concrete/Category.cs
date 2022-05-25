namespace Solution.Models.Concrete;

public class Category: Entity
{
    private string _name;
    public string Name
    {
        get => _name;
        set { _name = value; OnPropertyChanged(); }
    }
}
