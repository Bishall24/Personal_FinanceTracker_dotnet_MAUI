using System.Text.Json;

namespace Todo.Data;

public static class DebtsService
{
    private static readonly string DebtsFilePath = "debts.json";
    private static readonly string DeletedDebtsFilePath = "deleted_debts.json";


    private static void SaveAll(Guid userId, List<Debt> debts)
    {
        string appDataDirectoryPath = Utils.GetAppDirectoryPath();
        string userDebtsFilePath = GetUserDebtsFilePath(userId);

        if (!Directory.Exists(appDataDirectoryPath))
        {
            Directory.CreateDirectory(appDataDirectoryPath);
        }

        var json = JsonSerializer.Serialize(debts, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(userDebtsFilePath, json);
    }

    private static string GetUserDebtsFilePath(Guid userId)
    {
        return Path.Combine(Utils.GetAppDirectoryPath(), $"{userId}_debts.json");
    }

    public static List<Debt> GetAll(Guid userId)
    {
        string userDebtsFilePath = GetUserDebtsFilePath(userId);
        if (!File.Exists(userDebtsFilePath))
        {
            return new List<Debt>();
        }

        var json = File.ReadAllText(userDebtsFilePath);
        return JsonSerializer.Deserialize<List<Debt>>(json) ?? new List<Debt>();
    }

    public static List<Debt> Create(Guid userId, string taskName, string SourceofDebt, string debtAmount, DateTime dueDate)
    {
        if (dueDate < DateTime.Today)
        {
            throw new Exception("Due date must be in the future.");
        }

        List<Debt> debts = GetAll(userId);
        debts.Add(new Debt
        {
            Id = Guid.NewGuid(),
            TaskName = taskName,
            SourceofDebt = SourceofDebt,
            DebtAmount = debtAmount,
            DueDate = dueDate,
            CreatedBy = userId,
            CreatedAt = DateTime.Now,
            IsDone = false
        });

        SaveAll(userId, debts);
        return debts;
    }

    //public static List<Debt> Delete(Guid userId, Guid id)
    //{
    //    List<Debt> debts = GetAll(userId);
    //    Debt debt = debts.FirstOrDefault(x => x.Id == id);

    //    if (debt == null)
    //    {
    //        throw new Exception("Debt not found.");
    //    }

    //    // Remove from the current debts list
    //    debts.Remove(debt);
    //    debt.IsDone=true;
    //    debts.Add(debt);
    //    SaveAll(userId, debts);

    //    // Save to deleted debts
    //    List<Debt> deletedDebts = GetDeletedDebts();
    //    deletedDebts.Add(debt);
    //    SaveDeletedDebts(deletedDebts);

    //    return debts;
    //}

    public static List<Debt> Delete(Guid userId, Guid id)
    {
        // Fetch all debts for the user
        List<Debt> debts = GetAll(userId);
        // Find the specific debt to clear
        Debt debt = debts.FirstOrDefault(x => x.Id == id);

        if (debt == null)
        {
            throw new Exception("Debt not found.");
        }

        // Remove the debt from the current debts list
        debts.Remove(debt);

        // Save the updated debts list to the file
        SaveAll(userId, debts);

        // Mark the debt as done and save it to the deleted debts list
        debt.IsDone = true;
        List<Debt> deletedDebts = GetDeletedDebts();
        deletedDebts.Add(debt);
        SaveDeletedDebts(deletedDebts);

        // Return the updated debts list
        return debts;
    }


    public static void DeleteByUserId(Guid userId)
    {
        string userDebtsFilePath = GetUserDebtsFilePath(userId);
        if (File.Exists(userDebtsFilePath))
        {
            File.Delete(userDebtsFilePath);
        }
    }

    public static List<Debt> Update(Guid userId, Guid id, string taskName, string SourceofDebt, string debtAmount, DateTime dueDate, bool isDone)
    {
        List<Debt> debts = GetAll(userId);
        Debt debtToUpdate = debts.FirstOrDefault(x => x.Id == id);

        if (debtToUpdate == null)
        {
            throw new Exception("Debt not found.");
        }

        debtToUpdate.TaskName = taskName;
        debtToUpdate.SourceofDebt = SourceofDebt;
        debtToUpdate.DebtAmount = debtAmount;
        debtToUpdate.DueDate = dueDate;
        debtToUpdate.IsDone = isDone;

        SaveAll(userId, debts);
        return debts;
    }
    private static void SaveDeletedDebts(List<Debt> deletedDebts)
    {

        System.Diagnostics.Debug.WriteLine($"Number of deleted debts: {deletedDebts.Count}");

        string appDataDirectoryPath = Utils.GetAppDirectoryPath();
        System.Diagnostics.Debug.WriteLine($"App Directory Path in SaveDeletedDebts: {appDataDirectoryPath}");
        string fullPath = Path.Combine(appDataDirectoryPath, DeletedDebtsFilePath);

        if (!Directory.Exists(appDataDirectoryPath))
        {
            System.Diagnostics.Debug.WriteLine("Directory does not exist. Creating...");
            Directory.CreateDirectory(appDataDirectoryPath);
            System.Diagnostics.Debug.WriteLine("Directory created.");
        }

        var json = JsonSerializer.Serialize(deletedDebts, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(fullPath, json);
        
    }

    // Get deleted debts from JSON
    public static List<Debt> GetDeletedDebts()
    {


        string fullPath = Path.Combine(Utils.GetAppDirectoryPath(), DeletedDebtsFilePath);
        // Debug log added here to output the resolved path
        System.Diagnostics.Debug.WriteLine($"Deleted Debts File Path: {fullPath}");

        if (!File.Exists(fullPath))
        {
            

            return new List<Debt>();
        }

        var json = File.ReadAllText(fullPath);
        
        return JsonSerializer.Deserialize<List<Debt>>(json) ?? new List<Debt>();
        System.Diagnostics.Debug.WriteLine($"Deleted Debts File Path: {fullPath}");
    }

    public class DebtStateService
    {
        private List<Debt> _deletedDebts = new List<Debt>();

        public List<Debt> GetDeletedDebts() => _deletedDebts;

        public void ClearDeletedDebts() => _deletedDebts.Clear();
    }

}

