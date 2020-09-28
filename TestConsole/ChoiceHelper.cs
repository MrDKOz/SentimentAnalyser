using System;
using System.IO;

namespace TestConsole
{
    public static class ChoiceHelper
    {
        /// <summary>
        /// Keeps asking for a choice until a valid one is provided.
        /// </summary>
        /// <param name="max">The maximum choice allowed.</param>
        /// <returns>The choice when valid.</returns>
        public static int MenuChoice(int max)
        {
            string choice;
            int numericalChoice;

            do
            {
                Console.WriteLine("Enter your choice (Default 1):");
                choice = Console.ReadLine();
                choice = string.IsNullOrEmpty(choice) ? "1" : choice;
            } while (!int.TryParse(choice, out numericalChoice) || numericalChoice < 1 || numericalChoice > max);

            return numericalChoice;
        }

        /// <summary>
        /// Keeps asking for a string input with the provided message,
        /// until one is provided.
        /// </summary>
        /// <param name="message">The message to display.</param>
        /// <returns>The entered string when valid.</returns>
        public static string TextInput(string message)
        {
            string validString;

            do
            {
                Console.WriteLine(message);

                validString = Console.ReadLine()?.Trim();

            } while (string.IsNullOrEmpty(validString));

            return validString;
        }

        /// <summary>
        /// Keeps asking for a number between the given range,
        /// until a valid one is provided.
        /// </summary>
        /// <param name="message">The message to display.</param>
        /// <param name="min">The minimum choice allowed.</param>
        /// <param name="max">The maximum choice allowed.</param>
        /// <returns>The choice when valid.</returns>
        public static int NumericalInput(string message, int min, int max)
        {
            string choice;
            int numericalChoice;

            do
            {
                Console.WriteLine(message);
                choice = Console.ReadLine();
                choice = string.IsNullOrEmpty(choice) ? "1" : choice;
            } while (!int.TryParse(choice, out numericalChoice) || numericalChoice < min || numericalChoice > max);

            return numericalChoice;
        }

        /// <summary>
        /// Keeps asking for a file path with the provided message,
        /// until one is provided.
        /// </summary>
        /// <param name="message">The message to display.</param>
        /// <returns>The entered path when valid.</returns>
        public static string FilePathInput(string message)
        {
            string path;

            do
            {
                Console.WriteLine(message);

                path = Console.ReadLine()?.Trim();

            } while (string.IsNullOrEmpty(path) || !File.Exists(path));

            return path;
        }

        /// <summary>
        /// Keeps asking for a valid directory path, until one exists
        /// (The directory does not have to exist for it to be valid)
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>The directory path to use</returns>
        public static string DirectoryPath(string message)
        {
            string dirPath;

            do
            {
                Console.WriteLine(message);

                dirPath = Console.ReadLine();
            } while (!IsValidPath(dirPath));

            return dirPath;
        }

        private static bool IsValidPath(string path)
        {
            return TryGetFullPath(path, out _);
        }

        private static bool TryGetFullPath(string path, out string result)
        {
            var status = false;
            result = string.Empty;

            if (string.IsNullOrEmpty(path)) return false;

            try
            {
                result = Path.GetFullPath(path);
                status = true;
            }
            catch (Exception)
            {
                // ignored
            }

            return status;
        }
    }
}
