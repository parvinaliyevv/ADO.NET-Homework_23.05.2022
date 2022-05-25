using Solution.Commands;
using Solution.Models.Concrete;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Controls;

namespace Solution.ViewModels;

public class MainViewModel
{
    private readonly SqlConnection _dbConnection;


    public ObservableCollection<Author> Authors { get; set; }
    public ObservableCollection<Category> Categories { get; set; }
    public ObservableCollection<Book> Books { get; set; }

    public Author SelectedAuthor { get; set; }
    public Category SelectedCategory { get; set; }

    public string SearchValue { get; set; }

    public RelayCommand AuthorSelectedChangedCommand { get; set; }
    public RelayCommand CategorySelectedChangedCommand { get; set; }
    public RelayCommand SearchBooksCommand { get; set; }


    public RelayCommand UpdateBookCommand { get; set; }
    public RelayCommand DeleteBookCommand { get; set; }


    public MainViewModel()
    {
        Authors = new ObservableCollection<Author>();
        Categories = new ObservableCollection<Category>();
        Books = new ObservableCollection<Book>();

        _dbConnection = new(ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString);

        AuthorSelectedChangedCommand = new(sender => ParseBooks());
        CategorySelectedChangedCommand = new(sender => ParseBooks());
        SearchBooksCommand = new(sender => SearchBook());

        UpdateBookCommand = new(sender => UpdateBook((sender as DataGridCellInfo?)?.Item as Book));
        DeleteBookCommand = new(sender => DeleteBook((sender as DataGridCellInfo?)?.Item as Book));


        string command = "SELECT Id, FirstName + ' ' + LastName as FullName FROM Authors; SELECT * FROM Categories";

        var dataAdapter = new SqlDataAdapter(command, _dbConnection);

        var dataSet = new DataSet();
        dataAdapter.Fill(dataSet);

        var authorsTable = dataSet.Tables[0];
        var categoriesTable = dataSet.Tables[1];

        foreach (DataRow author in authorsTable.Rows)
            Authors.Add(new() { Id = Convert.ToInt32(author["Id"]), Name = Convert.ToString(author["FullName"]) });

        foreach (DataRow category in categoriesTable.Rows)
            Categories.Add(new() { Id = Convert.ToInt32(category["Id"]), Name = Convert.ToString(category["Name"]) });

        SelectedAuthor = Authors[0];
        SelectedCategory = Categories[0];

        ParseBooks();
    }

  
    public void ParseBooks()
    {
        string command = @"SELECT Books.Id, Books.Name, Press.Name as PublisherName, Pages, YearPress, Quantity, Comment
                         FROM Books
                         LEFT JOIN Press ON Press.Id = Books.Id
                         WHERE Id_Author = @authorId and Id_Category = @categoryId";


        var dataAdapter = new SqlDataAdapter(command, _dbConnection);

        dataAdapter.SelectCommand.Parameters.Add("@authorId", SqlDbType.Int).Value = SelectedAuthor.Id;
        dataAdapter.SelectCommand.Parameters.Add("@categoryId", SqlDbType.Int).Value = SelectedCategory.Id;

        var dataSet = new DataSet();
        dataAdapter.Fill(dataSet);

        var books = dataSet.Tables[0];
        Books.Clear();

        foreach (DataRow book in books.Rows)
        {
            Books.Add(new()
            {
                Id = Convert.ToInt32(book["Id"]),
                Name = Convert.ToString(book["Name"]),
                PublisherName = Convert.ToString(book["PublisherName"]),
                Pages = Convert.ToInt32(book["Pages"]),
                YearPress = Convert.ToInt32(book["YearPress"]),
                Quantity = Convert.ToInt32(book["Quantity"]),
                Comment = Convert.ToString(book["Comment"])
            });
        }
            
    }

    public void SearchBook()
    {
        if (string.IsNullOrWhiteSpace(SearchValue)) ParseBooks();
        else
        {
            var books = new List<Book>();

            foreach (var item in Books)
                if (!item.Name.Contains(SearchValue)) books.Add(item);

            foreach (var item in books)
                Books.Remove(item);
        }
    }

    private void UpdateBook(Book book)
    {
        if (book is null) return;

        try
        {
            _dbConnection.Open();

            var command = new SqlCommand(@"UPDATE Books
                                           SET Books.Name = @bookName,
                                           Books.Pages = @bookPage,
                                           Books.YearPress = @bookYearPress,
                                           Books.Quantity = @bookQuantity,
                                           Books.Comment = @BookComment
                                           WHERE Books.Id = @bookId", _dbConnection);


            command.Parameters.AddWithValue("@bookId", book.Id);
            command.Parameters.AddWithValue("@bookName", book.Name);
            command.Parameters.AddWithValue("@bookPage", book.Pages);
            command.Parameters.AddWithValue("@bookYearPress", book.YearPress);
            command.Parameters.AddWithValue("@bookQuantity", book.Quantity);
            command.Parameters.AddWithValue("@bookComment", book.Comment);

            command.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
        finally
        {
            _dbConnection.Close();
        }
    }

    private void DeleteBook(Book book)
    {
        if (book is null) return;

        try
        {
            _dbConnection.Open();

            var command = new SqlCommand("DELETE FROM Books WHERE Books.Id = @bookId", _dbConnection);

            command.Parameters.AddWithValue("@bookId", book.Id);

            command.ExecuteNonQuery();

            Books.Remove(book);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
        finally
        {
            _dbConnection.Close();
        }
    }
}
