using booksy.API.Models.DTOs;
using booksy.API.Services.Interfaces;
using System.Text;
using System.Text.Json;

namespace booksy.API.Services
{
    public class AISearchService : IAISearchService
    {
        private readonly IConfiguration _config;
        private readonly HttpClient _httpClient;

        public AISearchService(IConfiguration config)
        {
            _config = config;
            _httpClient = new HttpClient();
        }

        public async Task<IEnumerable<int>> SearchHardwareIdsAsync(string query, IEnumerable<HardwareDto> hardwareList)
        {
            if (string.IsNullOrWhiteSpace(query)) return [];

            var apiKey = _config["Gemini:ApiKey"];
            if (string.IsNullOrEmpty(apiKey))
            {
                Console.WriteLine("Gemini API Key is missing.");
                return [];
            }

            var hardwareSummary = hardwareList.Select(h => new
            {
                h.Id,
                h.Name,
                h.Brand,
                h.Notes,
                h.History
            });

            var promptText = $@"
                You are a hardware search assistant. A user is looking for equipment with the following request: ""{query}""

                Here is the list of available hardware in JSON format:
                {JsonSerializer.Serialize(hardwareSummary)}

                Based on the user request, identify the most relevant pieces of hardware. 
                Return ONLY a JSON array of integers representing the IDs of the relevant items, ordered by most relevant first.
                If nothing is relevant, return an empty array [].
                Do not include any text other than the JSON array.";

            var requestBody = new
            {
                contents = new[]
                {
                    new
                    {
                        parts = new[]
                        {
                            new { text = promptText }
                        }
                    }
                }
            };

            var url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.5-flash:generateContent?key={apiKey}";
            
            try
            {
                var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(url, content);
                response.EnsureSuccessStatusCode();

                var responseString = await response.Content.ReadAsStringAsync();
                using var doc = JsonDocument.Parse(responseString);
                
                // Gemini response structure: candidates[0].content.parts[0].text
                var responseText = doc.RootElement
                    .GetProperty("candidates")[0]
                    .GetProperty("content")
                    .GetProperty("parts")[0]
                    .GetProperty("text")
                    .GetString()?.Trim() ?? "[]";

                // Cleanup markdown if present
                if (responseText.StartsWith("```json")) responseText = responseText.Replace("```json", "").Replace("```", "").Trim();
                else if (responseText.StartsWith("```")) responseText = responseText.Replace("```", "").Trim();

                var ids = JsonSerializer.Deserialize<List<int>>(responseText);
                return ids ?? [];
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Gemini Search Error: {ex.Message}");
                return [];
            }
        }
    }
}
