using static Part2.Pages.LocalEventsModel; // Import the LocalEventsModel class for use within this class

namespace Part2.Pages
{
    public class UserPreferences // Class to store user preferences for events
    {
        // Property to store preferred event categories
        public HashSet<string> PreferredCategories { get; set; } = new HashSet<string>();

        // Property to store user's search history
        public HashSet<string> SearchHistory { get; set; } = new HashSet<string>();

        // Method to update user preferences based on a search query and a list of events
        public void UpdatePreferences(string search, List<Event> events)
        {
            // Check if the search query is not empty
            if (!string.IsNullOrEmpty(search))
            {
                // Add the search query to the search history
                SearchHistory.Add(search);

                // Find events that match the search query by category
                var matchingEvents = events.Where(e => e.Category.Equals(search, StringComparison.OrdinalIgnoreCase)).ToList();

                // Loop through the matching events and add their categories to the preferred categories
                foreach (var ev in matchingEvents)
                {
                    PreferredCategories.Add(ev.Category);
                }
            }
        }
    }
}
