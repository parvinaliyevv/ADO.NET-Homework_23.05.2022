namespace Solution.Models.Concrete;

public class Book: Entity
{
    public string Name { get; set; }
    public string Comment { get; set; }

    public string PublisherName { get; set; }

    public int Pages { get; set; }
    public int Quantity { get; set; }
    public int YearPress { get; set; }
}
