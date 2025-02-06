using System.Text.Json;
using Teste2.Model;
using Teste2.Utils;

namespace Teste2.Client
{
    public class HackerRanClient
    {
        private readonly HttpClient _client = new HttpClient();
        private readonly string? _url;

        public HackerRanClient(string? Url)
        {
            _client.Timeout = TimeSpan.FromSeconds(15);
            _url = Url;
        }

        public async Task<int> getTotalScoredGoals()
        {
            try
            {
                HttpResponseMessage? response = await _client.GetAsync(_url);

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();

                    var result = JsonSerializer.Deserialize<ApiResponse>(responseBody);

                    int totalGoals = 0;

                    if (ListUtils<Datas>.ValidateList(result?.Data))
                    {
                        result.Data.ToList().ForEach(d =>
                        {
                            if (int.TryParse(d.Team1Goals, out int team1Goals))
                            {
                                totalGoals += team1Goals;
                            }
                        });
                    }

                    return totalGoals;
                }
                else
                {
                    Console.WriteLine($"Erro ao bater no endpoint: {response.StatusCode}");
                    return 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro: {ex.Message}\n{ex.InnerException}");
                return 0;
            }

        }        
    }
}
