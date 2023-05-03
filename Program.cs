using System;
using System.Collections.Generic;
using System.IO;

namespace Intelitrader_Desafio_LeonardoBigeschi
{
    class Program
    {
        private static Boolean ControladorCod(List<string> controlador, string codProduto)
        {
            int i = 0;
            while (i < controlador.Count && codProduto != controlador[i])
                i++;

            if(i == controlador.Count)
            {
                controlador.Add(codProduto);
                return true;
            }

            return false;
        }

        private static void ArquivoTransfere(string arquivoP, string auxVend, int qtVendas, StreamWriter sw)
        {
            int qtCO = 0, qtMin = 0, estoque=0, necessidade=0, transf;
            string[] vtProdutos, auxProd;
            

            vtProdutos = arquivoP.Split("\n");
             
            for(int i=0; i < vtProdutos.Length-1; i++)
            {
                auxProd = vtProdutos[i].Split(";");

                if(auxProd[0] == auxVend)
                {
                    qtCO = Convert.ToInt32(auxProd[1]);
                    qtMin = Convert.ToInt32(auxProd[2]);
                    estoque = qtCO - qtVendas;

                    if (estoque < qtMin)
                        necessidade = qtMin - estoque;

                    if (necessidade >= 1 && necessidade <= 10)
                    {
                        transf = 10;
                    }
                    else
                        transf = necessidade;

                    //ordenarProdutos.Add(auxProd[0]);

                    try
                    {
                        sw.WriteLine(auxVend + "\t\t " + qtCO + "\t " + qtMin + " \t " + qtVendas + "\t\t\t" + estoque + "\t\t\t\t " + necessidade + "\t\t\t\t\t" + transf);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }

                }

            }
        }

        private static void ArquivoDivergencias(string arquivoP, string arquivoV, StreamWriter swdiv)
        {
            int contLinha=1;
            string[] vtVendas, vendas;
            string produtos = arquivoP;

            vtVendas = arquivoV.Split("\n");

            for (int i = 0; i < vtVendas.Length - 1; i++)
            {
                vendas = vtVendas[i].Split(";");

                if(produtos.Contains(vendas[0]))
                {
                    if (vendas[2] == "135")
                    {
                        swdiv.WriteLine("Linha " + contLinha + " - Venda cancelada");
                        
                    }

                    if (vendas[2] == "190")
                    {
                        swdiv.WriteLine("Linha " + contLinha + " - Venda não finalizada");
                        
                    }

                    if (vendas[2] == "999")
                    {
                        swdiv.WriteLine("Linha " + contLinha + " - Erro desconhecido. Acionar equipe de TI");
                        
                    }

                    contLinha++;
                }
                else
                {
                    swdiv.WriteLine("Linha " + contLinha + " - Código de Produto não encontrado " + vendas[0]);
                    contLinha++;
                }
                
            }

        }

        private static void ArquivoTotCanais(string arquivoV, StreamWriter swtot)
        {
            string[] vendas, vtVendas;
           // string arqVendas = arquivoV;
            int totalRep = 0, totalWeb = 0, totalAnd = 0, totalIPh = 0;

            vtVendas = arquivoV.Split("\n");

            for (int i = 0; i < vtVendas.Length - 1; i++)
            {
                vendas = vtVendas[i].Split(";");

                if(vendas[2] == "100" || vendas[2] == "102")
                {
                    if (vendas[3] == "1")
                        totalRep += Convert.ToInt32(vendas[1]);

                    if (vendas[3] == "2")
                        totalWeb += Convert.ToInt32(vendas[1]);

                    if (vendas[3] == "3")
                        totalAnd += Convert.ToInt32(vendas[1]);

                    if (vendas[3] == "4")
                        totalIPh += Convert.ToInt32(vendas[1]);
                }
            }

            swtot.WriteLine("1 - Representantes \t\t " + totalRep);
            swtot.WriteLine("2 - Website \t\t     " + totalWeb);
            swtot.WriteLine("3 - App móvel Android \t " + totalAnd);
            swtot.WriteLine("4 - App móvel IPhone \t " + totalIPh);
        }

        private static void ControladorGeral(string arquivoP, string arquivoV)
        {
            int qtVendas = 0;
            string[] vtVendas, divideV, auxVend;

            StreamWriter sw = null;
            StreamWriter swdiv = null;
            StreamWriter swtot = null;

            List<string> controlador = new List<string>();

            vtVendas = arquivoV.Split("\n");

            String path = System.AppDomain.CurrentDomain.BaseDirectory.ToString();
            string[] newPath = path.Split(@"\");

            string pathorig = "";
            for (int i = 0; i < newPath.Length; i++)
            {
                if (!newPath[i].Equals(""))
                {
                    if (newPath[i] != "bin" && newPath[i] != "Debug" && newPath[i] != "net5.0")
                        pathorig += newPath[i] + @"\";
                }
            }
            pathorig += @"Arquivos";

            //Gerando arquivo [tranfere.txt] para passar por parâmetro
            try
            {
                //string pathTransfere = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                //pathTransfere += @"\Intelitrader-Desafio-LeonardoBigeschi\Arquivos\transfere.txt";

                string pathTransfere = pathorig;
                pathTransfere += @"\transfere.txt";

                sw = new StreamWriter(pathTransfere);
                sw.WriteLine("Necessidade de Tranferência Armazém para C0");
                sw.WriteLine("");
                sw.WriteLine("Produto \t QtCO \t QtMin \t QtVendas \t Est.apósVendas \t Necess. \t Trasnf. de Arm p/ CO");
                
            }
            catch (Exception e)
            { Console.WriteLine("Exception: " + e.Message); }

            //Gerando arquivo [divergencias.txt] para passar por parâmetro
            try
            {
                //string pathDivergencia = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                //pathDivergencia += @"\Intelitrader-Desafio-LeonardoBigeschi\Arquivos\divergencias.txt";

                string pathDivergencia = pathorig;
                pathDivergencia += @"\divergencias.txt";

                swdiv = new StreamWriter(pathDivergencia);
                swdiv.WriteLine("");
            }
            catch (Exception e)
            { Console.WriteLine("Exception: " + e.Message); }

            //Gerando arquivo [totcanais.txt] para passar por parâmetro
            try
            {
                //string pathTotCanais = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                //pathTotCanais += @"\Intelitrader-Desafio-LeonardoBigeschi\Arquivos\totcanais.txt";

                string pathTotCanais = pathorig;
                pathTotCanais += @"\totcanais.txt";

                swtot = new StreamWriter(pathTotCanais);
                swtot.WriteLine("");
            }
            catch (Exception e)
            { Console.WriteLine("Exception: " + e.Message); }

            for (int i = 0; i < vtVendas.Length-1; i++)
            {
                auxVend = vtVendas[i].Split(";");
                int statusI = Convert.ToInt32(auxVend[2]);

                if (statusI == 100 || statusI == 102)
                {
                    Boolean verificaControlador = ControladorCod(controlador, auxVend[0]);
                        
                    if(verificaControlador) 
                    {
                        qtVendas += Convert.ToInt32(auxVend[1]);

                        for (int j = i + 1; j < vtVendas.Length; j++)
                        {
                            divideV = vtVendas[j].Split(";");

                            if (auxVend[0] == divideV[0])
                            {
                                int statusJ = Convert.ToInt32(divideV[2]);

                                if (statusJ == 100 || statusJ == 102)
                                    qtVendas += Convert.ToInt32(divideV[1]);
                            }
                        }

                        ArquivoTransfere(arquivoP, auxVend[0], qtVendas, sw);
                        
                        qtVendas = 0;
                    }
                        
                }
                
            }

            sw.Close();

            ArquivoDivergencias(arquivoP, arquivoV, swdiv);
            swdiv.Close();

            ArquivoTotCanais(arquivoV, swtot);
            swtot.Close();
        }

        private static void ExecutaC1()
        {
            String line, lineV;

            String path = System.AppDomain.CurrentDomain.BaseDirectory.ToString();
            string[] newPath = path.Split(@"\");

            string pathorig = "";
            for (int i = 0; i < newPath.Length; i++)
            {
                if (!newPath[i].Equals(""))
                {
                    if (newPath[i] != "bin" && newPath[i] != "Debug" && newPath[i] != "net5.0")
                        pathorig += newPath[i] + @"\";
                }
            }
            pathorig += @"Arquivos";

            try
            {
                // LENDO OS ARQUIVOS DE PRODUTOS E VENDAS
                //string pathProdutos = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                //pathProdutos += @"\Intelitrader-Desafio-LeonardoBigeschi\Arquivos\c1_produtos.txt";

                string pathProdutos = pathorig;
                pathProdutos += @"\c1_produtos.txt";

                string pathVendas = pathorig;
                pathVendas += @"\c1_vendas.txt";

                //string pathVendas = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                //pathVendas += @"\Intelitrader-Desafio-LeonardoBigeschi\Arquivos\c1_vendas.txt";

                string prod = "", vend = "";

                StreamReader sr = new StreamReader(pathProdutos);
                StreamReader srV = new StreamReader(pathVendas);

                Console.WriteLine("LENDO ARQUIVO DE PRODUTOS");
                Console.WriteLine("");

                line = sr.ReadLine();

                while (line != null)
                {
                    Console.WriteLine(line);
                    prod += line;
                    prod += "\n";

                    line = sr.ReadLine();
                }

                Console.WriteLine("");
                Console.WriteLine("LENDO ARQUIVO DE VENDAS");
                Console.WriteLine("");

                lineV = srV.ReadLine();

                while (lineV != null)
                {
                    Console.WriteLine(lineV);
                    vend += lineV;
                    vend += "\n";

                    lineV = srV.ReadLine();
                }

                ControladorGeral(prod, vend);

                sr.Close();
                srV.Close();
            }
            catch (Exception e)
            { Console.WriteLine("Exception: " + e.Message); }
        }

        private static void ExecutaC2()
        {
            String line, lineV;
            String path = System.AppDomain.CurrentDomain.BaseDirectory.ToString();
            string[] newPath = path.Split(@"\");

            string pathorig = "";
            for (int i = 0; i < newPath.Length; i++)
            {
                if (!newPath[i].Equals(""))
                {
                    if (newPath[i] != "bin" && newPath[i] != "Debug" && newPath[i] != "net5.0")
                        pathorig += newPath[i] + @"\";
                }
            }
            pathorig += @"Arquivos";


            try
            {
                // LENDO OS ARQUIVOS DE PRODUTOS E VENDAS
                //string pathProdutos = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                //pathProdutos += @"\Intelitrader-Desafio-LeonardoBigeschi\Arquivos\c2_produtos.txt";

                string pathProdutos = pathorig;
                pathProdutos += @"\c2_produtos.txt";

                string pathVendas = pathorig;
                pathVendas += @"\c2_vendas.txt";

                //string pathVendas = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                //pathVendas += @"\Intelitrader-Desafio-LeonardoBigeschi\Arquivos\c2_vendas.txt";

                string prod = "", vend = "";

                StreamReader sr = new StreamReader(pathProdutos);
                StreamReader srV = new StreamReader(pathVendas);

                Console.WriteLine("LENDO ARQUIVO DE PRODUTOS");
                Console.WriteLine("");

                line = sr.ReadLine();

                while (line != null)
                {
                    Console.WriteLine(line);
                    prod += line;
                    prod += "\n";

                    line = sr.ReadLine();
                }

                Console.WriteLine("");
                Console.WriteLine("LENDO ARQUIVO DE VENDAS");
                Console.WriteLine("");

                lineV = srV.ReadLine();

                while (lineV != null)
                {
                    Console.WriteLine(lineV);
                    vend += lineV;
                    vend += "\n";

                    lineV = srV.ReadLine();
                }

                ControladorGeral(prod, vend);

                sr.Close();
                srV.Close();
            }
            catch (Exception e)
            { Console.WriteLine("Exception: " + e.Message); }
        }

        //Main
        static void Main(string[] args)
        {
            Console.WriteLine("");
            Console.WriteLine("Digite 1 para Caso de Teste 1 ou Digite 2 para Caso de Teste 2: ");
            
            if(Console.ReadKey().Key == ConsoleKey.D1)
            {
                Console.WriteLine("");
                ExecutaC1();
            }
            else
            {
                Console.WriteLine("");
                ExecutaC2();
            }

        }
    }
}
