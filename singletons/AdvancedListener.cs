namespace StereoStructure
{
    static class AdvancedListener
    {
        private static AdvancedWindow advancedWindow;
        private static bool isAdvancedWindowOpened = false;

        public static void Show(MainWindow main)
        {
            advancedWindow = new AdvancedWindow(main);
            advancedWindow.Show();
            isAdvancedWindowOpened = true;
        }

        public static void Close()
        {
            advancedWindow.Close();
            SetAdvancedWindowOpened(false);
        }

        public static bool IsOpened()
        {
            return isAdvancedWindowOpened;
        }

        public static void SetAdvancedWindowOpened(bool value)
        {
            isAdvancedWindowOpened = value;
        }
    }
}
