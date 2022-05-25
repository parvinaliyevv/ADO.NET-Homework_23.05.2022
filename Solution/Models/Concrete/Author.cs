namespace Solution.Models.Concrete;

public class Author : Entity
{
    private string _name;
    public string Name
    {
        get => _name;
        set { _name = value; OnPropertyChanged(); }
    }
}
