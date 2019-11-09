using Cloning_Vat.Events;
using Cloning_Vat.Serialization;
using Cloning_Vat.Serialization.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Cloning_Vat.Service
{
    /// <summary>
    /// Back end service for performing the cloning operation
    /// </summary>
    public class CloningVatService
    {
        private ConfigurationFile configuration;

        /// <summary>
        /// True when the cloning operation has completed.
        /// </summary>
        public bool IsFinished { get; private set; } = false;

        /// <summary>
        /// Stores the current status of the cloning operation
        /// </summary>
        public string Status { get; private set; }


        public CloningVatService(string configurationFilePath)
        {
            configuration = ParseConfigFile(configurationFilePath);
        }

        public CloningVatService(ConfigurationFile file)
        {
            configuration = file;
        }

        private ConfigurationFile ParseConfigFile(string path)
        {
            ConfigurationSerializer serializer = new ConfigurationSerializer();

            return serializer.LoadFromFile(path);
        }

        /// <summary>
        /// Invoke this method to begin the cloning process with the loaded config data
        /// </summary>
        public void ExecuteCloningProcess()
        {
            UpdateStatus("Executing Cloning Process...");
            UpdateStatus($"Found {configuration.CloneTasks.Count} cloning tasks in the xml config.");
            
            foreach (CloneTask task in configuration.CloneTasks)
            {
                UpdateStatus($"Begin next cloning task.  Source directory is '{task.Source}'.");

                // if the source doesn't exist, abort so the user doesn't do bad things
                if (!Directory.Exists(task.Source))
                {
                    throw new DirectoryNotFoundException($"Unable to find the source directory: '{task.Source}");
                }

                foreach (CloneDestination destination in task.Destinations)
                {
                    UpdateStatus($"[X] Wipe directory '{destination.Path}'");
                    WipeDirectory(destination);

                    UpdateStatus($"[+] Clone Contents of '{task.Source}' into '{destination.Path}'");
                    CloneDirectory(task.Source, destination.Path, true);
                }
            }
            UpdateStatus("Finished cloning all the things.");
        }

        private void WipeDirectory(CloneDestination destination)
        {
            // Delete the entire folder if it exists
            if (Directory.Exists(destination.Path))
            {
                UpdateStatus("\tDirectory existed.  Not anymore...");
                Directory.Delete(destination.Path, true);
            }

            UpdateStatus("\t(Re)Created directory.");
            // Create a new folder there
            Directory.CreateDirectory(destination.Path);
        }

        /// <summary>
        /// I literally stole this from the internet somewhere
        /// </summary>
        /// <param name="sourceDirName"></param>
        /// <param name="destDirName"></param>
        /// <param name="copySubDirs"></param>
        private void CloneDirectory(string sourceDirName, string destDirName, bool copySubDirs)
        {
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);
            DirectoryInfo[] dirs = dir.GetDirectories();

            // If the source directory does not exist, throw an exception.
            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            // If the destination directory does not exist, create it.
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }


            // Get the file contents of the directory to copy.
            FileInfo[] files = dir.GetFiles();

            foreach (FileInfo file in files)
            {
                // Create the path to the new copy of the file.
                string temppath = Path.Combine(destDirName, file.Name);

                // Copy the file.
                file.CopyTo(temppath, false);
            }

            // If copySubDirs is true, copy the subdirectories.
            if (copySubDirs)
            {

                foreach (DirectoryInfo subdir in dirs)
                {
                    // Create the subdirectory.
                    string temppath = Path.Combine(destDirName, subdir.Name);

                    // Copy the subdirectories.
                    CloneDirectory(subdir.FullName, temppath, copySubDirs);
                }
            }
        }

        #region status update event handling


        private void UpdateStatus(string status)
        {
            string timestampedStatus = $"[{DateTime.Now}]: {status}";
            Status = timestampedStatus;
            OnStatusUpdated(new StatusUpdatedEventArgs(timestampedStatus));
        }

        /// <summary>
        /// Provides an event hook to display the current status to the user.
        /// </summary>
        public event EventHandler StatusUpdated;

        protected virtual void OnStatusUpdated(StatusUpdatedEventArgs args)
        {
            StatusUpdated?.Invoke(this, args);
        }

        #endregion

    }
}
