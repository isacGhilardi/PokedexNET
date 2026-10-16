using Pokedex.Data;

namespace Pokedex.Views;

public partial class PokemonListaPage : ContentPage
{
    private readonly PokemonDatabase _database;

    public PokemonListaPage(PokemonDatabase database)
    {
        InitializeComponent();
        _database = database;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        try
        {
            PokemonsCollection.ItemsSource =
                await _database.ListarAsync();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex);

            await DisplayAlertAsync(
                "Erro",
                "Não foi possível carregar os Pokémon.",
                "OK");
        }
    }

    private async void OnCadastrarClicked(
        object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("cadastro");
    }
}