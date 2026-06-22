using Assignment1.Models;
using Assignment1.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel;

namespace Assignment1.Pages;

public class EditModel : PageModel
{
    // インターフェースのDI(下のコンストラクタとセット)
    private readonly ISqlRepository _sqlrepositry;

    public EditModel(ISqlRepository sqlrepositry)
    {
        _sqlrepositry = sqlrepositry;
    }

    [BindProperty]
    public TaskModel EditTask { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }
            try
        {
            EditTask = await _sqlrepositry.GetEdit(id);

            if (EditTask == null)
            {
                return NotFound();
            }

        }
        catch (Exception ex)
        {
            //DB異常発生があれば404に遷移
            return StatusCode(500);
        }
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        // 更新日時の取得
        EditTask.UPDATE_DATETIME = DateTime.Now;

        try
        {
            await _sqlrepositry.UpdateTask(EditTask);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", "DB更新時にエラーが発生しました");
            return Page();
        }
        return RedirectToPage("/Index");

    }
}
