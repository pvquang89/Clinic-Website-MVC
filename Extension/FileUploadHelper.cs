namespace WebPhongKham.Extension
{
    //class hepler để xử lý file
    public class FileUploadHelper
    {
       public async Task<string> UploadFileAsync(IFormFile file, string folderPath, string fileNamePrefix)
        {
            if (file != null || file.Length > 0)
            {
                //B1 : xác định đường dẫn save file
                var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", folderPath);
                //B2 : tạo folder nếu chưa có 
                if (!Directory.Exists(uploadFolder))
                    Directory.CreateDirectory(uploadFolder);
                //B3: Tạo tên file
                // Lấy phần mở rộng của file (ví dụ: .jpg, .png)
                var fileExtension = Path.GetExtension(file.FileName);
                //Tạo file name 
                var uniqueFileName = $"{fileNamePrefix}{fileExtension}";

                //B4 : path đầy đủ để save file
                var filePath = Path.Combine(uploadFolder, uniqueFileName);

                //Save file lên server 
                using(var stream = new FileStream(filePath, FileMode.Create))
                {
                    //sao chép dữ liệu từ tệp gốc vào luồng stream
                    await file.CopyToAsync(stream);
                }

                // Trả về đường dẫn lưu file
                return $"/{folderPath}/{uniqueFileName}";
            }
            return null;
        }

    }
}

