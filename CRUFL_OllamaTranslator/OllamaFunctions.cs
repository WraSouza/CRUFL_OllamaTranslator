using System;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;

namespace CRUFL_OllamaTranslator
{
    // Interface COM com GUID único
    [Guid("A1B2C3D4-E5F6-7890-1234-56789ABCDEF0")]
    [InterfaceType(ComInterfaceType.InterfaceIsDual)]
    [ComVisible(true)]
    public interface IOllamaFunctions
    {
        [DispId(1)]
        string Translate(string text, string targetLanguage);
    }

    // A classe DEVE implementar a interface e ter o atributo ProgId com o prefixo CRUFL
    [Guid("B2C3D4E5-F6A7-8901-2345-6789ABCDEF01")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("CRUFL_OllamaTranslator.OllamaFunctions")]
    [ComVisible(true)]
    public class OllamaFunctions : IOllamaFunctions
    {
        private const string API_URL = "<IP_DA_API>:PORTA/api/translate";

        public string Translate(string text, string targetLanguage)
        {
            if (string.IsNullOrWhiteSpace(text)) return string.Empty;

            try
            {
                string lang = string.IsNullOrWhiteSpace(targetLanguage) ? "EN" : targetLanguage.Trim().ToUpper();
                string jsonBody = "{\"text\":\"" + EscapeJson(text) + "\",\"targetLanguage\":\"" + lang + "\"}";

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(API_URL);
                request.Method = "POST";
                request.ContentType = "application/json";
                request.Timeout = 5000;

                byte[] bodyBytes = Encoding.UTF8.GetBytes(jsonBody);
                using (Stream reqStream = request.GetRequestStream())
                {
                    reqStream.Write(bodyBytes, 0, bodyBytes.Length);
                }

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    string jsonResponse = reader.ReadToEnd();

                    string key = "\"translatedText\":\"";
                    int startIndex = jsonResponse.IndexOf(key);
                    if (startIndex != -1)
                    {
                        startIndex += key.Length;
                        int endIndex = jsonResponse.IndexOf("\"", startIndex);
                        if (endIndex != -1)
                        {
                            return jsonResponse.Substring(startIndex, endIndex - startIndex);
                        }
                    }
                }
            }
            catch
            {
                return text;
            }

            return text;
        }

        private string EscapeJson(string str)
        {
            return str.Replace("\\", "\\\\").Replace("\"", "\\\"").Replace("\r", "").Replace("\n", " ");
        }
    }
}