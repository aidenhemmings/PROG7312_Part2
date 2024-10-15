using Microsoft.AspNetCore.Mvc; // Necessary for MVC pattern
using Microsoft.AspNetCore.Mvc.RazorPages; // Required for Razor Pages

namespace Part2.Pages
{
    public class LocalEventsModel : PageModel // Defines the page model for Local Events
    {
        public SortedDictionary<DateTime, Event> Events { get; set; } = new SortedDictionary<DateTime, Event>(); // Stores events sorted by date

        public Queue<Event> UpcomingEventsQueue { get; set; } = new Queue<Event>(); // Stores upcoming events in a queue

        public HashSet<string> UniqueCategories { get; set; } = new HashSet<string>(); // Stores unique event categories

        private static Dictionary<string, UserPreferences> UserPreferencesDict = new Dictionary<string, UserPreferences>(); // Stores user preferences for event recommendations

        private const string DefaultUserId = "LocalUser"; // Default user ID for local app usage

        // Method that handles GET requests to the Local Events page
        public void OnGet(string search)
        {
            // List of dummy events to populate the Events dictionary
            var allEvents = new List<Event>
            {
                new Event { Name = "Community Cleanup", Category = "Environment", Date = new DateTime(2024, 10, 20), Description = "Join us for a community cleanup day." },
                new Event { Name = "Local Farmers Market", Category = "Market", Date = new DateTime(2024, 10, 15), Description = "Visit local vendors and buy fresh produce." },
                new Event { Name = "Music in the Park", Category = "Music", Date = new DateTime(2024, 10, 25), Description = "Enjoy live music performances in the park." },
                new Event { Name = "Art Festival", Category = "Art", Date = new DateTime(2024, 11, 5), Description = "A showcase of local artists and their work." },
                new Event { Name = "Food Truck Rally", Category = "Food", Date = new DateTime(2024, 10, 30), Description = "Try delicious food from various food trucks." },
                new Event { Name = "Tech Meetup", Category = "Technology", Date = new DateTime(2024, 11, 1), Description = "Discuss the latest tech trends and network with professionals." },
                new Event { Name = "Outdoor Yoga", Category = "Health", Date = new DateTime(2024, 10, 18), Description = "Join us for a refreshing outdoor yoga session." },
                new Event { Name = "Book Fair", Category = "Literature", Date = new DateTime(2024, 11, 3), Description = "Explore a variety of books from different genres." },
                new Event { Name = "Holiday Parade", Category = "Festivals", Date = new DateTime(2024, 12, 20), Description = "Celebrate the holiday season with a festive parade." },
                new Event { Name = "Film Screening", Category = "Movies", Date = new DateTime(2024, 10, 22), Description = "Watch an exclusive screening of a new indie film." },
                new Event { Name = "Charity Run", Category = "Sports", Date = new DateTime(2024, 11, 10), Description = "Participate in a 5K charity run to support local causes." },
                new Event { Name = "Cooking Workshop", Category = "Food", Date = new DateTime(2024, 11, 7), Description = "Learn new recipes and cooking techniques in this interactive workshop." },
                new Event { Name = "Photography Exhibition", Category = "Art", Date = new DateTime(2024, 11, 15), Description = "A display of stunning photography from local and international artists." },
                new Event { Name = "Startup Pitch Event", Category = "Business", Date = new DateTime(2024, 10, 27), Description = "Watch startups pitch their ideas to investors and industry experts." },
                new Event { Name = "Environmental Summit", Category = "Environment", Date = new DateTime(2024, 11, 12), Description = "Discuss environmental challenges and solutions with industry leaders." }
            };

            // Populate the Events dictionary, upcoming events queue, and unique categories
            foreach (var ev in allEvents)
            {
                Events[ev.Date] = ev; // Add event to the sorted dictionary

                if (ev.Date >= DateTime.Today) // Check if the event is in the future
                {
                    UpcomingEventsQueue.Enqueue(ev); // Enqueue upcoming events
                }

                UniqueCategories.Add(ev.Category); // Add event category to the unique categories set
            }

            // If a search query is provided, filter the events based on the search
            if (!string.IsNullOrEmpty(search))
            {
                DateTime searchDate; // Variable to hold parsed date
                // Try to parse the search string as a date
                bool isDateSearch = DateTime.TryParseExact(search, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out searchDate);

                // Filter events based on category or date
                Events = new SortedDictionary<DateTime, Event>( // Create a new sorted dictionary for filtered events
                    Events.Where(e =>
                        e.Value.Category.Contains(search, StringComparison.OrdinalIgnoreCase) || // Match category
                        (isDateSearch && e.Key.Date == searchDate.Date) // Match date if search is a valid date
                    ).ToDictionary(e => e.Key, e => e.Value) // Convert filtered events to dictionary
                );
            }

            // Set user ID for preferences
            string userId = DefaultUserId;
            // Initialize user preferences if not present
            if (!UserPreferencesDict.ContainsKey(userId))
            {
                UserPreferencesDict[userId] = new UserPreferences();
            }

            // Update user preferences based on search and all events
            UserPreferencesDict[userId].UpdatePreferences(search, allEvents);

            // Generate event recommendations based on user preferences
            var recommendedEvents = GenerateRecommendations(userId);

            // Store the recommended events in ViewData for rendering in the view
            ViewData["RecommendedEvents"] = recommendedEvents;
        }

        // Method to generate event recommendations based on user preferences
        private List<Event> GenerateRecommendations(string userId)
        {
            // Check if user preferences exist for the given user ID
            if (!UserPreferencesDict.ContainsKey(userId))
                return new List<Event>(); // Return empty list if no preferences found

            var preferences = UserPreferencesDict[userId]; // Get user preferences
            // Filter events based on preferred categories and search history
            return Events.Values
                .Where(ev => preferences.PreferredCategories.Contains(ev.Category) || preferences.SearchHistory.Contains(ev.Name))
                .ToList(); // Return the filtered list of recommended events
        }
    }
}
