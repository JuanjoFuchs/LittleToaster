using System;
using System.Windows.Forms;

namespace LittleToaster
{
  static class Program
  {
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main(string[] args)
    {
      var message = string.Empty;
      if (args != null && args.Length > 0) {
        message = args[0];
      }

      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);
      Application.Run(new Toaster(message));
    }
  }
}
