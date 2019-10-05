using System;
using System.Linq;
using System.Reflection;
using System.Windows;
using TaskDialogInterop;

namespace SharedControls
{
    public static class TaskDialogService
    {
        private static string AddLineBreaks(this string source, int count = 1)
        {
            return (source ?? string.Empty) + string.Concat(Enumerable.Repeat(Environment.NewLine, count));
        }

        private static string PrefixLineBreaks(this string source, int count = 1)
        {
            return string.Concat(Enumerable.Repeat(Environment.NewLine, count)) + (source ?? string.Empty);
        }

        private static string GetVersionString(this Assembly source)
        {
            var version = source.GetName().Version;

            return version.Build + "." + version.Major + "." + version.Minor + "." + version.Revision;
        }

        public static void ShowApplicationError(Window that, Exception ex)
        {
            ShowError(that,
                "We are sorry an unrecoverable error occured during the last operation. You can attempt it again and if problems persist please contact our support department using the" +
                " hyperlink below. Please feel free to add additional information, especially what you were doing when the application occurred."
                , "Error Message:".PrefixLineBreaks().AddLineBreaks() + ex.Message, (dialog, args, data) =>
                {
                    if (args.Notification == VistaTaskDialogNotification.HyperlinkClicked)
                    {
                        var body = "Please fill in as much of the information below as you can: ".AddLineBreaks(4);
                        body += "Application Version: " + Assembly.GetExecutingAssembly().GetVersionString().AddLineBreaks(2);
                        body += "Windows Version: " + Environment.OSVersion.ToString().AddLineBreaks(2);
                        body += "Detailed Exception: ".AddLineBreaks(2);
                        body += ex.ToString();

                        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo($"mailto:{args.Hyperlink}?subject=Diagram Designer Error&body={body}"));

                        return true;
                    }

                    return false;
                });
        }


        public static void ShowValidationError(Window that, string mainInstruction)
        {
            var config = new TaskDialogOptions
            {
                Owner = that,
                Title = "Validation Error",
                MainInstruction = mainInstruction,
                CustomButtons = new[] { "&Ok" },
                MainIcon = VistaTaskDialogIcon.Warning,
            };

            TaskDialog.Show(config);
        }

        public static void ShowError(Window that, string mainInstruction, string expandedInfo, TaskDialogCallback handler = null)
        {
            var config = new TaskDialogOptions
            {
                Owner = that,
                Title = "Adventure World",
                MainInstruction = mainInstruction,
                ExpandedInfo = expandedInfo,
                CustomButtons = new[] {"&Ok"},
                FooterIcon = VistaTaskDialogIcon.Shield,
                FooterText = "MLD Computing Support: <a href=\"enquiries@mldcomputing.com\">enquiries@mldcomputing.com</a>.",
                MainIcon = VistaTaskDialogIcon.Error,
                Callback = handler
            };

            TaskDialog.Show(config);
        }


        public static void ShowWarning(Window that, string mainInstruction, TaskDialogCallback handler = null)
        {
            var config = new TaskDialogOptions
            {
                Owner = that,
                Title = "Adventure World",
                MainInstruction = mainInstruction,
                CustomButtons = new[] { "&Ok" },
                FooterIcon = VistaTaskDialogIcon.Shield,
                FooterText = "MLD Computing Support: <a href=\"enquiries@mldcomputing.com\">enquiries@mldcomputing.com</a>.",
                MainIcon = VistaTaskDialogIcon.Warning,
                Callback = handler
            };

            TaskDialog.Show(config);
        }

        public static TaskDialogResult AskQuestion(Window that, string mainInstruction, string verificationText,
            string[] customButtons)
        {
            var config = new TaskDialogOptions
            {
                Owner = that,
                Title = "Adventure World",
                MainInstruction = mainInstruction,
                VerificationText = verificationText,
                CustomButtons = customButtons,
                MainIcon = VistaTaskDialogIcon.Warning
            };
            
           return TaskDialog.Show(config);
        }
    }
}
