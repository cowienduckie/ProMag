using System.Diagnostics.CodeAnalysis;

namespace Shared.Storage.Extensions;

public static class FileExtensions
{
    public enum FileType
    {
        Image = 1,
        Video = 2,
        Pdf = 3,
        Text = 4,
        Doc = 5,
        Docx = 6,
        Ppt = 7
    }

    public static bool IsValidFile(this byte[] byteFile, FileType fileType, string fileContentType)
    {
        var isValid = false;

        switch (fileType)
        {
            case FileType.Image:
                isValid = IsValidImageFile(byteFile, fileContentType);
                break;
            case FileType.Video:
                isValid = IsValidVideoFile(byteFile, fileContentType);
                break;
            case FileType.Pdf:
                isValid = IsValidPdfFile(byteFile, fileContentType);
                break;
            case FileType.Text:
                break;
            case FileType.Doc:
                break;
            case FileType.Docx:
                break;
            case FileType.Ppt:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(byteFile), fileType, null);
        }

        return isValid;
    }


    [SuppressMessage("ReSharper", "CognitiveComplexity")]
    private static bool IsValidImageFile(IReadOnlyList<byte> byteFile, string fileContentType)
    {
        var isValid = false;

        byte[] chkByteJpg = { 255, 216, 255, 224 };
        byte[] chkByteBmp = { 66, 77 };
        byte[] chkByteGif = { 71, 73, 70, 56 };
        byte[] chkBytePng = { 137, 80, 78, 71 };

        var imageFileExtension = ImageFileExtension.None;

        if (fileContentType.Contains("jpg") | fileContentType.Contains("jpeg"))
        {
            imageFileExtension = ImageFileExtension.Jpg;
        }
        else if (fileContentType.Contains("png"))
        {
            imageFileExtension = ImageFileExtension.Png;
        }
        else if (fileContentType.Contains("bmp"))
        {
            imageFileExtension = ImageFileExtension.Bmp;
        }
        else if (fileContentType.Contains("gif"))
        {
            imageFileExtension = ImageFileExtension.Gif;
        }

        switch (imageFileExtension)
        {
            case ImageFileExtension.Jpg:
            {
                if (byteFile.Count >= 4)
                {
                    var j = 0;
                    for (var i = 0; i <= 3; i++)
                    {
                        if (byteFile[i] != chkByteJpg[i])
                        {
                            continue;
                        }

                        j = j + 1;
                        if (j == 3)
                        {
                            isValid = true;
                        }
                    }
                }

                break;
            }
            case ImageFileExtension.Png:
            {
                if (byteFile.Count >= 4)
                {
                    var j = 0;
                    for (var i = 0; i <= 3; i++)
                    {
                        if (byteFile[i] != chkBytePng[i])
                        {
                            continue;
                        }

                        j = j + 1;
                        if (j == 3)
                        {
                            isValid = true;
                        }
                    }
                }

                break;
            }
            case ImageFileExtension.Bmp:
            {
                if (byteFile.Count >= 4)
                {
                    var j = 0;
                    for (var i = 0; i <= 1; i++)
                    {
                        if (byteFile[i] != chkByteBmp[i])
                        {
                            continue;
                        }

                        j = j + 1;
                        if (j == 2)
                        {
                            isValid = true;
                        }
                    }
                }

                break;
            }
        }

        if (imageFileExtension != ImageFileExtension.Gif)
        {
            return isValid;
        }

        {
            if (byteFile.Count < 4)
            {
                return isValid;
            }

            var j = 0;
            for (var i = 0; i <= 1; i++)
            {
                if (byteFile[i] != chkByteGif[i])
                {
                    continue;
                }

                j = j + 1;
                if (j == 3)
                {
                    isValid = true;
                }
            }
        }

        return isValid;
    }

    [SuppressMessage("ReSharper", "CognitiveComplexity")]
    private static bool IsValidVideoFile(this IReadOnlyList<byte> byteFile, string fileContentType)
    {
        var isValid = false;

        byte[] chkByteWmv = { 48, 38, 178, 117 };
        byte[] chkByteAvi = { 82, 73, 70, 70 };
        byte[] chkByteFlv = { 70, 76, 86, 1 };
        byte[] chkByteMpg = { 0, 0, 1, 186 };
        byte[] chkByteMp4 = { 0, 0, 0, 20 };

        var videoFileExt = VideoFileExtension.None;
        if (fileContentType.Contains("wmv"))
        {
            videoFileExt = VideoFileExtension.Wmv;
        }
        else if (fileContentType.Contains("mpg") || fileContentType.Contains("mpeg"))
        {
            videoFileExt = VideoFileExtension.Mpg;
        }
        else if (fileContentType.Contains("mp4"))
        {
            videoFileExt = VideoFileExtension.Mp4;
        }
        else if (fileContentType.Contains("avi"))
        {
            videoFileExt = VideoFileExtension.Avi;
        }
        else if (fileContentType.Contains("flv"))
        {
            videoFileExt = VideoFileExtension.Flv;
        }

        switch (videoFileExt)
        {
            case VideoFileExtension.Wmv when byteFile.Count < 4:
                return false;
            case VideoFileExtension.Wmv:
            {
                var j = 0;
                for (var i = 0; i <= 3; i++)
                {
                    if (byteFile[i] != chkByteWmv[i])
                    {
                        continue;
                    }

                    j = j + 1;
                    if (j == 3)
                    {
                        isValid = true;
                    }
                }

                break;
            }
            case VideoFileExtension.Mpg:
            {
                if (byteFile.Count < 4)
                {
                    return isValid;
                }

                var j = 0;
                for (var i = 0; i <= 3; i++)
                {
                    if (byteFile[i] != chkByteMpg[i])
                    {
                        continue;
                    }

                    j = j + 1;
                    if (j == 3)
                    {
                        isValid = true;
                    }
                }

                break;
            }
            case VideoFileExtension.Mp4 when byteFile.Count < 4:
                return isValid;
            case VideoFileExtension.Mp4:
            {
                var j = 0;
                for (var i = 0; i <= 3; i++)
                {
                    if (byteFile[i] != chkByteMp4[i])
                    {
                        continue;
                    }

                    j = j + 1;
                    if (j == 3)
                    {
                        isValid = true;
                    }
                }

                break;
            }
            case VideoFileExtension.Avi when byteFile.Count < 4:
                return isValid;
            case VideoFileExtension.Avi:
            {
                var j = 0;
                for (var i = 0; i <= 3; i++)
                {
                    if (byteFile[i] != chkByteAvi[i])
                    {
                        continue;
                    }

                    j = j + 1;
                    if (j == 3)
                    {
                        isValid = true;
                    }
                }

                break;
            }
            case VideoFileExtension.Flv when byteFile.Count < 4:
                return isValid;
            case VideoFileExtension.Flv:
            {
                var j = 0;
                for (var i = 0; i <= 3; i++)
                {
                    if (byteFile[i] != chkByteFlv[i])
                    {
                        continue;
                    }

                    j = j + 1;
                    if (j == 3)
                    {
                        isValid = true;
                    }
                }

                break;
            }
        }

        return isValid;
    }

    private static bool IsValidPdfFile(this IReadOnlyList<byte> byteFile, string fileContentType)
    {
        var isValid = false;

        byte[] chkBytePdf = { 37, 80, 68, 70 };

        var pdfFileExtension = PdfFileExtension.None;

        if (fileContentType.Contains("pdf"))
        {
            pdfFileExtension = PdfFileExtension.Pdf;
        }

        if (pdfFileExtension != PdfFileExtension.Pdf)
        {
            return false;
        }

        if (byteFile.Count < 4)
        {
            return false;
        }

        var j = 0;
        for (var i = 0; i <= 3; i++)
        {
            if (byteFile[i] != chkBytePdf[i])
            {
                continue;
            }

            j = j + 1;
            if (j == 3)
            {
                isValid = true;
            }
        }

        return isValid;
    }

    private enum ImageFileExtension
    {
        None = 0,
        Jpg = 1,
        Jpeg = 2,
        Bmp = 3,
        Gif = 4,
        Png = 5
    }

    private enum VideoFileExtension
    {
        None = 0,
        Wmv = 1,
        Mpg = 2,
        Mpeg = 3,
        Mp4 = 4,
        Avi = 5,
        Flv = 6
    }

    private enum PdfFileExtension
    {
        None = 0,
        Pdf = 1
    }
}