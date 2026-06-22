using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Assignment1.Models;
using Assignment1.Repositories;

namespace Assignment1.Pages;

public class IndexModel : PageModel
{
    // インターフェースのDI(下のコンストラクタとセット)
    private readonly ISqlRepository _sqlrepositry;

    public IndexModel(ISqlRepository sqlrepositry)
    {
        _sqlrepositry = sqlrepositry;
    }

    public IEnumerable<TaskModel> Tasks { get; set; }

    [BindProperty(SupportsGet = true)]
    public int Filter { get; set; }

    [BindProperty(SupportsGet = true)]
    public string? UserName { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        try
        {
            // SQLを実行(インターフェースから呼び出し)
            var tasklist = await _sqlrepositry.GetIndex();
            var task = new List<TaskModel>();

            switch (Filter)
            {
                case 1:
                    tasklist = tasklist.Where(t=> t.STATUS == "未着手");
                    break;
                case 2:
                    tasklist = tasklist.Where(t => t.STATUS == "進行中");
                    break;
                case 3:
                    tasklist = tasklist.Where(t => t.STATUS == "完了");
                    break;
                default:
                    break;
            }

            if(!string.IsNullOrWhiteSpace(UserName))
            {
                tasklist = tasklist.Where(t => t.ASSIGNEE.Contains(UserName));
            }

            Tasks = tasklist.ToList();
        }
        catch (Exception ex)
        {
            // DB異常発生があれば異常画面表示
            return StatusCode(500);

        }
        return Page();
    }
}
