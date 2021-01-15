using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using EcommApiCoreV3.BAL;
using EcommApiCoreV3.BAL.Interface;
using EcommApiCoreV3.Entities;
using EcommApiCoreV3.Repository;
using EcommApiCoreV3.Repository.Interface;
using System.Drawing;

namespace EcommApiCoreV3.Controllers.Common
{
    public class Utilities
    {
        public void SaveImage(int ProductId, string[] FileSource, string Type, string WebRootPath)
        {
            if (ProductId > 0)
            {
                if (FileSource.Length > 0)
                {
                    string FolderPath;
                    FolderPath = WebRootPath + "\\ProductImage\\" + ProductId + "\\" + Type + "\\";
                    bool folderExists = System.IO.Directory.Exists(FolderPath);
                    if (!folderExists)
                        Directory.CreateDirectory(FolderPath);
                    DirectoryInfo directory = new DirectoryInfo(FolderPath);
                    //foreach (FileInfo file in directory.GetFiles())
                    //{
                    //    file.Delete();
                    //}
                    for (int i = 0; i < FileSource.Length; i++)
                    {
                        string filename = ProductId.ToString() + '-' + DateTime.Now.ToString("MMddyyyyhhmmss") + "-" + (i + 1) + ".jpg";
                        string fileNameWitPath = FolderPath + filename;
                        if (FileSource[i].Contains("data:image/jpeg;base64,") || FileSource[i].Contains("data:image/jpg;base64,") || FileSource[i].Contains("data:image/png;base64,"))
                        {
                            //using (FileStream fs = new FileStream(fileNameWitPath, FileMode.Create))
                            //{
                            //    using (BinaryWriter bw = new BinaryWriter(fs))
                            //    {
                            int Img_Height = 1000;
                            int Img_Width = 736;
                            if (FileSource[i].Contains("data:image/jpeg;base64,"))
                            {
                                byte[] data = Convert.FromBase64String(FileSource[i].Replace("data:image/jpeg;base64,", ""));
                                //bw.Write(data);
                                //bw.Close();

                                Bitmap bmpFromString = Base64StringToBitmap(data);

                                ImageCompress imgCompress = ImageCompress.GetImageCompressObject;
                                imgCompress.GetImage = new System.Drawing.Bitmap(bmpFromString);
                                imgCompress.Height = Img_Height;
                                imgCompress.Width = Img_Width;
                                imgCompress.Save(filename, FolderPath);

                            }
                            if (FileSource[i].Contains("data:image/jpg;base64,"))
                            {
                                byte[] data = Convert.FromBase64String(FileSource[i].Replace("data:image/jpg;base64,", ""));
                                //bw.Write(data);
                                //bw.Close();

                                Bitmap bmpFromString = Base64StringToBitmap(data);

                                ImageCompress imgCompress = ImageCompress.GetImageCompressObject;
                                imgCompress.GetImage = new System.Drawing.Bitmap(bmpFromString);
                                imgCompress.Height = Img_Height;
                                imgCompress.Width = Img_Width;
                                imgCompress.Save(filename, FolderPath);
                            }
                            if (FileSource[i].Contains("data:image/png;base64,"))
                            {
                                byte[] data = Convert.FromBase64String(FileSource[i].Replace("data:image/png;base64,", ""));
                                //bw.Write(data);
                                //bw.Close();

                                Bitmap bmpFromString = Base64StringToBitmap(data);

                                ImageCompress imgCompress = ImageCompress.GetImageCompressObject;
                                imgCompress.GetImage = new System.Drawing.Bitmap(bmpFromString);
                                imgCompress.Height = Img_Height;
                                imgCompress.Width = Img_Width;
                                imgCompress.Save(filename, FolderPath);
                            }
                            if (Type == "bannerImage" || Type == "frontImage")
                            {
                                ProductRepository obj = new ProductRepository();
                                Product product = new Product();
                                product.ImagePath = filename;
                                product.ProductID = ProductId;
                                product.Type = Type;
                                obj.SaveProductImages(product);
                            }

                            //    }
                            //}
                        }
                    }

                }
            }
        }
        public string[] ProductImagePath(int ProductId, string Type, string WebRootPath)
        {
            string[] base64ImageRepresentation = new string[0];
            string folderPath;
            folderPath = WebRootPath + "\\ProductImage\\" + ProductId + "\\" + Type + "\\";
            if (Directory.Exists(folderPath))
            {
                string[] AllFiles = Directory.GetFiles(folderPath, "*", SearchOption.AllDirectories);
                base64ImageRepresentation = new string[AllFiles.Length];
                for (int i = 0; i < AllFiles.Length; i++)
                {
                    //byte[] imageArray = System.IO.File.ReadAllBytes(AllFiles[i]);
                    //base64ImageRepresentation[i] = "data:image/jpeg;base64," + Convert.ToBase64String(imageArray);
                    base64ImageRepresentation[i] = AllFiles[i].Split('\\').LastOrDefault();
                }
            }
            return base64ImageRepresentation;
        }

        public string[] ProductImage(int ProductId, string Type, string WebRootPath, int ProductSizeColorId)
        {
            string[] ImageRepresentation = new string[0];
            string folderPath;
            folderPath = WebRootPath + "\\ProductImage\\" + ProductId + "\\" + Type + "\\" + ProductSizeColorId + "\\";
            if (Directory.Exists(folderPath))
            {
                string[] AllFiles = Directory.GetFiles(folderPath, "*", SearchOption.AllDirectories);
                ImageRepresentation = new string[AllFiles.Length];
                for (int i = 0; i < AllFiles.Length; i++)
                {
                    ImageRepresentation[i] = AllFiles[i].Split('\\').LastOrDefault();
                }
            }
            return ImageRepresentation;
        }

        public void DeleteProductImagePath(int ProductId, string Type, string WebRootPath)
        {
            string[] base64ImageRepresentation = new string[0];
            string folderPath;
            folderPath = WebRootPath + "\\ProductImage\\" + ProductId + "\\" + Type + "\\";
            if (Directory.Exists(folderPath))
            {
                string[] AllFiles = Directory.GetFiles(folderPath, "*", SearchOption.AllDirectories);
                for (int i = 0; i < AllFiles.Length; i++)
                {
                    File.Delete(AllFiles[i]);
                }
                Directory.Delete(folderPath);
            }
        }

        public void DeleteProductImage(string ImagePath, string WebRootPath)
        {
            string filePath;
            filePath = WebRootPath + ImagePath;
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        public static Bitmap Base64StringToBitmap(byte[] byteBuffer)
        {
            Bitmap bmpReturn = null;
            //Convert Base64 string to byte[]
            //byte[] byteBuffer = Convert.FromBase64String(base64String);
            MemoryStream memoryStream = new MemoryStream(byteBuffer);

            memoryStream.Position = 0;

            bmpReturn = (Bitmap)Bitmap.FromStream(memoryStream);

            memoryStream.Close();
            memoryStream = null;
            byteBuffer = null;

            return bmpReturn;
        }

        public void SaveUserDocumentImages(int UserId, string[] FileSource, string WebRootPath)
        {
            if (UserId > 0)
            {
                if (FileSource.Length > 0)
                {
                    string FolderPath;
                    FolderPath = WebRootPath + "\\UserDocument\\" + UserId + "\\";
                    bool folderExists = System.IO.Directory.Exists(FolderPath);
                    if (!folderExists)
                        Directory.CreateDirectory(FolderPath);
                    DirectoryInfo directory = new DirectoryInfo(FolderPath);
                    //foreach (FileInfo file in directory.GetFiles())
                    //{
                    //    file.Delete();
                    //}
                    for (int i = 0; i < FileSource.Length; i++)
                    {
                        string filename = UserId.ToString() + '-' + DateTime.Now.ToString("MMddyyyyhhmmss") + "-" + (i + 1);
                        if (FileSource[i].Contains("data:application/pdf;base64,"))
                        {
                            filename = filename + ".pdf";
                        }
                        else
                        {
                            filename = filename + ".jpg";
                        }
                        string fileNameWitPath = FolderPath + filename;
                        if (FileSource[i].Contains("data:image/jpeg;base64,") || FileSource[i].Contains("data:image/jpg;base64,") || FileSource[i].Contains("data:image/png;base64,") || FileSource[i].Contains("data:application/pdf;base64,"))
                        {
                            //using (FileStream fs = new FileStream(fileNameWitPath, FileMode.Create))
                            //{
                            //    using (BinaryWriter bw = new BinaryWriter(fs))
                            //    {
                            //int Img_Height = 285;
                            //int Img_Width = 285;
                            if (FileSource[i].Contains("data:image/jpeg;base64,"))
                            {
                                byte[] data = Convert.FromBase64String(FileSource[i].Replace("data:image/jpeg;base64,", ""));
                                //bw.Write(data);
                                //bw.Close();

                                Bitmap bmpFromString = Base64StringToBitmap(data);

                                ImageCompress imgCompress = ImageCompress.GetImageCompressObject;
                                imgCompress.GetImage = new System.Drawing.Bitmap(bmpFromString);
                                //imgCompress.Height = Img_Height;
                                //imgCompress.Width = Img_Width;
                                imgCompress.Save(filename, FolderPath);

                            }
                            if (FileSource[i].Contains("data:image/jpg;base64,"))
                            {
                                byte[] data = Convert.FromBase64String(FileSource[i].Replace("data:image/jpg;base64,", ""));
                                //bw.Write(data);
                                //bw.Close();

                                Bitmap bmpFromString = Base64StringToBitmap(data);

                                ImageCompress imgCompress = ImageCompress.GetImageCompressObject;
                                imgCompress.GetImage = new System.Drawing.Bitmap(bmpFromString);
                                //imgCompress.Height = Img_Height;
                                //imgCompress.Width = Img_Width;
                                imgCompress.Save(filename, FolderPath);
                            }
                            if (FileSource[i].Contains("data:image/png;base64,"))
                            {
                                byte[] data = Convert.FromBase64String(FileSource[i].Replace("data:image/png;base64,", ""));
                                //bw.Write(data);
                                //bw.Close();

                                Bitmap bmpFromString = Base64StringToBitmap(data);

                                ImageCompress imgCompress = ImageCompress.GetImageCompressObject;
                                imgCompress.GetImage = new System.Drawing.Bitmap(bmpFromString);
                                //imgCompress.Height = Img_Height;
                                //imgCompress.Width = Img_Width;
                                imgCompress.Save(filename, FolderPath);
                            }
                            if (FileSource[i].Contains("data:application/pdf;base64,"))
                            {
                                using (FileStream fs = new FileStream(fileNameWitPath, FileMode.Create))
                                {
                                    using (BinaryWriter bw = new BinaryWriter(fs))
                                    {
                                        byte[] data = Convert.FromBase64String(FileSource[i].Replace("data:application/pdf;base64,", ""));
                                        bw.Write(data);
                                        bw.Close();
                                    }
                                }
                            }
                        }
                    }

                }
            }
        }

        public string[] UserDocument(int UserId, string WebRootPath)
        {
            string[] ImageRepresentation = new string[0];
            string folderPath;
            folderPath = WebRootPath + "\\UserDocument\\" + UserId + "\\";
            if (Directory.Exists(folderPath))
            {
                string[] AllFiles = Directory.GetFiles(folderPath, "*", SearchOption.AllDirectories);
                ImageRepresentation = new string[AllFiles.Length];
                for (int i = 0; i < AllFiles.Length; i++)
                {
                    ImageRepresentation[i] = AllFiles[i].Split('\\').LastOrDefault();
                }
            }
            return ImageRepresentation;
        }
    }
}
