using System;
using Microsoft.Deployment.WindowsInstaller;

namespace CustomSetupAction
{
  public static class CustomActions
  {
    [CustomAction]
    public static ActionResult UpgradeConfigFiles(Session session)
    {
      try
      {
        session.Log("Starting CustomAction");
        TryUpgrade(session);
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

    private static void TryUpgrade(Session session)
    {
      int numericValue = Convert.ToInt32(session.CustomActionData["TYPE"]);
      bool installToUserProfile = Convert.ToBoolean(numericValue);
      string targetDir = session.CustomActionData["LOCATION"]; ;
      Terminals.UpgradeConfigFiles.CheckPortableInstallType(targetDir, installToUserProfile);
    }
  }
}
