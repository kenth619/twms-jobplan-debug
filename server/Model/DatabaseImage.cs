namespace TWMSServer.Model
{
    public class DatabaseImage
    {
        // By encoding the image data in Base64 and formulating the right type of string, 
        // we can directly set the image in HTML without having it be stored somewhere.
        public static string EncodedImage(byte[]? imageData, string? imageType)
        {
            if (null == imageData || !imageData.Any() || null == imageType)
            {
                return "";
            }

            return $"data:{imageType};base64,{Convert.ToBase64String(imageData)}";
        }
    }
}
