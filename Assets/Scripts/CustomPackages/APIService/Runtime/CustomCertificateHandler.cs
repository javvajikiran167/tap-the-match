using UnityEngine.Networking;

namespace Custom.Services.Network.RestAPI
{
    public class CustomCertificateHandler : CertificateHandler
    {
        protected override bool ValidateCertificate(byte[] certificateData)
        {
            return true;
        }
    }
}
