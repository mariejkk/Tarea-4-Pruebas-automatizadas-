namespace GestionCanchas.Application.Common
{
    public class ServiceResult
    {
        public bool Exitoso { get; set; }
        public List<string> Errores { get; set; } = new();

        public static ServiceResult Ok() => new() { Exitoso = true };

        public static ServiceResult Fallo(params string[] errores) => new()
        {
            Exitoso = false,
            Errores = errores.ToList()
        };
    }

    public class ServiceResult<T> : ServiceResult
    {
        public T? Data { get; set; }

        public static ServiceResult<T> Ok(T data) => new()
        {
            Exitoso = true,
            Data = data
        };

        public new static ServiceResult<T> Fallo(params string[] errores) => new()
        {
            Exitoso = false,
            Errores = errores.ToList()
        };
    }
}
