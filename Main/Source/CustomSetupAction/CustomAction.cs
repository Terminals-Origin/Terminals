using System;
using Microsoft.Deployment.WindowsInstaller;

namespace CustomSetupAction
{
  public static class CustomActions
  {
    [CustomAction]
    public static ActionResult UpgradeLog4NetConfig(Session session)
    {
      try
      {
        session.Log("Starting CustomAction");
        CustomMethodInCustomAction(session);
        return ActionResult.Success;
      }
      catch (Exception exception)
      {
        session.Log("Custom action failed with message {0} at:\r\n{1}", exception.Message, exception.StackTrace);
        return ActionResult.Failure;
      }
      finally
      {
        session.Log("Ending CustomAction");
      }
    }

    private static void CustomMethodInCustomAction(Session session)
    {
      int numericValue = Convert.ToInt32(session["INSTALLTYPE"]);
      bool installToUserProfile = Convert.ToBoolean(numericValue);
      string targetDir = session["INSTALLFOLDER"];
      Terminals.UpgradeConfigFiles.CheckPortableInstallType(targetDir, installToUserProfile);
    }
  }
}
