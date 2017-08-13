using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Configuration.Install;

namespace NS_Collector_1_HvvApi
{
    [RunInstaller(true)]
    public class ServiceInstallerClass : Installer
    {
        private ServiceInstaller m_ThisService;
        private ServiceProcessInstaller m_ThisServiceProcess;

        public ServiceInstallerClass()
        {
            m_ThisService = new ServiceInstaller();
            m_ThisServiceProcess = new ServiceProcessInstaller();

            m_ThisServiceProcess.Account = ServiceAccount.LocalSystem;
            m_ThisService.ServiceName = "Attachment Dumper";
            m_ThisService.StartType = ServiceStartMode.Automatic;

            Installers.Add(m_ThisService);
            Installers.Add(m_ThisServiceProcess);
        }
    }
}
