using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net.Mail;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml.Serialization;

namespace Domain.Util
{
    public class Global
    {
        #region Singleton

        

        //Variaveis Privadas
        private static volatile Global i_instance = null;

        /// <summary>
        /// Singleton
        /// </summary>
        public static Global Instance
        {
            get
            {
                if (i_instance == null)
                {
                    lock (typeof(Global))
                    {
                        if (i_instance == null)
                        {
                            i_instance = new Global();
                        }
                    }
                }
                return i_instance;
            }
        }

        //Inicialização Privada
       
        private Global()
        {

        }

        #endregion

        #region Proriedades
        private readonly string PathLog = String.Format(@"{0}\\{1}\\", System.AppDomain.CurrentDomain.BaseDirectory, "Temp");

        public int IntervaloExecucao { get; set; }

        public string CpfConsultante { get; set; }

        public string CertificadoCliente { get; set; }

        public string CertificadoClienteSenha { get; set; }

        public string CertificadoService { get; set; }

        public string TipoPedido { get; set; }//tipoPedido = "1"; 1-consulta 2- Emissao, 3-consulta Emissao

        public string CodSistemaInfoconv { get; set; } //codSistemaInfoconv = "5";

        public string CodPerfilInfoconv { get; set; } //codPerfilInfoconv = "1";

        public string ChavePedido { get; set; } //chavePedido = "";

        public bool ServicoOn { get; set; } //chavePedido = "";

        public bool GravaFatura { get; set; } //chavePedido = "";

        public string SmtpclientHost { get; set; }

        public int SmtpPort { get; set; }

        public string[] DestinatariosEmail { get; set; }

        public string AssuntoEmail { get; set; }

        public string AutorEmail { get; set; }

        public DateTime LastEmailSended { get; set; }

        public int TempoDeEnvioDeEmail { get; set; }

        #endregion

        #region Log

        /// <summary>
        /// Registra Mensagem TXT caso nao consiga registrar no banco
        /// </summary>
        /// <param name="a_Metodo"></param>
        /// <param name="a_Log"></param>
        public void RegistraMensagem(String a_Metodo, String a_Log)
        {
            try
            {
                if (!Directory.Exists(PathLog))
                {
                    Directory.CreateDirectory(PathLog);
                }
                StreamWriter l_SrLog = new StreamWriter(PathLog + "Log_" + DateTime.Now.ToString("ddMMyyyy") + ".txt", true, Encoding.UTF8);
                l_SrLog.WriteLine(DateTime.Now.ToString("dd/MM/yy HH:mm") + " - " + a_Metodo + " " + a_Log + "\n");
                l_SrLog.Flush();
                l_SrLog.Close();
            }
            catch (Exception e)
            {
            }
        }

        public void GravarArquivo(String path, String conteudo, String nomeArquivo)
        {
            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                StreamWriter l_SrLog = new StreamWriter(path + nomeArquivo, false, Encoding.UTF8);
                l_SrLog.WriteLine(conteudo);
                l_SrLog.Flush();
                l_SrLog.Close();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// /
        /// </summary>
        /// <param name="a_Metodo"></param>
        /// <param name="a_Log"></param>
        public void GravaLog(String a_Metodo, String a_Log, bool email = false)
        {
            try
            {
                Global.Instance.RegistraMensagem(a_Metodo, a_Log);
                if (email)
                {
                    EnviarEmailLog(a_Metodo, a_Log);
                }

            }
            catch (Exception ex)
            {
                //i_EventLog.WriteEntry(a_Metodo + " - Erro: Falha ao gravar notificação no arquivo de LOG: " + ex.Message, EventLogEntryType.Error);

                // this.RegistraMensagem(a_Metodo, a_Log);
            }
        }

        private void EnviarEmailLog(string a_Metodo, string a_Log)
        {

            if (this.LastEmailSended == null || (DateTime.Now - LastEmailSended).Hours >= this.TempoDeEnvioDeEmail)
            {
                this.LastEmailSended = DateTime.Now;

                SmtpClient smtpClient = new SmtpClient();
                MailMessage message = new MailMessage();

                MailAddress autorEmail = new MailAddress(this.AutorEmail);

                smtpClient.Host = this.SmtpclientHost;

                smtpClient.Port = this.SmtpPort;

                message.From = autorEmail;

                foreach (var Destino in this.DestinatariosEmail)
                {
                    message.To.Add(Destino);
                }

                message.Subject = this.AssuntoEmail;

                message.IsBodyHtml = true;

                message.Body = String.Format("Mensagem de Log Do InfoConv: Metodo: {0}; Log: {1}", a_Metodo, a_Log);

                smtpClient.Send(message);
            }
        }

        #endregion

        /// <summary>
        /// retorna o certificado do Cliente, na pasta Certificados
        /// </summary>
        /// <returns></returns>
        public X509Certificate2 GetCertificateCliente()
        {
            //try
            //{
                
                var path = String.Format(@"{0}\\{1}\\{2}", System.AppDomain.CurrentDomain.BaseDirectory, "Certificados", this.CertificadoCliente);

                if (
                    File.Exists(path))
                {
                    GravaLog("GetCertificateCliente", "Arquivo EXITE");
                    //var Cert =
                    //    new X509Certificate2(
                    //        String.Format(@"{0}\\{1}\\{2}", System.AppDomain.CurrentDomain.BaseDirectory, "Certificados",
                    //            this.CertificadoCliente), this.CertificadoClienteSenha);
                }
                else
                {
                    GravaLog("GetCertificateCliente", "Arquivo nao Existe");
                }
        //    }
            //catch (Exception ex)
            //{
            //    GravaLog("GetCertificateCliente", string.Format(" Tipo ERRO: {3}: ERRO: {0} INNER: {1}  TRACE: {2}", ex.Message, ex.InnerException!=null? ex.InnerException.Message:"", ex.StackTrace, ex.GetBaseException().Source));
            //}

            return new X509Certificate2(String.Format(@"{0}\\{1}\\{2}", System.AppDomain.CurrentDomain.BaseDirectory, "Certificados", this.CertificadoCliente), this.CertificadoClienteSenha);
        }

        /// <summary>
        /// Retorna o Certificado de acesso a Maquina da SERPRO
        /// 
        /// </summary>
        /// <returns></returns>
        public X509Certificate2 GetCertificateService()
        {
            return new X509Certificate2(String.Format(@"{0}\\{1}\\{2}", System.AppDomain.CurrentDomain.BaseDirectory, "Certificados", this.CertificadoService));
        }
        
        public object ReturnObjeto(object retorno, object reflection)
        {

            foreach (var propertyIbj in reflection.GetType().GetProperties())
            {
                if (retorno.GetType().GetProperty(propertyIbj.Name) != null)
                {
                    retorno.GetType().GetProperty(propertyIbj.Name).SetValue(retorno, propertyIbj.GetValue(reflection));
                }
            }

            return retorno;
        }

        public T ReflectionObject<T>(T objetoRetorno, object objetoReflec)
        {
            foreach (var propertyIbj in objetoReflec.GetType().GetProperties())
            {
                objetoRetorno.GetType().GetProperty(propertyIbj.Name).SetValue(objetoRetorno, propertyIbj.GetValue(objetoReflec));
            }

            return objetoRetorno;
        }
        
        public void IniciarCargarReceita()
        {
            Global.Instance.GravaLog("[IniciarCargarReceita]", String.Format("{0}", "Buscar Fornecedores para Consulta "));

            // var test = new CargaReceitaRepository();

        }

        public String Serializar(object a_Objeto)
        {
            StringWriter l_stringWriter = new StringWriter();
            XmlSerializer l_xmlSerializer = new XmlSerializer(a_Objeto.GetType());

            l_xmlSerializer.Serialize(l_stringWriter, a_Objeto);

            return l_stringWriter.ToString();
        }

        public void CarregarParametros(NameValueCollection appSettings)
        {
            Global.Instance.CpfConsultante = appSettings["CpfConsultante"];               // "72604859572";
            Global.Instance.CertificadoCliente = appSettings["CertificadoCliente"];           // "certificadocadeias.pfx";
            Global.Instance.CertificadoClienteSenha = appSettings["CertificadoClienteSenha"];      // "senha";
            Global.Instance.CertificadoService = appSettings["CertificadoService"];           // "infoconv.receita.fazenda.gov.br.crt";
            Global.Instance.TipoPedido = appSettings["TipoPedido"];                   // "infoconv.receita.fazenda.gov.br.crt";
            Global.Instance.CodSistemaInfoconv = appSettings["CodSistemaInfoconv"];           // "infoconv.receita.fazenda.gov.br.crt";
            Global.Instance.CodPerfilInfoconv = appSettings["CodPerfilInfoconv"];            // "infoconv.receita.fazenda.gov.br.crt";
            Global.Instance.ChavePedido = appSettings["ChavePedido"];                  // "infoconv.receita.fazenda.gov.br.crt";


        }

        public List<string> ListarArquivosDiretorio(string Diretorio)
        {

            if (!Directory.Exists(Diretorio))
            {
                Directory.CreateDirectory(Diretorio);
            }

            var Listbox = new List<string>();
            DirectoryInfo Dir = new DirectoryInfo(@Diretorio);
            // Busca automaticamente todos os arquivos em todos os subdiretórios
            FileInfo[] Files = Dir.GetFiles("*", SearchOption.AllDirectories);
            foreach (FileInfo File in Files)
            {
                // Retira o diretório iformado inicialmente
                string FileName = File.FullName.Replace(Dir.FullName, "");
                Listbox.Add(FileName);
            }
            return Listbox;
        }

        public string TrnasformarObjetoEmLinha(object objetRetorno, string separador, bool header)
        {
            StringBuilder retorno = new StringBuilder();
            if (header)
            {
                foreach (var propertyInfo in retorno.GetType().GetProperties())
                {

                    retorno.Append(String.Format("{0};", propertyInfo.Name));

                }
                retorno.Remove(retorno.Length - 1, 1);
            }

            bool firstTime = true;
            foreach (var propertyInfo in retorno.GetType().GetProperties())
            {

                if (firstTime)
                {
                    retorno.AppendLine(String.Format("{0};", (propertyInfo.GetValue(objetRetorno) ?? "").ToString()));
                    firstTime = false;

                }
                else
                {
                    retorno.Append(String.Format("{0};", (propertyInfo.GetValue(objetRetorno) ?? "").ToString()));
                }


            }

            retorno.Remove(retorno.Length - 1, 1);

            return retorno.ToString();
        }

        public  bool IsCpf(string cpf)
        {
            int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            string tempCpf;
            string digito;
            int soma;
            int resto;
            cpf = cpf.Trim();
            cpf = cpf.Replace(".", "").Replace("-", "");
            if (cpf.Length != 11)
                return false;
            tempCpf = cpf.Substring(0, 9);
            soma = 0;

            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];
            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = resto.ToString();
            tempCpf = tempCpf + digito;
            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];
            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = digito + resto.ToString();
            return cpf.EndsWith(digito);
        }


    }
}
