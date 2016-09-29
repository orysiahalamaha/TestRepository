using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Web.Http;
using System.Web.UI;

namespace EmpeekTestProject.Controllers
{
    [RoutePrefix("api/files")]
    public class FileController : ApiController
    {
        public FileInfo[] files;
        public DirectoryInfo currentDirectory;
        public DirectoryInfo[] directories;

        public int lessThanTen = 0;
        public int betweenTenAndFifty = 0;
        public int moreThanOneHundred = 0;

        public const long ONE_MEGABYTE = 1048576;
        public const long TEN_MEGABYTES = ONE_MEGABYTE * 10;
        public const long FIFTY_MEGABYTES = ONE_MEGABYTE * 50;
        public const long ONE_HUNDRED_MEGABYTES = ONE_MEGABYTE * 100;


        /// <summary>
        /// function that search all files and subdirectories in current directory
        /// </summary>
        /// <param name="path"> directory path </param>
        /// <returns>return all files and subdirectories in current directory</returns>
        [Route("{*path}")]
        public IEnumerable<string> GetFiles(string path)
        {
            List<string> result = new List<string>();
            List<string> fullPath = new List<string>(); 
            try
            {
                currentDirectory = new DirectoryInfo(@path);
                files = currentDirectory.GetFiles("*.*");
                directories = currentDirectory.GetDirectories("*.*");            
            }
            catch (Exception ex)
            {
                result.Add(string.Format("Error:" + ex.Message));
                return result;
            }

            foreach (var directory in directories)
            {
                  result.Add(directory.FullName);   
            }

            foreach (var file in files)
            {
                result.Add(file.FullName);
            }

            return result;
        }
    
        /// <summary>
        /// the recursive function divides count of files in current directory and subdirectories according their size
        /// </summary>
        /// <param name="path">directory path</param>
        /// <returns>return count of files where file size less than 10mb, between 10mb and 50mb, more than 100 mb</returns>
        [Route("getfilesbysize")]
        public IEnumerable<int> GetFilesBySize(string path)
        {
            DirectoryInfo dir = new DirectoryInfo(path);
            DirectoryInfo[] subDir = new DirectoryInfo[0];
            FileInfo[] allFiles = new FileInfo[0];
            int[] sizes = new int[3];
            int[] temp = new int[3];
            try
            {
                subDir = dir.GetDirectories("*.*");
                allFiles = dir.GetFiles("*.*");

                foreach (var file in allFiles)
                {
                    if (file.Length <= TEN_MEGABYTES)
                    {
                        lessThanTen++;
                    }
                    else if (file.Length > TEN_MEGABYTES && file.Length <= FIFTY_MEGABYTES)
                    {
                        betweenTenAndFifty++;
                    }
                    else if (file.Length >= ONE_HUNDRED_MEGABYTES)
                    {
                        moreThanOneHundred++;
                    }
                }

                sizes = new int[] { lessThanTen, betweenTenAndFifty, moreThanOneHundred };
            }
            catch (Exception ex)
            {
           
            }

            foreach (var sub in subDir)
            {
                temp = GetFilesBySize(sub.FullName).ToArray();
                sizes[0] = temp[0];
                sizes[1] = temp[1];
                sizes[2] = temp[2];
            }

            return sizes;        
        }
    }
}
