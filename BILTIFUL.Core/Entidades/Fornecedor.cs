﻿using BILTIFUL.Core.Entidades.Enums;
using System;

namespace BILTIFUL.Core.Entidades
{
    public class Fornecedor
    {
        public long cnpj { get; set; }
        public string rsocial { get; set; }
        public DateTime dabertura { get; set; }
        public DateTime ucompra { get; set; } = DateTime.Now;
        public DateTime dcadastro { get; set; } = DateTime.Now;
        public Situacao situacao { get; set; } = Situacao.Ativo;

        public Fornecedor()
        {
        }

        public Fornecedor(long cnpj, string rsocial, DateTime dabertura)
        {
            this.cnpj = cnpj;
            this.rsocial = rsocial;
            this.dabertura = dabertura;
        }

        public string ConverterParaEDI()
        {
            return $"{cnpj}{rsocial.PadRight(50, ' ')}{dabertura.ToString("dd/MM/yyyy")}{ucompra.ToString("dd/MM/yyyy")}{dcadastro.ToString("dd/MM/yyyy")}{(char)situacao}";
        }
    }
}
