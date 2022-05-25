namespace Solution.Models.Concrete;

public class Book: Entity
{
    private string _name;
    public string Name
    {
        get => _name;
        set { _name = value; OnPropertyChanged(); }
    }

    private string _comment;
    public string Comment
    {
        get => _comment;
        set { _comment = value; OnPropertyChanged(); }
    }

    private string _publisherName;
    public string PublisherName
    {
        get => _publisherName;
        set { _publisherName = value; OnPropertyChanged(); }
    }

    private int _pages;
    public int Pages
    {
        get => _pages;
        set { _pages = value; OnPropertyChanged(); }
    }

    private int _quantity;
    public int Quantity
    {
        get => _quantity;
        set { _quantity = value; OnPropertyChanged(); }
    }

    private int _yearPress;
    public int YearPress
    {
        get => _yearPress;
        set { _yearPress = value; OnPropertyChanged(); }
    }
}
