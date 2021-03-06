﻿using Flurl;
using ImageResizer;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Web;
using Web.Helpers;
using Web.Interfaces;
using Web.Models;

namespace Web.Service
{
    public class ImageService : IImageService
    {
        public List<SavedImage> SaveFiles(string projectCode, string internalPath, string imageSize, HttpPostedFileBase file, bool newFile)
        {
            try
            {
                var inputFileName = Path.GetFileNameWithoutExtension(file.FileName);
                var inputFileExtension = Path.GetExtension(file.FileName);

                //for clean url path, if someone enter \, replaces double \\, triple need two calls to replace
                internalPath = internalPath.Replace("\\", "/").Replace("//", "/").Replace("//", "/");

                var projectDir = Path.Combine(AppSettings.MediaPath, projectCode);
                var projectWeb = Url.Combine(AppSettings.MediaUrl, projectCode);

                var serverDir = Path.Combine(projectDir, internalPath);
                Directory.CreateDirectory(serverDir);
                var serverSavePath = Path.Combine(serverDir, inputFileName + inputFileExtension);

                if (newFile)
                {
                    inputFileName = CheckFileExist(inputFileName, inputFileExtension, serverDir, 0);
                    serverSavePath = Path.Combine(serverDir, inputFileName + inputFileExtension);
                }

                file.SaveAs(serverSavePath);

                List<SavedImage> files = new List<SavedImage>();
                if (!string.IsNullOrEmpty(imageSize))
                {
                    var sizes = imageSize.ToLower().Split(',');
                    
                    foreach (var sizeXY in sizes)
                    {
                        var subDir = Path.Combine(projectDir, "_" + sizeXY, internalPath);
                        var subWeb = Url.Combine(projectWeb, "_" + sizeXY, internalPath);
                        var size = sizeXY.Split('x');
                        if (size.Length == 2)
                        {
                            int.TryParse(size[0], out int x);
                            int.TryParse(size[1], out int y);
                            if (x > 0 && y > 0)
                            {
                                ResizeSettings resizeSetting = new ResizeSettings
                                {
                                    Width = x,
                                    Height = y,
                                    Format = "png",
                                    Mode = FitMode.Pad,
                                    PaddingColor = Color.White,
                                };

                                Directory.CreateDirectory(subDir);
                                var subFile = Path.Combine(subDir, inputFileName + ".png");
                                ImageBuilder.Current.Build(serverSavePath, subFile, resizeSetting);
                                var saved = new SavedImage();
                                saved.Name = inputFileName + ".png";
                                saved.Size = sizeXY;
                                saved.Url = Url.Combine(subWeb, inputFileName + ".png");
                                files.Add(saved);
                            }
                        }
                    }
                }
                return files;
            }
            catch (System.Exception e)
            {
                var saved = new SavedImage();
                saved.Name = "ERROR";
                saved.Size = e.Message;
                saved.Url = AppSettings.WebSite;
                return new List<SavedImage>()
                    {
                        saved
                    };              
            }            
        }

        private static string CheckFileExist(string inputFileName, string inputFileExtension, string serverDir, int number)
        {
            var serverSavePath = Path.Combine(serverDir, inputFileName + GetNumberFileExtension(number) + inputFileExtension);
            if (File.Exists(serverSavePath))
            {
                return CheckFileExist(inputFileName, inputFileExtension, serverDir, ++number);

            }
            return inputFileName + GetNumberFileExtension(number);
        }

        private static string GetNumberFileExtension(int number)
        {
            if (number == 0)
            {
                return string.Empty;
            }
            return "_" + number.ToString();
        }
    }
}