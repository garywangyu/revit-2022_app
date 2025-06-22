using Autodesk.Revit.UI;

namespace RevitPlugin.Managers
{
    internal static class DynamoPlayerUtil
    {
        public static void OpenPlayer(UIApplication app)
        {
            var cmdId = RevitCommandId.LookupPostableCommandId(PostableCommand.DynamoPlayer);
            app.PostCommand(cmdId);
        }
    }
}
