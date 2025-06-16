using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;
using System.Windows.Forms;
using Autodesk.Revit.DB;

namespace RevitPlugin.Commands
{
    [Transaction(TransactionMode.Manual)]
    public class Command2_7 : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            MessageBox.Show("2_7 待製作中", "Command 2_7");
            return Result.Succeeded;
        }
    }
}
