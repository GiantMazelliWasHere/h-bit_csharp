using System;
using System.Collections.Generic;
using System.Linq;

namespace MyApp
{
    internal class Program
    {
        // Arrays internos para armazenar os dados
        static List<string> tipos = new List<string>();
        static List<DateTime> datas = new List<DateTime>();
        static List<double> valores = new List<double>();

        // Dicionário para mapear tipos de atividade às suas unidades
        static Dictionary<string, string> unidades = new Dictionary<string, string>()
        {
            { "Corrida", "minutos" },
            { "Caminhada", "minutos" },
            { "Água", "litros" },
            { "Sono", "horas" },
            { "Exercício", "minutos" }
        };

        static void Main(string[] args)
        {
            int opcao;

            do
            {
                Console.WriteLine("\n=== Controle de Atividades de Saúde ===");
                Console.WriteLine("1. Adicionar registro");
                Console.WriteLine("2. Listar registros");
                Console.WriteLine("3. Exibir estatísticas por tipo");
                Console.WriteLine("4. Sair");
                Console.Write("Escolha uma opção: ");

                if (int.TryParse(Console.ReadLine(), out opcao))
                {
                    switch (opcao)
                    {
                        case 1:
                            RegistrarAtividade();
                            break;
                        case 2:
                            ListarAtividade();
                            break;
                        case 3:
                            ExibirEstatisticas();
                            break;
                        case 4:
                            Console.WriteLine("Encerrando o programa...");
                            break;
                        default:
                            Console.WriteLine("Opção inválida!");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Por favor, digite um número válido!");
                }

            } while (opcao != 4);
        }

        /// <summary>
        /// Metodo para registrar uma nova atividade
        /// </summary>
        static void RegistrarAtividade()
        {
            Console.WriteLine("\nSelecione o tipo de atividade:");


            var listaTipos = unidades.Keys.ToList();
            for (int i = 0; i < listaTipos.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {listaTipos[i]}");
            }

            Console.Write("Escolha o número da atividade: ");
            if (!int.TryParse(Console.ReadLine(), out int escolha) || escolha < 1 || escolha > listaTipos.Count)
            {
                Console.WriteLine("Opção inválida! Registro cancelado.");
                return;
            }

            string tipo = listaTipos[escolha - 1];
            string unidade = unidades[tipo];

            Console.Write("Data (dd/mm/aaaa): ");
            if (!DateTime.TryParse(Console.ReadLine(), out DateTime data))
            {
                Console.WriteLine("Data inválida! Registro cancelado.");
                return;
            }

            Console.Write($"Valor em {unidade}: ");
            if (!double.TryParse(Console.ReadLine(), out double valor) || valor < 0)
            {
                Console.WriteLine("Valor inválido! Não pode ser negativo ou não numérico.");
                return;
            }

            tipos.Add(tipo);
            datas.Add(data);
            valores.Add(valor);

            Console.WriteLine("Registro adicionado com sucesso!");

        }

        /// <summary>
        /// Metodo para listar todas as atividades registradas
        /// </summary>
        static void ListarAtividade()
        {
            if (tipos.Count == 0)
            {
                Console.WriteLine("\nNenhum registro encontrado.");
                return;
            }

            Console.WriteLine("\n=== Lista de Registros ===");
            for (int i = 0; i < tipos.Count; i++)
            {
                string unidade = unidades.ContainsKey(tipos[i]) ? unidades[tipos[i]] : "";
                Console.WriteLine($"{i + 1}. {tipos[i]} - {datas[i].ToShortDateString()} - {valores[i]} {unidade}");
            }

        }

        /// <summary>
        /// Metodo para exibir estatísticas por tipo de atividade
        /// </summary>
        static void ExibirEstatisticas()
        {
            if (tipos.Count == 0)
            {
                Console.WriteLine("\nNenhum registro disponível para estatísticas.");
                return;
            }

            Console.WriteLine("\n=== Estatísticas por Tipo de Atividade ===");


            var agrupados = tipos
                .Select((tipo, index) => new { Tipo = tipo, Valor = valores[index] })
                .GroupBy(x => x.Tipo);


            foreach (var grupo in agrupados)
            {
                string unidade = unidades.ContainsKey(grupo.Key) ? unidades[grupo.Key] : "";
                double soma = grupo.Sum(x => x.Valor);
                double media = grupo.Average(x => x.Valor);

                Console.WriteLine($"\nTipo: {grupo.Key} ({unidade})");
                Console.WriteLine($"Total registrado: {soma} {unidade}");
                Console.WriteLine($"Média registrada: {media:F2} {unidade}");
                Console.WriteLine($"Registros: {grupo.Count()} entradas");
            }
        }
    }
}