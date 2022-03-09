﻿using BILTIFUL.Core;
using BILTIFUL.Core.Controles;
using BILTIFUL.Core.Entidades;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace BILTIFUL.ModuloCompra
{
    public class CompraService
    {
        CadastroService cadastroService = new CadastroService();

        //List<Fornecedor> testes = new List<Fornecedor>();
        //public void AdicionarFornecedor()
        //{

        //    testes.Add(new Fornecedor(1, "fornecedor1"));
        //    testes.Add(new Fornecedor(2, "fornecedor2"));
        //}
        string cnpj;
        public void SubMenu()
        {
            Console.Clear();
            Console.WriteLine("\n\t\t\t\t\t __________________________________________________");
            Console.WriteLine("\t\t\t\t\t|+++++++++++++++++++| COMPRAS |+++++++++++++++++++|");
            Console.WriteLine("\t\t\t\t\t|1| - CADASTRAR COMPRA                            |");
            Console.WriteLine("\t\t\t\t\t|2| - LOCALIZAR COMPRA                            |");
            Console.WriteLine("\t\t\t\t\t|3| - EXIBIR COMPRAS CADASTRADAS                  |");
            Console.WriteLine("\t\t\t\t\t|0| - SAIR                                        |");
            Console.Write("\t\t\t\t\t|_________________________________________________|\n" +
                          "\t\t\t\t\t|Opção: ");
           
            string opc = Console.ReadLine();
            switch (opc)
            {
                case "1":
                    CadastrarCompra();

                    break;
                case "2":
                    LocalizarCompra(cadastroService.cadastros.compras, cadastroService.cadastros.itenscompra);
                    break;
                case "3":
                    if (cadastroService.cadastros.compras.Count() != 0)
                        new Registros(cadastroService.cadastros.compras, cadastroService.cadastros.itenscompra);
                    else
                        Console.WriteLine("Nenhum produto registrado");
                    break;
                case "0":
                    break;
                default:
                    break;
            }
        }

        public void LocalizarCompra(List<Compra> compras, List<ItemCompra> itens)
        {
            string opc;
            do
            {
                Console.Clear();
                Console.WriteLine("\t________________________________________________");
                Console.WriteLine("\t|++++++++++++| MENU DE LOCALIZAÇÃO |+++++++++++|");
                Console.WriteLine("\t|1| - LOCALIZAR POR DATA                       |");
                Console.WriteLine("\t|2| - LOCALIZAR POR FORNECEDOR                 |");                
                Console.WriteLine("\t|3| - LOCALIZAR POR ID                         |");
                Console.WriteLine("\t|0| - VOLTAR                                   |");
                Console.Write("\t|______________________________________________|\n" +
                              "\t|Opção: ");
                opc = Console.ReadLine();
                bool encontrado = false;
                Console.Clear();
                switch (opc)
                {
                    case "1":
                        Console.Write("Digite o data de compra que deseja localizar(dd/mm/aaaa): ");
                        DateTime dcompra = DateTime.Parse(Console.ReadLine());
                        List<Compra> localizacompra = cadastroService.cadastros.compras.FindAll(p => p.DataCompra == dcompra);
                        if (localizacompra != null)
                        {
                            encontrado = true;
                            foreach (Compra p in localizacompra)
                            {
                                Console.WriteLine(p.DadosCompra());
                                Console.WriteLine("Itens: ");
                                foreach (ItemCompra i in cadastroService.cadastros.itenscompra)
                                {
                                    if (i.Id == p.Id)
                                        Console.WriteLine(i.DadosItemCompra());
                                    
                                }
                                Console.ReadKey();
                            }
                        }
                        break;

                    case "2":
                        Console.Write("Digite o CNPJ do fornecedor que deseja localizar: ");
                        long cnpj = long.Parse(Console.ReadLine());
                        List<Compra> localizacnpj = cadastroService.cadastros.compras.FindAll(p=> p.Fornecedor == cnpj);
                        if (localizacnpj != null)
                        {
                            encontrado = true;
                            foreach (Compra p in localizacnpj)
                            {
                                Console.WriteLine(p.DadosCompra());
                                Console.WriteLine("Itens: ");
                                foreach (ItemCompra i in cadastroService.cadastros.itenscompra)
                                {
                                    if (i.Id == p.Id)
                                        Console.WriteLine(i.DadosItemCompra());
                                    
                                }
                                Console.ReadKey();
                            }
                        }

                        break;
                    case "3":
                        Console.Write("Digite o ID da compra que deseja localizar: ");
                        string idCompra = Console.ReadLine();
                        List<Compra> localizaId = cadastroService.cadastros.compras.FindAll(p => p.Id == idCompra);
                        if (localizaId != null)
                        {
                            encontrado = true;
                            foreach (Compra p in localizaId)
                            {
                                Console.WriteLine(p.DadosCompra());
                                Console.WriteLine("Itens: ");
                                foreach (ItemCompra i in cadastroService.cadastros.itenscompra)
                                {
                                    if (i.Id == p.Id)
                                        Console.WriteLine(i.DadosItemCompra());

                                }
                                Console.ReadKey();
                            }
                        }
                        break;
                }

            } while (opc != "0");
        }




        public void CadastrarCompra()
        {


            string opc = "a";

            Console.Clear();
            do
            {
                Console.WriteLine("Informe o CNPJ do forncedor");
                cnpj = Console.ReadLine().Trim().Replace(".", "").Replace("-", "").Replace("/", "");
                if(BuscarBloqueado(cnpj, cadastroService.cadastros.bloqueados))
                {
                    Console.WriteLine("Fornecedor bloqueado para compra");
                    return;
                }              

                Fornecedor fornecedorCompra = BuscarCnpj(cnpj, cadastroService.cadastros.fornecedores);
                if (fornecedorCompra == null)
                {
                    Console.WriteLine("Fornecedor nao encontrado.");
                }
                else
                {
                    Console.WriteLine(fornecedorCompra.DadosFornecedorCompra());
                    Console.WriteLine("[1]SIM [0]NAO");
                    Console.WriteLine("Confirma dados do Fornecedor?");
                    opc = Console.ReadLine();
                    if (opc == "0")
                    {
                        Console.WriteLine("");

                    }
                }
            } while (opc != "1");
            ItemCompra();


        }
        public void ItemCompra()
        {
            int cont = 0;
            string saida = "a";
            string opcp = "a";
            string[] stringValor = new string[4];
            double valor,quantidadeTeste;
            double[] valorQuantidade = new double[4];
            double[] quantidade = new double[4];
            double[] totalItem = new double[4];
            string[] idMPrima = new string[4];
            string[] quantidadeString = new string[4];
            double valorTotal = 0;
            string[] totalItemString = new string[4];
            do
            {

                do
                {
                    opcp = "a";
                    string buscarMPrima;
                    Console.Clear();
                    do
                    {
                        Console.WriteLine("Informe o nome da Materia-Prima");
                        buscarMPrima = Console.ReadLine();

                    } while (ImprimirMPrima(cadastroService.cadastros.materiasprimas, buscarMPrima)!= true);
                        Console.WriteLine("Informe o ID referente a Materia-Prima que deseja adicionar");
                        idMPrima[cont] = Console.ReadLine().ToUpper();
                    
                    MPrima mPrimaCompra = BuscaMPrima(idMPrima[cont],cadastroService.cadastros.materiasprimas);
                    if (mPrimaCompra == null)
                    {
                        Console.WriteLine("Materia-Prima nao encontrada.");
                    }
                    else
                    {
                        Console.WriteLine("------------------------------");
                        Console.WriteLine(mPrimaCompra.DadosMateriaPrima());
                        Console.WriteLine("------------------------------");
                        Console.WriteLine("[1]SIM [0]NAO");
                        Console.WriteLine("Confirma dados da Materia-Prima?");
                        opcp = Console.ReadLine();
                        if (opcp == "0")
                        {
                            Console.WriteLine("");

                        }
                        else
                        {

                            do
                            {
                                Console.Clear();
                                Console.WriteLine("-----------------------------------------");
                                Console.WriteLine("Informe o valor unitario da Materia-Prima");
                                Console.Write("Valor($$$,$$)(valor precisa ser menor que 1000,00): ");
                                stringValor[cont] = Console.ReadLine();
                                valorQuantidade[cont] = double.Parse(stringValor[cont]);
                                stringValor[cont] = stringValor[cont].Trim().Replace(".", "").Replace(",", "");
                                if (!double.TryParse(stringValor[cont], out valor) || (valor > 99999) || (valor <= 0))
                                    Console.WriteLine("Valor invalido!");
                            } while (!double.TryParse(stringValor[cont], out valor) || (valor > 99999) || (valor <= 0));
                            do
                            {
                                do
                                {
                                    Console.Write("Informe a quantidade: ");
                                    quantidadeString[cont] = Console.ReadLine();
                                    quantidade[cont] = double.Parse(quantidadeString[cont]);
                                    quantidadeString[cont] = quantidadeString[cont].Trim().Replace(".", "").Replace(",", "");
                                    totalItemString[cont] = (quantidade[cont] * valorQuantidade[cont]).ToString("F2");
                                    totalItem[cont] = (quantidade[cont] * valorQuantidade[cont]);
                                    if (!double.TryParse(quantidadeString[cont], out quantidadeTeste) || (quantidadeTeste > 99999) || (quantidadeTeste <= 0))
                                        Console.WriteLine("Valor invalido!");
                                } while (!double.TryParse(quantidadeString[cont], out quantidadeTeste) || (quantidadeTeste > 99999) || (quantidadeTeste <= 0));
                            } while (TotalItem(valorQuantidade[cont], quantidade[cont]) != true);
                        }
                    }

                } while (opcp != "1");
                Console.WriteLine("Materia-Prima:\t{0} Valor Unitario:\t{1} Quantidade:\t{2} Total Item:\t{3}", idMPrima[cont], valorQuantidade[cont], quantidade[cont], totalItemString[cont]);
                Console.ReadKey();

                valorTotal = valorTotal + totalItem[cont];
                cont++;
                if (cont == 3)
                {
                    Console.WriteLine("Limite de Materia-Prima atingido por compra");
                    Console.ReadKey();
                }
                else
                {
                    Console.WriteLine("Deseja adicionar mais materia-prima\n[1]SIM [0]NAO");
                    saida = Console.ReadLine();
                }

            } while ((saida != "0") & (cont != 3));
            for (int i = 0; i < cont; i++)
            {
                Console.WriteLine("Materia-Prima:\t{0} Valor Unitario:\t{1} Quantidade:\t{2} Total Item:\t{3}", idMPrima[i], stringValor[i], valorQuantidade[i], totalItemString[i]);
            }
            Console.Write("Confirmar a compra?[1]SIM [0]NAO :");
            string confirmar = Console.ReadLine();
            if (confirmar == "1")
            {
                string cod = "" + (++cadastroService.cadastros.codigos[3]);
                cadastroService.SalvarCodigos();
                string valorTotalString = (valorTotal.ToString("F2"));
                valorTotalString = valorTotalString.Trim().Replace(".", "").Replace(",", "");
                
                

                Compra compra = new Compra(cod, long.Parse(cnpj), valorTotalString);
                cadastroService.cadastros.compras.Add(compra);
                new Controle(compra);
                for (int i = 0; i < cont; i++)
                {
                    
                    quantidadeString[i] = quantidadeString[i].Trim().Replace(".", "").Replace(",", "");                    
                    totalItemString[i] = totalItemString[i].Trim().Replace(".", "").Replace(",", "");


                    ItemCompra itemCompra = new ItemCompra(cod, idMPrima[i], quantidadeString[i], stringValor[i], totalItemString[i]);
                    new Controle(itemCompra);
                    cadastroService.cadastros.itenscompra.Add(itemCompra);
                }
            }
            else
            {
                SubMenu();
            }
        }


        public bool TotalItem(double valor, double quantidade)
        {

            double totalCompra = valor * quantidade;
            if (totalCompra >= 10000)
            {
                Console.WriteLine("Valor ultrapasssou o valor total permetido por compra");
                Console.WriteLine("Favor informar outra quantidade e outro valor de Materia-Prima");
                Console.WriteLine("Quantidade disponivel para compra: {0}", 9999 / valor);
                return false;
            }
            else
            { return true; }
        }
        public Fornecedor BuscarCnpj(string fcnpj, List<Fornecedor> fornecedor)
        {
            Fornecedor fornecedorcompra = fornecedor.Find(delegate (Fornecedor f) { return f.CNPJ == long.Parse(fcnpj); });

            return fornecedorcompra;
        }

        public bool BuscarBloqueado(string fcnpj, List<string> bloqueados)
        {
            string buscar = bloqueados.Find(x => x == fcnpj);
            if (buscar == null)
            {
                return false;
            }
            else
            {
                return true;
            }

        }

        public MPrima BuscaMPrima(string idMPrima, List<MPrima> mPrima)
        {
            MPrima mPrimaCompra = mPrima.Find(delegate (MPrima mP) { return mP.Id == idMPrima; });

            return mPrimaCompra;
        }
        
        public bool ImprimirMPrima(List<MPrima> mPrima,string buscarMPrima)
        {
            bool buscar = false;
            List<MPrima> listaMprima = mPrima.FindAll(delegate (MPrima m) { return m.Nome.ToLower() == buscarMPrima.ToLower(); });
            listaMprima.ForEach(delegate (MPrima m)
            {
                Console.WriteLine(m.DadosMateriaPrima());
                Console.WriteLine("-----------------------------------------");
                buscar = true;
            });
            if(buscar == false)
            {
                Console.WriteLine("Materia-Prima nao encontrada");
                Console.ReadKey();
            }
            return buscar;
        }



    }
}
