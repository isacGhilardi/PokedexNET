using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pokedex.Models
{
    public class Poder
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        [Indexed]
        public int PokemonId { get; set; }

        public string Nome { get; set; } = "";
    }
}
