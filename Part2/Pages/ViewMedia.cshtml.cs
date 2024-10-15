using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Part2.Pages
{
    public class ViewMediaModel : PageModel
    {
        public string MediaPath { get; private set; }
        public void OnGet(string mediaPath)
        {
            MediaPath = mediaPath;
        }
    }
}
