
using System.Text.Json;

namespace Todo.Data;

public static class TodosService
{
    private static void SaveAll(Guid userId, List<Transaction> todos)
    {
        string appDataDirectoryPath = Utils.GetAppDirectoryPath();
        string todosFilePath = Utils.GetTodosFilePath(userId);

        if (!Directory.Exists(appDataDirectoryPath))
        {
            Directory.CreateDirectory(appDataDirectoryPath);
        }

        var json = JsonSerializer.Serialize(todos);
        File.WriteAllText(todosFilePath, json);
    }

    public static List<Transaction> GetAll(Guid userId)
    {
        string todosFilePath = Utils.GetTodosFilePath(userId);
        if (!File.Exists(todosFilePath))
        {
            return new List<Transaction>();
        }

        var json = File.ReadAllText(todosFilePath);
        return JsonSerializer.Deserialize<List<Transaction>>(json);
    }

    public static List<Transaction> GetTransactionsByType(Guid userId, string transactionType)
    {
        List<Transaction> todos = GetAll(userId);
        return todos.Where(t => t.TaskName == transactionType).ToList();
    }

    public static List<Transaction> Create(Guid userId, string taskName, string Amount, string Notes, string Tag, DateTime dueDate)
    {
        if (dueDate < DateTime.Today)
        {
            throw new Exception("Due date must be in the future.");
        }

        List<Transaction> todos = GetAll(userId);
        todos.Add(new Transaction
        {
            Id = Guid.NewGuid(),
            TaskName = taskName, // "Inflows" or "Outflows"
            Amount = Amount,
            Notes = Notes,
            Tag = Tag,
            DueDate = dueDate,
            CreatedBy = userId,
            CreatedAt = DateTime.Now
        });
        SaveAll(userId, todos);
        return todos;
    }

    public static List<Transaction> Delete(Guid userId, Guid id)
    {
        List<Transaction> todos = GetAll(userId);
        Transaction todo = todos.FirstOrDefault(x => x.Id == id);

        if (todo == null)
        {
            throw new Exception("Todo not found.");
        }

        todos.Remove(todo);
        SaveAll(userId, todos);
        return todos;
    }

    public static void DeleteByUserId(Guid userId)
    {
        string todosFilePath = Utils.GetTodosFilePath(userId);
        if (File.Exists(todosFilePath))
        {
            File.Delete(todosFilePath);
        }
    }

    public static List<Transaction> Update(Guid userId, Guid id, string taskName, string Amount, string Notes, string Tag, DateTime dueDate, bool isDone)
    {
        List<Transaction> todos = GetAll(userId);
        Transaction todoToUpdate = todos.FirstOrDefault(x => x.Id == id);

        if (todoToUpdate == null)
        {
            throw new Exception("Todo not found.");
        }

        todoToUpdate.TaskName = taskName;
        todoToUpdate.Amount = Amount;
        todoToUpdate.Notes = Notes;
        todoToUpdate.Tag = Tag;
        todoToUpdate.IsDone = isDone;
        todoToUpdate.DueDate = dueDate;
        SaveAll(userId, todos);
        return todos;
    }
}

