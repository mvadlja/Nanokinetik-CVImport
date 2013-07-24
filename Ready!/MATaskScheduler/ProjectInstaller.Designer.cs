namespace SandozTaskScheduler
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
            this.MATaskSchedulerTestProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.MATaskSchedulerTestInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // MATaskSchedulerTestProcessInstaller
            // 
            this.MATaskSchedulerTestProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalService;
            this.MATaskSchedulerTestProcessInstaller.Password = null;
            this.MATaskSchedulerTestProcessInstaller.Username = null;
            // 
            // MATaskSchedulerTestInstaller
            // 
            this.MATaskSchedulerTestInstaller.ServiceName = "MATaskScheduler";
            this.MATaskSchedulerTestInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.MATaskSchedulerTestProcessInstaller,
            this.MATaskSchedulerTestInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller MATaskSchedulerTestProcessInstaller;
        private System.ServiceProcess.ServiceInstaller MATaskSchedulerTestInstaller;
    }
}