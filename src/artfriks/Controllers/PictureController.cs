using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using ImageSharp;
using Microsoft.Net.Http.Headers;
using Microsoft.AspNetCore.Hosting;

namespace artfriks.Controllers
{

    public class pictureController : Controller
    {
        private readonly IHostingEnvironment _appEnvironment;
        public pictureController(IHostingEnvironment appEnvironment)
        {
            _appEnvironment = appEnvironment;
        }
        static byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }
        [HttpGet]
        [Route("api/picture/delete")]
        public ActionResult DeleteImage(string imageId)
        {
            string pathString;
            if (imageId == null)
            {
                return NotFound(new { Message = "No image selected", status = "success", code = "403" });
            }
            else
            {

                try
                {
                    var originalDirectory = new DirectoryInfo(string.Format("{0}\\wwwroot\\WallImages", _appEnvironment.ContentRootPath));
                    pathString = System.IO.Path.Combine(originalDirectory.ToString(), "imagepath");

                    FileInfo fname2 = new FileInfo(pathString + "\\" + imageId);
                    FileInfo thumb1 = new FileInfo(pathString + "\\thumb-" + imageId);
                    thumb1.Delete();
                    fname2.Delete();

                    return Json(new { Message = "Success", status = 0 });
                }
                catch (Exception ex)
                {
                    return Json(new { Message = "verall" + ex.Message, status = 1 });
                }

            }
        }


        [HttpPost]
        [Route("api/picture/save")]
     
        public ActionResult SaveUploadedFile()
        {
            string fName = "";
            string fname2 = "";
            string c = "";
            try
            {
                foreach (var file in Request.Form.Files)
                {
                    c = Request.Headers["h-id"].ToString();
                    // c = Request.Form.Keys.ToString();
                    var parsedContentDisposition = ContentDispositionHeaderValue.Parse(file.ContentDisposition);
                    fName = parsedContentDisposition.FileName.Trim('"');
                    if (file != null && file.ContentDisposition.Length > 0)
                    {

                        var originalDirectory = new DirectoryInfo(string.Format("{0}\\wwwroot\\WallImages", _appEnvironment.ContentRootPath));

                        string pathString = System.IO.Path.Combine(originalDirectory.ToString(), "imagepath");

                        var fileName1 = Path.GetFileName(ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"'));

                        bool isExists = System.IO.Directory.Exists(pathString);

                        if (!isExists)
                            System.IO.Directory.CreateDirectory(pathString);

                        int i = 0;
                        string filemask = "scart{0}" + ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                        fname2 = string.Format(filemask, i);
                        do
                        {
                            i = i + 1;
                            fname2 = String.Format(filemask, i);

                        } while (System.IO.File.Exists(pathString + "\\" + fname2));

                        string path = string.Format("{0}\\{1}", pathString, fname2);
                        string Originalpath = string.Format("{0}\\{1}", pathString, "og-" + fname2);
                        string thumbpath = string.Format("{0}\\{1}", pathString, "thumb-" + fname2);

                        Size size = new Size(150, 0);
                        var fileStream = file.OpenReadStream();
                        Image image = new Image(fileStream);
                        Image image2 = new Image(fileStream);
                        FileStream file2 = new FileStream(path, FileMode.Create, System.IO.FileAccess.Write);
                        int Sizer = 500;
                        float width = image.Width;
                        float height = image.Height;
                        int x = 0;
                        int y = 0;
                        if (width > height)
                        {
                            width = (width / height) * Sizer;
                            height = Sizer;
                            x = Convert.ToInt32(Math.Ceiling((double)((width - height) / 2)));
                        }
                        else if (height > width)
                        {
                            height = (height / width) * Sizer;
                            width = Sizer;
                            y = Convert.ToInt32(Math.Ceiling((double)((height - width) / 2)));
                        }
                        else
                        {
                            width = Sizer;
                            height = Sizer;
                        }
                        int maxWidth = Sizer;
                        int maxHeight = Sizer;
                        int newWidth = image.Width;
                        int newHeight = image.Height;
                        double aspectRatio = (double)image.Width / (double)image.Height;
                        if (aspectRatio <= 1 && image.Width > maxWidth)
                        {
                            newWidth = maxWidth;
                            newHeight = (int)Math.Round(newWidth / aspectRatio);
                        }
                        else if (aspectRatio > 1 && image.Height > maxHeight)
                        {
                            newHeight = maxHeight;
                            newWidth = (int)Math.Round(newHeight * aspectRatio);
                        }
                        Point pt = new Point(x, y);
                        ResizeOptions ogoptions = new ResizeOptions()
                        {
                            Size = new Size(newWidth, newHeight),
                            Mode = ResizeMode.Pad,
                            Sampler = new NearestNeighborResampler()

                        };
                        Size sz = new Size(newWidth, newHeight);
                        image.Resize(ogoptions).Save(file2);
                        ResizeOptions options = new ResizeOptions()
                        {
                            Size = new Size(400, 250),
                            Mode = ResizeMode.Crop,
                            Sampler = new NearestNeighborResampler()

                        };
                        FileStream file3 = new FileStream(thumbpath, FileMode.Create, System.IO.FileAccess.Write);
                        image2.Resize(options).Save(file3);
                        fileStream.Dispose();


                        file3.Dispose();
                        file2.Dispose();
                        var pid = Request.Headers["h-id"];
                        var ptype = Request.Headers["h-type"];
                        c = Request.Headers["h-id"].ToString() + "header," + x + "," + y;
                    }
                }

                return Json(new { Message = fname2, status = 0 });
            }
            catch (Exception ex)
            {
                return Json(new { Message = ex.Message + ex.InnerException, status = 1 });
            }

        }
    }
}