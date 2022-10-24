#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Esame.Pages
{
    public class NewConnectionModel : PageModel
    {

        [BindProperty]
        public Provider Input { get; set; }

        
        public void OnGet()
        {

        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
           
            if(Input.provider.Equals(DbProvider.SQLite))
            {
                return RedirectToPage("./FormSQLite");
            }
            else
            {
                return RedirectToPage("./FormPostgres");
            }
        }
    }
}
