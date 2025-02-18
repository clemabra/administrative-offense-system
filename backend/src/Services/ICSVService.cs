namespace src.Services
{
	public interface ICSVService
	{
		public IEnumerable<T> ReadCSV<T>(Stream file);
	}
}