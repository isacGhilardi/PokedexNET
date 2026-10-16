using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pokedex.Models
{
    public class Pokemon

    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        private string Nome = "";
        private string Tipo = "";
        private string imagem = "";

        public int Numero { get; set; }

        public string Nome1 { get => Nome; set => Nome = value; }
        public string Tipo1 { get => Tipo; set => Tipo = value; }
        public string Imagem { get => imagem; set => imagem = value; }

        [Ignore]
        public List<Poder> Poderes { get; set; } = new();
    }
}
