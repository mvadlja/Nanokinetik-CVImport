namespace EMAService
{
    partial class ProjectInstaller
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.EMAServiceProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.EMAServiceInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // EMAServiceProcessInstaller
            // 
            this.EMAServiceProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalService;
            this.EMAServiceProcessInstaller.Password = null;
            this.EMAServiceProcessInstaller.Username = null;
            // 
            // EMAServiceInstaller
            // 
            this.EMAServiceInstaller.ServiceName = "EMAService";
            this.EMAServiceInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.EMAServiceProcessInstaller,
            this.EMAServiceInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller EMAServiceProcessInstaller;
        private System.ServiceProcess.ServiceInstaller EMAServiceInstaller;
    }
}