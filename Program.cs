using System;
using System.Diagnostics;
using System.Linq;

namespace GitBoom
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("[GitBoom] Cleaning process is starting...");
            Console.ResetColor();

            // STEP 1: Security Shield (Protected branches)
            string[] protectedBranches = { "main", "master", "develop", "dev" };
            string activeBranch = RunGitCommand("branch --show-current");

            if (string.IsNullOrEmpty(activeBranch))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("ERROR: This is not a Git repository!");
                Console.ResetColor();
                return;
            }

            // STEP 2: Intelligence Gathering (Listing targets)
            string branchesOutput = RunGitCommand("branch");
            string[] lines = branchesOutput.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

            // Filtering out protected and active branches
            var trashBranches = lines
                .Select(line => line.Replace("*", "").Trim())
                .Where(branch => !string.IsNullOrEmpty(branch) && !protectedBranches.Contains(branch) && branch != activeBranch.Trim())
                .ToList();

            // If everything is already clean, exit immediately
            if (trashBranches.Count == 0)
            {
                Console.WriteLine("=================================================");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Already clean! No trash branches found except protected ones.");
                Console.ResetColor();
                Console.WriteLine("=================================================");
                return;
            }

            // --- INTERACTIVE CONFIRMATION MECHANISM ---
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write($"WARNING: Detected {trashBranches.Count} trash branch(es). Are you sure you want to blast them all? [y/N]: ");
            Console.ResetColor();
            
            string input = Console.ReadLine()?.Trim().ToLower() ?? "";

            if (input != "y")
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\nOperation canceled by user. Nothing was deleted.");
                Console.ResetColor();
                return; 
            }

            Console.WriteLine("\nAccess granted! Detonating trash branches...\n");

            // STEP 3: Loop & Destroy
            int deletedBranches = 0;
            foreach (var branch in trashBranches)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[BOOM] {branch} deleted.");
                Console.ResetColor();

                RunGitCommand($"branch -D {branch}");
                deletedBranches++;
            }

            // STEP 4: Reporting
            Console.WriteLine("=================================================");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"SUCCESS: Successfully destroyed {deletedBranches} trash branch(es)!");
            Console.ResetColor();
        }

        // Helper function to run Git commands in the background
        static string RunGitCommand(string arguments)
        {
            try
            {
                using var process = new Process();
                process.StartInfo.FileName = "git";
                process.StartInfo.Arguments = arguments;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;
                
                process.Start();
                string output = process.StandardOutput.ReadToEnd();
                process.WaitForExit();
                
                return process.ExitCode == 0 ? output : string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}
