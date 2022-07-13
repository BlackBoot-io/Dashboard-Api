using IronBarCode;
using System.Drawing;

namespace BlackBoot.Shared.Core;

public class Qr
{
    public static string Generate(string qrMessage)
    {
        try
        {
            GeneratedBarcode barcode = QRCodeWriter.CreateQrCode(qrMessage, 200);
            barcode.AddBarcodeValueTextBelowBarcode();
            barcode.SetMargins(10);
            barcode.ChangeBarCodeColor(Color.Black);
            byte[] bytes = barcode.ToJpegBinaryData();
            string base64String = Convert.ToBase64String(bytes, 0, bytes.Length);
            return $"data:image/png;base64,{base64String}";
        }
        catch (Exception)
        {
        }
        return string.Empty;
    }
}

