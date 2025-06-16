using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;
using System.Windows.Forms;
using Autodesk.Revit.DB;

namespace RevitPlugin.Commands
{
    [Transaction(TransactionMode.Manual)]
    public class Command2_8 : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            MessageBox.Show("2_8 待製作中", "Command 2_8");
            return Result.Succeeded;
        }
    }
}
