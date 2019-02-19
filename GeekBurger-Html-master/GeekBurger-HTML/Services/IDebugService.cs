using System.Threading.Tasks;

namespace GeekBurger_HTML.Services
{
    public interface IDebugService
    {
        Task<bool> SendMessageAsync(string topic, string label, string filePath);
    }
}