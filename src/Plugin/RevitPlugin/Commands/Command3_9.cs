using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;
using System.Windows.Forms;
using Autodesk.Revit.DB;

namespace RevitPlugin.Commands
{
    [Transaction(TransactionMode.Manual)]
    public class Command3_9 : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            MessageBox.Show("3_9 待製作中", "Command 3_9");
            return Result.Succeeded;
        }
    }
}
