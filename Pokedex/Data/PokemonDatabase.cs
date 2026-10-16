using Pokedex.Models;
using SQLite;

namespace Pokedex.Data;

public class PokemonDatabase
{
    public const string PokemonDB = "pokedex.db3";
    public const SQLite.SQLiteOpenFlags Flags =
        // open the database in read/write mode
        SQLite.SQLiteOpenFlags.ReadWrite |
        // create the database if it doesn't exist
        SQLite.SQLiteOpenFlags.Create |
        // enable multi-threaded database access
        SQLite.SQLiteOpenFlags.SharedCache;

    public static string DatabasePath =>
        Path.Combine(FileSystem.AppDataDirectory, PokemonDB);
    private readonly SQLiteAsyncConnection _conexao;
    private readonly Lazy<Task> _inicializacao;

    public PokemonDatabase()
    {
        _conexao = new SQLiteAsyncConnection(DatabasePath, Flags);
        _inicializacao = new Lazy<Task>(CriarTabelasAsync);
    }

    private async Task CriarTabelasAsync()
    {
        await _conexao.CreateTableAsync<Pokemon>();
        await _conexao.CreateTableAsync<Poder>();
    }
    public async Task CadastrarAsync(Pokemon pokemon)
    {
        await _inicializacao.Value;

        await _conexao.RunInTransactionAsync(conexao =>
        {
            conexao.Insert(pokemon);

            foreach (var poder in pokemon.Poderes)
            {
                poder.PokemonId = pokemon.ID;
                conexao.Insert(poder);
            }
        });
    }
    public async Task<List<Pokemon>> ListarAsync()
    {
        await _inicializacao.Value;
        var pokemons = await _conexao.Table<Pokemon>().ToListAsync();
        foreach (var pokemon in pokemons)
        {
            pokemon.Poderes = await _conexao.Table<Poder>()
                .Where(p => p.PokemonId == pokemon.ID)
                .ToListAsync();
        }
        return pokemons;
    }
}
