using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using VenuoliktaPaskaita.Models;

namespace VenuoliktaPaskaita
{
    class Program
    {
        static void Main(string[] args)
        {

            while (true)
            {
                Console.WriteLine("Choose an option:");
                Console.WriteLine("1. List all authors");
                Console.WriteLine("2. List all books");
                Console.WriteLine("3. Add a new book");
                Console.WriteLine("4. Update a book");
                Console.WriteLine("5. Delete a book");
                Console.WriteLine("0. Exit");

                int choice = int.Parse(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        GetAllAuthors();
                        break;
                    case 2:
                        GetAllBooks();
                        break;
                    case 3:
                        Console.WriteLine("Enter book title:");
                        string title = Console.ReadLine();
                        Console.WriteLine("Enter publication year:");
                        int year = int.Parse(Console.ReadLine());
                        Console.WriteLine("Enter genre:");
                        string genre = Console.ReadLine();
                        Console.WriteLine("Enter author ID:");
                        int authorId = int.Parse(Console.ReadLine());
                        AddBook(title, year, genre, authorId);
                        break;
                    case 4:
                        Console.WriteLine("Enter book ID to update:");
                        int updateId = int.Parse(Console.ReadLine());
                        Console.WriteLine("Enter new title:");
                        string newTitle = Console.ReadLine();
                        Console.WriteLine("Enter new publication year:");
                        int newYear = int.Parse(Console.ReadLine());
                        Console.WriteLine("Enter new genre:");
                        string newGenre = Console.ReadLine();
                        Console.WriteLine("Enter new author ID:");
                        int newAuthorId = int.Parse(Console.ReadLine());
                        UpdateBook(updateId, newTitle, newYear, newGenre, newAuthorId);
                        break;
                    case 5:
                        Console.WriteLine("Enter book ID to delete:");
                        int deleteId = int.Parse(Console.ReadLine());
                        DeleteBook(deleteId);
                        break;
                    case 0:
                        return;
                    default:
                        Console.WriteLine("Invalid option, please try again.");
                        break;
                }
            }
            // Example usage
           // GetAllAuthors();
          //  GetAllBooks();
            // Add your methods for adding, updating, deleting, and getting lists of books here.
        }

        static void GetAllAuthors()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM Author";
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();

                List<Author> authors = new List<Author>();
                while (reader.Read())
                {
                    authors.Add(new Author
                    {
                        Id = (int)reader["Id"],
                        FirstName = reader["First_Name"].ToString(),
                        LastName = reader["Last_Name"].ToString(),
                        BirthDate = (DateTime)reader["BirthDate"],
                        Country = reader["Country"].ToString()
                    });
                }

                reader.Close();

                foreach (var author in authors)
                {
                    Console.WriteLine($"{author.FirstName} {author.LastName}, {author.Country}, Author ID {author.Id}");

                }
            }
        }
        static string connectionString = "Server=LAPTOP-2I3V8F0J;Database=LibraryDB;integrated security = true;";
        static void AddBook(string title, int publicationYear, string genre, int authorId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "INSERT INTO Books (Title, PublicationYear, Genre, AuthorId) VALUES (@Title, @PublicationYear, @Genre, @AuthorId)";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Title", title);
                command.Parameters.AddWithValue("@PublicationYear", publicationYear);
                command.Parameters.AddWithValue("@Genre", genre);
                command.Parameters.AddWithValue("@AuthorId", authorId);

                command.ExecuteNonQuery();
            }
        }

        static void UpdateBook(int id, string title, int publicationYear, string genre, int authorId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "UPDATE Books SET Title = @Title, PublicationYear = @PublicationYear, Genre = @Genre, AuthorId = @AuthorId WHERE Id = @Id";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", id);
                command.Parameters.AddWithValue("@Title", title);
                command.Parameters.AddWithValue("@PublicationYear", publicationYear);
                command.Parameters.AddWithValue("@Genre", genre);
                command.Parameters.AddWithValue("@AuthorId", authorId);

                command.ExecuteNonQuery();
            }
        }

        static void DeleteBook(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "DELETE FROM Books WHERE Id = @Id";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", id);

                command.ExecuteNonQuery();
            }
        }

        static void GetAllBooks()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM Books";
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();

                List<Book> books = new List<Book>();
                while (reader.Read())
                {
                    books.Add(new Book
                    {
                        Id = (int)reader["Id"],
                        Title = reader["Title"].ToString(),
                        PublicationYear = (int)reader["PublicationYear"],
                        Genre = reader["Genre"].ToString(),
                        AuthorId = (int)reader["AuthorId"]
                    });
                }

                reader.Close();

                foreach (var book in books)
                {
                    Console.WriteLine($"{book.Title}, {book.PublicationYear}, {book.Genre}, Book ID: {book.Id}");
                }
            }
        }

        

        // Add methods for adding, updating, deleting, and getting lists of books and book copies.
    }

}