namespace WebApplication1.Services
{
    public class ServiceResult<T>
    {
        public bool Success { get; private set; }
        public string Message { get; private set; }
        public T Data { get; private set; }
        public bool HasChanges { get; private set; }

        // Construtor privado para forçar o uso dos métodos estáticos (mais limpo)
        private ServiceResult(bool success, string message, T data, bool hasChanges)
        {
            Success = success;
            Message = message;
            Data = data;
            HasChanges = hasChanges;
        }

        // Métodos auxiliares para facilitar a escrita no Service
        public static ServiceResult<T> Ok(T data)
            => new ServiceResult<T>(true, "Operação realizada com sucesso", data, true);

        public static ServiceResult<T> NoChanges(T data)
            => new ServiceResult<T>(true, "Nenhuma alteração detectada", data, false);

        public static ServiceResult<T> Failure(string message)
            => new ServiceResult<T>(false, message, default, false);
    }
}
