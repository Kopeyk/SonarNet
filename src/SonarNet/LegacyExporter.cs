namespace SonarNet
{
    /// <summary>Експорт замовлень у текстовий файл.</summary>
    public class LegacyExporter
    {
        public string ExportLine(Order order, decimal total) =>
            string.Format("Order {0}: total {1}, country {2}", order.Id, total, order.Country);

        // Помилка запису більше не ігнорується: про неї повідомляється через onError
        public bool Save(string path, string text, Action<string> onError)
        {
            try
            {
                File.WriteAllText(path, text);
                return true;
            }
            catch (IOException ex)
            {
                onError(ex.Message);
                return false;
            }
        }

        // Пароль не зберігається в коді: рядок підключення береться зі змінної середовища
        public string ConnectionString() =>
            Environment.GetEnvironmentVariable("SONARNET_DB") ?? string.Empty;
    }
}
