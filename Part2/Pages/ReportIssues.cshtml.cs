using Microsoft.AspNetCore.Http; // Required for handling HTTP requests and form files
using Microsoft.AspNetCore.Mvc; // Required for MVC functionality
using Microsoft.AspNetCore.Mvc.RazorPages; // Required for Razor Pages

namespace Part2.Pages
{
    public class ReportIssuesModel : PageModel // PageModel for the Report Issues page
    {
        // Bind properties to hold user input from the form
        [BindProperty]
        public string Location { get; set; }

        [BindProperty]
        public string Category { get; set; }

        [BindProperty]
        public string Description { get; set; }

        [BindProperty]
        public IFormFile Media { get; set; } // Property to hold the uploaded media file

        // Async method to handle form submission
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) // Check if the model state is valid
            {
                return Page(); // Return the page with validation errors
            }

            // Create a new report as a dictionary to store the report data
            var newReport = new Dictionary<string, object>
            {
                { "Location", Location }, // Add location to the report
                { "Category", Category }, // Add category to the report
                { "Description", Description } // Add description to the report
            };

            // Check if a media file has been uploaded
            if (Media != null && Media.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads"); // Define the uploads folder path
                var fileName = Path.GetFileName(Media.FileName); // Get the file name of the uploaded media
                var filePath = Path.Combine(uploadsFolder, fileName); // Create the full file path

                // Check if the uploads folder exists, if not, create it
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                // Save the uploaded file to the server
                using (var stream = new FileStream(filePath, FileMode.Create)) // Create a file stream to write the file
                {
                    await Media.CopyToAsync(stream); // Asynchronously copy the file to the specified path
                }

                // Add the media path to the report
                newReport.Add("MediaPath", "/uploads/" + fileName);
            }

            // Add the new report to the report storage
            ReportStorage.Reports.Add(newReport);

            // Redirect to the ReportIssues page after successfully submitting the form
            return RedirectToPage("/ReportIssues");
        }
    }
}
