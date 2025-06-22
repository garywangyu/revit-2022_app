using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace RevitPlugin.Commands
{
    [Transaction(TransactionMode.Manual)]
    public class Command1_Manage : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            using var form = new ButtonImageForm();
            form.ShowDialog();
            return Result.Succeeded;
        }
    }
}
