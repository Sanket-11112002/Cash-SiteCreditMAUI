using System.Text.Json;
using CardGameCorner.Models;

namespace CardGameCorner.Services
{
    public class ListBoxService : IListboxService
    {

        private readonly GlobalSettingsService _globalSettings;
        public string SelectedLanguage;
        public string SelectedGame;

        public GlobalSettingsService GlobalSettings => GlobalSettingsService.Current;

        public ListBoxService()
        {
            _globalSettings = GlobalSettingsService.Current;
        }


        public async Task<List<LanguageModal>> GetLanguagesAsync()
        {
            SelectedLanguage = _globalSettings.SelectedLanguage;
            SelectedGame = _globalSettings.SelectedGame;
            try
            {
                string language = SelectedLanguage == "English" ? "en" : SelectedLanguage == "Italian" ? "it" : "en";
                string game = SelectedGame;

                // Construct the URL with selected language and game
                string url = $"https://api.magiccorner.it/api/mclistboxes/{game}/{language}";

                //string url = "https://api.magiccorner.it/api/mclistboxes/pokemon/en";

                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", "0d1bb073-9dfb-4c6d-a1c0-1e8f7d5d8e9f");
                var response = await httpClient.GetStringAsync(url);

                Console.WriteLine(response);

                // Deserialize the API response
                var apiResponse = JsonSerializer.Deserialize<ListBoxViewModel>(response, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                // Ensure the response is not null
                if (apiResponse?.Listboxes == null)
                {
                    Console.WriteLine("No listboxes found in the API response.");
                    return new List<LanguageModal>();  // Return an empty list
                }

                // Find the listbox for languages (filter: "language")
                var languageListbox = apiResponse.Listboxes.FirstOrDefault(lb => lb.Filter == "language");

                if (languageListbox == null)
                {
                    Console.WriteLine("No language listbox found.");
                    return new List<LanguageModal>();  // Return an empty list
                }

                // Map to LanguageModal
                var languages = languageListbox?.Options.Select(opt => new LanguageModal
                {
                    Id = opt.Value,
                    Language = opt.Name
                }).ToList();

                return languages ?? new List<LanguageModal>();  // Return empty list if no languages found
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error in GetLanguagesAsync: {e.Message}");
                return new List<LanguageModal>();  // Return an empty list in case of error
            }
        }


        public async Task<List<ConditionModal>> GetConditionsAsync()
        {
            SelectedLanguage = _globalSettings.SelectedLanguage;
            SelectedGame = _globalSettings.SelectedGame;

            string language = SelectedLanguage == "English" ? "en" : SelectedLanguage == "Italian" ? "it" : "en";
            string game = SelectedGame;

            // Construct the URL with selected language and game
            string url = $"https://api.magiccorner.it/api/mclistboxes/{game}/{language}";

            //string url = "https://api.magiccorner.it/api/mclistboxes/pokemon/en";


            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", "0d1bb073-9dfb-4c6d-a1c0-1e8f7d5d8e9f");
            var response = await httpClient.GetStringAsync(url);

            Console.WriteLine(response);  // Log the raw response for debugging

            // Deserialize the API response
            var apiResponse = JsonSerializer.Deserialize<ListBoxViewModel>(response, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });


            // Deserialize the API response


            // Find the listbox for conditions (filter: "condition")
            var conditionListbox = apiResponse?.Listboxes.FirstOrDefault(lb => lb.Filter == "condition");

            // Map to ConditionModal
            var conditions = conditionListbox?.Options.Select(opt => new ConditionModal
            {
                Id = opt.Value,
                Condition = opt.Name
            }).ToList();

            return conditions ?? new List<ConditionModal>();
        }


        public async Task<List<Listbox>> GetPlaceOrderDetailsAsync()
        {
            SelectedLanguage = _globalSettings.SelectedLanguage;
            SelectedGame = _globalSettings.SelectedGame;
            try
            {
                string language = SelectedLanguage == "English" ? "en" : SelectedLanguage == "Italian" ? "it" : "en";
                string game = SelectedGame ?? "magic";

                // Construct the URL with selected language and game
                string url = $"https://api.magiccorner.it/api/mclistboxes/{game}/{language}";

                // string url = "https://api.magiccorner.it/api/mclistboxes/pokemon/en";

                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", "0d1bb073-9dfb-4c6d-a1c0-1e8f7d5d8e9f");

                var response = await httpClient.GetStringAsync(url);
                var apiResponse = JsonSerializer.Deserialize<ListBoxViewModel>(response, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return apiResponse?.Listboxes ?? new List<Listbox>();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error fetching listboxes: {e.Message}");
                return new List<Listbox>();
            }
        }
    }











}
