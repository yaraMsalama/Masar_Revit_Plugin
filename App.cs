using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Masarr_Revit_Plugin.App
{
    public class App : IExternalApplication
    {
        public Result OnShutdown(UIControlledApplication application)
        {
            try
            {
                // Create ribbon tab
                string tabName = "Auto Sheet Generation";
                application.CreateRibbonTab(tabName);

                // Create ribbon panel
                RibbonPanel panel = application.CreateRibbonPanel(tabName, "Tools");

                // Create push button
                string assemblyPath = Assembly.GetExecutingAssembly().Location;
                PushButtonData buttonData = new PushButtonData(
                    "AutoSheetGeneration",
                    "Auto Sheet\nGeneration",
                    assemblyPath,
                    typeof(ShowWpfCommand).FullName)
                {
                    ToolTip = "Open Auto Sheet Generation tool"
                };

                // Add icon (optional, ensure icon.png exists in project)
                string iconPath = Path.Combine(Path.GetDirectoryName(assemblyPath), "icon.png");
                if (File.Exists(iconPath))
                {
                    buttonData.LargeImage = new BitmapImage(new Uri(iconPath));
                }

                panel.AddItem(buttonData);
                return Result.Succeeded;
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Error", $"Failed to initialize add-in: {ex.Message}");
                return Result.Failed;
            }
        }

        public Result OnStartup(UIControlledApplication application)
        {
            return Result.Succeeded;
        }
        [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
        public class ShowWpfCommand : IExternalCommand
        {
            public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
            {
                try
                {
                    Views.MainWindow window = new Views.MainWindow(commandData);
                    window.ShowDialog();
                    return Result.Succeeded;
                }
                catch (Exception ex)
                {
                    message = ex.Message;
                    return Result.Failed;
                }
            }
        }
    }
}
