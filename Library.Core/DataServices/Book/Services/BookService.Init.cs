namespace Library.Core.DataServices.Book.Services;

public partial class BookService
{
    private readonly List<(string, string)> _books = new List<(string, string)>
    {
        ("To Kill a Mockingbird", "Harper Lee"),
        ("1984", "George Orwell"),
        ("The Great Gatsby", "F. Scott Fitzgerald"),
        ("Pride and Prejudice", "Jane Austen"),
        ("The Catcher in the Rye", "J.D. Salinger"),
        ("The Hobbit", "J.R.R. Tolkien"),
        ("Harry Potter and the Sorcerer's Stone", "J.K. Rowling"),
        ("The Lord of the Rings", "J.R.R. Tolkien"),
        ("Animal Farm", "George Orwell"),
        ("The Little Prince", "Antoine de Saint-Exupéry"),
        ("Brave New World", "Aldous Huxley"),
        ("The Da Vinci Code", "Dan Brown"),
        ("The Alchemist", "Paulo Coelho"),
        ("Gone with the Wind", "Margaret Mitchell"),
        ("Crime and Punishment", "Fyodor Dostoevsky"),
        ("Catch-22", "Joseph Heller"),
        ("Lord of the Flies", "William Golding"),
        ("The Shining", "Stephen King"),
        ("The Bell Jar", "Sylvia Plath"),
        ("One Hundred Years of Solitude", "Gabriel García Márquez"),
        ("Moby-Dick", "Herman Melville"),
        ("The Picture of Dorian Gray", "Oscar Wilde"),
        ("Dracula", "Bram Stoker"),
        ("The Grapes of Wrath", "John Steinbeck"),
        ("A Tale of Two Cities", "Charles Dickens"),
        ("The Hitchhiker's Guide to the Galaxy", "Douglas Adams"),
        ("War and Peace", "Leo Tolstoy"),
        ("The Hunger Games", "Suzanne Collins"),
        ("Frankenstein", "Mary Shelley"),
        ("The Chronicles of Narnia", "C.S. Lewis"),
        ("The Road", "Cormac McCarthy"),
        ("Slaughterhouse-Five", "Kurt Vonnegut"),
        ("Anna Karenina", "Leo Tolstoy"),
        ("The Sun Also Rises", "Ernest Hemingway"),
        ("Wuthering Heights", "Emily Brontë"),
        ("The Odyssey", "Homer"),
        ("A Clockwork Orange", "Anthony Burgess"),
        ("The Brothers Karamazov", "Fyodor Dostoevsky"),
        ("The Secret Garden", "Frances Hodgson Burnett"),
        ("Les Misérables", "Victor Hugo"),
        ("Lolita", "Vladimir Nabokov"),
        ("Anne of Green Gables", "L.M. Montgomery"),
        ("The Stand", "Stephen King"),
        ("The Girl with the Dragon Tattoo", "Stieg Larsson"),
        ("The Stranger", "Albert Camus"),
        ("The Old Man and the Sea", "Ernest Hemingway"),
        ("Little Women", "Louisa May Alcott"),
        ("A Farewell to Arms", "Ernest Hemingway"),
        ("The Metamorphosis", "Franz Kafka"),
        ("Of Mice and Men", "John Steinbeck"),
        ("The Count of Monte Cristo", "Alexandre Dumas"),
        ("The Color Purple", "Alice Walker"),
        ("Fahrenheit 451", "Ray Bradbury"),
        ("The Wind-Up Bird Chronicle", "Haruki Murakami"),
        ("The Kite Runner", "Khaled Hosseini"),
        ("The Joy Luck Club", "Amy Tan"),
        ("The Divine Comedy", "Dante Alighieri"),
        ("The Road Less Traveled", "M. Scott Peck"),
        ("The Martian", "Andy Weir"),
        ("Watership Down", "Richard Adams"),
        ("Charlotte's Web", "E.B. White"),
        ("The Bell Jar", "Sylvia Plath"),
        ("Beloved", "Toni Morrison"),
        ("The Handmaid's Tale", "Margaret Atwood"),
        ("Where the Red Fern Grows", "Wilson Rawls"),
        ("The Outsiders", "S.E. Hinton"),
        ("The Secret History", "Donna Tartt"),
        ("East of Eden", "John Steinbeck"),
        ("The Night Circus", "Erin Morgenstern"),
        ("The Goldfinch", "Donna Tartt"),
        ("The Name of the Wind", "Patrick Rothfuss"),
        ("The Giver", "Lois Lowry"),
        ("The Help", "Kathryn Stockett"),
        ("The Cider House Rules", "John Irving"),
        ("Jurassic Park", "Michael Crichton"),
        ("Memoirs of a Geisha", "Arthur Golden"),
        ("The Lovely Bones", "Alice Sebold"),
        ("The Fault in Our Stars", "John Green"),
        ("The Glass Castle", "Jeannette Walls"),
        ("The Road", "Cormac McCarthy"),
        ("The Art of Racing in the Rain", "Garth Stein"),
        ("Life of Pi", "Yann Martel"),
        ("The Book Thief", "Markus Zusak"),
        ("The Help", "Kathryn Stockett"),
        ("The Curious Incident of the Dog in the Night-Time", "Mark Haddon"),
        ("A Wrinkle in Time", "Madeleine L'Engle"),
        ("The Ocean at the End of the Lane", "Neil Gaiman"),
        ("The Martian Chronicles", "Ray Bradbury"),
        ("The Night Circus", "Erin Morgenstern"),
        ("The Goldfinch", "Donna Tartt"),
        ("American Gods", "Neil Gaiman"),
        ("The Kite Runner", "Khaled Hosseini"),
        ("The Name of the Wind", "Patrick Rothfuss"),
        ("The Giver", "Lois Lowry"),
        ("The Picture of Dorian Gray", "Oscar Wilde"),
        ("The Help", "Kathryn Stockett"),
        ("The Cider House Rules", "John Irving")
    };

    public override async Task InitializeAsync()
    {
        var count = await CountAsync();
        
        if(count > 0)
        {
            return;
        }
        
        var entities = _books.Select(b => new Data.Entities.Book
        {
            Title = b.Item1,
            Author = b.Item2
        });

        await BulkInsertAsync(entities);
    }
}