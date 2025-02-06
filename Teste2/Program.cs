using Teste2.Client;

class Program
{
    static async Task Main(string[] args)
    {
        string teamName = "Paris Saint-Germain";
        int year = 2013;
        var totalGoals = await getTotalScoredGoals(teamName, year);

        Console.WriteLine($"Team {teamName} scored {totalGoals} goals in {year}");

        teamName = "Chelsea";
        year = 2014;
        totalGoals = await getTotalScoredGoals(teamName, year); 

        Console.WriteLine($"Team {teamName} scored {totalGoals} goals in {year}");
    }
    
    async static Task<int> getTotalScoredGoals(string team, int year)
    {
        string treatwhitespace = Uri.EscapeDataString(team);
        string Url = $"https://jsonmock.hackerrank.com/api/football_matches?year={year}&team1={treatwhitespace}";

        var client = new HackerRanClient(Url);

        var result = await client.getTotalScoredGoals();

        return result;
    }
}

