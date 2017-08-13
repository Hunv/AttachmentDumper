using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace AttachmentDumper
{
    public partial class DumperService : ServiceBase
    {
        List<ConfigurationSet> Configuration = new List<ConfigurationSet>();
        System.Timers.Timer TmrRecheck = new System.Timers.Timer(60000);

        public DumperService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
#if DEBUG
            Debugger.Launch();
#endif

            LoadSettings();
            TmrRecheck.Elapsed += TmrRecheck_Elapsed;
            TmrRecheck.Start();
        }

        private void TmrRecheck_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            var imapConnector = new ImapConnector();

            foreach (var aCfg in Configuration)
            {
                if (!System.IO.Directory.Exists(aCfg.OutputDirectory))
                    System.IO.Directory.CreateDirectory(aCfg.OutputDirectory);

                imapConnector.GetAttachments(
                    aCfg.Server,
                    aCfg.Port,
                    aCfg.Username,
                    aCfg.Password,
                    aCfg.OutputDirectory,
                    aCfg.FileExtensions);
            }
        }

        protected override void OnStop()
        {
        }

        private void LoadSettings()
        {
            var sR = new System.IO.StreamReader(AppDomain.CurrentDomain.BaseDirectory + "\\config.cfg");
            ConfigurationSet cfg = null;

            while(sR.Peek() >= 0)
            {
                var line = sR.ReadLine().Split(new char[] { '=' }, 2);

                if (line[0].Length == 0 || line.Length == 1 || line[0].StartsWith("#"))
                    continue;

                switch(line[0].ToLower())
                {
                    case "server":
                        if (cfg != null)
                            Configuration.Add(cfg);
                        cfg = new ConfigurationSet();
                        cfg.Server = line[1].Trim();
                        break;
                    case "port":
                        cfg.Port = Convert.ToInt32(line[1].Trim());
                        break;
                    case "username":
                        cfg.Username = line[1].Trim();
                        break;
                    case "password":
                        cfg.Password = line[1].Trim();
                        break;
                    case "outputdir":
                        cfg.OutputDirectory = line[1].Trim();
                        break;
                    case "fileextensions":
                        cfg.FileExtensions = line[1].Trim().Split(',');
                        break;
                }
            }
                        
            sR.Close();
            Configuration.Add(cfg);
        }
    }
}
