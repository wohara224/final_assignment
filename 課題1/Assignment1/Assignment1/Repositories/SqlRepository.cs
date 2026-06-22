using Assignment1.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace Assignment1.Repositories;

public class SqlRepository : ISqlRepository
{
    private readonly IConfiguration _config;

    // DI
    public SqlRepository(IConfiguration config)
    {
        _config = config;
    }

    public async Task<IEnumerable<TaskModel>> GetIndex()
    {
        using var connection
            = new SqlConnection(_config.GetConnectionString("DefaultConnection"));

        var sql = @"
            SELECT 
                TASK_ID,
                TASK_NAME,
                ASSIGNEE,
                DUE_DATE,
                STATUS,
                CREATE_DATETIME,
                UPDATE_DATETIME
            FROM TASKS
            ORDER BY TASK_ID;
            ";

        var tasks = await connection.QueryAsync<TaskModel>(sql);
        return tasks;
    }

    public async Task<TaskModel?> GetEdit(int TaskId)
    {
        using var connection
            = new SqlConnection(_config.GetConnectionString("DefaultConnection"));

        var sql = @"
            SELECT 
                TASK_ID,
                TASK_NAME,
                ASSIGNEE,
                DUE_DATE,
                STATUS,
                CREATE_DATETIME,
                UPDATE_DATETIME
            FROM TASKS
            WHERE TASK_ID = @TaskId;
            ";

        var task = await connection.QueryFirstOrDefaultAsync<TaskModel>(sql, new { TaskId });
        return task;
    }

    public async Task UpdateTask(TaskModel Task)
    {
        using var connection
            = new SqlConnection(_config.GetConnectionString("DefaultConnection"));

        var sql = @"
            UPDATE TASKS
            SET
                TASK_NAME = @TASK_NAME,
                ASSIGNEE = @ASSIGNEE,
                DUE_DATE = @DUE_DATE,
                STATUS = @STATUS,
                UPDATE_DATETIME = @UPDATE_DATETIME
            WHERE TASK_ID = @TASK_ID;
            ";

        await connection.ExecuteAsync(sql, Task);
    }
}
