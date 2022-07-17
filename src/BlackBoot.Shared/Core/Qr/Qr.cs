using QRCoder;

namespace BlackBoot.Shared.Core;

public class Qr
{
    public static string Generate(string qrMessage)
    {
        try
        {
            QRCodeGenerator qRCodeGenerator = new();
            QRCodeData qrCodeData = qRCodeGenerator.CreateQrCode(qrMessage, QRCodeGenerator.ECCLevel.Q);
            Base64QRCode base64QRCode = new(qrCodeData);
            return $"data:image/png;base64,{base64QRCode.GetGraphic(5)}";
        }
        catch (Exception e)
        {
            FileLoger.Error(e);
        }
        return string.Empty;
    }
}

