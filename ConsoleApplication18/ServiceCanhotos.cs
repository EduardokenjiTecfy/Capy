using ConsoleApplication18.Evento;
using ConsoleApplication18.Model;
using ConsoleApplication18.OcrImpletentacao;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Util;
using GdPicture;
using Google.Cloud.Vision.V1;
using ImgRecognitionEmGu;
using IronPython.Hosting;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.Scripting.Hosting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.ServiceProcess;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace ConsoleApplication18
{
    partial class ServiceCanhotos : ServiceBase
    {
        public ServiceCanhotos()
        {
            InitializeComponent();
            this.ServiceName = "ServicoCanhoto";
        }
        private static System.Timers.Timer timer;

        public static void ProcessarTreinamento()
        {
            //IOcr ocr = new GoogleOcr();
            //var db = new Entidade();

            //foreach (var proj in db.PROJETO)
            //{
            //    List<String> data = new List<string>();

            //    var arquivo = Guid.NewGuid();
            //    var processamento = db.ModeloProcessamento.Where(_s => _s.ProjetoId == proj.ID);
            //    var dbModelo = new Entidade();
            //    var dbModeloProcessado = new Entidade();

            //    foreach (var item in processamento)
            //    {
            //        var arquivosModelos = db.ProcessarTreinamento.Where(_s => _s.ModeloId == item.ModeloId);
            //        foreach (var modelos in arquivosModelos)
            //        {

            //            var modelobanco = dbModelo.ProcessarTreinamento.Where(_s => _s.Id == modelos.Id).FirstOrDefault();
            //            if (modelobanco.Texto == null || modelobanco.Texto =="")
            //                modelobanco.Texto = ocr.executarOcr(modelos.caminho).ToString();
            //            modelobanco.verificado = true;
            //            var modelo = db.Modelo.Where(_s => _s.ID == modelos.ModeloId).FirstOrDefault();
            //            modelobanco.Modelo = modelo.nome;
            //            data.Add(modelo.nome + "," + "'" + modelobanco.Texto.Replace("'", "") + "'");
            //            dbModelo.SaveChanges();
            //        }

            //        item.Processar = false;
            //        var pr = dbModeloProcessado.ModeloProcessamento.Where(_s => _s.Id == item.Id).FirstOrDefault();
            //        pr.Processar = false;
            //        dbModeloProcessado.SaveChanges();


            //    }
            //    if (data.Count() > 0)
            //    {
            //        var atibrutos = "";
            //        var modelosAtributos = db.PROJETO_MODELO.Where(_s => _s.PROJETO_ID == proj.ID).ToList();
            //        for (int i = 0; i < modelosAtributos.Count(); i++)
            //        {
            //            atibrutos += modelosAtributos[i].Modelo.nome;
            //            if (i < modelosAtributos.Count() - 1)
            //            {
            //                atibrutos += ",";
            //            }
            //        }
            //        atibrutos += ",NAO_CLASSIFICADO";
            //        string cabecalho = "@relation processar" + "_" + proj.ID;





            //        string atributo = "@attribute spamclass {" + atibrutos + "} ";
            //        string atributoTexto = "@attribute text String     ";
            //        string datacabecalho = "@data";

            //        string arff = "";
            //        arff += cabecalho + "\n";
            //        arff += atributo + "\n";
            //        arff += atributoTexto + "\n";
            //        arff += datacabecalho + "\n";
            //        foreach (var itemArq in data)
            //        {
            //            arff += itemArq.Replace("\n", "") + "\n";

            //        }
            //        string treinamento = ConfigurationManager.AppSettings["treinamento"];

            //        using (FileStream fs = File.Create(treinamento + arquivo + ".arff"))
            //        {

            //            Byte[] info = new UTF8Encoding(true).GetBytes(arff);
            //            // Add some information to the file.
            //            fs.Write(info, 0, info.Length);
            //        }
            //        HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://35.192.175.82:8080/webmineracaoHttp/Hello?nomeArquivo=" + arquivo + "&elementos=" + atibrutos + "&projetoId=" + proj.ID);
            //        request.Timeout = 5000;

            //        try
            //        {
            //            using (WebResponse response = (HttpWebResponse)request.GetResponse())
            //            {

            //            }
            //        }
            //        catch (WebException)
            //        {
            //            // Console.WriteLine("Error Occured");
            //        }
            //    }

            //    ///




            //}

        }

        public static void captura(String caminho)
        {





            string itemCANHOTO = ConfigurationManager.AppSettings["caminhoTiff"];


            ////caso não seja capturado e tenha delay
            var arquivos = Directory.GetFiles(caminho, "*.*", System.IO.SearchOption.AllDirectories);
            foreach (var arquivo in arquivos)
            {
                try
                {
                    OnChangedDelay(arquivo);
                    if (Path.GetExtension(arquivo).ToLower() == ".tif")
                    {
                        File.Delete(arquivo);
                    }
                }
                catch (Exception ex)
                 { }
            }

            // Create a new FileSystemWatcher and set its properties.
            FileSystemWatcher watcher = new FileSystemWatcher();
            ////diretorio raiz
            watcher.Path = caminho;
            /* Watch for changes in LastAccess and LastWrite times, and
               the renaming of files or directories. */
            watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
               | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            // Only watch text files.

            watcher.Created += new FileSystemEventHandler(OnChanged);

            watcher.IncludeSubdirectories = true;
            // Begin watching.
            watcher.EnableRaisingEvents = true;

        }

        public static PROJETO getprojeto(int projetoId)
        {
            var context = new Entidade();
            var projeto = context.PROJETO.Where(_id => _id.ID == projetoId).FirstOrDefault();
            return projeto;
        }


        private static void OnChangedDelay(string caminho)
        {

            if (Path.GetExtension(caminho).ToUpper() == ".TIaF" || Path.GetExtension(caminho).ToUpper() == ".TIFaF" || Path.GetExtension(caminho).ToUpper() == ".PDF")
            {
                // File.Delete(caminho);
            }
            else
            {
                using (FileStream fs = File.Create(@"c:\changedelay.txt"))
                {

                    Byte[] info = new UTF8Encoding(true).GetBytes("changedelay ");
                    // Add some information to the file.
                    fs.Write(info, 0, info.Length);
                }
                string saidaProcessado = ConfigurationManager.AppSettings["saidaProcessado"];
                criarPastasSaidaProjetos(saidaProcessado);
                string ModelosClassificador = ConfigurationManager.AppSettings["ModelosClassificador"];
                criarPastasModelo(ModelosClassificador);
                criarPastasSaida(ModelosClassificador);
                string itemCANHOTO = ConfigurationManager.AppSettings["caminhoTiff"];
                criarPastasProjetos(itemCANHOTO);

                criarPastasProjetosEntradaRep(itemCANHOTO);




                string str = ConfigurationManager.AppSettings["saidaPdf"];
                string ScoreApp = ConfigurationManager.AppSettings["Score"];
                string item1 = ConfigurationManager.AppSettings["saidaErro"];
                string str1 = ConfigurationManager.AppSettings["saidaBack"];
                string item2 = ConfigurationManager.AppSettings["ftpUrl"];
                string str2 = ConfigurationManager.AppSettings["usuarioFtp"];

                string item3 = ConfigurationManager.AppSettings["senhaFtp"];



                PROJETO projeto = null;
                var context = new Entidade();
                string[] d = caminho.Split('\\');
                using (FileStream fs = File.Create(@"c:\logprocessando" + d.Count() + ".txt"))
                {
                    Byte[] info = new UTF8Encoding(true).GetBytes(d.Count().ToString());
                    // Add some information to the file.
                    fs.Write(info, 0, info.Length);
                }
                //C:\clientes\Documentos\ENTRADA\1004\admin @admin.com.br
                if (d.Count() >= 6)
                {

                    if (d[3].ToUpper().Trim() == "SAIDA")
                    {
                        return;
                    }

                    int idProjeto = 0;
                    int.TryParse(d[4], out idProjeto);

                    projeto = getprojeto(idProjeto);

                    var _hub = getInstance();
                    try
                    {
                        var caminhoArquivo = caminho;
                        IOcr ocr = Factory.FactoryOcr.FactoryOcrInstancia(idProjeto);
                        // var usuario = d[5];//Directory.GetParent(pastas.caminhoArquivo);
                        string usuarioEmail = d[5];
                        var usuarioAut = context.Usuario.Where(_usu => _usu.email == usuarioEmail).FirstOrDefault();
                        var usuarioId = usuarioAut.Id.ToString();
                        string arquivo = caminhoArquivo;// pastas.caminhoArquivo;

                        if (!File.Exists(arquivo))
                        {
                            return;
                        }


                        if (arquivo.Contains("ANDROID"))
                        {
                            var prop = Path.GetFileName(arquivo).Split('$');
                            CanhotosLeitura.Documento doc = new CanhotosLeitura.Documento();
                            doc.Nota = prop[1];
                            doc.Serie = prop[2];
                            doc.binario = File.ReadAllBytes(arquivo);
                            doc.extensao = Path.GetExtension(arquivo).Replace(".", "");
                            doc.localizacao = prop[3];
                            Modelo md = new Modelo();
                            md.nome = prop[0];

                            CanhotosLeitura.UploadDocCloudFile(doc, md);
                            File.Delete(arquivo);
                        }
                        else
                        {
                            //
                            if (projeto.agrupamento != null && projeto.agrupamento.Value != false)
                            {
                                if (arquivo.Contains(".pdf"))
                                {
                                    var arquivok = cast(arquivo, Path.GetDirectoryName(arquivo));
                                    Modelo md = null;
                                    string createFolder = "";

                                    foreach (var item in arquivok)
                                    {


                                        var sb = ocr.executarOcr(item);


                                        var modeloAchado = CanhotosLeitura.verificaModelo(sb, projeto.ID, 0);
                                        string agrupamento = ConfigurationManager.AppSettings["agrupamento"];

                                        if (modeloAchado != null)
                                        {

                                            if (md != null && md.ID == modeloAchado.ID)
                                            {
                                                File.Move(item, createFolder + Path.GetFileName(item));
                                                continue;
                                            }
                                            createFolder = agrupamento + modeloAchado.nome + @"\";
                                            if (!Directory.Exists(createFolder))
                                            {
                                                Directory.CreateDirectory(createFolder);
                                            }


                                            File.Move(item, createFolder + Path.GetFileName(item));
                                            md = modeloAchado;
                                        }
                                        else
                                        {
                                            if (createFolder != "")
                                            {
                                                File.Move(item, createFolder + Path.GetFileName(item));
                                            }
                                            else
                                            {
                                                if (!Directory.Exists(saidaProcessado + " Arquivo_NAO_Classificado"))
                                                {
                                                    Directory.CreateDirectory(saidaProcessado + "Arquivo_NAO_Classificado" + @"\");
                                                }
                                                File.Move(item, saidaProcessado + "Arquivo_NAO_Classificado" + @"\" + Path.GetFileName(item));
                                            }
                                        }
                                    }
                                }
                                IList<string> diretoriosInse = new List<string>();
                                if (arquivo.Contains(".tif") || arquivo.Contains(".tiff"))
                                {

                                    var arquivok = cast(arquivo, Path.GetDirectoryName(arquivo));
                                    Modelo md = null;
                                    string createFolder = "";

                                    var idPasta = Guid.NewGuid();
                                    var nomePasta = Path.GetFileNameWithoutExtension(arquivo) + "_" + idPasta;
                                    int numeroPagina = 1;
                                    foreach (var item in arquivok)
                                    {


                                        var sb = ocr.executarOcr(item);


                                        var modeloAchado = CanhotosLeitura.verificaModelo(sb, projeto.ID, numeroPagina, (Bitmap)Bitmap.FromFile(item));
                                        string agrupamento = ConfigurationManager.AppSettings["agrupamento"];

                                        if (modeloAchado != null)
                                        {
                                            createFolder = agrupamento + @"\" + nomePasta + @"\" + modeloAchado.nome + @"\";


                                            if (md != null && md.ID == modeloAchado.ID)
                                            {
                                                File.Move(item, createFolder + Path.GetFileName(item));
                                                continue;
                                            }
                                            if (!Directory.Exists(createFolder))
                                            {
                                                diretoriosInse.Add(createFolder);
                                                Directory.CreateDirectory(createFolder);
                                            }


                                            File.Move(item, createFolder + Path.GetFileName(item));
                                            md = modeloAchado;
                                            var indices = CanhotosLeitura.indicesAnalise(md.nome, modeloAchado, sb.ToString(), sb.ToString());
                                            List<String> indicesLidos = new List<string>();
                                            foreach (var inx in indices)
                                            {
                                                indicesLidos.Add(inx.Key + " : " + inx.Value.ToString());
                                            }
                                            File.AppendAllLines(createFolder + Path.GetFileNameWithoutExtension(item) + ".txt", indicesLidos.ToArray());

                                        }
                                        else
                                        {
                                            if (md != null)
                                            {
                                                File.Move(item, createFolder + Path.GetFileName(item));
                                            }
                                            else
                                            {
                                                createFolder = agrupamento + @"\" + nomePasta + @"\" + "Arquivo_NAO_Classificado" + @"\";

                                                if (!Directory.Exists(createFolder))
                                                {
                                                    Directory.CreateDirectory(createFolder);
                                                }

                                                File.Move(item, createFolder + Path.GetFileName(item));
                                            }

                                        }
                                        numeroPagina++;



                                    }

                                    var diretorios = Directory.GetDirectories(ConfigurationManager.AppSettings["agrupamento"]);
                                    var oGdPictureImaging = new GdPictureImaging();
                                    oGdPictureImaging.SetLicenseNumber("4118106456693265856441854");
                                    string inxenvio = "";
                                    foreach (var dir in diretoriosInse)
                                    {
                                        string nomeArquivoInx = "";
                                        var arquivosIndice = new DirectoryInfo(dir).GetFiles().Where(_s => _s.Extension.Contains(".txt")).Select(_s => _s.FullName).ToArray();
                                        if (arquivosIndice.Count() > 0)
                                        {
                                            using (StreamReader sr = new StreamReader(arquivosIndice[0]))
                                            {

                                                nomeArquivoInx = sr.ReadToEnd();//!= ""? sr.ReadToEnd().Replace(':',' ') :Guid.NewGuid().ToString()
                                                if (nomeArquivoInx == "")
                                                {
                                                    nomeArquivoInx = Guid.NewGuid().ToString();

                                                }
                                                else
                                                {
                                                    nomeArquivoInx = nomeArquivoInx.Replace(':', ' ');
                                                    nomeArquivoInx = nomeArquivoInx.Replace('/', ' ');
                                                    nomeArquivoInx = Regex.Replace(nomeArquivoInx, @"\\r\\n", "").Trim();
                                                }

                                            }
                                        }

                                        if (nomeArquivoInx == "")
                                        {
                                            nomeArquivoInx = Guid.NewGuid().ToString();
                                        }
                                        string codigo = "";
                                        var arquivos = new DirectoryInfo(dir).GetFiles().Where(_s => _s.Extension.Contains(".png")).Select(_s => _s.FullName).ToArray();

                                        if (md.ID == 4061 || md.ID == 4062 || md.ID == 23)
                                        {
                                            codigo = nomeArquivoInx;
                                        }
                                        else
                                        {
                                            codigo = Regex.Replace(nomeArquivoInx, @"[^\d]", "");
                                        }
                                        inxenvio += codigo + "_";
                                        string lastFolderName = Path.GetFileName(Path.GetDirectoryName(dir));
                                        if (md.ID == 4061 || md.ID == 4062 || md.ID == 23)
                                        {
                                            oGdPictureImaging.TiffMergeFiles(arquivos, dir + md.nome + "_" + codigo + ".tif", TiffCompression.TiffCompressionLZW);

                                            File.Copy(dir + md.nome + "_" + codigo + ".tif", @"C:\AR\EntradaSplit\" + md.nome + "_" + codigo + "_" + Guid.NewGuid().ToString() + ".tif");
                                        }
                                        else
                                        {
                                            oGdPictureImaging.TiffMergeFiles(arquivos, dir + lastFolderName + "_" + codigo + ".tif", TiffCompression.TiffCompressionLZW);
                                            File.Copy(dir + lastFolderName + "_" + codigo + ".tif", @"C:\AR\Entrada\" + lastFolderName + "_" + codigo + ".tif");

                                        }

                                        foreach (var ar in arquivos)
                                        {
                                            File.Delete(ar);
                                        }
                                        foreach (var ar in arquivosIndice)
                                        {
                                            File.Delete(ar);
                                        }

                                    }
                                    if (md.ID == 4061 || md.ID == 4062 || md.ID == 23)
                                    {
                                        File.Copy(arquivo, @"C:\AR\Entrada\$_" + inxenvio + ".tif");
                                    }

                                    File.Delete(arquivo);


                                }

                            }
                            else
                            {
                                if (arquivo.Contains(".pdf"))
                                {
                                    // arquivo = cast(arquivo, Path.GetDirectoryName(arquivo));
                                }
                                if (Path.GetExtension(arquivo).Contains(".tif") || Path.GetExtension(arquivo).Contains(".tiff"))
                                {
                                    GdPictureImaging gdImage = new GdPictureImaging();
                                    gdImage.SetLicenseNumber("4118106456693265856441854");
                                    var imagemid = gdImage.CreateGdPictureImageFromFile(arquivo);

                                    //File.Delete(arquivo);
                                    gdImage.SaveAsPNG(imagemid, arquivo + ".png");

                                    arquivo = arquivo + ".png";

                                    //  arquivo
                                }


                                var antesCapturarDocumento = new Entidade().Projeto_modelo_evento.Where(_s => _s.PROJETO_MODELO.PROJETO_ID == projeto.ID && _s.Eventos.codigo == "C001").FirstOrDefault();
                                /////evento antes da indexacao
                                if (antesCapturarDocumento != null)
                                {
                                    RetornoEventoDTO retorno = null;

                                    /////evento antes da indexacao
                                    var classe = "ConsoleApplication18.Evento._" + antesCapturarDocumento.PROJETO_MODELO.ID + "_" + projeto.ID + "_" + antesCapturarDocumento.PROJETO_MODELO.MODELO_ID;
                                    using (FileStream fs = File.Create(@"c:\classe.txt"))
                                    {

                                        Byte[] info = new UTF8Encoding(true).GetBytes(classe);
                                        // Add some information to the file.
                                        fs.Write(info, 0, info.Length);
                                    }
                                    Type eventos = Type.GetType(classe);
                                    var instancia = (IEvento)Activator.CreateInstance(eventos);
                                    retorno = (RetornoEventoDTO)instancia.C001(null);

                                }

                                using (FileStream fs = File.Create(@"c:\ListaPalavras.txt"))
                                {

                                    Byte[] info = new UTF8Encoding(true).GetBytes("ListaPalavras ");
                                    // Add some information to the file.
                                    fs.Write(info, 0, info.Length);
                                }

                                var ListaPalavras = ocr.executarOcrCoordenadas(arquivo);
                                StringBuilder sb = new StringBuilder();
                                try
                                {
                                    //  var sb = ocr.executarOcr(arquivo);
                                    sb = castListaPalavras(ListaPalavras);

                                }
                                catch (Exception ex)
                                {

                                }
                                var modeloAchado = CanhotosLeitura.verificaModelo(sb, projeto.ID, 0);

                                if (modeloAchado != null)
                                {

                                    var projetoModelo = buscarProjetoModelo(projeto.ID, modeloAchado.ID);
                                    if (projetoModelo != null)
                                    {
                                        if (projetoModelo.Padrao != null && projetoModelo.Padrao != "")
                                        {
                                            switch (projetoModelo.Padrao)
                                            {
                                                case "Layout":

                                                    using (FileStream fs = File.Create(@"c:\Layout.txt"))
                                                    {

                                                        Byte[] info = new UTF8Encoding(true).GetBytes("Layout ");
                                                        // Add some information to the file.
                                                        fs.Write(info, 0, info.Length);
                                                    }
                                                    var nome = Path.GetFileName(caminho);
                                                    var nomemutex = nome.Replace(" ", "");
                                                    string caminhoretono = caminho.Replace(nome, nomemutex);
                                                    if (!File.Exists(caminhoretono))
                                                    {
                                                        File.Move(caminho, caminhoretono);
                                                        caminho = caminhoretono;
                                                    }
                                                    arquivo = caminho;
                                                    deskewImage(caminho);
                                                    string python = @"C:\Python27\Python.exe";

                                                    // python app to call 
                                                    string myPythonApp = ConfigurationManager.AppSettings["myPythonApp"];

                                                    // Create new process start info 
                                                    ProcessStartInfo myProcessStartInfo = new ProcessStartInfo(python);

                                                    // make sure we can read the output from stdout 
                                                    myProcessStartInfo.UseShellExecute = false;
                                                    myProcessStartInfo.RedirectStandardOutput = true;

                                                    // start python app with 3 arguments  
                                                    // 1st arguments is pointer to itself,  
                                                    // 2nd and 3rd are actual arguments we want to send 
                                                    myProcessStartInfo.Arguments = myPythonApp + " " + arquivo + " " + Guid.NewGuid().ToString();
                                                    using (FileStream fs = File.Create(@"c:\myPythonApp.txt"))
                                                    {

                                                        Byte[] info = new UTF8Encoding(true).GetBytes(myPythonApp + " " + arquivo + " " + Guid.NewGuid().ToString());
                                                        // Add some information to the file.
                                                        fs.Write(info, 0, info.Length);
                                                    }
                                                    Process myProcess = new Process();
                                                    // assign start information to the process 
                                                    myProcess.StartInfo = myProcessStartInfo;
                                                    myProcess.StartInfo.CreateNoWindow = true;
                                                    // start the process 
                                                    myProcess.Start();

                                                    // Read the standard output of the app we called.  
                                                    // in order to avoid deadlock we will read output first 
                                                    // and then wait for process terminate: 
                                                    StreamReader myStreamReader = myProcess.StandardOutput;
                                                    string myString = myStreamReader.ReadLine();

                                                    /*if you need to read multiple lines, you might use: 
                                                        string myString = myStreamReader.ReadToEnd() */

                                                    // wait exit signal from the app we called and then close it. 
                                                    myProcess.WaitForExit();
                                                    myProcess.Close();

                                                    string[] cortes = myString.Split('$');

                                                    List<PosicaoLabel> arquivosIndices = new List<PosicaoLabel>();
                                                    List<String> arquivosIndicesCapturados = new List<string>();
                                                    List<PosicaoLabel> PosLabel = new List<PosicaoLabel>();
                                                    foreach (var Corte in cortes)
                                                    {

                                                        var posicoes = Corte.Split(' ');
                                                        if (posicoes.Length > 2)
                                                        {
                                                            var coor = new Coordenadas()
                                                            {
                                                                X = Convert.ToInt16(posicoes[0]),
                                                                Y = Convert.ToInt16(posicoes[1]),
                                                                W = Convert.ToInt16(posicoes[2]),
                                                                H = Convert.ToInt16(posicoes[3])

                                                            };
                                                            var bp = Bitmap.FromFile(arquivo);

                                                            var arquivoCorte = CanhotosLeitura.CropImage(bp, coor.X, coor.Y, coor.W, coor.H);
                                                            arquivoCorte.Save(Guid.NewGuid() + ".png");
                                                            bp.Dispose();

                                                            ///  arquivoCorte.Save(@"C:\Users\Administrador\Documents\Visual Studio 2015\Projects\ConsoleApplication18\ConsoleApplication18\bin\Debug\Crop\" + Corte + ".png");
                                                            MemoryStream ms = new MemoryStream();
                                                            arquivoCorte.Save(ms, ImageFormat.Png);
                                                            byte[] bmpBytes = ms.ToArray();
                                                            PosicaoLabel pol = new PosicaoLabel();
                                                            pol.Coordenadas = coor;
                                                            // pol.Label = ocr.executarOcr(bmpBytes).ToString();

                                                            arquivosIndices.Add(pol);
                                                            arquivoCorte.Dispose();
                                                            ms.Close();

                                                        }
                                                    }
                                                    var db = new Entidade();

                                                    Dictionary<string, string> indiceLidos = new Dictionary<string, string>();
                                                    List<Coordenadas> lista = new List<Coordenadas>();


                                                    var informacoesModelo = db.OrientacaoDocumento.Where(_s => _s.IdModelo == projetoModelo.MODELO_ID).ToList();/// ocr.executarOcrCoordenadas(caminhoModelo);
                                                    foreach (var item in informacoesModelo)
                                                    {
                                                        using (FileStream fs = File.Create(@"c:\item.txt"))
                                                        {
                                                            Byte[] info = new UTF8Encoding(true).GetBytes(item.Palavra);
                                                            // Add some information to the file.
                                                            fs.Write(info, 0, info.Length);
                                                        }
                                                        var pal = item.Palavra.Trim();
                                                        var s = ListaPalavras.Where(_s => _s.texto == pal).ToList();
                                                        string fdsa = "";
                                                        foreach (var sssaz in ListaPalavras)
                                                        {
                                                            fdsa += sssaz.texto + "\n  ";
                                                        }

                                                        using (FileStream fs = File.Create(@"c:\fdsa.txt"))
                                                        {
                                                            Byte[] info = new UTF8Encoding(true).GetBytes(fdsa);
                                                            // Add some information to the file.
                                                            fs.Write(info, 0, info.Length);
                                                        }

                                                        foreach (var ss in s)
                                                        {

                                                            var dsss = arquivosIndices.Where(_s => (_s.Coordenadas.X <= ss.x && (_s.Coordenadas.X + 100 > ss.x)) && (_s.Coordenadas.Y <= ss.y && (_s.Coordenadas.Y + 100 > ss.y))).OrderBy(x => x.Coordenadas.X).ThenBy(x => x.Coordenadas.Y).ToList();
                                                            using (FileStream fs = File.Create(@"c:\dsss.txt"))
                                                            {
                                                                Byte[] info = new UTF8Encoding(true).GetBytes(dsss.Count.ToString());
                                                                // Add some information to the file.
                                                                fs.Write(info, 0, info.Length);
                                                            }
                                                            foreach (var c in dsss)
                                                            {
                                                                var bp = Bitmap.FromFile(arquivo);
                                                                var arquivoCorte = CanhotosLeitura.CropImage(bp, c.Coordenadas.X, c.Coordenadas.Y, c.Coordenadas.W, c.Coordenadas.H);

                                                                var arquivoCorteIndice = Guid.NewGuid() + ".png";

                                                                using (FileStream fs = File.Create(@"c:\ssex.txt"))
                                                                {
                                                                    Byte[] info = new UTF8Encoding(true).GetBytes("ssex".ToString());
                                                                    // Add some information to the file.
                                                                    fs.Write(info, 0, info.Length);
                                                                }
                                                                arquivoCorte.Save(arquivoCorteIndice);
                                                                var btArray = ToByteArray(arquivoCorte, ImageFormat.Png);
                                                                PosLabel.Add(new PosicaoLabel() { Coordenadas = c.Coordenadas, Label = ocr.executarOcr(arquivoCorteIndice).ToString() });

                                                                bp.Dispose();
                                                                File.Delete(arquivoCorteIndice);
                                                            }
                                                        }

                                                        if (item.Palavra == null)
                                                        {
                                                            continue;

                                                        }
                                                        else
                                                        {
                                                            //var ds = arquivosIndices.Where(_s => _s.Label.Contains("LA")).ToList();
                                                            foreach (var indicies in PosLabel)
                                                            {


                                                                var regex = ""; //"/(13)|(Operadora)|(-)|(Codigo)/g";
                                                                int contador = 0;
                                                                var palavras = item.Palavra.Split(' ');
                                                                regex = "";
                                                                for (int i = 0; i < palavras.Length; i++)
                                                                {

                                                                    if (indicies.Label.Trim().ToUpper().Contains(palavras[i].ToUpper()))
                                                                    {


                                                                        regex += "(" + palavras[i] + ")";
                                                                        if (contador < palavras.Length && contador > 0)
                                                                            regex += "|";
                                                                        if (contador == 0 && palavras.Length > 1)
                                                                            regex += "|";
                                                                        contador++;
                                                                    }
                                                                }
                                                                if (contador == palavras.Length)
                                                                {

                                                                    if (item.Evento.HasValue && item.Evento.Value)
                                                                    {

                                                                        var DurtanteCapturarDocumento = new Entidade().Projeto_modelo_evento.Where(_s => _s.PROJETO_MODELO.ID == projetoModelo.ID && _s.Eventos.codigo == "C004").ToList();
                                                                        /////evento antes da indexacao
                                                                        if (DurtanteCapturarDocumento.Count > 0)////significa que tem evento
                                                                        {

                                                                            using (FileStream fs = File.Create(@"c:\logeventodurante.txt"))
                                                                            {
                                                                                Byte[] info = new UTF8Encoding(true).GetBytes(DurtanteCapturarDocumento.ToString());
                                                                                // Add some information to the file.
                                                                                fs.Write(info, 0, info.Length);
                                                                            }
                                                                            if (item.PARAMETROENTRADA != "")
                                                                            {


                                                                                using (FileStream fs = File.Create(@"c:\PARAMETROENTRADA.txt"))
                                                                                {

                                                                                    Byte[] info = new UTF8Encoding(true).GetBytes("PARAMETROENTRADA ");
                                                                                    // Add some information to the file.
                                                                                    fs.Write(info, 0, info.Length);
                                                                                }

                                                                                Enum.TryParse(item.PARAMETROENTRADA, out EnumEntrada entrada);

                                                                                var classe = "ConsoleApplication18.Evento._" + projetoModelo.ID + "_" + projetoModelo.PROJETO_ID + "_" + modeloAchado.ID;

                                                                                using (FileStream fs = File.Create(@"c:\PARAMETROENTRADAclasse"))
                                                                                {

                                                                                    Byte[] info = new UTF8Encoding(true).GetBytes(classe);
                                                                                    // Add some information to the file.
                                                                                    fs.Write(info, 0, info.Length);
                                                                                }

                                                                                Type eventos = Type.GetType(classe);
                                                                                var instancia = (IEvento)Activator.CreateInstance(eventos);
                                                                                switch (entrada)
                                                                                {
                                                                                    case EnumEntrada.TEXTO:
                                                                                        break;
                                                                                    case EnumEntrada.NUMERO:
                                                                                        break;
                                                                                    case EnumEntrada.DATA:

                                                                                        var retorno = (RetornoEventoDTO)instancia.C004(indicies, entrada);
                                                                                        if (retorno != null && retorno.Retono != null)

                                                                                        {
                                                                                            if (retorno.RetornoTipo == EnumRetornoEvento.RETORNO_IMAGEM)
                                                                                            {
                                                                                                indiceLidos.Add(item.Palavra, indicies + "_parametro_" + ((string)retorno.Retono).ToString() + "_mensagem_" + retorno.MensagemRetorno);
                                                                                            }
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            indiceLidos.Add(item.Palavra, indicies.Label);
                                                                                        }
                                                                                        break;
                                                                                    case EnumEntrada.IMAGEM:
                                                                                        var bitmap = Bitmap.FromFile(arquivo);
                                                                                        var dcrops = CanhotosLeitura.CropImage(bitmap, indicies.Coordenadas.X, indicies.Coordenadas.Y, indicies.Coordenadas.W, indicies.Coordenadas.H);
                                                                                        retorno = (RetornoEventoDTO)instancia.C004((System.Drawing.Image)dcrops, EnumEntrada.IMAGEM);
                                                                                        dcrops.Dispose();
                                                                                        if (retorno != null)
                                                                                        {
                                                                                            if (retorno.RetornoTipo == EnumRetornoEvento.RETORNO_STRING)
                                                                                            {
                                                                                                indiceLidos.Add(item.Palavra, ((string)retorno.Retono).ToString());
                                                                                            }
                                                                                        }
                                                                                        break;
                                                                                    default:
                                                                                        break;
                                                                                }



                                                                            }
                                                                        }
                                                                    }
                                                                    else if (item.Regex != null && item.Regex != "")
                                                                    {

                                                                        var rgxarra = indicies.Label.Split(' ');
                                                                        foreach (var sp in rgxarra)
                                                                        {

                                                                            Regex r = new Regex(item.Regex, RegexOptions.None);

                                                                            // Match the regular expression pattern against a text string.
                                                                            Match m = r.Match(sp.Trim());
                                                                            int matchCount = 0;
                                                                            if (m.Success)
                                                                            {
                                                                                if (indiceLidos.Where(_s => _s.Key == item.Palavra).Count() == 0)
                                                                                    indiceLidos.Add(item.Palavra, m.Value);
                                                                                break;

                                                                            }

                                                                        }


                                                                    }
                                                                    else
                                                                    {
                                                                        regex += "";
                                                                        Regex rgx = new Regex(regex);
                                                                        string replacement = "";
                                                                        string result = rgx.Replace(indicies.Label, replacement);
                                                                        string output = Regex.Replace(indicies.Label, regex, "");
                                                                        var resultadoIndice = result.Replace("\n", String.Empty).Split(' ').Distinct();
                                                                        string rtnIndc = "";
                                                                        foreach (var itemidc in resultadoIndice)
                                                                        {
                                                                            rtnIndc += itemidc + " ";
                                                                        }


                                                                        indiceLidos.Add(item.Palavra, rtnIndc);
                                                                        break;
                                                                    }

                                                                }


                                                            }

                                                        }

                                                        if (item.EspacoBranco != null && item.EspacoBranco.Trim() == "")
                                                        {
                                                            string key = "";
                                                            string indiceArrumado = "";
                                                            var indice = indiceLidos.Where(_s => _s.Key == item.Palavra).FirstOrDefault();
                                                            key = indice.Key;
                                                            if (item.EspacoBranco == "FIM")
                                                            {
                                                                indiceArrumado = indice.Value.TrimEnd();
                                                            }
                                                            if (item.EspacoBranco == "INICIO")
                                                            {
                                                                indiceArrumado = indice.Value.TrimStart();
                                                            }
                                                            if (item.EspacoBranco == "TODOS")
                                                            {
                                                                indiceArrumado = indice.Value.Replace(" ", "").Trim();
                                                            }
                                                            if (item.EspacoBranco == "NENHUM")
                                                            {
                                                                indiceArrumado = indice.Value;
                                                            }

                                                            indiceLidos.Remove(key);
                                                            indiceLidos.Add(key, indiceArrumado);
                                                        }
                                                    }
                                                    var indices = indiceLidos;
                                                    string modeloMove = saidaProcessado + @"\" + projeto.ID + @"\" + usuarioAut.email.Trim() + @"\" + Path.GetFileName(arquivo);
                                                    if (!File.Exists(modeloMove))
                                                        File.Move(arquivo, modeloMove);


                                                    ////aqui eu faço a captura dos indices
                                                    string nomeGerado = Guid.NewGuid().ToString();



                                                    escreveLog(modeloAchado.ID, projeto.ID, sb, Path.GetFileName(arquivo), ocr.tipo(), modeloAchado.nome, modeloMove, nomeGerado, indices, usuarioAut);
                                                    bool warning = false;
                                                    foreach (var item in indices)
                                                    {
                                                        if (item.Value.Contains("_parametro_"))
                                                        {
                                                            warning = true
                                                                ;
                                                            break;

                                                        }
                                                    }
                                                    try
                                                    {
                                                        if (warning)
                                                        {
                                                            //     _hub.Invoke("Hello", new object[] { Path.GetFileName(arquivo), usuarioId, "WARNING", indices });
                                                            sendMessage(new object[] { Path.GetFileName(arquivo), usuarioId, "WARNING", indices });
                                                        }
                                                        else
                                                        {
                                                            // _hub.Invoke("Hello", new object[] { Path.GetFileName(arquivo), usuarioId, "OK", indices });
                                                            sendMessage(new object[] { Path.GetFileName(arquivo), usuarioId, "OK", indices });
                                                        }
                                                    }
                                                    catch (Exception ex)
                                                    {

                                                    }



                                                    break;
                                                case "Regex":

                                                    string saidaPasta = itemCANHOTO + @"SAIDA\" + projeto.ID + @"\" + modeloAchado.nome;
                                                    if (!Directory.Exists(saidaPasta))
                                                    {
                                                        Directory.CreateDirectory(saidaPasta);

                                                    }

                                                    modeloMove = saidaProcessado + @"\" + projeto.ID + @"\" + usuarioAut.email.Trim() + @"\" + Path.GetFileName(arquivo);
                                                    if (!File.Exists(modeloMove))
                                                    {
                                                        File.Move(arquivo, modeloMove);
                                                    }

                                                    ////aqui eu faço a captura dos indices
                                                    nomeGerado = Guid.NewGuid().ToString();
                                                    var ssb = ocr.executarOcr(modeloMove);
                                                    indices = CanhotosLeitura.indicesAnalise(modeloMove, modeloAchado, ssb.ToString(), sb.ToString());


                                                    //  CanhotosLeitura.criarDocumentoIndice(indices, itemCANHOTO + modeloAchado.nome + @"\", nomeGerado);
                                                    //  CanhotosLeitura.criarDocumentoIndice(indices, itemCANHOTO + modeloAchado.nome + @"\", nomeGerado);
                                                    escreveLog(modeloAchado.ID, projeto.ID, sb, Path.GetFileName(arquivo), ocr.tipo(), modeloAchado.nome, modeloMove, nomeGerado, indices, usuarioAut);
                                                    try
                                                    {
                                                        //  _hub.Invoke("Hello", new object[] { Path.GetFileName(arquivo), usuarioId, "OK", indices });
                                                        sendMessage(new object[] { Path.GetFileName(arquivo), usuarioId, "OK", indices });
                                                    }
                                                    catch (Exception ex)
                                                    {

                                                    }
                                                    break;
                                                case "Cortes":



                                                    modeloMove = saidaProcessado + @"\" + projeto.ID + @"\" + usuarioAut.email.Trim() + @"\" + Path.GetFileName(arquivo);

                                                    if (!File.Exists(modeloMove))
                                                        File.Move(arquivo, modeloMove);


                                                    ////aqui eu faço a captura dos indices
                                                    nomeGerado = Guid.NewGuid().ToString();
                                                    indices = CanhotosLeitura.indices(modeloMove, modeloAchado, projetoModelo.PROJETO_ID, ListaPalavras);
                                                    //  CanhotosLeitura.criarDocumentoIndice(indices, itemCANHOTO + modeloAchado.nome + @"\", nomeGerado);
                                                    //   CanhotosLeitura.criarDocumentoIndice(indices, itemCANHOTO + modeloAchado.nome + @"\", nomeGerado);
                                                    escreveLog(modeloAchado.ID, projeto.ID, sb, Path.GetFileName(arquivo), ocr.tipo(), modeloAchado.nome, modeloMove, nomeGerado, indices, usuarioAut);
                                                    warning = false;
                                                    foreach (var item in indices)
                                                    {
                                                        if (item.Value.Contains("_parametro_"))
                                                        {
                                                            warning = true;
                                                            break;

                                                        }
                                                    }
                                                    try
                                                    {
                                                        if (warning)
                                                        {
                                                            // _hub.Invoke("Hello", new object[] { Path.GetFileName(arquivo), usuarioId, "WARNING", indices });
                                                            sendMessage(new object[] { Path.GetFileName(arquivo), usuarioId, "WARNING", indices });
                                                        }
                                                        else
                                                        {
                                                            //_hub.Invoke("Hello", new object[] { Path.GetFileName(arquivo), usuarioId, "OK", indices });
                                                            sendMessage(new object[] { Path.GetFileName(arquivo), usuarioId, "OK", indices });
                                                        }
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        using (FileStream fs = File.Create(@"c:\logerrosgugneal.txt"))
                                                        {
                                                            Byte[] info = new UTF8Encoding(true).GetBytes(ex.Message);
                                                            // Add some information to the file.
                                                            fs.Write(info, 0, info.Length);
                                                        }

                                                    }


                                                    break;

                                            }
                                        }


                                    }
                                    ////erro 
                                    else
                                    {
                                        try
                                        {
                                            //   _hub.Invoke("Hello", new String[] { "erro", usuarioId, "ERRO" });
                                            sendMessage(new object[] { "erro", usuarioId, "ERRO" });
                                        }
                                        catch (Exception ex)
                                        {

                                        }
                                    }

                                }
                                else
                                {
                                    try
                                    {
                                        File.Delete(caminho);
                                        //     _hub.Invoke("Hello", new String[] { "", usuarioId, "WARNING", "Não foi encontrado modelo ", null });
                                        sendMessage(new object[] { "", usuarioId, "WARNING", "Não foi encontrado modelo ", null });
                                    }
                                    catch (Exception ex)
                                    {

                                    }
                                    //var lis = ProcessImage(arquivo, projeto.ID);
                                    //if (lis != null && lis.score > Convert.ToInt32(ScoreApp))
                                    //{

                                    //    string modeloMove = saidaProcessado + @"\" + projeto.ID + @"\" + usuarioAut.email + @"\" + Path.GetFileName(arquivo);
                                    //    if (!File.Exists(modeloMove))
                                    //        File.Move(arquivo, modeloMove);


                                    //    ////aqui eu faço a captura dos indices
                                    //    string nm = Guid.NewGuid().ToString();
                                    //    //  var indices = CanhotosLeitura.indices(modeloMove, modeloAchado);
                                    //    //   CanhotosLeitura.criarDocumentoIndice(indices, itemCANHOTO + modeloAchado.nome + @"\", nomeGerado);
                                    //    escreveLog(projeto.ID, sb, Path.GetFileName(arquivo), ocr.tipo(), lis.modelo.nome, modeloMove, nm, null, usuarioAut);


                                    //}


                                    if (!Directory.Exists(saidaProcessado + " Arquivo_NAO_Classificado"))
                                    {
                                        Directory.CreateDirectory(saidaProcessado + "Arquivo_NAO_Classificado" + @"\");
                                    }
                                    string nomeGerado = Guid.NewGuid().ToString();
                                    if(File.Exists(arquivo))
                                    File.Move(arquivo, saidaProcessado + "Arquivo_NAO_Classificado" + @"\" + Path.GetFileName(arquivo));
                                    if (modeloAchado == null)
                                    {
                                        escreveLog(0, projeto.ID, sb, Path.GetFileName(arquivo), ocr.tipo(), "Não ACHOU", saidaProcessado + "Arquivo_NAO_Classificado" + @"\" + Path.GetFileName(arquivo), nomeGerado, null, usuarioAut);

                                    }
                                    else
                                    {
                                        escreveLog(modeloAchado.ID, projeto.ID, sb, Path.GetFileName(arquivo), ocr.tipo(), "Não ACHOU", saidaProcessado + "Arquivo_NAO_Classificado" + @"\" + Path.GetFileName(arquivo), nomeGerado, null, usuarioAut);

                                    }
                                }
                            }

                            //pastas.@lock = false;
                            //context.documentocaptura.Remove(pastas);
                            //context.SaveChanges();



                        }
                    }
                    catch (Exception ex)
                    {
                        using (FileStream fs = File.Create(@"c:\errogeral.txt"))
                        {
                            var st = new StackTrace(ex, true);
                            // Get the top stack frame
                            var frame = st.GetFrame(0);
                            var line = frame.GetFileLineNumber();
                            Byte[] info = new UTF8Encoding(true).GetBytes("Line " + line + "   " + ex.Message + "\n" + ex.StackTrace + "\n" + ex.InnerException);
                            // Add some information to the file.
                            fs.Write(info, 0, info.Length);
                        }
                        throw new Exception(ex.Message);
                    }
                }
            }
        }
        public static byte[] ToByteArray(System.Drawing.Image image, ImageFormat format)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, format);
                return ms.ToArray();
            }
        }

        public static void sendMessage(object[] objeto)
        {
            string url = ConfigurationManager.AppSettings["url"];

            var connection = new HubConnection(url);
            //Make proxy to hub based on hub name on server
            var myHub = connection.CreateHubProxy("TestHub");
            //Start connection

            connection.Start().Wait();

            for (int i = 0; i < objeto.Length; i++)
            {
                if (objeto[i] == null)
                {
                    objeto[i] = new Dictionary<string, string>();
                }
            }
            myHub.Invoke<string>("Hello", objeto).Wait();
        }

        public static void deskewImage(string caminho)
        {
            //We assume that GdPicture has been correctly installed and unlocked.
            GdPictureImaging oGdPictureImaging = new GdPictureImaging();
            oGdPictureImaging.SetLicenseNumber("4118106456693265856441854");
            //Load the image from a file.
            int imageId = oGdPictureImaging.CreateGdPictureImageFromFile(caminho);
            //Check, if the image resource has been loaded correctly.
            if (oGdPictureImaging.GetStat() != GdPictureStatus.OK)
            {

            }
            else
            {
                //You can AutoDeskew your image.
                GdPictureStatus status = oGdPictureImaging.AutoDeskew(imageId);
                if (status != GdPictureStatus.OK)
                {

                }
                else
                {
                    //After you are done with your processing, you can save the image.
                    status = oGdPictureImaging.SaveAsPNG(imageId, caminho);
                    if (status != GdPictureStatus.OK)
                    {
                        //  MessageBox.Show("Error: " + oGdPictureImaging.GetStat().ToString());
                    }
                }
                oGdPictureImaging.ReleaseGdPictureImage(imageId);
            }
            oGdPictureImaging.Dispose();
        }
        //private static void OnChangedDelay(string caminho)
        //{

        //    try
        //    {

        //        deskewImage(caminho);


        //        string itemCANHOTO = ConfigurationManager.AppSettings["caminhoTiff"];
        //        string str = ConfigurationManager.AppSettings["saidaPdf"];
        //        string ScoreApp = ConfigurationManager.AppSettings["Score"];
        //        string item1 = ConfigurationManager.AppSettings["saidaErro"];
        //        string str1 = ConfigurationManager.AppSettings["saidaBack"];
        //        string item2 = ConfigurationManager.AppSettings["ftpUrl"];
        //        string str2 = ConfigurationManager.AppSettings["usuarioFtp"];
        //        string saidaProcessado = ConfigurationManager.AppSettings["saidaProcessado"];
        //        string item3 = ConfigurationManager.AppSettings["senhaFtp"];

        //        using (FileStream fs = File.Create(@"c:\logprocessando.txt"))
        //        {
        //            Byte[] info = new UTF8Encoding(true).GetBytes(caminho);
        //            // Add some information to the file.
        //            fs.Write(info, 0, info.Length);
        //        }


        //        PROJETO projeto = null;
        //        var context = new Entidade();
        //        string[] d = caminho.Split('\\');
        //        string s = "";
        //        for (int i = 0; i < d.Length; i++)
        //        {
        //            s += d[i] + "$";
        //        }
        //        using (FileStream fs = File.Create(@"c:\logprocessandoArqProjetoCount.txt"))
        //        {
        //            Byte[] info = new UTF8Encoding(true).GetBytes("processando" + d.Count() +" valo " +s);
        //            // Add some information to the file.
        //            fs.Write(info, 0, info.Length);
        //        }
        //        if (d.Count() >= 7)
        //        {
        //            int idProjeto = 0;
        //            int.TryParse(d[4], out idProjeto);

        //            projeto = getprojeto(idProjeto);

        //            using (FileStream fs = File.Create(@"c:\logprocessandoArqProjeto.txt"))
        //            {
        //                Byte[] info = new UTF8Encoding(true).GetBytes("processando" + projeto.Nome);
        //                // Add some information to the file.
        //                fs.Write(info, 0, info.Length);
        //            }



        //            if (d[3].ToUpper().Trim() == "SAIDA")
        //            {
        //                return;
        //            }
        //            var _hub = getInstance();
        //            try
        //            {
        //                var caminhoArquivo = caminho;
        //                IOcr ocr = Factory.FactoryOcr.FactoryOcrInstancia(idProjeto);

        //                string usuarioEmail = d[5];
        //                var usuarioAut = context.Usuario.Where(_usu => _usu.email == usuarioEmail).FirstOrDefault();
        //                var usuarioId = usuarioAut.Id.ToString();
        //                string arquivo = caminhoArquivo;// pastas.caminhoArquivo;

        //                if (!File.Exists(arquivo))
        //                {
        //                    return;
        //                }

        //                if (arquivo.Contains("ANDROID"))
        //                {
        //                    ///post.modelo + "$" + post.nota + "$" + post.serial + "$" + Path.GetFileName(file.FileName);
        //                    //ws.GravarFolderCloudDocs(Convert.ToDecimal(protocolo), 327, 2583, 3271, 2583, doc.Nota, doc.Serie, modelo.nome, "", "", "Canhoto Digitalizado", "", doc.binario, doc.extensao, 16349, "", 0);
        //                    var prop = Path.GetFileName(arquivo).Split('$');
        //                    CanhotosLeitura.Documento doc = new CanhotosLeitura.Documento();
        //                    doc.Nota = prop[1];
        //                    doc.Serie = prop[2];
        //                    doc.binario = File.ReadAllBytes(arquivo);
        //                    doc.extensao = Path.GetExtension(arquivo).Replace(".", "");
        //                    doc.localizacao = prop[3];
        //                    Modelo md = new Modelo();
        //                    md.nome = prop[0];

        //                    CanhotosLeitura.UploadDocCloudFile(doc, md);
        //                    File.Delete(arquivo);
        //                }
        //                else
        //                {
        //                    using (FileStream fs = File.Create(@"c:\logprocessandoArq.txt"))
        //                    {
        //                        Byte[] info = new UTF8Encoding(true).GetBytes("processando" +caminho);
        //                        // Add some information to the file.
        //                        fs.Write(info, 0, info.Length);
        //                    }

        //                    if (arquivo.Contains(".pdf"))
        //                    {
        //                        arquivo = cast(arquivo, Path.GetDirectoryName(arquivo));
        //                    }
        //                    if (arquivo.Contains(".tif") || arquivo.Contains(".tiff"))
        //                    {

        //                        arquivo = cast(arquivo, Path.GetDirectoryName(arquivo));
        //                        //  pastas.caminhoArquivo = arquivo;
        //                    }

        //                    var antesCapturarDocumento = new Entidade().Projeto_modelo_evento.Where(_s => _s.PROJETO_MODELO.PROJETO_ID == projeto.ID && _s.Eventos.codigo == "C001").FirstOrDefault();
        //                    /////evento antes da indexacao
        //                    if (antesCapturarDocumento != null)
        //                    {
        //                        RetornoEventoDTO retorno = null;

        //                        /////evento antes da indexacao
        //                        var classe = "ConsoleApplication18.Evento._" + antesCapturarDocumento.PROJETO_MODELO.ID + "_" + projeto.ID + "_" + antesCapturarDocumento.PROJETO_MODELO.MODELO_ID;
        //                        Type eventos = Type.GetType(classe);
        //                        var instancia = (IEvento)Activator.CreateInstance(eventos);
        //                        retorno = (RetornoEventoDTO)instancia.C001(null);

        //                    }


        //                    var ListaPalavras = ocr.executarOcrCoordenadas(arquivo);

        //                    //  var sb = ocr.executarOcr(arquivo);
        //                    var sb = castListaPalavras(ListaPalavras);

        //                    var modeloAchado = CanhotosLeitura.verificaModelo(sb, projeto.ID);
        //                    if (modeloAchado != null)
        //                    {

        //                        var projetoModelo = buscarProjetoModelo(projeto.ID, modeloAchado.ID);



        //                        if (projetoModelo != null)
        //                        {
        //                            if (projetoModelo.Padrao == null || projetoModelo.Padrao.Value == true)
        //                            {
        //                                using (FileStream fs = File.Create(@"c:\logPadrao.txt"))
        //                                {
        //                                    Byte[] info = new UTF8Encoding(true).GetBytes("padrao");
        //                                    // Add some information to the file.
        //                                    fs.Write(info, 0, info.Length);
        //                                }

        //                                string modeloMove = saidaProcessado + @"\" + projeto.ID + @"\" + usuarioAut.email + @"\" + Path.GetFileName(arquivo);

        //                                if (!File.Exists(modeloMove))
        //                                    File.Move(arquivo, modeloMove);


        //                                ////aqui eu faço a captura dos indices
        //                                string nomeGerado = Guid.NewGuid().ToString();
        //                                var indices = CanhotosLeitura.indices(modeloMove, modeloAchado, projetoModelo.PROJETO_ID, ListaPalavras);
        //                                //  CanhotosLeitura.criarDocumentoIndice(indices, itemCANHOTO + modeloAchado.nome + @"\", nomeGerado);
        //                                //   CanhotosLeitura.criarDocumentoIndice(indices, itemCANHOTO + modeloAchado.nome + @"\", nomeGerado);
        //                                escreveLog(projeto.ID, sb, Path.GetFileName(arquivo), ocr.tipo(), modeloAchado.nome, modeloMove, nomeGerado, indices, usuarioAut);
        //                                bool warning = false;
        //                                foreach (var item in indices)
        //                                {
        //                                    if (item.Value.Contains("_parametro_"))
        //                                    {
        //                                        warning = true
        //                                            ;
        //                                        break;

        //                                    }
        //                                }
        //                                try
        //                                {
        //                                    if (warning)
        //                                    {
        //                                        _hub.Invoke("Hello", new object[] { Path.GetFileName(arquivo), usuarioId, "WARNING", indices });
        //                                    }
        //                                    else
        //                                    {
        //                                        _hub.Invoke("Hello", new object[] { Path.GetFileName(arquivo), usuarioId, "OK", indices });
        //                                    }
        //                                }
        //                                catch (Exception ex)
        //                                {

        //                                }

        //                            }
        //                            else if (!projetoModelo.Padrao.Value)//////capturar somente por regex 
        //                            {

        //                                using (FileStream fs = File.Create(@"c:\logeNaoPadrao.txt"))
        //                                {
        //                                    Byte[] info = new UTF8Encoding(true).GetBytes("Nao padrao");
        //                                    // Add some information to the file.
        //                                    fs.Write(info, 0, info.Length);
        //                                }
        //                                string saidaPasta = itemCANHOTO + @"SAIDA\" + projeto.ID + @"\" + modeloAchado.nome;
        //                                if (!Directory.Exists(saidaPasta))
        //                                {
        //                                    Directory.CreateDirectory(saidaPasta);

        //                                }
        //                                string modeloMove = saidaPasta + @"\" + Path.GetFileName(arquivo);
        //                                if (!File.Exists(modeloMove))
        //                                    File.Move(arquivo, modeloMove);


        //                                ////aqui eu faço a captura dos indices
        //                                string nomeGerado = Guid.NewGuid().ToString();
        //                                var indices = CanhotosLeitura.indicesAnalise(modeloMove, modeloAchado, sb.ToString());
        //                                //  CanhotosLeitura.criarDocumentoIndice(indices, itemCANHOTO + modeloAchado.nome + @"\", nomeGerado);
        //                                //  CanhotosLeitura.criarDocumentoIndice(indices, itemCANHOTO + modeloAchado.nome + @"\", nomeGerado);
        //                                escreveLog(projeto.ID, sb, Path.GetFileName(arquivo), ocr.tipo(), modeloAchado.nome, modeloMove, nomeGerado, indices, usuarioAut);
        //                                try
        //                                {
        //                                    _hub.Invoke("Hello", new object[] { Path.GetFileName(arquivo), usuarioId, "OK", indices });
        //                                }
        //                                catch (Exception ex)
        //                                {

        //                                }
        //                            }
        //                        }
        //                        ////erro 
        //                        else
        //                        {
        //                            try
        //                            {
        //                                _hub.Invoke("Hello", new String[] { "erro", usuarioId, "ERRO" });
        //                            }
        //                            catch (Exception ex)
        //                            {

        //                            }
        //                        }

        //                    }
        //                    else
        //                    {
        //                        _hub.Invoke("Hello", new String[] { "", usuarioId, "WARNING", "Não foi encontrado modelo p", null });
        //                        var lis = ProcessImage(arquivo, projeto.ID);
        //                        if (lis != null && lis.score > Convert.ToInt32(ScoreApp))
        //                        {

        //                            string modeloMove = saidaProcessado + @"\" + projeto.ID + @"\" + usuarioAut.email + @"\" + Path.GetFileName(arquivo);
        //                            if (!File.Exists(modeloMove))
        //                                File.Move(arquivo, modeloMove);


        //                            ////aqui eu faço a captura dos indices
        //                            string nm = Guid.NewGuid().ToString();
        //                            //  var indices = CanhotosLeitura.indices(modeloMove, modeloAchado);
        //                            //   CanhotosLeitura.criarDocumentoIndice(indices, itemCANHOTO + modeloAchado.nome + @"\", nomeGerado);
        //                            escreveLog(projeto.ID, sb, Path.GetFileName(arquivo), ocr.tipo(), lis.modelo.nome, modeloMove, nm, null, usuarioAut);


        //                        }


        //                        if (!Directory.Exists(saidaProcessado + " Arquivo_NAO_Classificado"))
        //                        {
        //                            Directory.CreateDirectory(saidaProcessado + "Arquivo_NAO_Classificado" + @"\");
        //                        }
        //                        string nomeGerado = Guid.NewGuid().ToString();
        //                        File.Move(arquivo, saidaProcessado + "Arquivo_NAO_Classificado" + @"\" + Path.GetFileName(arquivo));
        //                        escreveLog(projeto.ID, sb, Path.GetFileName(arquivo), ocr.tipo(), "Não ACHOU", itemCANHOTO + "Arquivo_NAO_Classificado" + @"\" + Path.GetFileName(arquivo), nomeGerado, null, usuarioAut);
        //                    }
        //                }

        //                //pastas.@lock = false;
        //                //context.documentocaptura.Remove(pastas);
        //                //context.SaveChanges();



        //            }
        //            catch (Exception ex)
        //            {
        //                using (FileStream fs = File.Create(@"c:\logerroArquivo.txt"))
        //                {
        //                    Byte[] info = new UTF8Encoding(true).GetBytes(ex.Message);
        //                    // Add some information to the file.
        //                    fs.Write(info, 0, info.Length);
        //                }
        //            }
        //        }
        //    }
        //    catch(Exception ex)
        //    {
        //        using (FileStream fs = File.Create(@"c:\logerr.txt"))
        //        {
        //            Byte[] info = new UTF8Encoding(true).GetBytes(ex.Message);
        //            // Add some information to the file.
        //            fs.Write(info, 0, info.Length);
        //        }
        //    }
        //}



        private static void OnChanged(object source, FileSystemEventArgs e)
        {
            while (IsFileLocked(e.FullPath))
            {


            }

            OnChangedDelay(e.FullPath);

            //string saidaProcessado = ConfigurationManager.AppSettings["saidaProcessado"];
            //criarPastasSaidaProjetos(saidaProcessado);
            //string ModelosClassificador = ConfigurationManager.AppSettings["ModelosClassificador"];
            //criarPastasModelo(ModelosClassificador);
            //criarPastasSaida(ModelosClassificador);
            //string itemCANHOTO = ConfigurationManager.AppSettings["caminhoTiff"];
            //criarPastasProjetos(itemCANHOTO);

            //criarPastasProjetosEntradaRep(itemCANHOTO);
            //deskewImage(e.FullPath);

            //string str = ConfigurationManager.AppSettings["saidaPdf"];
            //string ScoreApp = ConfigurationManager.AppSettings["Score"];
            //string item1 = ConfigurationManager.AppSettings["saidaErro"];
            //string str1 = ConfigurationManager.AppSettings["saidaBack"];
            //string item2 = ConfigurationManager.AppSettings["ftpUrl"];
            //string str2 = ConfigurationManager.AppSettings["usuarioFtp"];

            //string item3 = ConfigurationManager.AppSettings["senhaFtp"];




            //PROJETO projeto = null;
            //var context = new Entidade();
            //string[] d = e.FullPath.Split('\\');

            //if (d.Count() >= 6)
            //{

            //    if (d[3].ToUpper().Trim() == "SAIDA")
            //    {
            //        return;
            //    }

            //    int idProjeto = 0;
            //    int.TryParse(d[4], out idProjeto);

            //    projeto = getprojeto(idProjeto);

            //    var _hub = getInstance();
            //    try
            //    {
            //        var caminhoArquivo = e.FullPath;
            //        IOcr ocr = Factory.FactoryOcr.FactoryOcrInstancia(idProjeto);
            //        // var usuario = d[5];//Directory.GetParent(pastas.caminhoArquivo);
            //        string usuarioEmail = d[5];

            //        var usuarioAut = context.Usuario.Where(_usu => _usu.email == usuarioEmail).FirstOrDefault();
            //        var usuarioId = usuarioAut.Id.ToString();
            //        string arquivo = caminhoArquivo;// pastas.caminhoArquivo;

            //        if (!File.Exists(arquivo))
            //        {
            //            return;
            //        }

            //        if (arquivo.Contains("ANDROID"))
            //        {
            //            ///post.modelo + "$" + post.nota + "$" + post.serial + "$" + Path.GetFileName(file.FileName);
            //            //ws.GravarFolderCloudDocs(Convert.ToDecimal(protocolo), 327, 2583, 3271, 2583, doc.Nota, doc.Serie, modelo.nome, "", "", "Canhoto Digitalizado", "", doc.binario, doc.extensao, 16349, "", 0);
            //            var prop = Path.GetFileName(arquivo).Split('$');
            //            CanhotosLeitura.Documento doc = new CanhotosLeitura.Documento();
            //            doc.Nota = prop[1];
            //            doc.Serie = prop[2];
            //            doc.binario = File.ReadAllBytes(arquivo);
            //            doc.extensao = Path.GetExtension(arquivo).Replace(".", "");
            //            doc.localizacao = prop[3];
            //            Modelo md = new Modelo();
            //            md.nome = prop[0];

            //            CanhotosLeitura.UploadDocCloudFile(doc, md);
            //            File.Delete(arquivo);
            //        }
            //        else
            //        {

            //            if (arquivo.Contains(".pdf"))
            //            {
            //                var listaarquivo = cast(arquivo, Path.GetDirectoryName(arquivo));
            //            }
            //            if (arquivo.Contains(".tif") || arquivo.Contains(".tiff"))
            //            {

            //                var listaarquivo = cast(arquivo, Path.GetDirectoryName(arquivo));
            //                //  pastas.caminhoArquivo = arquivo;
            //            }

            //            var antesCapturarDocumento = new Entidade().Projeto_modelo_evento.Where(_s => _s.PROJETO_MODELO.PROJETO_ID == projeto.ID && _s.Eventos.codigo == "C001").FirstOrDefault();
            //            /////evento antes da indexacao
            //            if (antesCapturarDocumento != null)
            //            {
            //                RetornoEventoDTO retorno = null;

            //                /////evento antes da indexacao
            //                var classe = "ConsoleApplication18.Evento._" + antesCapturarDocumento.PROJETO_MODELO.ID + "_" + projeto.ID + "_" + antesCapturarDocumento.PROJETO_MODELO.MODELO_ID;
            //                Type eventos = Type.GetType(classe);
            //                var instancia = (IEvento)Activator.CreateInstance(eventos);
            //                retorno = (RetornoEventoDTO)instancia.C001(null);

            //            }


            //            var ListaPalavras = ocr.executarOcrCoordenadas(arquivo);

            //            //  var sb = ocr.executarOcr(arquivo);
            //            var sb = castListaPalavras(ListaPalavras);

            //            var modeloAchado = CanhotosLeitura.verificaModelo(sb, projeto.ID);
            //            if (modeloAchado != null)
            //            {

            //                var projetoModelo = buscarProjetoModelo(projeto.ID, modeloAchado.ID);
            //                if (projetoModelo != null)
            //                {
            //                    if (projetoModelo.Padrao != null && projetoModelo.Padrao != "")
            //                    {
            //                        switch (projetoModelo.Padrao)
            //                        {
            //                            case "Cortes":
            //                                string python = @"C:\Python27\Python.exe";

            //                                string myPythonApp = ConfigurationManager.AppSettings["myPythonApp"];
            //                                // Create new process start info 
            //                                ProcessStartInfo myProcessStartInfo = new ProcessStartInfo(python);

            //                                // make sure we can read the output from stdout 
            //                                myProcessStartInfo.UseShellExecute = false;
            //                                myProcessStartInfo.RedirectStandardOutput = true;

            //                                // start python app with 3 arguments  
            //                                // 1st arguments is pointer to itself,  
            //                                // 2nd and 3rd are actual arguments we want to send 
            //                                myProcessStartInfo.Arguments = myPythonApp + " " + arquivo + " " + Guid.NewGuid().ToString();

            //                                Process myProcess = new Process();
            //                                // assign start information to the process 
            //                                myProcess.StartInfo = myProcessStartInfo;
            //                                myProcess.StartInfo.CreateNoWindow = true;
            //                                // start the process 
            //                                myProcess.Start();

            //                                // Read the standard output of the app we called.  
            //                                // in order to avoid deadlock we will read output first 
            //                                // and then wait for process terminate: 
            //                                StreamReader myStreamReader = myProcess.StandardOutput;
            //                                string myString = myStreamReader.ReadLine();

            //                                /*if you need to read multiple lines, you might use: 
            //                                    string myString = myStreamReader.ReadToEnd() */

            //                                // wait exit signal from the app we called and then close it. 
            //                                myProcess.WaitForExit();
            //                                myProcess.Close();

            //                                string[] cortes = myString.Split('$');

            //                                List<PosicaoLabel> arquivosIndices = new List<PosicaoLabel>();
            //                                List<String> arquivosIndicesCapturados = new List<string>();

            //                                foreach (var Corte in cortes)
            //                                {

            //                                    var posicoes = Corte.Split(' ');
            //                                    if (posicoes.Length > 2)
            //                                    {
            //                                        var coor = new Coordenadas()
            //                                        {
            //                                            X = Convert.ToInt16(posicoes[0]),
            //                                            Y = Convert.ToInt16(posicoes[1]),
            //                                            W = Convert.ToInt16(posicoes[2]),
            //                                            H = Convert.ToInt16(posicoes[3])

            //                                        };
            //                                        var bp = Bitmap.FromFile(arquivo);

            //                                        var arquivoCorte = CanhotosLeitura.CropImage(bp, coor.X, coor.Y, coor.W, coor.H);

            //                                        bp.Dispose();

            //                                        //arquivoCorte.Save(@"C:\Users\Administrador\Documents\Visual Studio 2015\Projects\ConsoleApplication18\ConsoleApplication18\bin\Debug\Crop\corte" + Guid.NewGuid() + ".png");
            //                                        MemoryStream ms = new MemoryStream();
            //                                        arquivoCorte.Save(ms, ImageFormat.Png);
            //                                        byte[] bmpBytes = ms.ToArray();
            //                                        PosicaoLabel pol = new PosicaoLabel();
            //                                        pol.Coordenadas = coor;
            //                                        pol.Label = ocr.executarOcr(bmpBytes).ToString();
            //                                        arquivosIndices.Add(pol);
            //                                        arquivoCorte.Dispose();
            //                                        ms.Close();

            //                                    }
            //                                }
            //                                var db = new Entidade();

            //                                Dictionary<string, string> indiceLidos = new Dictionary<string, string>();
            //                                List<Coordenadas> lista = new List<Coordenadas>();

            //                                var informacoesModelo = db.OrientacaoDocumento.Where(_s => _s.IdModelo == projetoModelo.MODELO_ID).ToList();/// ocr.executarOcrCoordenadas(caminhoModelo);
            //                                foreach (var item in informacoesModelo)
            //                                {


            //                                    if (item.Palavra == null)
            //                                    {
            //                                        continue;

            //                                    }
            //                                    else
            //                                    {
            //                                        foreach (var indicies in arquivosIndices)
            //                                        {


            //                                            var regex = ""; //"/(13)|(Operadora)|(-)|(Codigo)/g";
            //                                            int contador = 0;
            //                                            var palavras = item.Palavra.Split(' ');
            //                                            regex = "";
            //                                            for (int i = 0; i < palavras.Length; i++)
            //                                            {

            //                                                if (indicies.Label.Contains(palavras[i]))
            //                                                {


            //                                                    regex += "(" + palavras[i] + ")";
            //                                                    if (contador < palavras.Length && contador > 0)
            //                                                        regex += "|";
            //                                                    if (contador == 0 && palavras.Length > 1)
            //                                                        regex += "|";
            //                                                    contador++;
            //                                                }
            //                                            }
            //                                            if (contador == palavras.Length)
            //                                            {

            //                                                if (item.Evento.HasValue && item.Evento.Value)
            //                                                {

            //                                                    var DurtanteCapturarDocumento = new Entidade().Projeto_modelo_evento.Where(_s => _s.PROJETO_MODELO.ID == projetoModelo.ID && _s.Eventos.codigo == "C004").ToList();
            //                                                    /////evento antes da indexacao
            //                                                    if (DurtanteCapturarDocumento.Count > 0)////significa que tem evento
            //                                                    {

            //                                                        using (FileStream fs = File.Create(@"c:\logeventodurante.txt"))
            //                                                        {
            //                                                            Byte[] info = new UTF8Encoding(true).GetBytes(DurtanteCapturarDocumento.ToString());
            //                                                            // Add some information to the file.
            //                                                            fs.Write(info, 0, info.Length);
            //                                                        }
            //                                                        if (item.PARAMETROENTRADA != "")
            //                                                        {




            //                                                            Enum.TryParse(item.PARAMETROENTRADA, out EnumEntrada entrada);

            //                                                            var classe = "ConsoleApplication18.Evento._" + projetoModelo.ID + "_" + projetoModelo.PROJETO_ID + "_" + modeloAchado.ID;
            //                                                            Type eventos = Type.GetType(classe);
            //                                                            var instancia = (IEvento)Activator.CreateInstance(eventos);
            //                                                            switch (entrada)
            //                                                            {
            //                                                                case EnumEntrada.TEXTO:
            //                                                                    break;
            //                                                                case EnumEntrada.NUMERO:
            //                                                                    break;
            //                                                                case EnumEntrada.DATA:

            //                                                                    var retorno = (RetornoEventoDTO)instancia.C004(indicies, entrada);
            //                                                                    if (retorno != null && retorno.Retono != null)
            //                                                                    {
            //                                                                        if (retorno.RetornoTipo == EnumRetornoEvento.RETORNO_IMAGEM)
            //                                                                        {
            //                                                                            indiceLidos.Add(item.Palavra, indicies + "_parametro_" + ((string)retorno.Retono).ToString() + "_mensagem_" + retorno.MensagemRetorno);
            //                                                                        }
            //                                                                    }
            //                                                                    else
            //                                                                    {
            //                                                                        indiceLidos.Add(item.Palavra, indicies.Label);
            //                                                                    }
            //                                                                    break;
            //                                                                case EnumEntrada.IMAGEM:
            //                                                                    var bitmap = Bitmap.FromFile(arquivo);
            //                                                                    var dcrops = CanhotosLeitura.CropImage(bitmap, indicies.Coordenadas.X, indicies.Coordenadas.Y, indicies.Coordenadas.W, indicies.Coordenadas.H);
            //                                                                    retorno = (RetornoEventoDTO)instancia.C004((System.Drawing.Image)dcrops, EnumEntrada.IMAGEM);
            //                                                                    dcrops.Dispose();
            //                                                                    if (retorno != null)
            //                                                                    {
            //                                                                        if (retorno.RetornoTipo == EnumRetornoEvento.RETORNO_STRING)
            //                                                                        {
            //                                                                            indiceLidos.Add(item.Palavra, ((string)retorno.Retono).ToString());
            //                                                                        }
            //                                                                    }
            //                                                                    break;
            //                                                                default:
            //                                                                    break;
            //                                                            }



            //                                                        }
            //                                                    }
            //                                                }
            //                                                else if (item.Regex != null && item.Regex != "")
            //                                                {

            //                                                    var rgxarra = indicies.Label.Split(' ');
            //                                                    foreach (var sp in rgxarra)
            //                                                    {

            //                                                        Regex r = new Regex(item.Regex, RegexOptions.None);

            //                                                        // Match the regular expression pattern against a text string.
            //                                                        Match m = r.Match(sp.Trim());
            //                                                        int matchCount = 0;
            //                                                        if (m.Success)
            //                                                        {
            //                                                            if (indiceLidos.Where(_s => _s.Key == item.Palavra).Count() == 0)
            //                                                                indiceLidos.Add(item.Palavra, m.Value);
            //                                                            break;

            //                                                        }

            //                                                    }


            //                                                }
            //                                                else
            //                                                {
            //                                                    regex += "";
            //                                                    Regex rgx = new Regex(regex);
            //                                                    string replacement = "";
            //                                                    string result = rgx.Replace(indicies.Label, replacement);
            //                                                    string output = Regex.Replace(indicies.Label, regex, "");
            //                                                    var resultadoIndice = result.Replace("\n", String.Empty).Split(' ').Distinct();
            //                                                    string rtnIndc = "";
            //                                                    foreach (var itemidc in resultadoIndice)
            //                                                    {
            //                                                        rtnIndc += itemidc + " ";
            //                                                    }


            //                                                    indiceLidos.Add(item.Palavra, rtnIndc);
            //                                                    break;
            //                                                }

            //                                            }


            //                                        }

            //                                    }


            //                                }
            //                                var indices = indiceLidos;
            //                                string modeloMove = saidaProcessado + @"\" + projeto.ID + @"\" + usuarioAut.email + @"\" + Path.GetFileName(arquivo);
            //                                if (!File.Exists(modeloMove))
            //                                    File.Move(arquivo, modeloMove);


            //                                ////aqui eu faço a captura dos indices
            //                                string nomeGerado = Guid.NewGuid().ToString();



            //                                escreveLog(projeto.ID, sb, Path.GetFileName(arquivo), ocr.tipo(), modeloAchado.nome, modeloMove, nomeGerado, indices, usuarioAut);
            //                                bool warning = false;
            //                                foreach (var item in indices)
            //                                {
            //                                    if (item.Value.Contains("_parametro_"))
            //                                    {
            //                                        warning = true
            //                                            ;
            //                                        break;

            //                                    }
            //                                }
            //                                try
            //                                {
            //                                    if (warning)
            //                                    {
            //                                        // _hub.Invoke("Hello", new object[] { Path.GetFileName(arquivo), usuarioId, "WARNING", indices });
            //                                        sendMessage(new object[] { Path.GetFileName(arquivo), usuarioId, "WARNING", indices });
            //                                    }
            //                                    else
            //                                    {
            //                                        //   _hub.Invoke("Hello", new object[] { Path.GetFileName(arquivo), usuarioId, "OK", indices });
            //                                        sendMessage(new object[] { Path.GetFileName(arquivo), usuarioId, "OK", indices });
            //                                    }
            //                                }
            //                                catch (Exception ex)
            //                                {

            //                                }



            //                                break;
            //                            case "Regex":

            //                                string saidaPasta = itemCANHOTO + @"SAIDA\" + projeto.ID + @"\" + modeloAchado.nome;
            //                                if (!Directory.Exists(saidaPasta))
            //                                {
            //                                    Directory.CreateDirectory(saidaPasta);

            //                                }

            //                                modeloMove = saidaProcessado + @"\" + projeto.ID + @"\" + usuarioAut.email + @"\" + Path.GetFileName(arquivo);
            //                                if (!File.Exists(modeloMove))
            //                                    File.Move(arquivo, modeloMove);

            //                                var ssb = ocr.executarOcr(modeloMove);
            //                                ////aqui eu faço a captura dos indices
            //                                nomeGerado = Guid.NewGuid().ToString();
            //                                indices = CanhotosLeitura.indicesAnalise(modeloMove, modeloAchado, ssb.ToString());
            //                                //  CanhotosLeitura.criarDocumentoIndice(indices, itemCANHOTO + modeloAchado.nome + @"\", nomeGerado);
            //                                //  CanhotosLeitura.criarDocumentoIndice(indices, itemCANHOTO + modeloAchado.nome + @"\", nomeGerado);
            //                                escreveLog(projeto.ID, sb, Path.GetFileName(arquivo), ocr.tipo(), modeloAchado.nome, modeloMove, nomeGerado, indices, usuarioAut);
            //                                try
            //                                {
            //                                    //      _hub.Invoke("Hello", new object[] { Path.GetFileName(arquivo), usuarioId, "OK", indices });
            //                                    sendMessage(new object[] { Path.GetFileName(arquivo), usuarioId, "OK", indices });
            //                                }
            //                                catch (Exception ex)
            //                                {

            //                                }
            //                                break;
            //                            case "Layout":


            //                                using (FileStream fs = File.Create(@"c:\logPadrao.txt"))
            //                                {
            //                                    Byte[] info = new UTF8Encoding(true).GetBytes("padrao");
            //                                    // Add some information to the file.
            //                                    fs.Write(info, 0, info.Length);
            //                                }

            //                                modeloMove = saidaProcessado + @"\" + projeto.ID + @"\" + usuarioAut.email + @"\" + Path.GetFileName(arquivo);

            //                                if (!File.Exists(modeloMove))
            //                                    File.Move(arquivo, modeloMove);


            //                                ////aqui eu faço a captura dos indices
            //                                nomeGerado = Guid.NewGuid().ToString();
            //                                indices = CanhotosLeitura.indices(modeloMove, modeloAchado, projetoModelo.PROJETO_ID, ListaPalavras);
            //                                //  CanhotosLeitura.criarDocumentoIndice(indices, itemCANHOTO + modeloAchado.nome + @"\", nomeGerado);
            //                                //   CanhotosLeitura.criarDocumentoIndice(indices, itemCANHOTO + modeloAchado.nome + @"\", nomeGerado);
            //                                escreveLog(projeto.ID, sb, Path.GetFileName(arquivo), ocr.tipo(), modeloAchado.nome, modeloMove, nomeGerado, indices, usuarioAut);
            //                                warning = false;
            //                                foreach (var item in indices)
            //                                {
            //                                    if (item.Value.Contains("_parametro_"))
            //                                    {
            //                                        warning = true
            //                                            ;
            //                                        break;

            //                                    }
            //                                }
            //                                try
            //                                {
            //                                    if (warning)
            //                                    {
            //                                        //  _hub.Invoke("Hello", new object[] { Path.GetFileName(arquivo), usuarioId, "WARNING", indices });
            //                                        sendMessage(new object[] { Path.GetFileName(arquivo), usuarioId, "WARNING", indices });
            //                                    }
            //                                    else
            //                                    {
            //                                        //   _hub.Invoke("Hello", new object[] { Path.GetFileName(arquivo), usuarioId, "OK", indices });
            //                                        sendMessage(new object[] { Path.GetFileName(arquivo), usuarioId, "OK", indices });
            //                                    }
            //                                }
            //                                catch (Exception ex)
            //                                {

            //                                }


            //                                break;

            //                        }
            //                    }


            //                }
            //                ////erro 
            //                else
            //                {
            //                    try
            //                    {
            //                        /// _hub.Invoke("Hello", new String[] { "erro", usuarioId, "ERRO" });
            //                        sendMessage(new object[] { "erro", usuarioId, "ERRO" });
            //                    }
            //                    catch (Exception ex)
            //                    {

            //                    }
            //                }

            //            }
            //            else
            //            {
            //                try
            //                {
            //                    //      _hub.Invoke("Hello", new String[] { "", usuarioId, "WARNING", "Não foi encontrado modelo ", null });
            //                    sendMessage(new object[] { "", usuarioId, "WARNING", "Não foi encontrado modelo ", null });
            //                }
            //                catch (Exception ex)
            //                {

            //                }
            //                //var lis = ProcessImage(arquivo, projeto.ID);
            //                //if (lis != null && lis.score > Convert.ToInt32(ScoreApp))
            //                //{

            //                //    string modeloMove = saidaProcessado + @"\" + projeto.ID + @"\" + usuarioAut.email + @"\" + Path.GetFileName(arquivo);
            //                //    if (!File.Exists(modeloMove))
            //                //        File.Move(arquivo, modeloMove);


            //                //    ////aqui eu faço a captura dos indices
            //                //    string nm = Guid.NewGuid().ToString();
            //                //    //  var indices = CanhotosLeitura.indices(modeloMove, modeloAchado);
            //                //    //   CanhotosLeitura.criarDocumentoIndice(indices, itemCANHOTO + modeloAchado.nome + @"\", nomeGerado);
            //                //    escreveLog(projeto.ID, sb, Path.GetFileName(arquivo), ocr.tipo(), lis.modelo.nome, modeloMove, nm, null, usuarioAut);


            //                //}


            //                if (!Directory.Exists(saidaProcessado + " Arquivo_NAO_Classificado"))
            //                {
            //                    Directory.CreateDirectory(saidaProcessado + "Arquivo_NAO_Classificado" + @"\");
            //                }
            //                string nomeGerado = Guid.NewGuid().ToString();
            //                File.Move(arquivo, saidaProcessado + "Arquivo_NAO_Classificado" + @"\" + Path.GetFileName(arquivo));
            //                escreveLog(projeto.ID, sb, Path.GetFileName(arquivo), ocr.tipo(), "Não ACHOU", saidaProcessado + "Arquivo_NAO_Classificado" + @"\" + Path.GetFileName(arquivo), nomeGerado, null, usuarioAut);
            //            }
            //        }

            //        //pastas.@lock = false;
            //        //context.documentocaptura.Remove(pastas);
            //        //context.SaveChanges();



            //    }
            //    catch (Exception ex)
            //    {

            //    }
            //}
        }
        //private static void OnChanged(object source, FileSystemEventArgs e)
        //{
        //    string itemCANHOTO = ConfigurationManager.AppSettings["caminhoTiff"];
        //    string str = ConfigurationManager.AppSettings["saidaPdf"];
        //    string ScoreApp = ConfigurationManager.AppSettings["Score"];
        //    string item1 = ConfigurationManager.AppSettings["saidaErro"];
        //    string str1 = ConfigurationManager.AppSettings["saidaBack"];
        //    string item2 = ConfigurationManager.AppSettings["ftpUrl"];
        //    string str2 = ConfigurationManager.AppSettings["usuarioFtp"];
        //    string saidaProcessado = ConfigurationManager.AppSettings["saidaProcessado"];
        //    string item3 = ConfigurationManager.AppSettings["senhaFtp"];
        //    string caminho = ConfigurationManager.AppSettings["caminhoTiff"];

        //    criarPastasSaidaProjetos(saidaProcessado);
        //    string ModelosClassificador = ConfigurationManager.AppSettings["ModelosClassificador"];
        //    criarPastasModelo(ModelosClassificador);
        //    criarPastasSaida(ModelosClassificador);


        //    criarPastasProjetos(itemCANHOTO);

        //    criarPastasProjetosEntradaRep(itemCANHOTO);

        //    deskewImage(e.FullPath);




        //    PROJETO projeto = null;
        //    var context = new Entidade();
        //    string[] d = e.FullPath.Split('\\');
        //    if (d.Count() >= 7)
        //    {
        //        int idProjeto = 0;
        //        int.TryParse(d[4], out idProjeto);

        //        projeto = getprojeto(idProjeto);
        //        if (d[3].ToUpper().Trim() == "SAIDA")
        //        {
        //            return;
        //        }
        //        var _hub = getInstance();
        //        try
        //        {
        //            var caminhoArquivo = e.FullPath;
        //            IOcr ocr = Factory.FactoryOcr.FactoryOcrInstancia(idProjeto);

        //            string usuarioEmail = d[5];
        //            var usuarioAut = context.Usuario.Where(_usu => _usu.email == usuarioEmail).FirstOrDefault();
        //            var usuarioId = usuarioAut.Id.ToString();
        //            string arquivo = caminhoArquivo;// pastas.caminhoArquivo;

        //            if (!File.Exists(arquivo))
        //            {
        //                return;
        //            }

        //            if (arquivo.Contains("ANDROID"))
        //            {
        //                ///post.modelo + "$" + post.nota + "$" + post.serial + "$" + Path.GetFileName(file.FileName);
        //                //ws.GravarFolderCloudDocs(Convert.ToDecimal(protocolo), 327, 2583, 3271, 2583, doc.Nota, doc.Serie, modelo.nome, "", "", "Canhoto Digitalizado", "", doc.binario, doc.extensao, 16349, "", 0);
        //                var prop = Path.GetFileName(arquivo).Split('$');
        //                CanhotosLeitura.Documento doc = new CanhotosLeitura.Documento();
        //                doc.Nota = prop[1];
        //                doc.Serie = prop[2];
        //                doc.binario = File.ReadAllBytes(arquivo);
        //                doc.extensao = Path.GetExtension(arquivo).Replace(".", "");
        //                doc.localizacao = prop[3];
        //                Modelo md = new Modelo();
        //                md.nome = prop[0];

        //                CanhotosLeitura.UploadDocCloudFile(doc, md);
        //                File.Delete(arquivo);
        //            }
        //            else
        //            {

        //                if (arquivo.Contains(".pdf"))
        //                {
        //                    arquivo = cast(arquivo, Path.GetDirectoryName(arquivo));
        //                }
        //                if (arquivo.Contains(".tif") || arquivo.Contains(".tiff"))
        //                {

        //                    arquivo = cast(arquivo, Path.GetDirectoryName(arquivo));
        //                    //  pastas.caminhoArquivo = arquivo;
        //                }

        //                var antesCapturarDocumento = new Entidade().Projeto_modelo_evento.Where(_s => _s.PROJETO_MODELO.PROJETO_ID == projeto.ID && _s.Eventos.codigo == "C001").FirstOrDefault();
        //                /////evento antes da indexacao
        //                if (antesCapturarDocumento != null)
        //                {
        //                    RetornoEventoDTO retorno = null;

        //                    /////evento antes da indexacao
        //                    var classe = "ConsoleApplication18.Evento._" + antesCapturarDocumento.PROJETO_MODELO.ID + "_" + projeto.ID + "_" + antesCapturarDocumento.PROJETO_MODELO.MODELO_ID;
        //                    Type eventos = Type.GetType(classe);
        //                    var instancia = (IEvento)Activator.CreateInstance(eventos);
        //                    retorno = (RetornoEventoDTO)instancia.C001(null);

        //                }


        //                var ListaPalavras = ocr.executarOcrCoordenadas(arquivo);

        //                //  var sb = ocr.executarOcr(arquivo);
        //                var sb = castListaPalavras(ListaPalavras);

        //                var modeloAchado = CanhotosLeitura.verificaModelo(sb, projeto.ID);
        //                if (modeloAchado != null)
        //                {

        //                    var projetoModelo = buscarProjetoModelo(projeto.ID, modeloAchado.ID);



        //                    if (projetoModelo != null)
        //                    {
        //                        if (projetoModelo.Padrao == null || projetoModelo.Padrao.Value == true)
        //                        {


        //                            string modeloMove = saidaProcessado + @"\" + projeto.ID + @"\" + usuarioAut.email + @"\" + Path.GetFileName(arquivo);
        //                            if (!File.Exists(modeloMove))
        //                                File.Move(arquivo, modeloMove);


        //                            ////aqui eu faço a captura dos indices
        //                            string nomeGerado = Guid.NewGuid().ToString();
        //                            var indices = CanhotosLeitura.indices(modeloMove, modeloAchado, projetoModelo.PROJETO_ID, ListaPalavras);
        //                            //  CanhotosLeitura.criarDocumentoIndice(indices, itemCANHOTO + modeloAchado.nome + @"\", nomeGerado);
        //                            //   CanhotosLeitura.criarDocumentoIndice(indices, itemCANHOTO + modeloAchado.nome + @"\", nomeGerado);
        //                            escreveLog(projeto.ID, sb, Path.GetFileName(arquivo), ocr.tipo(), modeloAchado.nome, modeloMove, nomeGerado, indices, usuarioAut);
        //                            bool warning = false;
        //                            foreach (var item in indices)
        //                            {
        //                                if (item.Value.Contains("_parametro_"))
        //                                {
        //                                    warning = true
        //                                        ;
        //                                    break;

        //                                }
        //                            }
        //                            try
        //                            {
        //                                if (warning)
        //                                {
        //                                    _hub.Invoke("Hello", new object[] { Path.GetFileName(arquivo), usuarioId, "WARNING", indices });
        //                                }
        //                                else
        //                                {
        //                                    _hub.Invoke("Hello", new object[] { Path.GetFileName(arquivo), usuarioId, "OK", indices });
        //                                }
        //                            }
        //                            catch (Exception ex)
        //                            {

        //                            }

        //                        }
        //                        else if (!projetoModelo.Padrao.Value)//////capturar somente por regex 
        //                        {

        //                            string saidaPasta = itemCANHOTO + @"SAIDA\" + projeto.ID + @"\" + modeloAchado.nome;
        //                            if (!Directory.Exists(saidaPasta))
        //                            {
        //                                Directory.CreateDirectory(saidaPasta);

        //                            }
        //                            string modeloMove = saidaPasta + @"\" + Path.GetFileName(arquivo);
        //                            if (!File.Exists(modeloMove))
        //                                File.Move(arquivo, modeloMove);


        //                            ////aqui eu faço a captura dos indices
        //                            string nomeGerado = Guid.NewGuid().ToString();
        //                            var indices = CanhotosLeitura.indicesAnalise(modeloMove, modeloAchado, sb.ToString());
        //                            //  CanhotosLeitura.criarDocumentoIndice(indices, itemCANHOTO + modeloAchado.nome + @"\", nomeGerado);
        //                            //  CanhotosLeitura.criarDocumentoIndice(indices, itemCANHOTO + modeloAchado.nome + @"\", nomeGerado);
        //                            escreveLog(projeto.ID, sb, Path.GetFileName(arquivo), ocr.tipo(), modeloAchado.nome, modeloMove, nomeGerado, indices, usuarioAut);
        //                            try
        //                            {
        //                                _hub.Invoke("Hello", new object[] { Path.GetFileName(arquivo), usuarioId, "OK", indices });
        //                            }
        //                            catch (Exception ex)
        //                            {

        //                            }
        //                        }
        //                    }
        //                    ////erro 
        //                    else
        //                    {
        //                        try
        //                        {
        //                            _hub.Invoke("Hello", new String[] { "erro", usuarioId, "ERRO" });
        //                        }
        //                        catch (Exception ex)
        //                        {

        //                        }
        //                    }

        //                }
        //                else
        //                {
        //                    _hub.Invoke("Hello", new String[] { "", usuarioId, "WARNING", "Não foi encontrado modelo p", null });
        //                    var lis = ProcessImage(arquivo, projeto.ID);
        //                    if (lis != null && lis.score > Convert.ToInt32(ScoreApp))
        //                    {

        //                        string modeloMove = saidaProcessado + @"\" + projeto.ID + @"\" + usuarioAut.email + @"\" + Path.GetFileName(arquivo);
        //                        if (!File.Exists(modeloMove))
        //                            File.Move(arquivo, modeloMove);


        //                        ////aqui eu faço a captura dos indices
        //                        string nm = Guid.NewGuid().ToString();
        //                        //  var indices = CanhotosLeitura.indices(modeloMove, modeloAchado);
        //                        //   CanhotosLeitura.criarDocumentoIndice(indices, itemCANHOTO + modeloAchado.nome + @"\", nomeGerado);
        //                        escreveLog(projeto.ID, sb, Path.GetFileName(arquivo), ocr.tipo(), lis.modelo.nome, modeloMove, nm, null, usuarioAut);


        //                    }


        //                    if (!Directory.Exists(saidaProcessado + " Arquivo_NAO_Classificado"))
        //                    {
        //                        Directory.CreateDirectory(saidaProcessado + "Arquivo_NAO_Classificado" + @"\");
        //                    }
        //                    string nomeGerado = Guid.NewGuid().ToString();
        //                    File.Move(arquivo, saidaProcessado + "Arquivo_NAO_Classificado" + @"\" + Path.GetFileName(arquivo));
        //                    escreveLog(projeto.ID, sb, Path.GetFileName(arquivo), ocr.tipo(), "Não ACHOU", itemCANHOTO + "Arquivo_NAO_Classificado" + @"\" + Path.GetFileName(arquivo), nomeGerado, null, usuarioAut);
        //                }
        //            }

        //            //pastas.@lock = false;
        //            //context.documentocaptura.Remove(pastas);
        //            //context.SaveChanges();



        //        }
        //        catch (Exception ex)
        //        {
        //            using (FileStream fs = File.Create(@"c:\logerroArquivo.txt"))
        //            {
        //                Byte[] info = new UTF8Encoding(true).GetBytes(ex.Message);
        //                // Add some information to the file.
        //                fs.Write(info, 0, info.Length);
        //            }
        //        }
        //    }
        //}


        public object Google { get; private set; }
        private static IOcr ocr { get; set; }

        public static void criarPastasModelo(String caminho)
        {
            string agrupamento = ConfigurationManager.AppSettings["agrupamento"];
            if (!Directory.Exists(agrupamento))
            {

                Directory.CreateDirectory(agrupamento);
            }
            var context = new Entidade();
            var models = context.Modelo.ToList();

            foreach (var item in models)
            {
                if (!Directory.Exists(caminho + @"\" + item.ID))
                {

                    Directory.CreateDirectory(caminho + @"\" + item.ID);
                }
            }

        }
        public static void criarPastasSaida(String caminho)
        {
            var context = new Entidade();
            var models = context.Modelo.ToList();

            foreach (var item in models)
            {
                if (!Directory.Exists(caminho + @"\" + item.ID))
                {

                    Directory.CreateDirectory(caminho + @"\" + item.ID);
                }
            }

        }

        public static List<PastasProjetoMonitoramento> criarPastasSaidaProjetos(String caminho)
        {




            List<PastasProjetoMonitoramento> caminhosLidos = new List<PastasProjetoMonitoramento>();
            var context = new Entidade();
            var projetos = context.PROJETO.ToList();
            var usuarios = context.Usuario.ToList();
            foreach (var item in projetos)
            {

                if (!Directory.Exists(caminho + @"\" + item.ID))
                {
                    ////cria pasta de entrada do projeto
                    Directory.CreateDirectory(caminho + @"\" + item.ID);


                    foreach (var usuario in usuarios)
                    {
                        Directory.CreateDirectory(caminho + @"\" + item.ID + @"\" + usuario.email.Trim());
                    }

                }
                ////diretorio existe 
                else
                {

                    foreach (var usuario in usuarios)
                    {
                        if (!Directory.Exists(caminho + @"\" + item.ID + @"\" + usuario.email.Trim()))
                            Directory.CreateDirectory(caminho + @"\" + item.ID + @"\" + usuario.email.Trim());
                    }
                }



            }



            return caminhosLidos;
        }

        public static List<PastasProjetoMonitoramento> criarPastasProjetos(String caminho)
        {
            List<PastasProjetoMonitoramento> caminhosLidos = new List<PastasProjetoMonitoramento>();
            var context = new Entidade();
            var projetos = context.PROJETO.ToList();
            foreach (var item in projetos)
            {
                PastasProjetoMonitoramento prj = new PastasProjetoMonitoramento();
                if (!Directory.Exists(caminho + @"ENTRADA\" + item.ID))
                {
                    ////cria pasta de entrada do projeto
                    Directory.CreateDirectory(caminho + @"ENTRADA\" + item.ID);

                    var usuarios = context.Usuario.ToList();
                    foreach (var usuario in usuarios)
                    {
                        if (!Directory.Exists(caminho + @"ENTRADA\" + item.ID + @"\" + usuario.email))
                        {
                            Directory.CreateDirectory(caminho + @"ENTRADA\" + item.ID + @"\" + usuario.email);
                        }
                    }

                }
                else
                {
                    var usuarios = context.Usuario.ToList();
                    foreach (var usuario in usuarios)
                    {
                        if (!Directory.Exists(caminho + @"ENTRADA\" + item.ID + @"\" + usuario.email))
                        {
                            Directory.CreateDirectory(caminho + @"ENTRADA\" + item.ID + @"\" + usuario.email);
                        }
                    }
                }
                if (!Directory.Exists(caminho + @"SAIDA\" + item.ID))
                {
                    ////cria pasta de entrada do projeto
                    Directory.CreateDirectory(caminho + @"SAIDA\" + item.ID);
                }
                prj.caminhoRaiz = caminho;
                prj.projeto = item;
                prj.caminhoEntrada = caminho + @"ENTRADA\" + item.ID;
                prj.caminhoSaida = caminho + @"SAIDA\" + item.ID;
                caminhosLidos.Add(prj);


            }
            return caminhosLidos;
        }
        public static List<PastasProjetoMonitoramento> criarPastasProjetosEntradaRep(String caminho)
        {
            List<PastasProjetoMonitoramento> caminhosLidos = new List<PastasProjetoMonitoramento>();
            var context = new Entidade();
            var projetos = context.PROJETO.ToList();
            foreach (var item in projetos)
            {
                PastasProjetoMonitoramento prj = new PastasProjetoMonitoramento();
                if (!Directory.Exists(caminho + @"ENTRADAREP\" + item.ID))
                {
                    ////cria pasta de entrada do projeto
                    Directory.CreateDirectory(caminho + @"ENTRADAREP\" + item.ID);
                }
                if (!Directory.Exists(caminho + @"SAIDAREP\" + item.ID))
                {
                    ////cria pasta de entrada do projeto
                    Directory.CreateDirectory(caminho + @"SAIDAREP\" + item.ID);
                }
                prj.caminhoRaiz = caminho;
                prj.projeto = item;
                prj.caminhoEntrada = caminho + @"ENTRADAREP\" + item.ID;
                prj.caminhoSaida = caminho + @"SAIDAREP\" + item.ID;
                caminhosLidos.Add(prj);


            }
            return caminhosLidos;
        }
        public class ModeloScore
        {
            public long score;
            public Modelo modelo;

        }
        public class PastasProjetoMonitoramento
        {
            public String caminhoEntrada;
            public String caminhoSaida;
            public PROJETO projeto;
            public string caminhoRaiz;
        }

        public static PROJETO_MODELO buscarProjetoModelo(int idProjeto, int IdModelo)
        {
            var context = new Entidade();

            return context.PROJETO_MODELO.Where(_s => _s.MODELO_ID == IdModelo && _s.PROJETO_ID == idProjeto).FirstOrDefault();
        }



        public static string capturaPrimeiraPaginaPdf(string entrada, String extensaoDestino)
        {
            string retorno = "";
            GdPicturePDF oGdPicturePDF = new GdPicturePDF();
            GdPictureImaging oGdPictureImaging = new GdPictureImaging();

            oGdPicturePDF.SetLicenseNumber("4118106456693265856441854");
            oGdPictureImaging.SetLicenseNumber("4118106456693265856441854");
            if (entrada.Equals(".pdf"))
            {



                if (oGdPicturePDF.LoadFromFile(entrada, false) == GdPictureStatus.OK)
                {

                    for (int i = 0; i < oGdPicturePDF.GetPageCount(); i++)
                    {
                        //captura a primeira pagina
                        oGdPicturePDF.SelectPage(1);
                        //rasterize page or extract page bitmap.
                        int rasterPage = oGdPicturePDF.RenderPageToGdPictureImageEx(300, true);
                        string nome = entrada.Replace(Path.GetExtension(entrada), extensaoDestino);

                        oGdPictureImaging.SaveAsPNG(rasterPage, nome);
                        oGdPictureImaging.ReleaseGdPictureImage(rasterPage);
                        retorno = nome;
                        break;
                    }
                    oGdPicturePDF.CloseDocument();
                }





            }
            return retorno;
        }
        public static List<string> cast(string entrada, string saida)
        {
            string INPUT_DOCUMENT = entrada;
            //   string OUTPUT_DOCUMENT = "output.tif";
            List<string> retorno = new List<string>();
            const float DPI = 200F;
            GdPicturePDF oGdPicturePDF = new GdPicturePDF();
            GdPictureImaging oGdPictureImaging = new GdPictureImaging();
            string saidaAgrupamento = ConfigurationManager.AppSettings["saidaProcessado"];
            saidaAgrupamento += @"saidaAgrupamento";
            if (!Directory.Exists(saidaAgrupamento))
            {
                Directory.CreateDirectory(saidaAgrupamento);
            }
            oGdPicturePDF.SetLicenseNumber("4118106456693265856441854");
            oGdPictureImaging.SetLicenseNumber("4118106456693265856441854");
            if (Path.GetExtension(entrada).Equals(".pdf"))
            {



                if (oGdPicturePDF.LoadFromFile(INPUT_DOCUMENT, false) == GdPictureStatus.OK)
                {

                    for (int i = 1; i < oGdPicturePDF.GetPageCount(); i++)
                    {

                        oGdPicturePDF.SelectPage(i);
                        //rasterize page or extract page bitmap.
                        int rasterPage = oGdPicturePDF.RenderPageToGdPictureImageEx(300, true);

                        string nomegerado = (i).ToString();
                        oGdPictureImaging.SaveAsPNG(rasterPage, saidaAgrupamento + @"\" + nomegerado + ".png");
                        oGdPictureImaging.ReleaseGdPictureImage(rasterPage);

                        retorno.Add(saidaAgrupamento + @"\" + nomegerado + ".png");

                    }
                    oGdPicturePDF.CloseDocument();
                }
            }
            else
            {

                int imageId = oGdPictureImaging.CreateGdPictureImageFromFile(INPUT_DOCUMENT);
                int NumberOfPages = oGdPictureImaging.TiffGetPageCount(imageId);
                bool savePDF = true;
                //loop through pages
                for (int i = 1; i <= NumberOfPages; i++)
                {
                    string nomegerado = (i).ToString();
                    //select each page in TIFF file
                    oGdPictureImaging.TiffSelectPage(imageId, i);
                    //add selected TIFF page as a resource to pdf file and draw it on a new page
                    var d = saidaAgrupamento + @"\" + nomegerado + ".png";
                    oGdPictureImaging.SaveAsPNG(imageId, d);
                    //   oGdPictureImaging.ReleaseGdPictureImage(imageId);

                    retorno.Add(saidaAgrupamento + @"\" + nomegerado + ".png");
                }



                oGdPicturePDF.CloseDocument();
                oGdPictureImaging.ReleaseGdPictureImage(imageId);

            }
            return retorno;
        }
        public void inserirLogCaptura(documentocaptura doc)
        {
            var context = new Entidade();
            var quantidadeLock = context.documentocaptura.Where(_s => _s.caminhoArquivo == doc.caminhoArquivo && _s.@lock == true).Count();
            if (quantidadeLock == 0)
            {
                context.documentocaptura.Add(doc);
                context.SaveChanges();
            }
        }
        public static StringBuilder castListaPalavras(List<PalavrasCoordenadasDocumento> palavras)
        {
            StringBuilder retorno = new StringBuilder();
            foreach (var item in palavras)
            {
                retorno.Append(item.texto + " ");
            }
            return retorno;

        }













        public void escreverlog(string log, string passagem)
        {
            using (FileStream fs = File.Create(@"c:\" + passagem + ".txt"))
            {
                Byte[] info = new UTF8Encoding(true).GetBytes(log);
                // Add some information to the file.
                fs.Write(info, 0, info.Length);
            }
        }



        private static IHubProxy _hubInstance;

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static IHubProxy getInstance()
        {
            try
            {
                if (_hubInstance == null)
                {

                    string url = ConfigurationManager.AppSettings["url"];

                    var connection = new HubConnection(url);
                    _hubInstance = connection.CreateHubProxy("TestHub");
                    connection.Start();

                }
            }
            catch (Exception ex)
            {
                _hubInstance = null;

            }

            return _hubInstance;
        }
        public static void timer_Elapsed_machine(object sender, System.Timers.ElapsedEventArgs e)
        {
            Thread th = new Thread(ProcessarTreinamento);
            th.Start();
        }
        /// <summary>
        /// evento que verifica a leitura.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {





            string usuarioId = "";
            try
            {
                string saidaProcessado = ConfigurationManager.AppSettings["saidaProcessado"];
                criarPastasSaidaProjetos(saidaProcessado);
                string ModelosClassificador = ConfigurationManager.AppSettings["ModelosClassificador"];
                criarPastasModelo(ModelosClassificador);
                criarPastasSaida(ModelosClassificador);
                string itemCANHOTO = ConfigurationManager.AppSettings["caminhoTiff"];
                criarPastasProjetos(itemCANHOTO);

                criarPastasProjetosEntradaRep(itemCANHOTO);



                captura(itemCANHOTO);

            }
            catch (Exception ex)
            {
                using (FileStream fs = File.Create(@"c:\log.txt"))
                {
                    Byte[] info = new UTF8Encoding(true).GetBytes(ex.Message);
                    // Add some information to the file.
                    fs.Write(info, 0, info.Length);
                }
            }

        }

        /// <summary>
        /// realoiza o processamento
        /// </summary>
        /// <param name="completeImage"></param>
        /// <param name="detailImage"></param>
        /// <returns></returns>
        //private static ModeloScore ProcessImage(string completeImage, int projetoId)
        //{
        //    List<ModeloScore> lis = new List<ModeloScore>();
        //    string ModelosClassificador = ConfigurationManager.AppSettings["ModelosClassificador"];

        //    var context = new Entidade();

        //    var modelosPermitidos = context.PROJETO_MODELO.Where(_s => _s.PROJETO_ID == projetoId).Select(_s => _s.MODELO_ID).ToArray();


        //    var mod = context.Modelo.Where(_modelo => modelosPermitidos.Contains(_modelo.ID)).ToList();


        //    string ScoreApp = ConfigurationManager.AppSettings["Score"];

        //    foreach (var itempasta in mod)
        //    {

        //        foreach (var item in Directory.GetFiles(ModelosClassificador + @"\" + itempasta.ID))
        //        {


        //            try
        //            {
        //                long score;
        //                long matchTime;

        //                using (Mat modelImage = CvInvoke.Imread(item, ImreadModes.Color))
        //                using (Mat observedImage = CvInvoke.Imread(completeImage, ImreadModes.Color))
        //                {
        //                    Mat homography;
        //                    VectorOfKeyPoint modelKeyPoints;
        //                    VectorOfKeyPoint observedKeyPoints;

        //                    using (var matches = new VectorOfVectorOfDMatch())
        //                    {
        //                        Mat mask;
        //                        DrawMatches.FindMatch(modelImage, observedImage, out matchTime, out modelKeyPoints, out observedKeyPoints, matches,
        //                           out mask, out homography, out score);
        //                    }


        //                }
        //                ModeloScore ms = new ModeloScore();
        //                ms.modelo = itempasta;
        //                ms.score = score;
        //                lis.Add(ms);
        //                if (score > Convert.ToInt32(ScoreApp))
        //                {
        //                    return lis.OrderByDescending(_s => _s.score).FirstOrDefault();
        //                }
        //            }
        //            catch (Exception ex)
        //            { return null; }

        //        }

        //    }

        //    return lis.OrderByDescending(_s => _s.score).FirstOrDefault();

        //}
        public static void escreveLog(int modeloId, int projetoId, StringBuilder log, string nome, string motor, string modelo, string caminhoArquivo, string Id, Dictionary<string, string> indices, Usuario usuario)
        {

            //GdPictureImaging oGdPictureImaging = new GdPictureImaging();
            //GdPicturePDF oGdPicturePDF = new GdPicturePDF();
            //oGdPicturePDF.SetLicenseNumber("4118106456693265856441854");
            //oGdPictureImaging.SetLicenseNumber("4118106456693265856441854");

            var context = new Entidade();
            LogOcr logocr = new LogOcr();
            logocr.Data = DateTime.Now;
            logocr.nome = nome;
            logocr.Modelo = modelo;
            logocr.IdDocumento = Id;
            logocr.UsuarioId = usuario.Id;
            logocr.usuario = usuario.email;
            logocr.ProjetoId = projetoId;
            logocr.Motor = motor;
            logocr.Resultado = log.ToString();
            string indiceArquivo = "";
            string indiceArquivoNome = "";
            if (indices != null)
            {
                foreach (var item in indices)
                {
                    indiceArquivo += item.Key + " " + item.Value + "#";
                    indiceArquivoNome += item.Value + "_";
                }
            }

            logocr.Indice = indiceArquivo;

            context.LogOcr.Add(logocr);


            if (indices != null)
            {
                foreach (var item in indices)
                {
                    Indice inx = new Indice();
                    inx.Indice1 = item.Key;
                    inx.Valor = item.Value;
                    inx.IdLog = logocr.Id;
                    context.Indice.Add(inx);
                }
            }


            Mensagem m = new Mensagem();
            m.Lido = false;
            m.Mensagem1 = "Novo documento Processado : " + nome;
            m.UsuarioId = usuario.Id;
            m.Data = DateTime.Now;
            context.Mensagem.Add(m);
            context.SaveChanges();

            string DocumentoView = ConfigurationManager.AppSettings["DocumentoView"];
            String pasta = ConfigurationManager.AppSettings["Documento"] + logocr.IdDocumento + ".png";
            String pastaView = DocumentoView + logocr.IdDocumento + ".png";
            caminhoArquivo = caminhoArquivo.Replace(@"\\\\", @"\\");
            File.Copy(caminhoArquivo, pasta);
            //File.Move(DocumentoView + Path.GetFileName(caminhoArquivo), pastaView);
            File.Copy(pasta, pastaView);

            var prj = context.PROJETO.Where(_prj => _prj.ID == projetoId).FirstOrDefault();

            if (prj.exportId != null)
            {

                var projetoModelo = context.PROJETO_MODELO.Where(_s => _s.MODELO_ID == modeloId && _s.PROJETO_ID == projetoId).FirstOrDefault();

                if (projetoModelo != null)
                {
                    if (projetoModelo.ControleQualidade != null && projetoModelo.ControleQualidade.Value)
                    {
                        ControleQualidadeFile(logocr.Id, prj.exportId.Value, pasta, indiceArquivoNome, usuario.email.Trim(), projetoId);
                    }
                    else
                    {
                        UploadFtpFile(prj.exportId.Value, pastaView, indiceArquivoNome, usuario.email.Trim(), projetoId);
                    }
                }


            }
            File.Delete(caminhoArquivo);
            //  var caminho = GerarDocumentoPesquisavelPdf(pasta, oGdPictureImaging, oGdPicturePDF, caminhoArquivo, true, "por", "", "", "", "", "", 300);
            //
            //File.Copy(caminho, Directory.GetParent(caminhoArquivo) + @"/" + logocr.IdDocumento + ".pdf");
            //   File.Delete(caminhoArquivo);



        }

        public static void ControleQualidadeFile(int logId, int exp, string caminho, string nome, string usuario, int projetoId)
        {

            var context = new Entidade();
            ControleQualidadeLog log = new ControleQualidadeLog();
            log.caminho = caminho;
            log.data = DateTime.Now;
            log.exportId = exp;
            log.processado = true;
            log.liberado = false;
            log.usuario = usuario;
            log.indice = nome;
            log.LogOcrId = logId;
            log.projetoId = projetoId;
            context.ControleQualidadeLog.Add(log);
            context.SaveChanges();


        }
        public static void UploadFtpFile(int exp, string caminho, string nome, string usuario, int projetoId)
        {

            if (nome.Trim() == "")
            {
                nome = DateTime.Now.ToString("ddMMyyyyhhmmssffff");
            }
            nome = nome.Replace("\r\n", string.Empty);
            nome = nome.Replace("\n", string.Empty);
            var pattern = new Regex(@"[:!@#$%^&*()}{|\"":?><\[\]\\;'/.,~]");
            nome = pattern.Replace(nome, "-");

            string saidaProcessado = ConfigurationManager.AppSettings["saidaProcessado"];
            saidaProcessado += @"/" + projetoId + @"/" + usuario + @"/" + nome + Path.GetExtension(caminho);
            if (!File.Exists(saidaProcessado))
            {
                File.Copy(caminho, saidaProcessado);
            }


            var context = new Entidade();
            var expbase = context.Export.Where(_s => _s.Id == exp).FirstOrDefault();
            if (expbase != null)
            {
                string url = expbase.Url, fileName = caminho, usuarioFtp = expbase.Usuario, senhaFtp = expbase.Senha;
                using (System.Net.WebClient client = new System.Net.WebClient())
                {
                    client.Credentials = new System.Net.NetworkCredential(usuarioFtp, senhaFtp);
                    client.UploadFile(url + nome + Path.GetExtension(caminho), "STOR", fileName);
                }
            }
        }



        public static bool IsFileLocked(string filePath)
        {
            try
            {
                using (File.Open(filePath, FileMode.Open)) { }
            }
            catch (IOException e)
            {
                var errorCode = Marshal.GetHRForException(e) & ((1 << 16) - 1);

                return errorCode == 32 || errorCode == 33;
            }

            return false;
        }

        public static string castTopdf(string file, GdPictureImaging oGdPictureImaging, GdPicturePDF oGdPicturePDF, string nome)
        {
            string retorno = "";
            oGdPictureImaging.TiffOpenMultiPageForWrite(false);
            //Loading the Image from a file
            int imageId = oGdPictureImaging.CreateGdPictureImageFromFile(file);
            if (imageId != 0)
            {

                oGdPicturePDF.NewPDF();
                if (oGdPictureImaging.TiffIsMultiPage(imageId) == false) //One page TIFF image
                {
                    //adding image as a resource and drawing it in a new page
                    oGdPicturePDF.AddImageFromGdPictureImage(imageId, false, true);
                    if (oGdPicturePDF.GetStat() == GdPictureStatus.OK)
                    {
                        retorno = file.Replace(Path.GetExtension(file), ".pdf");
                        oGdPicturePDF.SaveToFile(file.Replace(Path.GetExtension(file), ".pdf"));
                        if (oGdPicturePDF.GetStat() != GdPictureStatus.OK)
                        {

                        }
                        ////Save the new PDF to file
                        //String ph = Directory.GetDirectoryRoot(file) +@"\" + nome;
                        //retorno = ph + ".pdf" ;
                        //if (oGdPicturePDF.SaveToFile(ph) != GdPictureStatus.OK)
                        //{

                        //}
                    }

                    //Clear resources
                    oGdPicturePDF.CloseDocument();
                    oGdPictureImaging.ReleaseGdPictureImage(imageId);
                }
                else //multi page pdf image
                {
                    int NumberOfPages = oGdPictureImaging.TiffGetPageCount(imageId);
                    bool savePDF = true;
                    //loop through pages
                    for (int i = 1; i <= NumberOfPages; i++)
                    {
                        //select each page in TIFF file
                        oGdPictureImaging.TiffSelectPage(imageId, i);
                        //add selected TIFF page as a resource to pdf file and draw it on a new page
                        oGdPicturePDF.AddImageFromGdPictureImage(imageId, false, true);
                        if (oGdPicturePDF.GetStat() != GdPictureStatus.OK)
                        {

                            savePDF = false;
                            break;
                        }
                    }

                    if (savePDF) //check whether error occured in adding any image to pdf
                    {
                        retorno = file.Replace(Path.GetExtension(file), ".pdf");
                        oGdPicturePDF.SaveToFile(file.Replace(Path.GetExtension(file), ".pdf"));
                        if (oGdPicturePDF.GetStat() != GdPictureStatus.OK)
                        {

                        }
                    }
                    //clearing resources
                    oGdPicturePDF.CloseDocument();
                    oGdPictureImaging.ReleaseGdPictureImage(imageId);
                }
            }
            else
            {

            }
            //File.Delete(file);
            return retorno;

        }

        public static string GerarDocumentoPesquisavelPdf(string arquivopasta, GdPictureImaging _gdPictureImaging, GdPicturePDF _gdPicturePDF, string documento, bool pdfa = true, string idioma = "por", string titulo = null, string autor = null, string assunto = null, string palavrasChaves = null, string criador = null, int dpi = 200)
        {
            string IdDocumento = Guid.NewGuid().ToString();
            if (Path.GetExtension(documento) != ".pdf")
            {
                documento = castTopdf(documento, _gdPictureImaging, _gdPicturePDF, IdDocumento);
            }
            var retorno = arquivopasta;

            int pdfOcrID = 0;

            GdPictureStatus status = _gdPicturePDF.LoadFromFile(documento, true);

            var _diretorioIdiomas = ConfigurationManager.AppSettings["idiomas"].ToString();


            pdfOcrID = _gdPictureImaging.PdfOCRStart(retorno, pdfa, titulo, autor, assunto, palavrasChaves, criador);






            int totalPaginas = _gdPicturePDF.GetPageCount();


            for (int i = 1; i <= totalPaginas; i++)
            {
                _gdPicturePDF.SelectPage(i);
                int rasterPageID = _gdPicturePDF.RenderPageToGdPictureImageEx(dpi, true);
                if (rasterPageID != 0)
                {

                    _gdPictureImaging.PdfAddGdPictureImageToPdfOCR(pdfOcrID, rasterPageID, idioma, _diretorioIdiomas, "");
                    var d = _gdPictureImaging.OCRTesseractGetCharCount();
                    _gdPictureImaging.ReleaseGdPictureImage(rasterPageID);
                }
            }

            _gdPictureImaging.PdfOCRStop(pdfOcrID);
            _gdPicturePDF.CloseDocument();

            return retorno;
        }

        protected override void OnStart(string[] args)
        {

            try
            {
                string caminho = ConfigurationManager.AppSettings["caminhoTiff"];
                string saidaProcessado = ConfigurationManager.AppSettings["saidaProcessado"];
                criarPastasSaidaProjetos(saidaProcessado);
                string ModelosClassificador = ConfigurationManager.AppSettings["ModelosClassificador"];
                criarPastasModelo(ModelosClassificador);
                criarPastasSaida(ModelosClassificador);

                string itemCANHOTO = ConfigurationManager.AppSettings["caminhoTiff"];

                criarPastasProjetos(itemCANHOTO);

                criarPastasProjetosEntradaRep(itemCANHOTO);
                using (FileStream fs = File.Create(@"c:\logssss.txt"))
                {
                    Byte[] info = new UTF8Encoding(true).GetBytes(itemCANHOTO);
                    // Add some information to the file.
                    fs.Write(info, 0, info.Length);
                }
                captura(itemCANHOTO);
            }
            catch (Exception ex)
            {
                using (FileStream fs = File.Create(@"c:\log.txt"))
                {
                    Byte[] info = new UTF8Encoding(true).GetBytes(ex.Message + " " + ex.Source + " " + ex.InnerException);
                    // Add some information to the file.
                    fs.Write(info, 0, info.Length);
                }
            }
        }


        protected override void OnStop()
        {
        }
    }
}
