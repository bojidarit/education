public class TodoItem
{
    public TodoItem()
    {
        Title = string.Empty;
    }

    public string Title { get; set; }

    public bool IsDone { get; set; }

    public static TodoItem Create(string title) => 
        new TodoItem(){ Title = title };
}