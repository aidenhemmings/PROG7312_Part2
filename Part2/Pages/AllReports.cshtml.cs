using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Part2.Pages
{
    public class AllReportsModel : PageModel
    {
        public List<Dictionary<string, object>> Reports { get; set; }

        public void OnGet()
        {
            Reports = ReportStorage.Reports;
        }
    }
}
