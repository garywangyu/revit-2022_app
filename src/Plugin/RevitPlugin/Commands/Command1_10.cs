using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;
using System.Windows.Forms;
using Autodesk.Revit.DB;

namespace RevitPlugin.Commands
{
    [Transaction(TransactionMode.Manual)]
    public class Command1_10 : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            MessageBox.Show("1_10 待製作中", "Command 1_10");
            return Result.Succeeded;
        }
    }
}
