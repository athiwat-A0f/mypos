using mystock.Components;

namespace mystock.Helper
{
    public static class Alert
    {
        public static void Success(string msg)
        {
            new AlertDialog(msg, "success").ShowDialog();
        }

        public static void Warning(string msg)
        {
            new AlertDialog(msg, "warning").ShowDialog();
        }

        public static void Error(string msg)
        {
            new AlertDialog(msg, "error").ShowDialog();
        }

        public static void Info(string msg)
        {
            new AlertDialog(msg, "info").ShowDialog();
        }

        public static void Question(string msg)
        {
            new AlertDialog2(msg, "question").ShowDialog();
        }
    }
}