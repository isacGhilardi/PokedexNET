using Pokedex.Data;
using Pokedex.Models;

namespace Pokedex.Views;

public partial class PokemonCadastroPage : ContentPage
{
    private readonly PokemonDatabase _database;

    public PokemonCadastroPage(PokemonDatabase database)
    {
        InitializeComponent();
        _database = database;
    }

    private async void OnCadastrarClicked(object? sender, EventArgs e)
    {
        if (sender is not Button button)
            return;

        if (string.IsNullOrWhiteSpace(NomeEntry.Text) ||
            string.IsNullOrWhiteSpace(TipoEntry.Text) ||
            !int.TryParse(NumeroEntry.Text, out var numero) || numero <= 0)
        {
            await DisplayAlertAsync("Dados inválidos", "Informe o nome, o tipo e um número positivo da Pokédex.", "OK");
            return;
        }

        button.IsEnabled = false;
        try
        {
            await _database.CadastrarAsync(new Pokemon
            {
                Nome1 = NomeEntry.Text.Trim(),
                Tipo1 = TipoEntry.Text.Trim(),
                Numero = numero
            });
            NomeEntry.Text = string.Empty;
            TipoEntry.Text = string.Empty;
            NumeroEntry.Text = string.Empty;
            await DisplayAlertAsync("Cadastro", "Pokémon cadastrado com sucesso.", "OK");
        }
        catch (Exception)
        {
            await DisplayAlertAsync("Erro", "Não foi possível cadastrar o Pokémon. Tente novamente.", "OK");
        }
        finally
        {
            button.IsEnabled = true;
        }
    }
    private async void OnSalvarClicked(object sender, EventArgs e)
    {
        if (!SalvarButton.IsEnabled)
            return;

        var nomesPoderes = (PoderesEntry.Text ?? "")
            .Split(',',
                StringSplitOptions.RemoveEmptyEntries |
                StringSplitOptions.TrimEntries)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();

        if (string.IsNullOrWhiteSpace(NomeEntry.Text) ||
            string.IsNullOrWhiteSpace(TipoEntry.Text) ||
            string.IsNullOrWhiteSpace(ImagemEntry.Text) ||
            nomesPoderes.Count == 0)
        {
            await DisplayAlertAsync(
                "Confira os dados",
                "Preencha todos os campos e informe ao menos um poder.",
                "OK");
            return;
        }

        SalvarButton.IsEnabled = false;

        try
        {
            var pokemon = new Pokemon
            {
                Nome1 = NomeEntry.Text.Trim(),
                Tipo1 = TipoEntry.Text.Trim(),
                Imagem = ImagemEntry.Text.Trim(),
                Poderes = nomesPoderes
                    .Select(nome => new Poder { Nome = nome })
                    .ToList()
            };

            await _database.CadastrarAsync(pokemon);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex);

            await DisplayAlertAsync(
                "Erro",
                "Não foi possível salvar o Pokémon.",
                "OK");
            return;
        }
        finally
        {
            SalvarButton.IsEnabled = true;
        }

        await Shell.Current.GoToAsync("..");
    }
}
